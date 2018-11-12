namespace PandaWebApp.Services.Contracts
{
    using System.Collections.Generic;
    using PandaWebApp.Models;
    using PandaWebApp.ViewModels.Users;

    public interface IUserService
    {
        ApplicationUser GetUserByUsername(string username);

        ApplicationUser GetUserById(string id);

        IList<BaseUserViewModel> GetAllUsers();
    }
}
