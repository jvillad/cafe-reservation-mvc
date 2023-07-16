using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeanScene.Data
{
    public class BeanSceneApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; } = null!;
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; } = null!;

        public string FullName { get { return FirstName + " " + LastName; } }

        [NotMapped]
        public UserManager<BeanSceneApplicationUser>? UserManager = null;

        [NotMapped]
        public bool IsUser { get => IsInRole("User").Result; }
        public bool IsStaff { get => IsInRole("Staff").Result; }
        public bool IsManager { get => IsInRole("Manager").Result; }
        public async Task<bool> IsInRole(string roleName)
        {
            if (this.UserManager == null) return false;
            return await this.UserManager.IsInRoleAsync(this, roleName);
        }
    }

}
