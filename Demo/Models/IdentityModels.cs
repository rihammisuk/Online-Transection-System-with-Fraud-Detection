using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        [Display(Name ="Full Name")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Display(Name = "Account ID")]
        public string AccountID { get; set; }

        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        
        public bool IsFraud { get; set; }
        public string Acc_Create_Date { get; set; }

        
        public decimal Balance { get; set; }

        [Display(Name = "Phone No")]
        public int Phone_No { get; set; }

       
        public string Credit_Rating { get; set; }
     




        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            

        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }



    //Role Management
    public class ApplicationRole : IdentityRole
    {

        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }


    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<RoleViewModel> RoleViewModels { get; set; }

        public DbSet<UsersAccount> UsersAccounts { get; set; }

        public DbSet<MoneyRequest> MoneyRequests { get; set; }
        public DbSet<RiskIP> RiskIPs { get; set; }


        //public System.Data.Entity.DbSet<Demo.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public DbSet<RegisterViewModel> ApplicationUsers { get; set; }
    }
}