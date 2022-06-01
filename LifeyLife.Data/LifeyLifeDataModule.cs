using System;
using Autofac;
using LifeyLife.Core.Contracts;
using LifeyLife.Data.DataServices;
using Microsoft.Extensions.Configuration;

namespace LifeyLife.Data;

public class LifeyLifeDataModule : Module
{
    private readonly IConfiguration _configuration;

    public LifeyLifeDataModule(IConfiguration configuration)
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