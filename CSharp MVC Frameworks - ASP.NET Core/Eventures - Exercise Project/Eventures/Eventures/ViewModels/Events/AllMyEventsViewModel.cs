namespace Eventures.ViewModels.Events
{
    using X.PagedList;

    public class AllMyEventsViewModel
    {
        public virtual IPagedList<MyEventViewModel> Events { get; set; }

        public int CurrentPage { get; set; } 
    }
}
