using GymManagement.DAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.TrainerViewModels
{
    public class TrainerViewModel
    {
            public int Id { get; set; }
        public string? Photo{ get; set; }
            public string Name { get; set; } = null!;

            public string Email { get; set; } = null!;

            public string Phone { get; set; } = null!;

           public string Spectatty { get; set; }

        //MemberDetails

        public string DateofBirth { get; set; }
        public string Gender { get; set; }  
          public string? Address { get; set; }

    }
}
