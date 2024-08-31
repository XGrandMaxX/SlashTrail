using Develop._Scripts._Services.Abstractions;
using Develop._Scripts._Services.Behaviours;
using DI;
namespace Develop._Scripts._DI._Contexts
{
    public class SlashTrailProjectContext : ProjectContext
    {
        public override void RegisterDependencies()
        {
            Register<ISceneLoader,SceneLoader>(false);
            RegisterFromInstance(new PlayerInput());
        }
    }
}
