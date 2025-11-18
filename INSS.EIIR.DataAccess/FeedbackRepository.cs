using AutoMapper;
using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.FeedbackModels;
using Microsoft.EntityFrameworkCore;

namespace INSS.EIIR.DataAccess
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly EIIRContext _context;
        private readonly IMapper _mapper;
        private readonly TimeProvider _systemDateTime;

        public FeedbackRepository(
            EIIRContext eiirContext,
            IMapper mapper, 
            TimeProvider systemDateTime)
        {
            _context = eiirContext;
            _mapper = mapper;
            _systemDateTime = systemDateTime;
        }

        public async Task<IEnumerable<CaseFeedback>> GetFeedbackAsync()
        {
            List<CaseFeedback> caseFeedback = new();
            var results = await (from cf in _context.CiCaseFeedback
                                 orderby cf.FeedbackDate descending
                                 select new { cf }).ToListAsync();

            results.ToList().ForEach(s =>
            {
                var caseFB = _mapper.Map<CiCaseFeedback, CaseFeedback>(s.cf);
                caseFeedback.Add(caseFB); 
            });

            return caseFeedback;
        }

        public void CreateFeedback(CreateCaseFeedback feedback)
        {
            var addFeedback = _mapper.Map<CreateCaseFeedback, CiCaseFeedback>(feedback);
            _context.CiCaseFeedback.Add(addFeedback);   
            _context.SaveChanges();
        }

        public bool UpdateFeedbackStatus(int feedbackId, bool status)
        {
            DateTime? viewedDate = status == true ? DateTime.UtcNow : null;
            var updFeedback = _context.CiCaseFeedback.Where(x => x.FeedbackId == feedbackId).FirstOrDefault();
            if (updFeedback == null)
            {
                return false;
            }

            updFeedback.Viewed = status;
            updFeedback.ViewedDate = viewedDate;
            _context.CiCaseFeedback.Update(updFeedback);
            _context.SaveChanges();
            return true;           
        }

        public int DeleteViewedRecords(int hardDeleteMonths)
        {
            var hardDeleteCutOff = _systemDateTime.GetUtcNow().ToLocalTime().DateTime.AddMonths(-1 * hardDeleteMonths).Date;
            var toDelete = _context.CiCaseFeedback
                            .Where(x => x.Viewed == true && x.ViewedDate <= hardDeleteCutOff)
                            .ToList();
            _context.CiCaseFeedback.RemoveRange(toDelete);
            int deletedCount = _context.SaveChanges();
            return deletedCount;
        }   
    }
}
