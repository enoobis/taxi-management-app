using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Globalization;
using System.Windows.Data;
using Markdig;
using System.IO;
using System.Windows.Media;



namespace taxi_management_app
{
    public class TextBoxEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int length = (int)value;
            return length == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

   
    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        private HttpClient httpClient;

        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadOrders();
            LoadDrivers();
            LoadCallLog();
            LoadReports();

            MainTabControl.SelectionChanged += MainTabControl_SelectionChanged;
            httpClient = new HttpClient();
        }
        private async void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Header is StackPanel headerPanel)
            {
                if (headerPanel.Children.OfType<TextBlock>().FirstOrDefault()?.Text == "Помощь")
                {
                    string url = "https://raw.githubusercontent.com/enoobis/taxi-management-app/master/README.md";
                    string markdownContent = await LoadMarkdownContent(url);
                    MarkdownViewer.NavigateToString(ConvertMarkdownToHtml(markdownContent));
                }
            }
        }

        private async Task<string> LoadMarkdownContent(string url)
        {
            try
            {
                return await httpClient.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки README.md: {ex.Message}");
                return string.Empty;
            }
        }

        private string ConvertMarkdownToHtml(string markdownContent)
        {
            // Конвертация Markdown в HTML (например, с использованием библиотеки Markdig)
            var pipeline = new Markdig.MarkdownPipelineBuilder().Build();
            return Markdig.Markdown.ToHtml(markdownContent, pipeline);
        }

        private void InitializeDatabase()
        {
            try
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\enoobis\source\repos\taxi_management app\taxi_management app\app_db.mdf;Integrated Security=True";
                connection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации базы данных: {ex.Message}");
            }
        }

        private void LoadOrders()
        {
            LoadDataGrid(UnassignedOrdersGrid, "SELECT * FROM Orders WHERE Status='Unassigned'");
            LoadDataGrid(CurrentOrdersGrid, "SELECT * FROM Orders WHERE Status='Current'");
            LoadDataGrid(PlannedOrdersGrid, "SELECT * FROM Orders WHERE Status='Planned'");
            LoadDataGrid(CompletedOrdersGrid, "SELECT * FROM Orders WHERE Status='Completed'");
        }

        private void LoadDrivers()
        {
            LoadDataGrid(AvailableDriversGrid, "SELECT * FROM Drivers WHERE Status='Available'");
            LoadDataGrid(OnDutyDriversGrid, "SELECT * FROM Drivers WHERE Status='OnDuty'");
            LoadDataGrid(OnBreakDriversGrid, "SELECT * FROM Drivers Where Status='OnBreak'");
        }

        private void LoadCallLog()
        {
            LoadDataGrid(CallLogGrid, "SELECT * FROM CallLog");
        }

        private void LoadReports()
        {
            LoadOrdersReport();
        }

        private void LoadOrdersReport()
        {
            string queryOrderType = "SELECT OrderType, COUNT(*) AS OrderCount FROM Orders GROUP BY OrderType";
            LoadDataGrid(OrdersReportGrid, queryOrderType);
        }

        private void LoadDataGrid(DataGrid dataGrid, string query)
        {
            try
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGrid.ItemsSource = dataTable.DefaultView;
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow orderWindow = new AddOrderWindow();
            if (orderWindow.ShowDialog() == true)
            {
                var parameters = orderWindow.OrderParameters;

                // Убедитесь, что тип данных для PhoneNumber является строкой
                string phoneNumber = parameters.First(p => p.ParameterName == "@PhoneNumber").Value.ToString();
                LogCall(phoneNumber);

                string query = "INSERT INTO Orders (PhoneNumber, FromLocation, HasLuggage, HasChildren, ToLocation, OrderType, Status) VALUES (@PhoneNumber, @FromLocation, @HasLuggage, @HasChildren, @ToLocation, @OrderType, 'Unassigned')";
                ExecuteNonQuery(query, parameters);
                LoadOrders();
            }
        }




        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = GetSelectedOrdersDataGrid();
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItem;
                int driverId = row["DriverId"] != DBNull.Value ? Convert.ToInt32(row["DriverId"]) : -1;

                string query = "DELETE FROM Orders WHERE Id = @Id";
                try
                {
                    ExecuteNonQuery(query, new SqlParameter[] { new SqlParameter("@Id", Convert.ToInt32(row["Id"])) });

                    if (driverId != -1)
                    {
                        string driverQuery = "UPDATE Drivers SET Status='Available' WHERE Id=@DriverId";
                        ExecuteNonQuery(driverQuery, new SqlParameter[] { new SqlParameter("@DriverId", driverId) });
                    }

                    LoadOrders();
                    LoadDrivers(); // Update driver status in the UI
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления заказа: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.");
            }
        }

        private DataGrid GetSelectedOrdersDataGrid()
        {
            if (UnassignedOrdersGrid.SelectedItem != null)
                return UnassignedOrdersGrid;
            if (CurrentOrdersGrid.SelectedItem != null)
                return CurrentOrdersGrid;
            if (PlannedOrdersGrid.SelectedItem != null)
                return PlannedOrdersGrid;
            if (CompletedOrdersGrid.SelectedItem != null)
                return CompletedOrdersGrid;
            return null;
        }

        private string GetOrderClass(int orderId)
        {
            string query = "SELECT OrderType FROM Orders WHERE Id = @Id";
            SqlParameter[] parameters = { new SqlParameter("@Id", orderId) };
            DataTable dataTable = ExecuteQuery(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["OrderType"].ToString();
            }
            return null;
        }

        private DataTable ExecuteQuery(string query, SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddRange(parameters);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}");
            }
            return dataTable;
        }

        private void UpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = GetSelectedOrdersDataGrid();
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItem;
                UpdateOrderWindow updateOrderWindow = new UpdateOrderWindow(row);
                if (updateOrderWindow.ShowDialog() == true)
                {
                    string query = "UPDATE Orders SET PhoneNumber=@PhoneNumber, FromLocation=@FromLocation, HasLuggage=@HasLuggage, HasChildren=@HasChildren, ToLocation=@ToLocation, OrderType=@OrderType WHERE Id=@Id";
                    try
                    {
                        ExecuteNonQuery(query, updateOrderWindow.OrderParameters);
                        LoadOrders();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка обновления заказа: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для обновления.");
            }
        }

        private void ChangeOrderView_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = GetSelectedOrdersDataGrid();
            if (dataGrid != null)
            {
                ColumnVisibilityWindow columnVisibilityWindow = new ColumnVisibilityWindow(dataGrid.Columns);
                if (columnVisibilityWindow.ShowDialog() == true)
                {
                    foreach (var column in dataGrid.Columns)
                    {
                        column.Visibility = columnVisibilityWindow.ColumnVisibility[column.Header.ToString()];
                    }
                }
            }
        }

        private void ChangeDriverView_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = GetSelectedDriversDataGrid();
            if (dataGrid != null)
            {
                ColumnVisibilityWindow columnVisibilityWindow = new ColumnVisibilityWindow(dataGrid.Columns);
                if (columnVisibilityWindow.ShowDialog() == true)
                {
                    foreach (var column in dataGrid.Columns)
                    {
                        column.Visibility = columnVisibilityWindow.ColumnVisibility[column.Header.ToString()];
                    }
                }
            }
        }

        private void HighlightOrder_Click(object sender, RoutedEventArgs e)
        {
            // Highlight order logic here
        }

        private void CompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = GetSelectedOrdersDataGrid();
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItem;
                int driverId = row["DriverId"] != DBNull.Value ? Convert.ToInt32(row["DriverId"]) : -1;

                string query = "UPDATE Orders SET Status='Completed' WHERE Id=@Id";
                try
                {
                    ExecuteNonQuery(query, new SqlParameter[] { new SqlParameter("@Id", Convert.ToInt32(row["Id"])) });

                    if (driverId != -1)
                    {
                        string driverQuery = "UPDATE Drivers SET Status='Available' WHERE Id=@DriverId";
                        ExecuteNonQuery(driverQuery, new SqlParameter[] { new SqlParameter("@DriverId", driverId) });
                    }

                    LoadOrders();
                    LoadDrivers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка завершения заказа: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для завершения.");
            }
        }

        private void AssignOrder_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = GetSelectedOrdersDataGrid();
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItem;
                int orderId = Convert.ToInt32(row["Id"]);
                string orderClass = GetOrderClass(orderId);

                AssignOrderWindow assignOrderWindow = new AssignOrderWindow(row, orderClass);
                if (assignOrderWindow.ShowDialog() == true)
                {
                    string status = assignOrderWindow.IsPlanned ? "Planned" : "Current";
                    string orderQuery = "UPDATE Orders SET DriverId=@DriverId, Status=@Status, PlannedTime=@PlannedTime WHERE Id=@Id";
                    string driverQuery = "UPDATE Drivers SET Status=@DriverStatus WHERE Id=@DriverId";

                    SqlParameter[] orderParameters = new SqlParameter[]
                    {
                        new SqlParameter("@DriverId", assignOrderWindow.SelectedDriverId),
                        new SqlParameter("@Status", status),
                        new SqlParameter("@PlannedTime", (object)assignOrderWindow.PlannedTime ?? DBNull.Value),
                        new SqlParameter("@Id", Convert.ToInt32(row["Id"]))
                    };

                    SqlParameter[] driverParameters = new SqlParameter[]
                    {
                        new SqlParameter("@DriverStatus", assignOrderWindow.IsPlanned ? "Available" : "OnDuty"),
                        new SqlParameter("@DriverId", assignOrderWindow.SelectedDriverId)
                    };

                    try
                    {
                        ExecuteNonQuery(orderQuery, orderParameters);
                        ExecuteNonQuery(driverQuery, driverParameters);
                        LoadOrders();
                        LoadDrivers(); // Update driver status in the UI
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка присвоения заказа: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для присвоения.");
            }
        }

        private void AddDriver_Click(object sender, RoutedEventArgs e)
        {
            DriverWindow driverWindow = new DriverWindow();
            if (driverWindow.ShowDialog() == true)
            {
                string query = "INSERT INTO Drivers (FullName, CarBrand, CarModel, CarType, CarNumber, PhoneNumber, Year, Color, Status) VALUES (@FullName, @CarBrand, @CarModel, @CarType, @CarNumber, @PhoneNumber, @Year, @Color, 'Available')";
                try
                {
                    ExecuteNonQuery(query, driverWindow.DriverParameters);
                    LoadDrivers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления водителя: {ex.Message}");
                }
            }
        }

        private void DeleteDriver_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = GetSelectedDriversDataGrid();
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItem;
                string query = "DELETE FROM Drivers WHERE Id = @Id";
                try
                {
                    ExecuteNonQuery(query, new SqlParameter[] { new SqlParameter("@Id", Convert.ToInt32(row["Id"])) });
                    LoadDrivers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления водителя: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите водителя для удаления.");
            }
        }

        private DataGrid GetSelectedDriversDataGrid()
        {
            if (AvailableDriversGrid.SelectedItem != null)
                return AvailableDriversGrid;
            if (OnDutyDriversGrid.SelectedItem != null)
                return OnDutyDriversGrid;
            if (OnBreakDriversGrid.SelectedItem != null)
                return OnBreakDriversGrid;
            return null;
        }

        private void UpdateDriver_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = GetSelectedDriversDataGrid();
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItem;
                UpdateDriverWindow updateDriverWindow = new UpdateDriverWindow(row);
                if (updateDriverWindow.ShowDialog() == true)
                {
                    string query = "UPDATE Drivers SET FullName=@FullName, CarBrand=@CarBrand, CarModel=@CarModel, CarType=@CarType, CarNumber=@CarNumber, PhoneNumber=@PhoneNumber, Year=@Year, Color=@Color, Status=@Status WHERE Id=@Id";
                    SqlParameter[] parameters = updateDriverWindow.DriverParameters;
                    try
                    {
                        ExecuteNonQuery(query, parameters);
                        LoadDrivers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка обновления водителя: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите водителя для обновления.");
            }
        }

        private void FilterAllOrders_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void FilterStandardOrders_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid(UnassignedOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Standard' AND Status='Unassigned'");
            LoadDataGrid(CurrentOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Standard' AND Status='Current'");
            LoadDataGrid(PlannedOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Standard' AND Status='Planned'");
            LoadDataGrid(CompletedOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Standard' AND Status='Completed'");
        }

        private void FilterComfortOrders_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid(UnassignedOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Comfort' AND Status='Unassigned'");
            LoadDataGrid(CurrentOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Comfort' AND Status='Current'");
            LoadDataGrid(PlannedOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Comfort' AND Status='Planned'");
            LoadDataGrid(CompletedOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Comfort' AND Status='Completed'");
        }

        private void FilterBusinessOrders_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid(UnassignedOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Business' AND Status='Unassigned'");
            LoadDataGrid(CurrentOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Business' AND Status='Current'");
            LoadDataGrid(PlannedOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Business' AND Status='Planned'");
            LoadDataGrid(CompletedOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Business' AND Status='Completed'");
        }

        private void FilterOrganizationOrders_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid(UnassignedOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Organization' AND Status='Unassigned'");
            LoadDataGrid(CurrentOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Organization' AND Status='Current'");
            LoadDataGrid(PlannedOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Organization' AND Status='Planned'");
            LoadDataGrid(CompletedOrdersGrid, "SELECT * FROM Orders WHERE OrderType='Organization' AND Status='Completed'");
        }

        private void FilterAllDrivers_Click(object sender, RoutedEventArgs e)
        {
            LoadDrivers();
        }

        private void FilterStandardDrivers_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid(AvailableDriversGrid, "SELECT * FROM Drivers WHERE CarType='Standard' AND Status='Available'");
            LoadDataGrid(OnDutyDriversGrid, "SELECT * FROM Drivers WHERE CarType='Standard' AND Status='OnDuty'");
            LoadDataGrid(OnBreakDriversGrid, "SELECT * FROM Drivers Where CarType='Standard' AND Status='OnBreak'");
        }

        private void FilterComfortDrivers_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid(AvailableDriversGrid, "SELECT * FROM Drivers Where CarType='Comfort' AND Status='Available'");
            LoadDataGrid(OnDutyDriversGrid, "SELECT * FROM Drivers Where CarType='Comfort' AND Status='OnDuty'");
            LoadDataGrid(OnBreakDriversGrid, "SELECT * FROM Drivers Where CarType='Comfort' AND Status='OnBreak'");
        }

        private void FilterBusinessDrivers_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid(AvailableDriversGrid, "SELECT * FROM Drivers Where CarType='Business' AND Status='Available'");
            LoadDataGrid(OnDutyDriversGrid, "SELECT * FROM Drivers Where CarType='Business' AND Status='OnDuty'");
            LoadDataGrid(OnBreakDriversGrid, "SELECT * FROM Drivers Where CarType='Business' AND Status='OnBreak'");
        }

        private void FilterOrganizationDrivers_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid(AvailableDriversGrid, "SELECT * FROM Drivers Where CarType='Organization' AND Status='Available'");
            LoadDataGrid(OnDutyDriversGrid, "SELECT * FROM Drivers Where CarType='Organization' AND Status='OnDuty'");
            LoadDataGrid(OnBreakDriversGrid, "SELECT * FROM Drivers Where CarType='Organization' AND Status='OnBreak'");
        }

        private void ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}");
            }
        }


        private void ChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ChatBox.Text += ChatInput.Text + Environment.NewLine;
                ChatInput.Clear();
            }
        }
        private void RefreshCallLog_Click(object sender, RoutedEventArgs e)
        {
            LoadCallLog();
        }

        private void DeleteCallLog_Click(object sender, RoutedEventArgs e)
        {
            if (CallLogGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)CallLogGrid.SelectedItem;
                int callId = (int)row["Id"];

                string query = "DELETE FROM CallLog WHERE Id = @Id";
                ExecuteNonQuery(query, new SqlParameter[] { new SqlParameter("@Id", callId) });

                LoadCallLog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления.");
            }
        }
        private void EditCallLog_Click(object sender, RoutedEventArgs e)
        {
            if (CallLogGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)CallLogGrid.SelectedItem;
                EditCallLogWindow editWindow = new EditCallLogWindow(row);
                if (editWindow.ShowDialog() == true)
                {
                    string query = "UPDATE CallLog SET CallerName=@CallerName, CallerPhoneNumber=@CallerPhoneNumber, CallDuration=@CallDuration, CallNotes=@CallNotes WHERE Id=@Id";
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                new SqlParameter("@CallerName", editWindow.CallParameters[0].Value),
                new SqlParameter("@CallerPhoneNumber", editWindow.CallParameters[1].Value),
                new SqlParameter("@CallDuration", editWindow.CallParameters[2].Value),
                new SqlParameter("@CallNotes", editWindow.CallParameters[3].Value),
                new SqlParameter("@Id", row["Id"])
                    };
                    ExecuteNonQuery(query, parameters);
                    LoadCallLog();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для изменения.");
            }
        }
        private void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            MapControl.MapProvider = GMapProviders.GoogleMap;
            MapControl.Position = new PointLatLng(41.2044, 74.7661); // координаты Кыргызстана
            MapControl.MinZoom = 2;
            MapControl.MaxZoom = 17;
            MapControl.Zoom = 6;
            MapControl.Manager.Mode = AccessMode.ServerAndCache;
            MapControl.CanDragMap = true;
            MapControl.DragButton = MouseButton.Left;
        }

        private void LogCall(string phoneNumber)
        {
            string query = "INSERT INTO CallLog (CallerPhoneNumber, CallTime) VALUES (@CallerPhoneNumber, @CallTime)";
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@CallerPhoneNumber", phoneNumber),
        new SqlParameter("@CallTime", DateTime.Now)
            };
            ExecuteNonQuery(query, parameters);
        }

        private void DeleteReport_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersReportGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)OrdersReportGrid.SelectedItem;
                string query = "DELETE FROM Orders WHERE Id = @Id";
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@Id", row["Id"])
                };
                ExecuteNonQuery(query, parameters);
                LoadOrdersReport();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления.");
            }
        }

        private void EditReport_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersReportGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)OrdersReportGrid.SelectedItem;
                EditReportWindow editWindow = new EditReportWindow(row);
                if (editWindow.ShowDialog() == true)
                {
                    string query = "UPDATE Orders SET PhoneNumber=@PhoneNumber, FromLocation=@FromLocation, HasLuggage=@HasLuggage, HasChildren=@HasChildren, ToLocation=@ToLocation, OrderType=@OrderType WHERE Id=@Id";
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                new SqlParameter("@PhoneNumber", editWindow.OrderParameters[0].Value),
                new SqlParameter("@FromLocation", editWindow.OrderParameters[1].Value),
                new SqlParameter("@HasLuggage", editWindow.OrderParameters[2].Value),
                new SqlParameter("@HasChildren", editWindow.OrderParameters[3].Value),
                new SqlParameter("@ToLocation", editWindow.OrderParameters[4].Value),
                new SqlParameter("@OrderType", editWindow.OrderParameters[5].Value),
                new SqlParameter("@Id", row["Id"])
                    };
                    ExecuteNonQuery(query, parameters);
                    LoadOrdersReport();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для изменения.");
            }
        }

        private void RefreshReport_Click(object sender, RoutedEventArgs e)
        {
            LoadOrdersReport();
        }



        ///////////
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string selectedLanguage = (comboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Логика смены языка
            if (selectedLanguage == "English")
            {
                // Пример логики для смены языка на английский
                ChangeLanguageToEnglish();
            }
            else if (selectedLanguage == "Русский")
            {
                // Пример логики для смены языка на русский
                ChangeLanguageToRussian();
            }
        }

        private void ChangeLanguageToEnglish()
        {
            // Пример кода для смены интерфейса на английский
            // Здесь необходимо изменить все тексты в интерфейсе на английский язык
        }

        private void ChangeLanguageToRussian()
        {
            // Пример кода для смены интерфейса на русский
            // Здесь необходимо изменить все тексты в интерфейсе на русский язык
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string selectedTheme = (comboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Логика смены темы
            if (selectedTheme == "Темная")
            {
                // Пример логики для смены на темную тему
                Application.Current.Resources["BackgroundColor"] = Brushes.Black;
                Application.Current.Resources["ForegroundColor"] = Brushes.White;
                SetDarkTheme();
            }
            else if (selectedTheme == "Светлая")
            {
                // Пример логики для смены на светлую тему
                Application.Current.Resources["BackgroundColor"] = Brushes.White;
                Application.Current.Resources["ForegroundColor"] = Brushes.Black;
                SetLightTheme();
            }
        }

        private void SetDarkTheme()
        {
            // Пример кода для установки темной темы
            this.Background = Brushes.Black;
            // Обновите все элементы управления, чтобы применить темную тему
        }

        private void SetLightTheme()
        {
            // Пример кода для установки светлой темы
            this.Background = Brushes.White;
            // Обновите все элементы управления, чтобы применить светлую тему
        }

        private void WhatsAppChatButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для открытия чата WhatsApp
            System.Diagnostics.Process.Start("https://web.whatsapp.com/");
        }

        private void TelegramChatButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для открытия чата Telegram
            System.Diagnostics.Process.Start("https://web.telegram.org/");
        }
    }
}
