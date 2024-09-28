using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuliuGO.Models
{
    public class User
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("name")]
        public required string Name { get; set; }
        [Column("rank")]
        public int Rank { get; set; }
        
    }
}