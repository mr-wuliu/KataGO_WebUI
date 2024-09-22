using WuliuGO.Models;

public interface IKatagoRepository
{
    Task<KatagoQuery?> GetKatagoQueryByQueryIdAsync(string queryId);
    Task AddKatagoQueryAsync(KatagoQuery katagoQuery);
    Task UpdateKatagoQueryAsync(KatagoQuery katagoQuery);
    Task DeleteKatagoQueryByIdAsync(long id); // 按ID, 而不是queryId

}