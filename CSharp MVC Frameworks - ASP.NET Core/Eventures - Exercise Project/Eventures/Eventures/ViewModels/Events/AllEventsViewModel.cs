namespace Eventures.ViewModels.Events
{
    using System.Collections.Generic;

    public class AllEventsViewModel
    {
        public AllEventsViewModel()
        {
            this.Events= new HashSet<BaseEventViewModel>();
        }

        public ICollection<BaseEventViewModel> Events { get; set; }
    }
}
