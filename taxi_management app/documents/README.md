# taxi-management-app

## Overview

**Taxi Management App** is designed for taxi dispatchers to manage taxi orders, drivers, and related operations.
This application was inspired by the "Taxi-Master" program.

## Screenshots

### Taxi-Master
![image](https://github.com/enoobis/taxi-manager-app/assets/62465404/d07025e2-6ece-4df6-a588-6a05025fb496)

### Taxi Management App
![image](https://github.com/enoobis/taxi-manager-app/assets/62465404/d45dff5e-f893-46a6-81d0-6d2bbe32d261)
![image](https://github.com/enoobis/taxi-manager-app/assets/62465404/983e14ff-fcda-433b-87f0-c6cf914cb4d3)


## Features

### Orders
- **Add Order**: Opens a window to add a new order.
- **Delete Order**: Deletes the selected order from the list.
- **Update Order**: Opens a window to edit the selected order.
- **Change View**: Opens a window to modify the visibility of columns in the orders table.
- **Highlight Order**: Allows highlighting and styling specific orders.
- **Complete Order**: Marks the selected order as completed.
- **Assign Order**: Opens a window to assign a driver to the selected order.

### Drivers
- **Add Driver**: Opens a window to add a new driver.
- **Delete Driver**: Deletes the selected driver from the list.
- **Update Driver**: Opens a window to edit the selected driver's information.
- **Change View**: Opens a window to modify the visibility of columns in the drivers table.
- **Highlight Driver**: Allows highlighting and styling specific drivers.

### Filtering
- **Orders and Drivers Filtering by Categories**:
  - **All Orders/Drivers**: Displays all orders or drivers.
  - **Standard**: Displays orders or drivers of the "Standard" category.
  - **Comfort**: Displays orders or drivers of the "Comfort" category.
  - **Business**: Displays orders or drivers of the "Business" category.
  - **Organization**: Displays orders or drivers of the "Organization" category.

### Additional Features
- **Chat**: Simple chat interface for internal communication.
- **Map**: Displays a map centered on Kyrgyzstan.
- **Call Log**: View the call log associated with orders.
- **Report**: View statistics and reports on orders.

## Requirements

- **.NET 6.0 SDK or higher**
- **Visual Studio 2022 or higher**
- **SQL Server LocalDB**
- **NuGet Packages**:
  - `GMap.NET.Core`
  - `GMap.NET.WindowsPresentation`

## Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/enoobis/taxi-management-app.git
   cd taxi-management-app
   ```

2. **Open the solution**

   Open `taxi-management-app.sln` in Visual Studio.

3. **Restore NuGet packages**

   In Visual Studio, right-click the solution in Solution Explorer and select `Restore NuGet Packages`.

4. **Update the database connection string**

   Update the connection string to youre L DB 

   ```csharp
   string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Path\To\Your\app_db.mdf;Integrated Security=True";
   ```

5. **Build and run the application**

   Press `F5` to build and run the application.

## Usage

### Main Window

The main window contains tabs for managing orders, drivers, and viewing reports.

#### Orders Tab

- **Add Order**: Opens a window to add a new order. Enter order details and click "OK".
- **Delete Order**: Deletes the selected order. Select an order from the table and click "Delete".
- **Update Order**: Opens a window to edit the selected order. Select an order and click "Update".
- **Change View**: Opens a window to modify the visibility of columns in the orders table.
- **Highlight Order**: Allows highlighting and styling specific orders.
- **Complete Order**: Marks the selected order as completed. Select an order and click "Complete".
- **Assign Order**: Opens a window to assign a driver to the selected order. Select an order and click "Assign".

#### Drivers Tab

- **Add Driver**: Opens a window to add a new driver. Enter driver details and click "OK".
- **Delete Driver**: Deletes the selected driver. Select a driver from the table and click "Delete".
- **Update Driver**: Opens a window to edit the selected driver's information. Select a driver and click "Update".
- **Change View**: Opens a window to modify the visibility of columns in the drivers table.
- **Highlight Driver**: Allows highlighting and styling specific drivers.

#### Filtering Orders and Drivers

- **All Orders/Drivers**: Displays all orders or drivers.
- **Standard**: Displays orders or drivers of the "Standard" category.
- **Comfort**: Displays orders or drivers of the "Comfort" category.
- **Business**: Displays orders or drivers of the "Business" category.
- **Organization**: Displays orders or drivers of the "Organization" category.

### Chat Tab

- 

### Map Tab

- Displays a map centered on Kyrgyzstan using GMap.NET. The map allows users to zoom and pan.

![image](https://github.com/enoobis/taxi-manager-app/assets/62465404/3898a88c-062e-450e-ae34-668c354918b4)

### Call Log Tab

- View the call log displaying all calls associated with orders.

### Help


### Settings

- View statistics and reports on orders. Reports include data on order types and their counts.

## Troubleshooting

### Error: Could not load type 'GMap.NET.Singleton`1'

- Ensure correct versions of GMap.NET packages are installed.
- Check compatibility with the .NET version being used.

### Error: GMapControl is not supported in "Windows Presentation Foundation (WPF)" project

- Ensure the GMap namespace is correctly added in the XAML file:

   ```xaml
   xmlns:gmaps="clr-namespace:GMap.NET.WindowsPresentation;assembly=GMap.NET.WindowsPresentation"
   ```

- Ensure `GMap.NET.Core` and `GMap.NET.WindowsPresentation` packages are installed.

### Database connection issues

- Verify the connection string is correct and the database file path is accurate.
- Ensure SQL Server LocalDB is installed and running.

## Contributing

1. **Fork the repository**

   Click the `Fork` button in the top right corner of the repository page to create a copy of the repository in your GitHub account.

2. **Clone your fork**

   ```bash
   git clone https://github.com/yourusername/taxi_management_app.git
   cd taxi_management_app
   ```

3. **Create a branch**

   ```bash
   git checkout -b feature/your-feature-name
   ```

4. **Make your changes**

   Make changes and commit them with a clear message.

5. **Push your changes**

   ```bash
   git push origin feature/your-feature-name
   ```

6. **Open a pull request**

   Go to the original repository and open a pull request with a description of your changes.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
