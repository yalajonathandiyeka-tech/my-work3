# Event Ticketing System (ETS) – Developer Documentation

## Overview
The Event Ticketing System is a C# Windows Forms application designed for a local entertainment company. It supports two user roles — Administrator and Member — with full event management, ticket booking, and transaction history functionality.

---

## Project Structure

```
ETS/
├── Program.cs                  Entry point
├── AppData.cs                  Shared in-memory data store
├── ETS.csproj                  Project file (.NET 6 Windows)
│
├── Interfaces/
│   └── IBookable.cs            Interface: BookTickets, HasAvailableTickets
│
├── Models/
│   ├── Person.cs               Base class: Name, Email
│   ├── Member.cs               Derived class: inherits Person, adds Balance
│   ├── Event.cs                Implements IBookable
│   └── Transaction.cs          Stores booking records
│
└── Forms/
    ├── LoginForm.cs            Role-based login (Admin / Member)
    ├── AdminForm.cs            Event & User management (tabs)
    ├── MemberForm.cs           Event browsing & ticket booking
    └── TransactionsForm.cs     Personal transaction history
```

---

## OOP Concepts Demonstrated

| Concept           | Where Used                                                  |
|-------------------|-------------------------------------------------------------|
| Classes           | Person, Member, Event, Transaction                          |
| Encapsulation     | Properties with getters/setters; Deduct() method on Member  |
| Inheritance       | Member inherits from Person                                 |
| Interfaces        | IBookable implemented by Event                              |
| Lists             | List<Member>, List<Event>, List<Transaction> in AppData     |
| Exception Handling| try-catch in all booking and CRUD operations                |
| GUI Controls      | TextBox, ComboBox, DataGridView, NumericUpDown, Button      |

---

## Class Descriptions

### Person (Base Class)
```csharp
public class Person
{
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### Member : Person
```csharp
public class Member : Person
{
    public decimal Balance { get; set; }
    public bool Deduct(decimal amount)  // returns false if insufficient
}
```

### IBookable Interface
```csharp
public interface IBookable
{
    bool BookTickets(int quantity);
    bool HasAvailableTickets(int quantity);
}
```

### Event : IBookable
```csharp
public class Event : IBookable
{
    public string EventName { get; set; }
    public decimal Price { get; set; }
    public int AvailableTickets { get; set; }
    public bool BookTickets(int quantity)        // deducts from AvailableTickets
    public bool HasAvailableTickets(int quantity)
}
```

### Transaction
```csharp
public class Transaction
{
    public string EventName { get; set; }
    public decimal TotalAmount { get; set; }
    public string MemberEmail { get; set; }
    public int TicketsPurchased { get; set; }
    public DateTime Date { get; set; }
}
```

---

## Forms

### LoginForm
- RadioButton selection: Admin or Member
- ComboBox populated from AppData.Members
- Routes to AdminForm or MemberForm based on role

### AdminForm
- Two-tab layout: Event Management | User Management
- **Event tab**: DataGridView shows all events; inline form to Add / Edit / Delete
- **User tab**: DataGridView shows all members; inline form to Add new member

### MemberForm
- Account info panel (name, email, live balance)
- ComboBox lists all events from AppData
- NumericUpDown for ticket quantity
- Live cost preview (turns red if over budget)
- Confirm booking dialog with full validation

### TransactionsForm
- Filtered view of transactions for the logged-in member
- Shows event, tickets, total paid, date/time
- Summary row: total transaction count + total amount spent

---

## Exception Handling

Every user action is wrapped in a try-catch block. Specific scenarios handled:

| Scenario                         | Response                                   |
|----------------------------------|--------------------------------------------|
| Empty event/user name field      | MessageBox warning, operation aborted      |
| Booking more tickets than stock  | Warning: shows available count             |
| Insufficient member balance      | Warning: shows required vs available       |
| No event selected in ComboBox    | Warning: prompts user to select            |
| No row selected for edit/delete  | Warning: prompts user to select a row      |

---

## How to Run

### Requirements
- .NET 6.0 SDK (Windows)
- Visual Studio 2022 or VS Code with C# extension

### Steps
1. Open a terminal in the `ETS/` folder
2. Run: `dotnet run`
   Or open `ETS.csproj` in Visual Studio and press F5

### Seed Data
The system pre-loads sample data on startup (via AppData.cs):
- **Members**: Alice Dlamini (R1,500), Bob Nkosi (R800)
- **Events**: Jazz Night (R150/50 tickets), Comedy Fest (R200/30), Tech Conference (R350/100)

---

## Navigation Flow

```
LoginForm
├── [Admin] ──► AdminForm
│                ├── Events Tab (Add / Edit / Delete)
│                └── Users Tab (Add / View)
│
└── [Member] ──► MemberForm
                  ├── Book Tickets
                  └── [My Transactions] ──► TransactionsForm
```

---

## Design Notes
- All data is stored **in-memory** (no database required for this prototype)
- Forms share state via the static `AppData` class
- The Member's balance and Event ticket counts are updated immediately on booking
- Transactions are filtered per member by email address
