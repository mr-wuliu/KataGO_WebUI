using Microsoft.EntityFrameworkCore;
using WuliuGO.Models;

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
        }    }

    public async Task<Analysis?> GetKatagoQueryByQueryIdAsync(string queryId)
    {
        return await _context.AnalysisQuery.FirstOrDefaultAsync(q => q.QueryId == queryId);
    }

    public async Task UpdateKatagoQueryAsync(Analysis katagoQuery)
    {
        _context.AnalysisQuery.Update(katagoQuery);
        await _context.SaveChangesAsync();    }
}