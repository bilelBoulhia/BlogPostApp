using ArtcilesServer.Data;
using Microsoft.EntityFrameworkCore;

namespace ArtcilesServer.Repo
{
    public class ReportRepo
    {
        private readonly DbConn _context;

        public ReportRepo(DbConn dbContext)
        {
            _context = dbContext;
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
