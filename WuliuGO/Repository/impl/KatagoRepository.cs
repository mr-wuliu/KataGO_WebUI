using Microsoft.EntityFrameworkCore;
using WuliuGO.Models;

public class KatagoRepository : IKatagoRepository
{
    private readonly AppDbContext _context;
    public KatagoRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddKatagoQueryAsync(KatagoQuery katagoQuery)
    {
        await _context.KatagoQueries.AddAsync(katagoQuery);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteKatagoQueryByIdAsync(long id)
    {
        var query = await _context.KatagoQueries.FindAsync(id);
        if (query != null)
        {
            _context.KatagoQueries.Remove(query);
            await _context.SaveChangesAsync();
        }    }

    public async Task<KatagoQuery?> GetKatagoQueryByQueryIdAsync(string queryId)
    {
        return await _context.KatagoQueries.FirstOrDefaultAsync(q => q.QueryId == queryId);
    }

    public async Task UpdateKatagoQueryAsync(KatagoQuery katagoQuery)
    {
        _context.KatagoQueries.Update(katagoQuery);
        await _context.SaveChangesAsync();    }
}