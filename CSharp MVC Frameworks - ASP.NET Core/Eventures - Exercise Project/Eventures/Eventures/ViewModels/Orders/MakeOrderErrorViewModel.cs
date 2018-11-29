namespace Eventures.ViewModels.Orders
{
    public class MakeOrderErrorViewModel
    {
        public string EventName { get; set; }

        public int AllAvailableTicketsCount { get; set; }

        public int TryToBuyTicketsCount { get; set; }
    }
}
