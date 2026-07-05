using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize (Roles = "SuberAdmin")]
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IAttachmentService _attachmentService;

        public MembersController(IMemberService memberService,IAttachmentService attachmentService)
        {
            _memberService = memberService;
            _attachmentService = attachmentService;
        }

        #region Index
        //Index() >list all members 
        //GET baseurl/Members/Index 

        public async Task<IActionResult> Index(CancellationToken c)
        {
            var members = await _memberService.GetAllMemberAsync(c);
            return View(members);
        }

        #endregion

        #region MemberDetails
        //MemberDetails(int id)  >for 1 member
        //GET baseurl/Members/MemberDetails/{id} 
        public async Task<IActionResult> MemberDetails(int id, CancellationToken c)
        {
            //get member by id
            //check if member is null=> return index  with error massage
            //check if member is not null=> return view data
            var member = await _memberService.GetMemberDetailsByIdAsync(id, c);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        #endregion

        #region HealthRecordDetails
        //HealthRecordDetails(int id)   >for HealthRecord Detail for 1 member
        //GET baseurl/Members/HealthRecordDetails/{id} 
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken c)
        {
            //get Health Record by member id
            //check if  Health Record  is null=> return index  with error massage
            //check if  Health Record  is not null=> return view data
            var HealthRecord = await _memberService.GetMemberHealthRecordAsync(id, c);
            if (HealthRecord == null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(HealthRecord);
        }
        #endregion

        #region Createing Member > 2 steps
        //Create() >show empty form 
        //GET baseurl/Members/Create

        [HttpGet]        public IActionResult Create() => View();

        //CreateMember() >subbmit form 
        //post baseurl/Members/Create{Members} 
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken c)
        {
            //before talking with service [imp even if i di clint side validation because frontand can do inspect and change clint side validation and send request with invalid data]
            if (!ModelState.IsValid) return View(nameof(Create), model); //ارجع لنفس الفورم ومعاك البيانات والأخطاء]
            var result = await _memberService.CreateMemberAsync(model, c);
            if (result)
                TempData["successMessage"] = "Member Created Successfully";
            else
                TempData["ErrorMessage"] = "Failed to Create Member";
            return RedirectToAction(nameof(Index));//have 2 div depend on result[create or not]
        }

        #endregion

        #region Editing Member > 2 steps

        //MemberEdit(int id >disply edit form (have data)
        //GET baseurl/Members/MemberEdit/{id}
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken c)
        {
            var member = await _memberService.GetMemberToUpdateAsync(id, c);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        //MemberEdit(int id >subbmit form 
        //post baseurl/Members/MemberEdit{Members} 
        [HttpPost]
        //id used to get data to be tracked and then update[MemberToUpdateViewModel doesn't have id so i don't know where i should update]
        //كدا قدرت احدد يقدر يوصل منين لو وصل من الروت يبقي كان ليه صلاحيه عادي لو لا مجرد ما يكلم الاكشن مش هيوصل للفيو  بتاع الفورم اللي فيها الداتا بتاعه  العضو دا
        public async Task<IActionResult> EditMember([FromRoute] int id, MemberToUpdateViewModel model, CancellationToken c)
        {
            if (!ModelState.IsValid) return View(model);//هيدور علي فيو بنفس اسم الاكشن دا
            var result = await _memberService.UpdateMemberDetailsAsync(id, model, c);
            if (result)

                TempData["successMessage"] = "Member Updated Successfully";
            else
                TempData["ErrorMessage"] = "Failed to Updated Member";

            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region DeletingMember > 2 steps
        //Delete(int id) >show form 
        //GET baseurl/Members/Delete/{id} 
        [HttpGet]
        public async Task<IActionResult> Delete(int id,CancellationToken c)
        { 
            var member =await _memberService.GetMemberDetailsByIdAsync(id, c);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Is Not Found";
                return RedirectToAction(nameof(Index));//indexلو ملقاش 
            }
            return View();//go to DeleteConfirmed
        }
        //DeleteConfirmed(int id) >subbmit form 
        //post baseurl/Members/DeleteConfirmed/{id} 
        [HttpPost]                                      //route come from request not form or enything alse
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int id, CancellationToken c)
        {
          var result=await  _memberService.RemoveMemberAsync(id, c);
            if (result) //ف كل الحالات الجزء التاتي لازم يرجع رساله سواء تمام او فشل

                TempData["successMessage"] = "Member Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Failed to Delete Member";

            return RedirectToAction(nameof(Index)); //indexكدا كدا هرجع لل
        }
        #endregion



        //get member photo
        [HttpGet]
        public async Task<IActionResult> Picture(int id)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id);

            if (member is null || string.IsNullOrWhiteSpace(member.Photo))return NotFound();

            var result = _attachmentService.GetFile(member.Photo, "MembersPhoto");

            if (result is null)return NotFound();

            return File(result.Value.stream, result.Value.contentType);
        }
    }
}