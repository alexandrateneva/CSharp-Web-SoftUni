namespace Eventures.Services
{
    using Eventures.ViewModels.Admin;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Eventures.Models;

    public interface IAdminService
    {
        IEnumerable<BaseUserViewModel> GetAllUsers();

        ApplicationUser GetUserById(string userId);

        Task<ApplicationUser> ChangeUserRole(string userId, string newRoleName, string roleToRemove);
    }
}
