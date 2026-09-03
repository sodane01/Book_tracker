# 📚 Book Tracker

> A cozy full-stack web application for discovering books, tracking your reading journey and building your personal library.

**Book Tracker** is my final full-stack development project, built with **ASP.NET Core MVC, C#, Entity Framework Core and SQL Server**.

<img width="1370" height="730" alt="image" src="https://github.com/user-attachments/assets/f9445267-22ea-418b-b421-f5aa5163bae7" />


The project combines software development with my background in **Software Testing & QA**, with a strong focus on planning, testability, security, user-specific data and a stable MVP.

---

## ✨ Features

- 🔎 Search and discover books using Google Books API
- 📖 View book details, metadata and covers
- 📚 Organize books into:
  - Want to Read
  - Currently Reading
  - Read
- 📈 Track reading progress
- ♥ Mark books as favourites
- ★ Rate books
- 💬 Write, edit and delete your own reviews
- 📊 View personal reading statistics
- ☀️ Cozy Bookshop light theme
- 🌙 Midnight Library dark theme
- 🔐 Authentication and user-specific data
- 📱 Responsive design


<img width="1009" height="739" alt="image" src="https://github.com/user-attachments/assets/7f086b63-b53e-4c84-931b-23aa73f15974" />


---

## 🛠️ Tech Stack

### Backend

- C#
- ASP.NET Core MVC
- Entity Framework Core
- ASP.NET Core Identity

### Frontend

- Razor Views
- HTML
- CSS
- Bootstrap

### Database

- SQL Server

### External API

- Google Books API

---
<img width="997" height="750" alt="image" src="https://github.com/user-attachments/assets/7c5df681-1ee0-4a04-be81-d07cdd3dd6b6" />

## 🏗️ Architecture

Book Tracker follows an **ASP.NET Core MVC** structure with separation of responsibilities between:

**Razor Views → Controllers → Services → Database / External API**

ViewModels are used to provide the UI with the data required for each view.

Some of the main services include:

- `GoogleBooksService`
- `UserBookService`
- `ReviewService`
- `ProfileService`

The core data model consists of:

- `ApplicationUser`
- `Book`
- `UserBook`
- `Review`
- `Notification`

`UserBook` represents the relationship between a user and a book and stores personal reading data such as reading status, reading progress, favourite state and rating.

> **Book describes what the book is. UserBook describes what the book means to that user.**

---

## 🔐 Authentication & Security

Authentication is handled with **ASP.NET Core Identity**.

Protected functionality is enforced in the backend rather than relying only on hidden UI elements.

The application includes:

- User registration and login
- User-specific shelves and favourites
- Review ownership validation
- Backend authorization
- `ActiveUser` authorization policy
- Protection against cross-user modification

A blocked user is denied access to protected functionality even if an existing authentication cookie is still valid.

---

## 🧪 Testing & QA

My background in **Software Testing & QA** has influenced the development process throughout the project.

The general workflow has been:

**Build → Test → Break → Fix → Regression**

Testing includes areas such as:

- Functional testing
- Authentication and authorization
- Ownership and security scenarios
- Validation and error handling
- External API behaviour
- Responsive testing
- Accessibility checks
- Regression testing

Requirements and tests have also been tracked using a **Requirements Traceability Matrix (RTM)**.

---

## 📋 Planning & Documentation

The project was planned around a clearly defined and intentionally scoped MVP.

The development process included:

- Epics
- User stories
- Acceptance criteria
- Jira planning
- Database design
- API planning
- Test cases
- Requirements Traceability Matrix
- System specification

Project documentation can be found in the [`docs`](./docs) folder.

---

## 🚀 Running the Project

### Requirements

To run Book Tracker locally you will need:

- .NET SDK
- SQL Server / SQL Server LocalDB
- Visual Studio or another compatible IDE
- Entity Framework Core tools

### Clone the repository

```bash
git clone https://github.com/sodane01/Book_tracker.git
```

Navigate to the application:

```bash
cd Book_tracker/Book_tracker
```

Restore dependencies:

```bash
dotnet restore
```

Apply the database migrations:

```bash
dotnet ef database update
```

Run the application:

```bash
dotnet run
```

> Sensitive development settings are handled through configuration/User Secrets and are not stored in the repository.

---

## 🔮 Future Development

The MVP is intentionally scoped, but the project leaves room for future development such as:

- 📚 Custom book series and reading order
- ✨ Improved series metadata
- 🔔 Extended notification system
- ⚙️ Notification preferences
- 🗂️ Notification management and history
- 🛡️ Admin dashboard
- 📊 Advanced reading statistics
- ☁️ Deployment as a public web application

---

## 💡 What I Learned

Book Tracker gave me practical experience connecting several parts of full-stack development:

- ASP.NET Core MVC architecture
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- External API integration
- Authentication and authorization
- User-data ownership and security
- Responsive frontend development
- Structured testing and regression
- Project planning and documentation

Most importantly, the project reinforced something I already knew from QA:

> **Working software is more valuable than a pile of half-finished features.**

---

## 👩‍💻 About the Project

**Book Tracker** was created by **Anette Söderström** as a final full-stack development project.

Built with code, books, QA instincts and a probably unreasonable number of regression tests. 📚
