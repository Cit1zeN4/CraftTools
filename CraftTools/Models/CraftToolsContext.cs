namespace CraftTools.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class CraftToolsContext : DbContext
    {
        public CraftToolsContext()
            : base("name=CraftTools.Properties.Settings.CraftToolsContext")
        {
        }

        public DbSet<Material> Materials { get; set; }
        public DbSet<Ware> Wares { get; set; }
        public DbSet<Profit> Profits { get; set; }
        public DbSet<Loss> Losses { get; set; }
    }
}