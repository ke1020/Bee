using Bee.Base.Abstractions.Navigation;
using Bee.ViewModels.Images;

namespace Bee.Services.Impl.Navigation.Commands;

/// <summary>
/// 海报生成器导航命令
/// </summary>
public class PosterGeneratorNavigationCommand(PosterGeneratorViewModel vm) : 
    NavigationCommandBase<PosterGeneratorViewModel>("PosterGenerator", vm)
{

}
