using System.Data.Common;
using Autofac;

namespace LyfiLife.Data;
using Npgsql;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder AddPostgresInfrastructure(this ContainerBuilder builder, Func<string> getConnectionString)
    {
        builder.Register(_ => NpgsqlFactory.Instance).As<DbProviderFactory>().SingleInstance();
        builder.Register(_ => new NpgsqlConnectionStringBuilder(getConnectionString())).As<DbConnectionStringBuilder>().SingleInstance();
        builder.RegisterType<DbAdapter>().As<IDbAdapter>().InstancePerLifetimeScope();
        return builder;
    }
}