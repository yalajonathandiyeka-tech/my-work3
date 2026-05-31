using ETS.Interfaces;

namespace ETS.Models
{
    public class Event : IBookable
    {
        public string EventName { get; set; }
        public decimal Price { get; set; }
        public int AvailableTickets { get; set; }

        public Event(string eventName, decimal price, int availableTickets)
        {
            EventName = eventName;
            Price = price;
            AvailableTickets = availableTickets;
        }

        public bool HasAvailableTickets(int quantity)
        {
            return AvailableTickets >= quantity;
        }

        public bool BookTickets(int quantity)
        {
            if (!HasAvailableTickets(quantity)) return false;
            AvailableTickets -= quantity;
            return true;
        }

        public override string ToString() => EventName;
    }
}
