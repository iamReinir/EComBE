using EComBusiness.Entity;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ECom.Service
{
    public static class EdmModel
    {
        public static IEdmModel getModel()
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<Category>(nameof(Category));
            builder.EntitySet<Product>(nameof(Product));
            builder.EntitySet<User>(nameof(User));
            return builder.GetEdmModel();
        }
    }
}
