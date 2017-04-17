using Resolver;
using System.ComponentModel.Composition;

namespace Services
{
    [Export(typeof(IComponent))]
    public class DependencyResolver : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            // if more services just dup this line with <IOtherServ, OtherServ> ?
            registerComponent.RegisterType<IProductServices, ProductServices>();
            registerComponent.RegisterType<IUserServices, UserServices>();
            registerComponent.RegisterType<IBasicAuthTokenServices, BasicAuthTokenServices>();
            registerComponent.RegisterType<ITokenAuthServices, TokenAuthServices>();
        }
    }
}
