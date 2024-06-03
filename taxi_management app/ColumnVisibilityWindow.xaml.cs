using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace taxi_management_app
{
    public partial class ColumnVisibilityWindow : Window
    {
        public Dictionary<string, Visibility> ColumnVisibility { get; private set; }

        public ColumnVisibilityWindow(ICollection<DataGridColumn> columns)
        {
            InitializeComponent();
            ColumnVisibility = new Dictionary<string, Visibility>();

            foreach (var column in columns)
            {
                ColumnVisibility[column.Header.ToString()] = column.Visibility;
                var checkBox = new CheckBox
                {
                    Content = column.Header.ToString(),
                    IsChecked = column.Visibility == Visibility.Visible
                };
                checkBox.Checked += (s, e) => ColumnVisibility[checkBox.Content.ToString()] = Visibility.Visible;
                checkBox.Unchecked += (s, e) => ColumnVisibility[checkBox.Content.ToString()] = Visibility.Collapsed;
                ColumnListPanel.Children.Add(checkBox);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
