using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CineChat.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CineChat.DAL
{
        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext()
                : base("CineChatContext", throwIfV1Schema: false)
            {
            }


            public DbSet<Post> post { get; set; }
            public DbSet<Chat> chat { get; set; }
            public DbSet<Rates> rate { get; set; }
            public DbSet<Movie> movie { get; set; }
            public DbSet<Category> categorie { get; set; }
            public DbSet<Characters> character { get; set; }
            public DbSet<Person> person { get; set; }
            public DbSet<Writer> writer { get; set; }
            public DbSet<Director> director { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}