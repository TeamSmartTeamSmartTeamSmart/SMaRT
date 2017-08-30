namespace SMaRT.Shared.Util
{
    using System.Data.Entity;

    public static class EntityFrameworkExtensionMethods
    {
        public static void Clear<T>(this DbSet<T> dbset) where T : class
        {
            dbset.RemoveRange(dbset);
        }
    }
}
