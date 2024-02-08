namespace XPressPayments.Common.Dtos
{
    public class PagedList<T> : OperationResult<T>
    {
        public string Search { get; set; }
        public new IEnumerable<T> Result { get; set; }
        public PagedResponse Pagination { get; set; }

        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            Pagination = new PagedResponse(count, pageNumber, pageSize);
            Result = items;
        }

        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            var items = new List<T>();
            try
            {
                var page_number = (pageIndex <= 0) ? 1 : pageIndex;
                var page_size = (pageSize <= 0) ? 10 : pageSize;
                count = source.Count();
                items = source.Skip((page_number - 1) * page_size).Take(page_size).ToList();
                return new PagedList<T>(items, count, page_number, page_size);
            }
            catch (Exception ex)
            {
                return new PagedList<T>(items, count, 1, 0);
            }
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> items, int count = 0, int pageIndex = 1, int pageSize = 10)
        {
            var page_number = (pageIndex <= 0) ? 1 : pageIndex;
            var page_size = (pageSize <= 0) ? 10 : pageSize;
            return new PagedList<T>(items.ToList(), count, page_number, page_size);
        }
    }

    public class PagedResponse
    {
        public PagedResponse(int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
