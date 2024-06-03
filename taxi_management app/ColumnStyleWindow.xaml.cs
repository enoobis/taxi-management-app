using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace taxi_management_app
{
    public partial class ColumnStyleWindow : Window
    {
        public ObservableCollection<string> Columns { get; set; }
        public ObservableCollection<string> SelectedColumns { get; private set; }

        public ColumnStyleWindow(ObservableCollection<string> columns)
        {
            InitializeComponent();
            Columns = columns;
            SelectedColumns = new ObservableCollection<string>(columns);
            ColumnItemsControl.ItemsSource = Columns;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedColumns.Clear();
            foreach (var item in ColumnItemsControl.Items)
            {
                var checkBox = ColumnItemsControl.ItemContainerGenerator.ContainerFromItem(item) as CheckBox;
                if (checkBox != null && checkBox.IsChecked == true)
                {
                    SelectedColumns.Add((string)item);
                }
            }
            DialogResult = true;
            Close();
        }
    }
}
