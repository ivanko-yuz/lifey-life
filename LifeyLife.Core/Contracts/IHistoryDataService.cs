using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LifeyLife.Core.Models;

namespace LifeyLife.Core.Contracts;

public interface IHistoryDataService
{
    Task<List<RandomDareHistory>> ListHistory(Guid userUuid);
    Task SaveCompletedRandomDareInHistory(Guid userUuid, Guid randomDareUuid);
    
    Task SaveSkippedRandomDareInHistory(Guid userUuid, Guid randomDareUuid);

}