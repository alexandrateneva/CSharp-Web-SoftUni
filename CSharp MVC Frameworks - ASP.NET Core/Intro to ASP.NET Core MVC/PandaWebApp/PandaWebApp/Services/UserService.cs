namespace PandaWebApp.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using PandaWebApp.Data;
    using PandaWebApp.Models;
    using PandaWebApp.Services.Contracts;
    using PandaWebApp.ViewModels.Users;

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public ApplicationUser GetUserByUsername(string username)
        {
            var user = this.context.Users.FirstOrDefault(x => x.UserName == username);
            return user;
        }

        public ApplicationUser GetUserById(string id)
        {
            var user = this.context.Users.FirstOrDefault(x => x.Id == id);
            return user;
        }

        public IList<BaseUserViewModel> GetAllUsers()
        {
            var users = this.context.Users
                .Select(u => new BaseUserViewModel()
                {
                    Id = u.Id,
                    Username = u.UserName
                }).ToList();

            return users;
        }
    }
}
