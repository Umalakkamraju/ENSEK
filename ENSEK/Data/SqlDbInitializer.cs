

namespace ENSEK.Data
{
    public static class SqlDbInitializer
    {
        public static void Initialize(MeterReadingContext context)
        {
             
            context.Database.EnsureCreated(); 
            // Additional seeding logic can go here.
        }
    }

}
