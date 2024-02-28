using AutoMapper;
using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.FeedbackModels;
using Microsoft.EntityFrameworkCore;

namespace INSS.EIIR.DataAccess
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly EIIRContext _context;
        private readonly IMapper _mapper;

        public FeedbackRepository(
            EIIRContext eiirContext,
            IMapper mapper)
        {
            _context = eiirContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CaseFeedback>> GetFeedbackAsync()
        {
            List<CaseFeedback> caseFeedback = new();
            var results = await (from cf in _context.CiCaseFeedback
                                 orderby cf.FeedbackDate descending
                                 join c in _context.CiCases
                                    on cf.CaseId equals c.CaseNo
                                 join ci in _context.CiIndividuals
                                    on cf.CaseId equals ci.CaseNo
                                 select new { cf, c.InsolvencyType, c.CaseName, c.InsolvencyDate, ci.IndivNo }).ToListAsync();

            results.ToList().ForEach(s =>
            {
                var caseFB = _mapper.Map<CiCaseFeedback, CaseFeedback>(s.cf);
                caseFB.InsolvencyType = s.InsolvencyType;
                caseFB.CaseName = s.CaseName;
                caseFB.OrderDate = s.InsolvencyDate;
                caseFB.IndivNo = s.IndivNo;
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
    }
}
