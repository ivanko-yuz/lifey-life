using Autofac;
using LifeyLife.Core.Contracts.Authentication;
using LifeyLife.Core.Models;
using LifeyLife.Core.Services;
using LifeyLife.Core.Utils;

namespace LifeyLife.Core;

public class LifeyLifeCoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AccountsService>().As<IAccountsService>().InstancePerLifetimeScope();
        builder.RegisterType<PasswordHasher<User>>().As<IPasswordHasher<User>>().InstancePerLifetimeScope();
    }
}