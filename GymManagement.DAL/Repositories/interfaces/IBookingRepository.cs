using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.interfaces
{
    public interface IBookingRepository : IGenaricReposatory<Booking>
    {
        public Task<List<Booking>> GetBySessionIdAsync(int sessionId, CancellationToken ct = default);

    }
}
