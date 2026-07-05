using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class Member:GymUser
    {
        public string? Photo {  get; set; }
        // created at = joined at
        #region Relationships
        public HealthRecord HealthRecord { get; set; } = default!;
     public ICollection <MemberShip> MemberShips { get; set; } = default!;
        public ICollection<Booking> MemberSessions { get; set; } = default!;



        #endregion
    }
}
