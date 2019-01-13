namespace CraftTools.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class CraftToolsContext : DbContext
    {
        public CraftToolsContext()
            : base(Helpers.Tools.GetConnectionString())
        {
        }

        public DbSet<Material> Materials { get; set; }
        public DbSet<Ware> Wares { get; set; }
        public DbSet<WareMaterial> WareMaterials { get; set; }
        public DbSet<Profit> Profits { get; set; }
        public DbSet<Loss> Losses { get; set; }
    }
}