namespace Eventures.ViewModels.Admin
{
    using X.PagedList;

    public class AllUsersViewModel
    {
        public virtual IPagedList<BaseUserViewModel> Users { get; set; }

        public int CurrentPage { get; set; }
    }
}
