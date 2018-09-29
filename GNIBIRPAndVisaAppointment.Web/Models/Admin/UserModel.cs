using System.ComponentModel.DataAnnotations;
using GNIBIRPAndVisaAppointment.Web.Identity;

namespace GNIBIRPAndVisaAppointment.Web.Models.Admin
{
    public class UserModel
    {
        public UserModel() { }

        public UserModel(ApplicationUser entity)
        {
            Id = entity.Id;
            Name = entity.UserName;
            Role = entity.Role;
        }

        [Required]
        public string Id { get; set; }

        public string OldPassword { get; set; }

        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Role { get; set; }
    }
}