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

namespace taxi_management_app
{
    public partial class OrderWindow : Window
    {
        public SqlParameter[] OrderParameters { get; private set; }

        public OrderWindow()
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
