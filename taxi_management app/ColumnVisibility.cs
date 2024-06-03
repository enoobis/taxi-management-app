public class ColumnVisibility
{
    public string ColumnName { get; set; }
    public bool IsVisible { get; set; }

    public ColumnVisibility(string columnName, bool isVisible)
    {
        ColumnName = columnName;
        IsVisible = isVisible;
    }
}

