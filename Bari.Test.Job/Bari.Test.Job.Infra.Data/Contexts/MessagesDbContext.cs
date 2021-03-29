using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Bari.Test.Job.Infra.Data.Contexts
{
    public class MessagesDbContext : DbContext
    {

        public MessagesDbContext(DbContextOptions<MessagesDbContext> options) : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MessageMap());
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=MessagesDB;User ID=SA;Password=<SA@sa1234>;MultipleActiveResultSets=true;");
        //    }
        //}

        public DbSet<Message> Messages { get; set; }
    }
}
