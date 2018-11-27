using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.ViewModels.Events
{
    public class AllMyEventsViewModel
    {
        public AllMyEventsViewModel()
        {
            this.Events = new HashSet<MyEventViewModel>();
        }

        public ICollection<MyEventViewModel> Events { get; set; }
    }
}
