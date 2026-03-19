# Resellhub - Second-Hand Market (Console Application)

## Description

This project is a console-based application that simulates a simplified online second-hand marketplace, similar to platforms like Finn.no or Facebook Marketplace.
Users can register accounts, create listings, browse and search items, purchase items, and leave reviews for sellers. The application focuses on Object-Oriented Programming principles and modern C# features.

## Features

### User Accounts

* Register and login with username and password
* View personal profile
* See listings, purchases, and reviews

### Listings

* Create, edit, and delete item listings 
* Listings include title, description, category, condition, and price
* Listings can be marked as Available or Sold

### Browse & Search

* View all available listings
* Filter by category
* Search by keywords (title or description)

### Purchasing

* Buy available items (not your own)
* Transactions are stored for both buyer and seller

### Reviews

* Leave a rating (1–6) after a purchase
* Optional comment
* Each user has an average rating

### Transaction History

* View items bought and sold
* Each transaction includes item, price, date, and user

### How to Run

* Open the project in Rider
* Build the solution
  dotnet build
* Run the application
  dotnet run