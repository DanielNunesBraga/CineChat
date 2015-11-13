using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CineChat.DAL;

namespace CineChat.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
       /* [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int userID { get; set; }*/
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual ICollection<Movie> likes { get; set; }

        public virtual ICollection<Rates> rates { get; set; }

        public virtual ICollection<Post> posts { get; set; }

        public virtual ICollection<Chat> chat { get; set; }

        public virtual ICollection<ChatUsers> logeduser { get; set; }
    }


}