using InfoBridge.SuperLinq.Core.Projection;
using SuperOffice.CRM.Globalization;
using SuperOffice.Services75;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace InfoBridge.SuperLinq.Core.QueryBuilders
{
    public abstract class RestrictionBuilderBase
    {
        private ArchiveRestrictionInfo _last;
        private List<ArchiveRestrictionInfo> _restrictions = new List<ArchiveRestrictionInfo>();

        public List<ArchiveRestrictionInfo> GetRestrictions()
        {
            return _restrictions;
        }

        public ArchiveRestrictionInfo GetLast()
        {
            return _last;
        }

        protected void AddRestriction(string fullColumnName, EOperator op, InterRestrictionOperator interRestrictionOperator, int level, params object[] values)
        {
            AddRestriction(CreateRestriction(fullColumnName, op, interRestrictionOperator, level, values));
        }

        protected void AddRestriction<T>(string column, EOperator op, InterRestrictionOperator interRestrictionOperator, int level, params object[] values)
        {
            AddRestriction(CreateRestriction<T>(column, op, interRestrictionOperator, level, values));
        }

        protected void AddRestriction(ArchiveRestrictionInfo restriction)
        {
            _restrictions.Add(restriction);
            _last = restriction;
        }

        protected ArchiveRestrictionInfo CreateRestriction<T>(string column, EOperator op, InterRestrictionOperator interRestrictionOperator, int level, params object[] values)
        {
            string fullColumnName = DynamicPropertyHelper.GetFullDotSyntaxColumnName<T>(column);
            return CreateRestriction(fullColumnName, op, interRestrictionOperator, level, values);
        }

        /// <summary>
        /// Create the SuperOffice restriction
        /// </summary>
        /// <param name="fullColumnName">Column name including table (dot syntax)</param>
        /// <param name="op">Operator to compare value</param>
        /// <param name="interRestrictionOperator">And / Or</param>
        /// <param name="level">Level of the restriction (for parenthesis). This corrosponds directly with the InterParenthesis value of a SuperOffice ArchiveRestrictionInfo</param>
        /// <param name="values">Value(s) for column comparison in restriction</param>
        /// <returns>Newly created archive restriction info</returns>
        protected ArchiveRestrictionInfo CreateRestriction(string fullColumnName, EOperator op, InterRestrictionOperator interRestrictionOperator, int level, params object[] values)
        {
            ArchiveRestrictionInfo r = new ArchiveRestrictionInfo();
            r.Name = fullColumnName;
            r.Operator = op.ToString().ToLower();
            r.Values = ConvertValues(values);
            r.InterParenthesis = level;
            r.IsActive = true;

            SetPrevInterRestrictionOperator(interRestrictionOperator);

            return r;
        }

        protected void SetPrevInterRestrictionOperator(InterRestrictionOperator interRestrictionOperator)
        {
            if (GetRestrictions().Count > 0)
            {
                //set interoperator of last item
                GetRestrictions().LastOrDefault().InterOperator = interRestrictionOperator;
            }
        }

        protected string[] ConvertValues(object[] values)
        {
            if (values == null || values.Length == 1 && values[0] == null) { return null; }

            //check if paramters are incorrectly picked up as object[]{int[]} and if so, fix this
            if (values.Length == 1 && values[0].GetType().IsArray)
            {
                var tmpList = new List<object>();
                foreach (object o in (IEnumerable)values[0]) { tmpList.Add(o); }
                values = tmpList.ToArray();
            }

            string[] ret = new string[values.Length];

            for (int i = 0; i < ret.Length; i++)
            {
                string convVal = null;

                if (values[i] != null)
                {
                    if (values[i] is int) { convVal = CultureDataFormatter.EncodeInt((int)values[i]); }
                    else if (values[i] is DateTime) { convVal = CultureDataFormatter.EncodeDateTime((DateTime)values[i]); }
                    else if (values[i] is double) { convVal = CultureDataFormatter.EncodeDouble((double)values[i]); }
                    else if (values[i] is string) { convVal = (string)values[i]; }
                    else { convVal = CultureDataFormatter.Encode(values[i]); }
                }

                ret[i] = convVal;
            }
            return ret;
        }
    }
}
