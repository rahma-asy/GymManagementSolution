using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MembershipViewModels
{
    public class MemberShipForMemberViewModel
    {
        public string MemberName { get; set; } = default!;
        public string PlanName { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
        public int? RemainingDays
        {
            get
            {
                return (EndDate - DateTime.Now).Days;
            }
        }
    }
}
