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
        public static EntityBase UpdateAudit(this EntityBase entity)
        {
            entity.UpdatedAt = DateTime.Now;
            return entity;
        }

        public static EntityBase SoftDelete(this EntityBase entity)
        {
            entity.IsDeleted = true;
            return entity;
        }
    }
}
