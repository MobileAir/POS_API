using Resolver;
using System.ComponentModel.Composition;
using Services.Interface;

namespace Services
{
    [Export(typeof(IComponent))]
    public class DependencyResolver : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            // if more services just dup this line with <IOtherServ, OtherServ> ?
            registerComponent.RegisterType<IProductServices, ProductServices>();
            registerComponent.RegisterType<ICategoryServices, CategoryServices>();
            registerComponent.RegisterType<IUserServices, UserServices>();
            registerComponent.RegisterType<IBasicAuthTokenServices, BasicAuthTokenServices>();
            registerComponent.RegisterType<ITokenAuthServices, TokenAuthServices>();
            registerComponent.RegisterType<IErrorServices, ErrorServices>();
        }
    }
}
