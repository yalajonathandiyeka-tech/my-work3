using System;

namespace ETS.Models
{
    public class Transaction
    {
        public string EventName { get; set; }
        public decimal TotalAmount { get; set; }
        public string MemberEmail { get; set; }
        public int TicketsPurchased { get; set; }
        public DateTime Date { get; set; }

        public Transaction(string eventName, decimal totalAmount, string memberEmail, int ticketsPurchased)
        {
            EventName = eventName;
            TotalAmount = totalAmount;
            MemberEmail = memberEmail;
            TicketsPurchased = ticketsPurchased;
            Date = DateTime.Now;
        }
    }
}
