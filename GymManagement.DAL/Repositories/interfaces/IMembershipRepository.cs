using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.interfaces
{
    public interface IMembershipRepository : IGenaricReposatory<MemberShip>
    {
        Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<MemberShip, bool>>? predicate = null, CancellationToken ct = default);
    }
}
