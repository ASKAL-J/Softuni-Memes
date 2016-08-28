namespace Softuni___Memes.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Softuni___Memes.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Softuni___Memes.Models.ApplicationDbContext";
        }

        protected override void Seed(Softuni___Memes.Models.ApplicationDbContext context)
        {
           
        }
    }
}
