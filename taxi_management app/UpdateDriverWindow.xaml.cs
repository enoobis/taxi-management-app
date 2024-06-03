using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace taxi_management_app
{
    public partial class UpdateDriverWindow : Window
    {
        public SqlParameter[] DriverParameters { get; private set; }
        private DataRowView driverRow;

        public UpdateDriverWindow(DataRowView driverRow)
        {
            InitializeComponent();
            this.driverRow = driverRow;

            FullNameTextBox.Text = driverRow["FullName"].ToString();
            CarBrandTextBox.Text = driverRow["CarBrand"].ToString();
            CarModelTextBox.Text = driverRow["CarModel"].ToString();
            CarTypeComboBox.Text = driverRow["CarType"].ToString();
            CarNumberTextBox.Text = driverRow["CarNumber"].ToString();
            PhoneNumberTextBox.Text = driverRow["PhoneNumber"].ToString();
            YearTextBox.Text = driverRow["Year"].ToString();
            ColorTextBox.Text = driverRow["Color"].ToString();
            StatusComboBox.Text = driverRow["Status"].ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DriverParameters = new SqlParameter[]
            {
                new SqlParameter("@FullName", FullNameTextBox.Text),
                new SqlParameter("@CarBrand", CarBrandTextBox.Text),
                new SqlParameter("@CarModel", CarModelTextBox.Text),
                new SqlParameter("@CarType", ((ComboBoxItem)CarTypeComboBox.SelectedItem).Content),
                new SqlParameter("@CarNumber", CarNumberTextBox.Text),
                new SqlParameter("@PhoneNumber", PhoneNumberTextBox.Text),
                new SqlParameter("@Year", YearTextBox.Text),
                new SqlParameter("@Color", ColorTextBox.Text),
                new SqlParameter("@Status", ((ComboBoxItem)StatusComboBox.SelectedItem).Content),
                new SqlParameter("@Id", driverRow["Id"])
            };

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
