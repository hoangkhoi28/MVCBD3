using System.Collections.Generic;

namespace AppExtension
{
    public class PagedData<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}