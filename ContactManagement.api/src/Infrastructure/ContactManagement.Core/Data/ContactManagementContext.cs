using ContactManagement.Core.Mapping;
using ContactManagement.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ContactManagement.Core.Data
{
    public class ContactManagementContext : DbContext
    {
        public ContactManagementContext(DbContextOptions<ContactManagementContext> options) : base(options)
        {
        }

        //[DbFunction(FunctionName = "SOUNDEX", Schema = "")]
        //public static string SoundsLike(string keywords)
        //{
        //    throw new NotImplementedException();
        //}

        public DbSet<UserInfo> UserInfoes { get; set; }
        public DbSet<ContactCategory> ContactCategories { get; set; }
        public DbSet<ContactInfo> ContactInfoes { get; set; }
        //public DbSet<Language> Languages { get; set; }
        //public DbSet<Level> Levels { get; set; }
        //  public DbSet<Syllabus> Syllabuses { get; set; }
        //public DbSet<SyllabusLanguage> SyllabusLanguages { get; set; }
        //public DbSet<Trade> Trades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserInfoMapping());
            modelBuilder.ApplyConfiguration(new ContactCategoryMapping());
            modelBuilder.ApplyConfiguration(new ContactInfoMapping());
            //modelBuilder.ApplyConfiguration(new SyllabusLanguageMapping());
            //   modelBuilder.ApplyConfiguration(new SyllabusMapping());
            //modelBuilder.ApplyConfiguration(new TradeMapping());
        }
    }
}
