using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.Input;

namespace Bee.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static

    /// <summary>
    /// 改变主题方法
    /// </summary>
    [RelayCommand]
    private void ChangeTheme()
    {
        if (Application.Current is null)
        {
            return;
        }

        // 可以通过 Application.Current 设置或修改 RequestedThemeVariant 属性
        Application.Current.RequestedThemeVariant = Application.Current.RequestedThemeVariant == ThemeVariant.Dark
            ? ThemeVariant.Default : ThemeVariant.Dark;
    }

    /*
    private RelayCommand? changeThemeCommand;

    public IRelayCommand ChangeThemeCommand => changeThemeCommand ??= new RelayCommand(ChangeTheme);
    */
}
