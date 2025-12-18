using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;

namespace DataAccessLayer
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]

        public string? Password { get; set; }

        public string Email { get; set; }

        public int RoleId { get; set; }
       
        public Role? Role { get; set; }
    }

}
