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
        public string KifuData { get; set; } = string.Empty;
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
        [Column("input_move")]
        public string InputMove { get; set; } = string.Empty;
        [Column("analysis_info")]
        public string AnalysisInfo { get; set; } = string.Empty;
        [Column("output_move")]
        public string OutputMove { get; set; } = string.Empty;
        [Column("turn_number")]
        public int TurnNumber { get; set; }
        [Column("root_info")]
        public string RootInfo { get; set; } = string.Empty;
        [Column("hash")]
        public string Hash { get; set; } = string.Empty;
    }
}