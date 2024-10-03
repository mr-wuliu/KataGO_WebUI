using System.ComponentModel.DataAnnotations.Schema;

namespace WuliuGO.Models
{
    [Table("Kifu")]
    public class Kifu : CommonModel
    {
        [Column("player_1")]
        public int Player1Id { get; set; }
        [Column("player_2")]
        public int Player2Id { get; set; }
        [Column("mode")]
        public int Mode { get; set; }
        [Column("kifu_data", TypeName="jsonb")]
        public string? KifuData { get; set; }
        [Column("trun")]
        public bool Trun { get; set; }
        [Column("winner")]
        public int Winner { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("sgf")]
        public string Sgf { get; set; } = string.Empty;
    }
    [Table("Analysis")]
    public class Analysis : CommonModel
    {
        [Column("query_id")]
        public string QueryId { get; set; } = string.Empty;
        [Column("is_running")]
        public bool IsRunning { get; set; }
        [Column("input_move", TypeName="jsonb" )]
        public string? InputMove { get; set; }
        [Column("analysis_info", TypeName="jsonb" )]
        public string? AnalysisInfo { get; set; }
        [Column("output_move", TypeName="jsonb")]
        public string? OutputMove { get; set; }
        [Column("policy", TypeName = "jsonb")]
        public string? Policy { get; set; }
        [Column("turn_number")]
        public int TurnNumber { get; set; }
        [Column("root_info", TypeName="jsonb" )]
        public string? RootInfo { get; set; }
        [Column("hash")]
        public string Hash { get; set; } = string.Empty;
    }
}