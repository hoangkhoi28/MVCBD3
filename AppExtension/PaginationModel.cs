using System;

namespace AppExtension
{
    public class PaginationModel
    {
        public int PageSize { get; set; }
        public long NumberOfRows { get; set; }

        public int NumberOfPages
        {
            get
            {
                return Convert.ToInt32(Math.Ceiling((double)NumberOfRows / PageSize));
            }
        }

        public int CurrentPage { get; set; }
    }
}