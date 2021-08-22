using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Params.PagingParams
{
    public class PagingParamsReq
    {
        private const int MaxPageRowCount = 100000;
        private const int MinPageRowCount = 200;

        private int pageNumber = 1;
        private int pageRowCount = 200;
        private string orderAscDesc = "ASC";
        
        public int PageNumber
        {
            get { return pageNumber; }

            set
            {
                if (value < 1)
                {
                    pageNumber = 1;
                }
                else
                    pageNumber = value;
            }
        }

        /// <summary>
        /// number of rows in a page
        /// </summary>
        public int PageRowCount
        {
            get { return pageRowCount; }

            set
            {
                if ((value <= 0) && (value != -1))
                {
                    pageRowCount = MinPageRowCount;
                }
                else if (value > MaxPageRowCount)
                {
                    pageRowCount = MaxPageRowCount;
                }
                else
                    pageRowCount = value;
            }
        }

        public string OrderColumnName { get; set; }

        /// <summary>
        /// ASC | DESC
        /// </summary>
        public string OrderAscDesc
        {
            get { return orderAscDesc; }
            set { orderAscDesc = (value.ToUpper() != "ASC" && value.ToUpper() != "DESC") ? "ASC" : value; }
        }
    }
}