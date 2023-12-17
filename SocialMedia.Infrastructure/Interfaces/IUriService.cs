using SocialMedia.Core.QueryFilters;

namespace SocialMedia.Infrastructure.Interfaces
{
    public interface IUriService
    {
        Uri GetPostPaginatorUri(PostQueryFilter filter, string actionUrl);
    }
}