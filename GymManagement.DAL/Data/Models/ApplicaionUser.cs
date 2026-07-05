using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class ApplicaionUser:IdentityUser //built inعشان عايز معلومات اضافيه عن المستخد مش موجوده في ال     
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
