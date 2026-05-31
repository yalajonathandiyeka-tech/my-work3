namespace ETS.Interfaces
{
    public interface IBookable
    {
        bool BookTickets(int quantity);
        bool HasAvailableTickets(int quantity);
    }
}
