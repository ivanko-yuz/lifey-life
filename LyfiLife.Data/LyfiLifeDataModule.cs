using Autofac;
using LyfiLife.Core;
using LyfiLife.Core.Contracts;

namespace LyfiLife.Data;

public class LyfiLifeDataModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<HistoryDataService>().As<IHistoryDataService>();
    }
}