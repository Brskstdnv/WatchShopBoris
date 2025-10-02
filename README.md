Description

WatchShopBoris is a project for an online watch store.
The goal is to provide a platform where customers can browse, search, and purchase watches, while administrators can manage the catalog, stock, and orders.

Features

Some of the key features (you can update depending on the actual implementation):

User registration and authentication

Product catalog (list of watches)

Detailed product view (description, images, price, availability)

Search and filtering of products

Ordering and checkout process (or payment stage)

Order management

Admin panel for adding/editing/deleting products

User and role management

Database integration for storing products, users, and orders

Technologies & Architecture

The project is organized into several layers/modules (based on the current structure):

WatchShop.Infrastructure — infrastructure layer responsible for database access, configurations, etc.

WatchShopApp.Core — core business logic, interfaces, and models.

WatchShopApp — presentation layer (UI) and main application.

WatchShopApp.sln — Visual Studio solution file bringing everything together.

Technologies may include ASP.NET Core (for Web API / MVC), Entity Framework Core (for database access), and a frontend framework (Razor Pages, Blazor, or MVC Views), depending on the chosen implementation.

Installation / Setup

Clone the repository:

git clone https://github.com/Brskstdnv/WatchShopBoris.git


Open the solution in Visual Studio or VS Code.

Configure your connection string in appsettings.json (or relevant config file).

Apply database migrations (if using EF Core):

dotnet ef database update


Run the application.

Usage

Log in as administrator to add new watches, manage stock, and handle orders.

Log in as customer to browse the catalog, add products to the cart, and place orders.

Optionally, search and filter by brand, price, style, and other attributes.

Project Structure
WatchShopBoris/
│
├── WatchShop.Infrastructure/     # Database, repositories, configs
├── WatchShopApp.Core/           # Business logic, models, abstractions
├── WatchShopApp/                # Main app (UI, controllers, services)
├── WatchShopApp.sln             # Visual Studio solution
└── README.md                    # This file
