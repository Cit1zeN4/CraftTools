namespace CraftTools.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class CraftToolsContext : DbContext
    {
        public CraftToolsContext()
            : base("name=CraftToolsContext")
        {
        }

        DbSet<Material> Materials { get; set; }
        DbSet<Ware> Wares { get; set; }
        DbSet<Profit> Profits { get; set; }
        DbSet<Loss> Losses { get; set; }
    }
}