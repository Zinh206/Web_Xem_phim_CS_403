using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_Xem_phim_CS_403.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RoleName { get; set; } // Tên quyền (Admin, User, Moderator, etc.)

        // Liên kết với User
        public ICollection<Account> Accounts { get; set; }
    }
}
