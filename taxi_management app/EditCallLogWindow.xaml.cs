using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace taxi_management_app
{
    public partial class EditCallLogWindow : Window
    {
        public SqlParameter[] CallParameters { get; private set; }

        public EditCallLogWindow(DataRowView row)
        {
            InitializeComponent();
            CallerNameTextBox.Text = row["CallerName"].ToString();
            CallerPhoneNumberTextBox.Text = row["CallerPhoneNumber"].ToString();
            CallDurationTextBox.Text = row["CallDuration"].ToString();
            CallNotesTextBox.Text = row["CallNotes"].ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(CallDurationTextBox.Text, out int callDuration))
            {
                CallParameters = new SqlParameter[]
                {
                    new SqlParameter("@CallerName", CallerNameTextBox.Text),
                    new SqlParameter("@CallerPhoneNumber", CallerPhoneNumberTextBox.Text),
                    new SqlParameter("@CallDuration", callDuration),
                    new SqlParameter("@CallNotes", CallNotesTextBox.Text)
                };
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Длительность звонка должна быть числом.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
