using Autofac;
using LyfiLife.Core.Contracts;
using LyfiLife.Data.DataServices;
using Microsoft.Extensions.Configuration;

namespace LyfiLife.Data;

public class LyfiLifeDataModule : Module
{
    private readonly IConfiguration _configuration;

    public LyfiLifeDataModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.AddPostgresInfrastructure(() =>
            _configuration.GetConnectionString("aggregatordb")
            ?? (_configuration["DevConnectionString"] ??
                throw new InvalidOperationException("Connection string not defined.")));
        builder.RegisterType<HistoryDataService>().As<IHistoryDataService>();
        builder.RegisterType<RandomDareDataService>().As<IRandomDareDataService>();
    }
}