using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace taxi_management_app
{


    public partial class AssignOrderWindow : Window
    {
        public int SelectedDriverId { get; private set; }
        public bool IsPlanned { get; private set; }
        public DateTime? PlannedTime { get; private set; }

        private DataRowView orderRow;
        private string orderClass;

        public AssignOrderWindow(DataRowView orderRow, string orderClass)
        {
            InitializeComponent();
            this.orderRow = orderRow;
            this.orderClass = orderClass;

            LoadDrivers();
        }



        private void LoadDrivers()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\enoobis\source\repos\taxi_management app\taxi_management app\app_db.mdf;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Id, FullName FROM Drivers WHERE Status='Available' AND CarType=@CarType";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@CarType", orderClass);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    DriverComboBox.ItemsSource = dataTable.DefaultView;
                    DriverComboBox.DisplayMemberPath = "FullName";
                    DriverComboBox.SelectedValuePath = "Id";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки водителей: {ex.Message}");
                }
            }
        }

        private void PlannedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PlannedTimeTextBox.Visibility = Visibility.Visible;
        }

        private void PlannedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PlannedTimeTextBox.Visibility = Visibility.Collapsed;
            PlannedTime = null;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (DriverComboBox.SelectedValue != null)
            {
                SelectedDriverId = (int)DriverComboBox.SelectedValue;
                IsPlanned = PlannedCheckBox.IsChecked == true;

                if (IsPlanned)
                {
                    if (DateTime.TryParse(PlannedTimeTextBox.Text, out DateTime plannedTime))
                    {
                        PlannedTime = plannedTime;
                    }
                    else
                    {
                        MessageBox.Show("Некорректное время для запланированного заказа.");
                        return;
                    }
                }

                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите водителя.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}
