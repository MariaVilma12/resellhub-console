#  ResellHub Console Application

##  Project Description

ResellHub is a console-based second-hand marketplace application developed using C# and SQLite.  
The application allows users to register, log in, create listings, browse items, purchase products, and leave reviews.

It simulates a real-world marketplace where users can act as both buyers and sellers, managing their listings and tracking transactions.

---

##  Features

### User Management
- Register new users
- Login with username and password

### Listings
- Create listings
- Browse available items
- Search listings
- View listing details

### Transactions
- Buy items from other users
- Prevent buying own items
- Automatically mark items as sold
- Track:
  - Items bought
  - Items sold

### Reviews
- Leave reviews after purchase
- Rating system (1–6)
- Optional comments
- View reviews received

### User Dashboard
- My Listings
- My Sales
- My Purchases
- My Reviews

---

## Design Decisions (OOP Concepts)

* Encapsulation : Each model (User, Listing, Transaction, Review) encapsulates its properties and behavior.
 Sensitive data like passwords is managed inside the class.

* Separation of Concerns : This improves maintainability and readability.

* Abstraction : Database operations are handled inside service classes, hiding SQL details from the main program.

* Reusability : Reusable components like InputHelper are used for input validation and reduce duplication.

* Single Responsibility Principle (SRP) : Each class has one responsibility.


---


##  Technologies Used

- C# (.NET Console Application)
- SQLite (Microsoft.Data.Sqlite)
- Object-Oriented Programming (OOP)

---

##  How to Build and Run

###  Clone the Repository
```bash
git clone <your-repo-url>
cd resellhub-console
dotnet build
dotnet run
```

---

## AI tools were used during development for

- Debugging C# errors
- Fixing namespace and service issues
- Designing OOP architecture
- Writing SQL queries for SQLite
- Improving code structure and best practices
