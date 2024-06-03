using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace taxi_management_app
{
    public partial class UpdateOrderWindow : Window
    {
        public SqlParameter[] OrderParameters { get; private set; }
        private DataRowView orderRow;

        public UpdateOrderWindow(DataRowView orderRow)
        {
            InitializeComponent();
            this.orderRow = orderRow;

            PhoneNumberTextBox.Text = orderRow["PhoneNumber"].ToString();
            FromLocationTextBox.Text = orderRow["FromLocation"].ToString();
            HasLuggageCheckBox.IsChecked = orderRow["HasLuggage"] as bool?;
            HasChildrenCheckBox.IsChecked = orderRow["HasChildren"] as bool?;
            ToLocationTextBox.Text = orderRow["ToLocation"].ToString();
            OrderTypeComboBox.Text = orderRow["OrderType"].ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            OrderParameters = new SqlParameter[]
            {
                new SqlParameter("@PhoneNumber", PhoneNumberTextBox.Text),
                new SqlParameter("@FromLocation", FromLocationTextBox.Text),
                new SqlParameter("@HasLuggage", HasLuggageCheckBox.IsChecked),
                new SqlParameter("@HasChildren", HasChildrenCheckBox.IsChecked),
                new SqlParameter("@ToLocation", ToLocationTextBox.Text),
                new SqlParameter("@OrderType", ((ComboBoxItem)OrderTypeComboBox.SelectedItem).Content),
                new SqlParameter("@Id", orderRow["Id"])
            };

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
