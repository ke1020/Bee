using Bee.Base.Abstractions.Navigation;
using Bee.Base.Models.Navigation;
using Bee.ViewModels.Documents;

namespace Bee.Services.Impl.Navigation.Commands;

/// <summary>
/// 文档格式转换导航命令
/// </summary>
public class DocumentConverterNavigationCommand : INavigationCommand
{
    public string Key => "DocumentConverter";

    public void Execute(NavigationCommandContext context)
    {
        context.Navigator?.SetCurrentPage(new DocumentConverterViewModel());
    }
}
