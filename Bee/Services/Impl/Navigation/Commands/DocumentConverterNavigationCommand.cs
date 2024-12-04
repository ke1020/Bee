using Bee.Base.Abstractions.Navigation;
using Bee.ViewModels.Documents;

namespace Bee.Services.Impl.Navigation.Commands;

/// <summary>
/// 文档格式转换导航命令
/// </summary>
public class DocumentConverterNavigationCommand(DocumentConverterViewModel vm) : 
    NavigationCommandBase<DocumentConverterViewModel>("DocumentConverter", vm)
{

}
