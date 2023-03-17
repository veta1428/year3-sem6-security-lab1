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
        var selectedOptions = GetSelectedBlocks();
        bool isOk = GenerateData(selectedOptions);
        if(!isOk)
            return;
        stackPanelReportMenu.Visibility = Visibility.Hidden;
        baseStackPanel.Children.Clear();
        baseStackPanel.Children.Add(dataStackPanel);
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
        baseStackPanel.Children.Clear();
        dataStackPanel.Visibility = Visibility.Hidden;
        dataStackPanel.Children.Clear();
        baseStackPanel.Children.Add(stackPanelReportMenu);
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


    public bool GenerateData(IEnumerable<Block> blocks)
    {
        try
        {
            _manager.UpdateData(blocks);
        }
        catch(Exception)
        {
            MessageBox.Show("Sorry, something went wrong :(");
            return false;
        }

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
            Label label = new Label() { Content = block.GetAttribute<DisplayAttribute>()?.Name ?? block.ToString(), FontSize = 30 };
            DataGrid dataGrid = new DataGrid();
            dataGrid.CanUserSortColumns = false;
            dataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            dataGrid.MaxHeight = 200;
            dataGrid.ItemsSource = listData;
            dataStackPanel.Children.Add(label);
            dataStackPanel.Children.Add(dataGrid);
        }

        return true;
    }
}
