using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class HealthRecord:BaseEntity
    {
         public decimal Height { get; set; }
public decimal Weight { get; set; }
public string? Note { get; set; }
        public string BLoodType { get; set; }
        //  LastUpdated = UpdatedAt of BaseEntity

        #region Relationships
        public Member   Member { get; set; } = default!;
        public int MemberId { get; set; }   
        #endregion



    }
}
