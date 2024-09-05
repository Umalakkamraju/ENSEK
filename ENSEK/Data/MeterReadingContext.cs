using ENSEK.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;



namespace ENSEK.Data
{

 

    public class MeterReadingContext : DbContext
    {
        public MeterReadingContext(DbContextOptions<MeterReadingContext> options)
            : base(options)
        {
        }


        // No DbSet for MeterReadingUploadModel

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure MeterReadingUploadModel as a keyless entity if it is used for queries or other purposes
            modelBuilder.Entity<MeterReadingUploadModel>(entity =>
            {
                entity.HasNoKey(); // Indicates that this entity does not have a primary key
                entity.ToTable("MeterReadingUploadModel"); // Optional: specify a table name if necessary
            });
        }
    }

}
