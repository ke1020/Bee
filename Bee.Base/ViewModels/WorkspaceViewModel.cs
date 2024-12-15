using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bee.Base.ViewModels;

/// <summary>
/// 工作区视图模型
/// </summary>
public partial class WorkspaceViewModel : PageViewModelBase
{
    [ObservableProperty]
    private bool _isPaneOpen = false;

    [RelayCommand]
    private void PaneToggle()
    {
        IsPaneOpen = !IsPaneOpen;
    }
}