using Microsoft.EntityFrameworkCore;
using WuliuGO.Models;
using Newtonsoft.Json;
using Serilog;

public class KatagoRepository : IKatagoRepository
{
    private readonly AppDbContext _context;
    public KatagoRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddKatagoQueryAsync(Analysis katagoQuery)
    {
        await _context.AnalysisQuery.AddAsync(katagoQuery);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteKatagoQueryByIdAsync(long id)
    {
        var query = await _context.AnalysisQuery.FindAsync(id);
        if (query != null)
        {
            _context.AnalysisQuery.Remove(query);
            await _context.SaveChangesAsync();
        }        
    }

    public async Task<Analysis?> GetKatagoQueryByQueryIdAsync(string queryId)
    {
        return await _context.AnalysisQuery.FirstOrDefaultAsync(q => q.QueryId == queryId);
    }
    public async Task<List<double>?> GetKatagoPolicyByQueryIdAsync(string queryId)
    {
        var query = await _context.AnalysisQuery.FirstOrDefaultAsync(q => q.QueryId == queryId);
        if (query != null && query.Policy != null)
        {
            Log.Information($" Policy: {query.Policy}");
            return JsonConvert.DeserializeObject<List<double>?>(query.Policy);
        }
        return null;

    }

    public async Task UpdateKatagoQueryAsync(Analysis katagoQuery)
    {
        _context.AnalysisQuery.Update(katagoQuery);
        await _context.SaveChangesAsync();    }
}