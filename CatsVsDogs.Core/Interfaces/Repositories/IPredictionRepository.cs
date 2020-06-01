using System.Collections.Generic;
using CatsVsDogs.Core.Entities;

namespace CatsVsDogs.Core.Interfaces.Repositories
{
    public interface IPredictionRepository
    {
        void Save(PredictionHistory predictionHistory);
        IEnumerable<PredictionHistory> GetAll();
    }
}