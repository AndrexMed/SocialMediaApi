using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;

namespace SocialMedia.Infrastructure.Services
{
    public class UriService : IUriService
    {
        private readonly string _uriBase;

        public UriService(string uriBase)
        {
          _uriBase = uriBase;
        }

        public Uri GetPostPaginatorUri(PostQueryFilter filter, string actionUrl)
        {
            string baseUrl = $"{_uriBase}{actionUrl}";
            return new Uri(baseUrl);
        }
    }
}
