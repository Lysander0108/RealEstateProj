using System.ComponentModel.DataAnnotations;

namespace RealEstateProj.Data
{

    public class User
    {
        public int ID { get; set; }


        [Required, StringLength(25)] public string UserName { get; set; } = string.Empty;
        [Required , EmailAddress] public string Email { get; set; } = string.Empty ;
        [Required] public string PasswordHashed { get; set; } = string.Empty;
        public Role Role { get; set; }
    }

    public enum Role
    {+
        AGENT,
        USER
    }
}