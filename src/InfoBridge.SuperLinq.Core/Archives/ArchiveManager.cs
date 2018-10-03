﻿using InfoBridge.SuperLinq.Core.Attributes;
using InfoBridge.SuperLinq.Core.Projection;
using InfoBridge.SuperLinq.Core.QueryBuilders;
using SuperOffice.CRM.Globalization;
using SuperOffice.CRM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Archives
{
    public class ArchiveManager<T> : IArchiveManager<T>
    {
        private const int SKIP_MULTIPLE = 10;

        private readonly IArchiveExecutor _executor;
        private readonly IDateTimeConverter _dateTimeConverter;
        private readonly IArchiveExecutionContextProvider _archiveExecutionContextProvider;
        private readonly IResultParser _resultParser;

        public ArchiveManager(IArchiveExecutor executor, IArchiveExecutionContextProvider archiveExecutionContextProvider, IResultParser resultParser)
        {
            if (executor == null) { throw new ArgumentNullException(nameof(executor)); }
            if (archiveExecutionContextProvider == null) { throw new ArgumentNullException(nameof(archiveExecutionContextProvider)); }
            if (resultParser == null) { throw new ArgumentNullException(nameof(resultParser)); }

            _executor = executor;
            _archiveExecutionContextProvider = archiveExecutionContextProvider;
            _resultParser = resultParser;
        }

        public T DoQuerySingle(RestrictionBuilderBase restrictionBuilder, OrderByBuilder orderByBuilder = null)
        {
            IList<T> result = InnerDoQuery(restrictionBuilder, orderByBuilder, 1, 0);
            if (result != null) { return result.FirstOrDefault(); }
            else { return default(T); }
        }

        public IList<T> DoQuery(RestrictionBuilderBase restrictionBuilder, OrderByBuilder orderByBuilder = null, int noItems = 0, int skip = 0)
        {
            return InnerDoQuery(restrictionBuilder, orderByBuilder, noItems, skip);
        }

        private IList<T> InnerDoQuery(RestrictionBuilderBase restrictionBuilder, OrderByBuilder orderByBuilder, int max, int skip)
        {
            ArchiveExecutionContext executionContext = _archiveExecutionContextProvider.Get();
            if (executionContext == null) { throw new InvalidOperationException(nameof(executionContext) + " not set, cannot be null"); }

            ArchiveQueryParameters context = new ArchiveQueryParameters();
            context.ArchiveName = executionContext.ArchiveName;
            context.Entities = executionContext.Entities;
            context.RequestedColumns = DynamicPropertyHelper.GetAllDbColumnsPrefixed<T>();
            int page = 0;

            if (skip > 0)
            {
                page = SetPageSizeAndGetPage(skip, max, context);
            }
            if (restrictionBuilder != null)
            {
                context.Restrictions = restrictionBuilder.GetRestrictions();
            }
            if (orderByBuilder != null)
            {
                context.OrderBy = orderByBuilder.Get();
            }

            return _resultParser.Parse<T>(executionContext, _executor.GetItems(context, max, page));
        }

        private int SetPageSizeAndGetPage(int skip, int max, ArchiveQueryParameters context)
        {
            if (skip > ArchiveQueryParameters.MAX_PAGE_SIZE)
            {
                throw new NotSupportedException("Skip cannot exceed the maximum page size of " + ArchiveQueryParameters.MAX_PAGE_SIZE);
            }
            if (skip > 0 && max > 0 && skip % SKIP_MULTIPLE != 0)
            {
                throw new NotSupportedException("When specifying both Skip and Take (max) then Skip must be a multiple of " + SKIP_MULTIPLE);
            }

            if (skip > max && max > 0)
            {
                //we don't want to fetch a lot of useless results, so we use small page sizes to get to our max. 
                //the downsideis that we might be doing a lot of calls before we get to max
                context.PageSize = SKIP_MULTIPLE;
                return skip / context.PageSize;
            }
            else
            {
                //when no max is specified or if skip <= max we just fetch everything after the skipped rows
                context.PageSize = skip;
                return 1;
            }
        }
    }
}