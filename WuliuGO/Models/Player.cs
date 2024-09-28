using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuliuGO.Models
{
    public class Player
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("userId")]
        public long UserId { get; set; }
        [Column("status")]
        public long Status { get; set; }


    }
}