namespace Eventures.ViewModels.Events
{
    using X.PagedList;

    public class AllEventsViewModel
    {
        public virtual IPagedList<BaseEventViewModel> Events { get; set; }

        public int CurrentPage { get; set; } 
    }
}
