using Microsoft.Win32;
using SecurityReport.Data;
using SecurityReport.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Block = SecurityReport.Enums.Block;

namespace SecurityReport;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private static DataManager _manager = DataManager.GetManager();
    public MainWindow()
    {
        InitializeComponent();
        GenerateCheckboxMenu();
    }

    public void GenerateCheckboxMenu()
    {
        var blocks = (Block[])Enum.GetValues(typeof(Block));
        foreach (var block in blocks)
        {
            var name = block.GetAttribute<DisplayAttribute>()?.Name ?? block.ToString();
            stackPanelOptions.Children.Add(new CheckBox() { Name = block.ToString(), Content = name });
        }

        Button generateReport = new Button() { Content = "Generate Report" };
        stackPanelOptions.Children.Add(generateReport);
        generateReport.Click += OnGenerateReportButtonClicked;
    }

    public void OnGenerateReportButtonClicked(object sender, RoutedEventArgs e)
    {
        stackPanelReportMenu.Visibility = Visibility.Hidden;
        var selectedOptions = GetSelectedBlocks();
        GenerateData(selectedOptions);
        dataStackPanel.Visibility = Visibility.Visible;
    }

    public IEnumerable<Block> GetSelectedBlocks()
    {
        var selected = new List<Block>();
        foreach (UIElement child in stackPanelOptions.Children)
        {
            if(child is CheckBox checkBox)
            {
                if(checkBox.IsChecked ?? false)
                {
                    selected.Add(Enum.Parse<Block>(checkBox.Name));
                }
            }
        }

        return selected;
    }

    public void OnBackToMenuClicked(object sender, RoutedEventArgs e)
    {
        dataStackPanel.Visibility = Visibility.Hidden;
        dataStackPanel.Children.Clear();
        stackPanelReportMenu.Visibility = Visibility.Visible;
    }

    public void OnJsonSaveClicked(object sender, RoutedEventArgs e)
    {
        var stream = _manager.GetJson();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        if (saveFileDialog.ShowDialog() == true)
        {
            var path = saveFileDialog.FileName;
            using FileStream file = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write);
            stream.WriteTo(file);
            file.Close();
        }
    }


    public void GenerateData(IEnumerable<Block> blocks)
    {
        _manager.UpdateData(blocks);
        var data = _manager.Data;
        Button backToMenu = new Button() { Content = "Back to Menu" };
        backToMenu.Click += OnBackToMenuClicked;
        Button saveJson = new Button() { Content = "Save Report As Json" };
        saveJson.Click += OnJsonSaveClicked;

        dataStackPanel.Children.Add(backToMenu);
        dataStackPanel.Children.Add(saveJson);

        foreach (var block in blocks)
        {
            var listData = data[block];
            Label label = new Label() { Content = block.GetAttribute<DisplayAttribute>()?.Name ?? block.ToString() };
            DataGrid dataGrid = new DataGrid();
            dataGrid.CanUserSortColumns = false;
            //dataGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            //dataGrid.VerticalAlignment = VerticalAlignment.Stretch;
            //dataGrid.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            dataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            dataGrid.Height = 200;
            dataGrid.ItemsSource = listData;
            dataStackPanel.Children.Add(label);
            dataStackPanel.Children.Add(dataGrid);
        }
    }
}
