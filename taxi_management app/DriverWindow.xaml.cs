using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace taxi_management_app
{
    public partial class DriverWindow : Window
    {
        public SqlParameter[] DriverParameters { get; private set; }

        public DriverWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                DriverParameters = new SqlParameter[]
                {
                    new SqlParameter("@FullName", FullName.Text),
                    new SqlParameter("@CarBrand", CarBrand.Text),
                    new SqlParameter("@CarModel", CarModel.Text),
                    new SqlParameter("@CarType", ((ComboBoxItem)CarType.SelectedItem).Content.ToString()),
                    new SqlParameter("@CarNumber", CarNumber.Text),
                    new SqlParameter("@PhoneNumber", PhoneNumber.Text),
                    new SqlParameter("@Year", Year.Text),
                    new SqlParameter("@Color", Color.Text)
                };
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(FullName.Text)) return false;
            if (string.IsNullOrWhiteSpace(CarBrand.Text)) return false;
            if (string.IsNullOrWhiteSpace(CarModel.Text)) return false;
            if (CarType.SelectedItem == null) return false;
            if (string.IsNullOrWhiteSpace(CarNumber.Text)) return false;
            if (string.IsNullOrWhiteSpace(PhoneNumber.Text)) return false;
            if (!int.TryParse(Year.Text, out _)) return false;
            if (string.IsNullOrWhiteSpace(Color.Text)) return false;
            return true;
        }
    }
}
