namespace EComBusiness.HelperModel
{
    public class EntityBase
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

    public static class EntityExtension
    {
        public static TResult UpdateAudit<TResult>(this TResult entity) where TResult : EntityBase
        {
            entity.UpdatedAt = DateTime.Now;
            return entity;
        }

        public static TResult SoftDelete<TResult>(this TResult entity) where TResult : EntityBase
        {
            entity.IsDeleted = true;
            return entity;
        }

        public static IQueryable<TResult> NotDeleted<TResult>(this IQueryable<TResult> query) where TResult : EntityBase
        {
            return query.Where(item => item.IsDeleted == false);
        }
    }
}
