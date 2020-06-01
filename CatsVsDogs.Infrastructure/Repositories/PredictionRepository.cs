using System.Collections.Generic;
using CatsVsDogs.Core.Entities;
using CatsVsDogs.Core.Interfaces.Repositories;
using CatsVsDogs.Infrastructure.Data;

namespace CatsVsDogs.Infrastructure.Repositories
{
    public class PredictionRepository : IPredictionRepository
    {
        private readonly AppDbContext _context;

        public PredictionRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public void Save(PredictionHistory predictionHistory)
        {
            _context.PredictionHistory.Add(predictionHistory);
            _context.SaveChanges();
        }

        public IEnumerable<PredictionHistory> GetAll()
        {
            return _context.PredictionHistory;
        }
    }
}