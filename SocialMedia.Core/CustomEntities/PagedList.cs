namespace SocialMedia.Core.CustomEntities
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; } //Representa la página actual de la lista paginada.
        public int TotalPages { get; set; } //Representa el número total de páginas en la lista paginada.
        public int PageSize { get; set; } //Representa el número de elementos por página.
        public int TotalCount { get; set; } //Representa el número total de elementos en la lista sin paginar.
        public bool HasPreviousPage => CurrentPage > 1; // Devuelve true si hay una página anterior a la página actual, de lo contrario, devuelve false.
        public bool HasNextPage => CurrentPage < TotalCount; //Devuelve true si hay una página siguiente a la página actual, de lo contrario, devuelve false.
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : (int?)null; //Devuelve el número de la siguiente página si existe, o null si no hay una página siguiente.
        public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : (int?)null; //Devuelve el número de la página anterior si existe, o null si no hay una página anterior.

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
