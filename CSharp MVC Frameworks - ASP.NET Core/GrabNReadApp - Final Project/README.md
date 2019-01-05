# GRAB N READ APP

The goal of the project is to represent an online library and bookstore. The user also has the opportunity to add comments and to rate books. The app also has blog section. The target framework of the web project is ASP .NET Core MVC (version 2.2).

---

## Basic Additional NuGet Dependencies

- AutoMapper.Extensions.Microsoft.DependencyInjection Version="6.0.0",
- Cloudinary.Core Version="1.31.0",
- X.PagedList.Mvc.Core Version="7.6.0", 
- Microsoft.EntityFrameworkCore.SqlServer Version="2.2.0",
- XUnit Version="2.4.1",
- Microsoft.EntityFrameworkCore.InMemory Version="2.2.0"

## Start - development server

- by .NET Core CLI - listening on https://localhost:5001 and on http://localhost:5000

	$ GrabNReadApp.Web > dotnet run
	
- by Visual Studio - navigate to https://localhost:44393/
	
---

The application has:

- public part (accessible without authentication)
- private part (available for registered users) and
- administrative part (available for administrators only)

### Public part
The public part has a homе page including a slider and a books catalog with a search box, where the user can choose a book and see all the details about it like properties, comments and ratings. Every non-logged in user can also access the genre list, from there he can go to all the books of the desired genre. Of course, the public part includes login and register functionality.
 
### Private part
The private part includes buying and renting books, through a shopping cart and sending an order. This part also has adding and deleting of comments and rating of all of books functionality. The logged-in user also can add and delete their own articles in the blog section of the site. As GDPR requires, each user can view and modify the data stored on the site about her/him, as well as delete their registration.

### Аdministrative part
The administrative part inludes all CRUD operations for books and genres. Also the administrator can see all orders and delete them. Оnly she/he can change the status of an article from rejected to approved and thus make it visible to all users. The administrator also has the right to delete an article and a comment of another user, if she/he thinks the content is offensive or obscene.

---

## Structure

The app solution contains 5 projects:

1. GrabNReadApp.Data - DbContext, DbRepository and Migrations;                                    
2. GrabNReadApp.Data.Models - all Data Models;                                           
3. GrabNReadApp.Data.Services - all Data Services;                         
4. GrabNReadApp.Data.Tests - Unit Tests for all data services;                                    
5. GrabNReadApp.Web - five Areas were used for the structure of GrabNReadApp.Web project. Each of them contains subfolders that contain Controller, ViewModels and Views for an independent entity:
	- Blog - Article
	- Evaluation - Comments, Rating
	- Identity - Account (default use Razor Pages)
	- Products - Genre, Book
	- Store - Purchase, Rental, Order
    Also there are separate folders for AutoMapper, Constants, Helpers and Extensions, Middlewares and Scripts.
	
---

## Author

Alexandra Teneva

---

## License

This project is licensed under the MIT License

