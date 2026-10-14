using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Computer_shopp.modells
{
    public class Computer
    {
        [Key]
        [MaxLength(36)]
        public Guid ID { get; set; }
        [Column(TypeName = "varchar(30)")]   
        public string? Brand { get; set; }
        [Column(TypeName = "varchar(40)")]
        public string? Type { get; set; }
        [Column(TypeName = "Float")]
        public double Display { get; set; }
        public int memory { get; set; }
        public DateTime CreatedTime { get; set; }
        public Osystem? Osys { get; set; }
    }
}
