using System.Collections.Generic;
using ETS.Models;

namespace ETS
{
    public static class AppData
    {
        public static List<Member> Members { get; } = new List<Member>
        {
            new Member("Alice Dlamini", "alice@mail.com", 1500m),
            new Member("Bob Nkosi",    "bob@mail.com",   800m)
        };

        public static List<Event> Events { get; } = new List<Event>
        {
            new Event("Jazz Night",       150m, 50),
            new Event("Comedy Fest",      200m, 30),
            new Event("Tech Conference",  350m, 100)
        };

        public static List<Transaction> Transactions { get; } = new List<Transaction>();
    }
}
