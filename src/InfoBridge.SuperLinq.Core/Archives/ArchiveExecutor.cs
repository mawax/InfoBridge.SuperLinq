using InfoBridge.SuperLinq.Core.Archives.AgentWrapper;
using SuperOffice.Services75;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Archives
{
    public class ArchiveExecutor : IArchiveExecutor
    {
        private readonly Func<IArchiveAgentWrapper> _archiveAgentWrapperCreator;

        public ArchiveExecutor(Func<IArchiveAgentWrapper> archiveAgentWrapperCreator)
        {
            if (archiveAgentWrapperCreator == null) { throw new ArgumentNullException(nameof(archiveAgentWrapperCreator)); }
            _archiveAgentWrapperCreator = archiveAgentWrapperCreator;
        }

        public IList<ArchiveListItem> GetItems(ArchiveQueryParameters parameters, int maxItems, int page)
        {
            return GetMultiPageItems(parameters, maxItems, page);
        }

        private IList<ArchiveListItem> GetMultiPageItems(ArchiveQueryParameters parameters, int max, int page)
        {
            List<ArchiveListItem> items = new List<ArchiveListItem>();
            if (parameters.PageSize > max && max > 0 && page == 0)
            {
                //if we can get the requested amount of items in one go, do so. this only works if we don't want to start from a specific page
                parameters.PageSize = max;
            }

            using (IArchiveAgentWrapper agent = _archiveAgentWrapperCreator())
            {
                bool hasResult = true;
                int loopPage = page;
                while (hasResult) //loop while we still can get results
                {
                    //change the context if we are nearing our max, but only if we did not specify a page 
                    if (max > 0 && items.Count + parameters.PageSize > max && page == 0)
                    {
                        parameters.PageSize = max - items.Count;
                    }

                    IList<ArchiveListItem> itemsOnPage = GetItems(parameters, agent, loopPage);

                    //stop if there are no more items found
                    if (itemsOnPage == null)
                    {
                        hasResult = false;
                    }
                    else
                    {
                        items.AddRange(itemsOnPage);
                        loopPage++;

                        //check if the number of results matches the page-size, if not, stop
                        if (itemsOnPage.Count < parameters.PageSize)
                        {
                            hasResult = false;
                        }

                        //if there is a max and the item count is equal to or exceeded the max, we should stop as well
                        if (max > 0 && items.Count >= max)
                        {
                            hasResult = false;
                        }
                    }
                }
            }


            //remove items if we fetched too many (possible when max and page is specified
            if (max > 0 && items.Count > max)
            {
                items.RemoveRange(max, items.Count - max);
            }
            return items;
        }

        private IList<ArchiveListItem> GetItems(ArchiveQueryParameters parameters, IArchiveAgentWrapper agent, int page)
        {
            if (parameters.PageSize > ArchiveQueryParameters.MAX_PAGE_SIZE)
            {
                throw new NotSupportedException("Page size cannot be larger than " + ArchiveQueryParameters.MAX_PAGE_SIZE);
            }
            if (parameters.Restrictions == null || parameters.Restrictions.Count == 0)
            {
                throw new Exception("No restriction specified. The SuperOffice ArchiveAgent needs at least one restriction.");
            }

            ArchiveListItem[] results = agent.GetArchiveListByColumns(parameters.ArchiveName, GetColumns(parameters), GetOrderBy(parameters), GetRestrictions(parameters), parameters.Entities, page, parameters.PageSize);

            if (results == null || results.Length == 0) { return null; }
            else
            {
                return results;
            }
        }

        private string[] GetColumns(ArchiveQueryParameters context)
        {
            if (context.RequestedColumns.Count == 0) { throw new Exception("No columns specified"); }
            return context.RequestedColumns.Distinct().ToArray();
        }

        private ArchiveRestrictionInfo[] GetRestrictions(ArchiveQueryParameters context)
        {
            if (context.Restrictions == null) { return null; }
            return context.Restrictions.ToArray();
        }

        private ArchiveOrderByInfo[] GetOrderBy(ArchiveQueryParameters context)
        {
            if (context.OrderBy == null) { return null; }
            return context.OrderBy.ToArray();
        }
    }
}
