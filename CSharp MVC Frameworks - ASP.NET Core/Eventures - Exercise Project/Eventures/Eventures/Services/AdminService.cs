namespace Eventures.Services
{
    using System.Linq;
    using Eventures.Data;
    using Eventures.ViewModels.Admin;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using Eventures.Models;

    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;


        public AdminService(
            ApplicationDbContext context, IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IEnumerable<BaseUserViewModel> GetAllUsers()
        {
            var usersRoles = this.context.UserRoles.ToList();
            var users = this.userManager.Users
                .Select(e => this.mapper.Map<BaseUserViewModel>(e))
                .ToList();

            foreach (var user in users)
            {
                var role = "User";
                var userRoleId = usersRoles.FirstOrDefault(ur => ur.UserId == user.Id)?.RoleId;
                if (userRoleId != null)
                {
                    role = this.roleManager.FindByIdAsync(userRoleId).GetAwaiter().GetResult().Name;
                }

                user.Role = role;
            }

            return users;
        }

        public ApplicationUser GetUserById(string userId)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Id == userId);
            return user;
        }

        public async Task<ApplicationUser> ChangeUserRole(string userId, string newRoleName, string roleToRemove)
        {
            var user = this.GetUserById(userId);
            var result = await this.userManager.RemoveFromRoleAsync(user, roleToRemove);
            result = await this.userManager.AddToRoleAsync(user, newRoleName);
            if (result.Errors.Any())
            {
                var errors = result.Errors;
            }
            return user;
        }
    }
}
