using WuliuGO.Models;

public interface IKatagoRepository
{
    Task<Analysis?> GetKatagoQueryByQueryIdAsync(string queryId);
    Task<List<double>?> GetKatagoPolicyByQueryIdAsync(string queryId);
    Task AddKatagoQueryAsync(Analysis KatagouQuery);
    Task UpdateKatagoQueryAsync(Analysis KatagoQuery);
    Task DeleteKatagoQueryByIdAsync(long id); // 按ID, 而不是queryId
}