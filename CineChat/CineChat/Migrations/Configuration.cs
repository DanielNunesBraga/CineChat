namespace CineChat.Migrations
{
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CineChat.DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CineChat.DAL.ApplicationDbContext";
        }

        protected override void Seed(CineChat.DAL.ApplicationDbContext context)
        {

            if (!context.Roles.Any(r => r.Name == "AppAdmin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppAdmin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "admin@system.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "admin@system.com" };

                manager.Create(user, "systemadmin");
                manager.AddToRole(user.Id, "AppAdmin");
            }

            if(!context.categorie.Any(c => c.description == "Action"))
            {
                var category = new Category();
                category.description = "Action";
                context.categorie.Add(category);
            }

            if (!context.categorie.Any(c => c.description == "Adventure"))
            {
                var category = new Category();
                category.description = "Adventure";
                context.categorie.Add(category);
            }

            if (!context.categorie.Any(c => c.description == "Animation"))
            {
                var category = new Category();
                category.description = "Animation";
                context.categorie.Add(category);
            }

            if (!context.categorie.Any(c => c.description == "Biography"))
            {
                var category = new Category();
                category.description = "Biography";
                context.categorie.Add(category);
            }  

            if (!context.categorie.Any(c => c.description == "Comedy"))
            {
                var category = new Category();
                category.description = "Comedy";
                context.categorie.Add(category);
            }

            if (!context.categorie.Any(c => c.description == "Crime"))
            {
                var category = new Category();
                category.description = "Crime";
                context.categorie.Add(category);
            }  

            if (!context.categorie.Any(c => c.description == "Drama"))
            {
                var category = new Category();
                category.description = "Drama";
                context.categorie.Add(category);
            }

            if (!context.categorie.Any(c => c.description == "Fantasy"))
            {
                var category = new Category();
                category.description = "Fantasy";
                context.categorie.Add(category);
            }

            if (!context.categorie.Any(c => c.description == "Horror"))
            {
                var category = new Category();
                category.description = "Horror";
                context.categorie.Add(category);
            } 

            if (!context.categorie.Any(c => c.description == "Mystery"))
            {
                var category = new Category();
                category.description = "Mystery";
                context.categorie.Add(category);
            }  

            if (!context.categorie.Any(c => c.description == "Romance"))
            {
                var category = new Category();
                category.description = "Romance";
                context.categorie.Add(category);
            }  

            if (!context.categorie.Any(c => c.description == "Sci-Fi"))
            {
                var category = new Category();
                category.description = "Sci-Fi";
                context.categorie.Add(category);
            }

            if (!context.categorie.Any(c => c.description == "Thriller"))
            {
                var category = new Category();
                category.description = "Thriller";
                context.categorie.Add(category);
            }  

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }

    }
}
