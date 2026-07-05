using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberViewModels
{
   
        public class HealthRecordViewModel
        {
            [Range(0.1, 300, ErrorMessage = "Height must be greater than 0")]
            public decimal Height { get; set; }

            [Range(0.1, 500, ErrorMessage = "Weight must be greater than 0")]
            public decimal Weight { get; set; }

            [Required(ErrorMessage = "Blood Type Is Required")]
            [StringLength(3, ErrorMessage = "Blood type must be 3 characters or less")]
            public string BLoodType { get; set; } = default!;
        
            public string? Note { get; set; } = default!;
        }
   
}
