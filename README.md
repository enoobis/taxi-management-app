# taxi-management-app

## Overview

**Taxi Management App** is designed for taxi dispatchers to manage taxi orders, drivers, and related operations.
This application was inspired by the "Taxi-Master" program.

## Video Tutorial

(Setup)[https://www.youtube.com/watch?v=CODbg4kKZPg]

## Screenshots

### Taxi-Master
![image](https://github.com/enoobis/taxi-management-app/assets/62465404/5247c4e2-58b6-44a1-957d-3730d023bd9e)


### Taxi Management App
![image](https://github.com/enoobis/taxi-management-app/assets/62465404/8b6ce46a-a34a-4225-b3e7-a556e7cb8da5)
![image](https://github.com/enoobis/taxi-management-app/assets/62465404/49afe8e7-b2c4-4a47-aea7-c1d584d1ef9d)



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

Provides options for WhatsApp and Telegram integration.

### Report 

Displays various reports and statistics related to orders and drivers.
Buttons to delete, update, and refresh the report data.

![image](https://github.com/enoobis/taxi-management-app/assets/62465404/7d326e50-f2bd-4449-902c-c8553ce137af)


### Map Tab

Displays a map centered on Kyrgyzstan using GMap.NET. The map allows users to zoom and pan.

![image](https://github.com/enoobis/taxi-management-app/assets/62465404/f0c690b2-992a-4452-b30b-637801bb20e1)


### Call Log Tab

View the call log displaying all calls associated with orders.

![image](https://github.com/enoobis/taxi-management-app/assets/62465404/a540a7c6-c5c4-43ff-9610-2890284595dc)


### Help

Displays a markdown document from the repository for user assistance.

![image](https://github.com/enoobis/taxi-management-app/assets/62465404/6b3f3e58-20a2-49af-8d78-4c7c82a3e453)


### Settings

Options for changing language (Russian, English).
Options for switching between dark and light themes.
Buttons for integrating WhatsApp and Telegram chats.

![image](https://github.com/enoobis/taxi-management-app/assets/62465404/61c8e494-9140-4c54-a690-3eaaa097e9c6)


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
