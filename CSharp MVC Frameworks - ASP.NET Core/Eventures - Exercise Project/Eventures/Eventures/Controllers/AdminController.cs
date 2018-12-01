namespace Eventures.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Eventures.Services;
    using X.PagedList;
    using Eventures.ViewModels.Admin;

    public class AdminController : Controller
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Users(int? page)
        {
            var pageNumber = page ?? 1;
            var users = this.adminService.GetAllUsers();
            var onePageOfUsers = users.ToPagedList(pageNumber, 3);
            var model = new AllUsersViewModel()
            {
                Users = onePageOfUsers,
                CurrentPage = pageNumber
            };
            return this.View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Promote(string id)
        {
            await this.adminService.ChangeUserRole(id, "Admin", "User");
            return this.Redirect("/admin/users");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Demote(string id)
        {
            await this.adminService.ChangeUserRole(id, "User", "Admin");
            return this.Redirect("/admin/users");
        }
    }
}