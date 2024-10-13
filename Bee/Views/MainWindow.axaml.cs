using Avalonia.Controls;
using Avalonia.Input;
using Bee.ViewModels;

namespace Bee.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 搜索菜单输入框键盘抬起事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SerachMenuKeyUp(object sender, KeyEventArgs e)
    {
        ((MainWindowViewModel)DataContext!).OnSerachMenu(((TextBox)sender).Text);
    }
}