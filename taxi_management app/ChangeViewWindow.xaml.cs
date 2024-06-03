using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace taxi_management_app
{
    public partial class ChangeViewWindow : Window
    {
        private readonly DataGrid dataGrid;

        public ChangeViewWindow(DataGrid dataGrid)
        {
            InitializeComponent();
            this.dataGrid = dataGrid;
            LoadColumnVisibility();
        }

        private void LoadColumnVisibility()
        {
            List<ColumnVisibility> columnVisibilityList = dataGrid.Columns.Select(c => new ColumnVisibility
            {
                Name = c.Header.ToString(),
                IsVisible = c.Visibility == Visibility.Visible
            }).ToList();

            ColumnList.ItemsSource = columnVisibilityList;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateColumnVisibility();
            DialogResult = true;
            Close();
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            UpdateColumnVisibility();
        }

        private void UpdateColumnVisibility()
        {
            List<ColumnVisibility> columnVisibilityList = ColumnList.ItemsSource as List<ColumnVisibility>;

            foreach (var columnVisibility in columnVisibilityList)
            {
                var column = dataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == columnVisibility.Name);
                if (column != null)
                {
                    column.Visibility = columnVisibility.IsVisible ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
    }

    public class ColumnVisibility
    {
        public string Name { get; set; }
        public bool IsVisible { get; set; }
    }
}
