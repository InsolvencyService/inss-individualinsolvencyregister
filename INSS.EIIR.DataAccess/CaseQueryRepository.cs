using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.CaseModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataAccess
{
    public class CaseQueryRepository : ICaseQueryRepository
    {
        private readonly EIIRContext _context;

        public CaseQueryRepository(EIIRContext context)
        {
            _context = context;
        }

        public async Task<CaseResult> GetCaseAsync(CaseRequest searchModel)
        {

            List<CaseResult> results;

            using (_context)
            {
                var caseNo = new SqlParameter("@CaseNo", searchModel.CaseNo);
                var indivNo = new SqlParameter("@IndivNo", searchModel.IndivNo);

                _context.Database.SetCommandTimeout(600);

                results = await _context.CaseResults
                    .FromSqlRaw("exec getCaseByCaseNoIndivNo @CaseNo, @IndivNo", caseNo, indivNo).ToListAsync();

                CaseResult result = results.FirstOrDefault();

                return result;
            }
        }
    }
}