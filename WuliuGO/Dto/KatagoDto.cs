using Newtonsoft.Json;

namespace KatagoDtos
{
    public class QueryTestDto
    {
        public int id;
    }

    public class QueryDto
    {
        // the different between string[] and List<string> : https://stackoverflow.com/questions/4724816/where-to-use-string-vs-list-string-in-c-sharp
        public required List<List<string>> moves;

    }
    public class KatagoOutput
    {
        // 使用Newtonsoft设置序列化别名
        [JsonProperty("id")]
        public required string Id { get; set; }
        [JsonProperty("isDuringSearch")]
        public bool IsDuringSearch { get; set; }
        [JsonProperty("moveInfos")]
        public required List<object> MoveInfos { get; set; }
        [JsonProperty("rootInfo")]
        public object? RootInfo { get; set; }
        [JsonProperty("turnNumber")]
        public int TurnNumber { get; set; }
    }
    public class KatagoQueryRest
    {
        public string? Id { get; set; }
        public List<MoveInfo>? Moves { get; set; }
        public RootInfo? RootInfo { get; set; }

    }
    public class MoveInfo
    {
        [JsonProperty("edgeVisits")]
        public int EdgeVisits { get; set; }
        [JsonProperty("edgeWeight")]
        public double EdgeWeight { get; set; }
        [JsonProperty("lcb")]
        public double Lcb { get; set; }
        [JsonProperty("move")]
        public string Move { get; set; } = string.Empty; // 默认值避免 null
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("prior")]
        public double Prior { get; set; }
        [JsonProperty("pv")]
        public List<string> Pv { get; set; } = [];
        [JsonProperty("scoreLead")]
        public double ScoreLead { get; set; }
        [JsonProperty("scoreMean")]
        public double ScoreMean { get; set; }
        [JsonProperty("scoreSelfPlay")]
        public double ScoreSelfplay { get; set; }
        public double ScoreStdev { get; set; }
        [JsonProperty("utility")]
        public double Utility { get; set; }
        [JsonProperty("utilityLcb")]
        public double UtilityLcb { get; set; }
        [JsonProperty("visits")]
        public int Visits { get; set; }
        [JsonProperty("weight")]
        public double Weight { get; set; }
        [JsonProperty("winrate")]
        public double Winrate { get; set; }
        [JsonProperty("isSymmetryOf")]
        public string IsSymmetryOf { get; set; } = string.Empty;
    }
    public class RootInfo
    {
        [JsonProperty("currentPlayer")]
        public string CurrentPlayer { get; set; } = string.Empty;
        [JsonProperty("rawLead")]
        public double RawLead { get; set; }
        [JsonProperty("rawNoResultProb")]
        public double RawNoResultProb { get; set; }
        [JsonProperty("rawScoreSelfplay")]
        public double RawScoreSelfplay { get; set; }
        [JsonProperty("rawScoreSelfplayStdev")]
        public double RawScoreSelfplayStdev { get; set; }
        [JsonProperty("rawStScoreError")]
        public double RawStScoreError { get; set; }
        [JsonProperty("rawStWrError")]
        public double RawStWrError { get; set; }
        [JsonProperty("rawVarTimeLeft")]
        public double RawVarTimeLeft { get; set; }
        [JsonProperty("rawWinrate")]
        public double RawWinrate { get; set; }
        [JsonProperty("scoreLead")]
        public double ScoreLead { get; set; }
        [JsonProperty("scoreSelfplay")]
        public double ScoreSelfplay { get; set; }
        [JsonProperty("scoreStdev")]
        public double ScoreStdev { get; set; }
        [JsonProperty("symHash")]
        public string SymHash { get; set; } = string.Empty;
        [JsonProperty("thisHash")]
        public string ThisHash { get; set; } = string.Empty;
        [JsonProperty("utility")]
        public double Utility { get; set; }
        [JsonProperty("visits")]
        public int Visits { get; set; }
        [JsonProperty("weight")]
        public double Weight { get; set; }
        [JsonProperty("winrate")]
        public double Winrate { get; set; }
    }
    public class KatagoInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty("action")]
        public string Action { get; set; } = string.Empty;
        [JsonProperty("models")]
        public List<KatagoModel>? models;

    }

    public class KatagoModel
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("internalName")]
        public string InternalName { get; set; } = string.Empty;
        [JsonProperty("maxBatchSize")]
        public int MaxBatchSize { get; set; }
        [JsonProperty("usesHumanSLProfile")]
        public bool UsesHumanSLProfile { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; } = string.Empty;
        [JsonProperty("usingFP16")]
        public bool UsingFP16 { get; set; }
    }
}