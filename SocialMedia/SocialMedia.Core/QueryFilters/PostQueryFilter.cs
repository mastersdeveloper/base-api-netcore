using System;

namespace SocialMedia.Core.QueryFilters
{
    public class PostQueryFilter
    {
        public int? UserId { get; set; }
        public DateTime? Date { get; set; }
        public string Descripcion { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
