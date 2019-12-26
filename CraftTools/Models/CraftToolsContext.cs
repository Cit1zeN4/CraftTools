namespace CraftTools.Models
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;

    public class CraftToolsContext : DbContext
    {
        public CraftToolsContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
            Database.Migrate();
        }

        public DbSet<Material> Materials { get; set; }
        public DbSet<Ware> Wares { get; set; }
        public DbSet<WareMaterial> WareMaterials { get; set; }
        public DbSet<Profit> Profits { get; set; }
        public DbSet<Loss> Losses { get; set; }
    }
}