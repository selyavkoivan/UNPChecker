using System.ComponentModel.DataAnnotations;

namespace UNPChecker.Models
{
    public class UNP
    {
        [Key]
        public int id { get; private set; }
        [Required]
        public uint UNPCode { get; set; }
        [Required]
        [MaxLength (50)]
        public string Email { get; set; }

        public UNP(uint unpCode, string email)
        {
            UNPCode = unpCode;
            Email = email;
        }

        public UNP()
        {
        }
    }
}