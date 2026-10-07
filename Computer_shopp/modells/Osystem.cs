using Org.BouncyCastle.Bcpg.OpenPgp;
using System.ComponentModel.DataAnnotations;

namespace Computer_shopp.modells
{
    public class Osystem
    {
        [Key]
        [MaxLength(36)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int version { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
