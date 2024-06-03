using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace taxi_management_app
{
    public partial class AddOrderWindow : Window
    {
        public SqlParameter[] OrderParameters { get; private set; }

        public AddOrderWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                OrderParameters = new SqlParameter[]
                {
                    new SqlParameter("@PhoneNumber", PhoneNumber.Text),
                    new SqlParameter("@FromLocation", FromLocation.Text),
                    new SqlParameter("@HasLuggage", HasLuggage.IsChecked ?? false),
                    new SqlParameter("@HasChildren", HasChildren.IsChecked ?? false),
                    new SqlParameter("@ToLocation", ToLocation.Text),
                    new SqlParameter("@OrderType", ((ComboBoxItem)OrderType.SelectedItem).Content.ToString())
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
            if (string.IsNullOrWhiteSpace(PhoneNumber.Text)) return false;
            if (string.IsNullOrWhiteSpace(FromLocation.Text)) return false;
            if (string.IsNullOrWhiteSpace(ToLocation.Text)) return false;
            if (OrderType.SelectedItem == null) return false;
            return true;
        }
    }
}
