using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuliuGO.Models
{
    public class KatagoQuery
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("query_id")]
        public string? QueryId { get; set; }
        [Column("is_during_search")]
        public bool IsDuringSearch { get; set; }
        [Column("move_infos", TypeName = "jsonb")]
        public string? MoveInfos { get; set; }
        [Column("root_info", TypeName = "jsonb")]
        public string? RootInfo { get; set; }
        [Column("turn_number")]
        public int TurnNumber { get; set; }
    }
}