namespace SocialMedia.Core.QueryFilters
{
    public class PostQueryFilter
    {
        public int? userId {  get; set; }
        public DateTime? date { get; set; }
        public string? description { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
