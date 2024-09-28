
using KatagoDtos;
namespace WuliuGO.Services
{
    public interface IKatagoServer
    {
        bool GetStatus();
        Task<string> AnaylyzeBoardAsync(QueryDto dto);
        Task<KatagoQueryRest?> GetQueryByQueryId(string queryId);
        Task<string> GetKatagoInfoAsync();
    }
}