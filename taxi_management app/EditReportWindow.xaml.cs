using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace taxi_management_app
{
    public partial class EditReportWindow : Window
    {
        public SqlParameter[] OrderParameters { get; private set; }

        public EditReportWindow(DataRowView row)
        {
            InitializeComponent();
            PhoneNumberTextBox.Text = row["PhoneNumber"].ToString();
            FromLocationTextBox.Text = row["FromLocation"].ToString();
            HasLuggageTextBox.Text = row["HasLuggage"].ToString();
            HasChildrenTextBox.Text = row["HasChildren"].ToString();
            ToLocationTextBox.Text = row["ToLocation"].ToString();
            OrderTypeTextBox.Text = row["OrderType"].ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            OrderParameters = new SqlParameter[]
            {
                new SqlParameter("@PhoneNumber", PhoneNumberTextBox.Text),
                new SqlParameter("@FromLocation", FromLocationTextBox.Text),
                new SqlParameter("@HasLuggage", HasLuggageTextBox.Text),
                new SqlParameter("@HasChildren", HasChildrenTextBox.Text),
                new SqlParameter("@ToLocation", ToLocationTextBox.Text),
                new SqlParameter("@OrderType", OrderTypeTextBox.Text)
            };
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
