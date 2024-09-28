using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuliuGO.Models
{
    public class Room
    {
        [Key]
        [Column("id")]
        public long UserTwoId { get; set; }
        [Column("chess", TypeName = "jsonb")]
        public string Chess { get; set; } = string.Empty;
        [Column("turn")]
        public bool Turn { get; set; }
        [Column("winner")]
        public int Winner { get; set; }
        [Column("info", TypeName = "jsonb")]
        public string Info { get; set; } = string.Empty;
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
        [Column("update_time")]
        public DateTime UpdateTime { get; set; }        

    }
}