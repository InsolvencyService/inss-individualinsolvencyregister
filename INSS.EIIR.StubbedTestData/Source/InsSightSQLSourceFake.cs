using AutoMapper;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Models.IndexModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Fake.Source
{
    public class InsSightSQLSourceFake : IDataSourceAsync<InsolventIndividualRegisterModel>
    {
        private IMapper _mapper;

        public InsSightSQLSourceFake(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async IAsyncEnumerable<InsolventIndividualRegisterModel> GetInsolventIndividualRegistrationsAsync()
        {
            using (Stream r = new FileStream("searchdata.json", FileMode.Open))
            {
                await foreach (var record in JsonSerializer.DeserializeAsyncEnumerable<IndividualSearch>(r))
                {
                    if (record.InsolvencyType != InsolvencyType.DRO)
                    {
                        yield return _mapper.Map<CaseResult, InsolventIndividualRegisterModel>(_mapper.Map<IndividualSearch, CaseResult>(record.SetDynamicTestData()));
                     
                    }
                }
            }
        }

    }

    public static class IndvidualSearchExtension
    {
        /// <summary>
        /// Sets data dynamically for specific records based on the current date
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public static IndividualSearch SetDynamicTestData(this IndividualSearch rec)
        {

            var currentDateTime = DateTime.Now;

            //Record for BRU whose bankruptcy is yet to be discharged
            if (rec.CaseNumber == "704536982")
            {
                rec.InsolvencyDate = currentDateTime.AddDays(-339).ToString("dd/MM/yyyy");
                rec.CaseYear = currentDateTime.AddDays(-339).ToString("yyyy");
                rec.CaseStatus = rec.CaseStatus[0..^10] + currentDateTime.AddDays(-339).AddYears(1).ToString("dd/MM/yyyy");
                rec.RestrictionsStartDate = currentDateTime.AddDays(-32).Date;
                rec.RestrictionsEndDate = currentDateTime.AddDays(-32).AddYears(5).AddDays(-1);
            }

            //Record for BRU whose bankruptcy has been discharged
            if (rec.CaseNumber == "798652415")
            {
                rec.InsolvencyDate = currentDateTime.AddDays(-394).ToString("dd/MM/yyyy");
                rec.CaseYear = currentDateTime.AddDays(-394).ToString("yyyy");
                rec.CaseStatus = rec.CaseStatus[0..^10] + currentDateTime.AddDays(-394).AddYears(1).ToString("dd/MM/yyyy");
                rec.RestrictionsStartDate = currentDateTime.AddDays(-16).Date;
                rec.RestrictionsEndDate = currentDateTime.AddDays(-16).AddYears(5).AddDays(-1);
            }

            return rec; 
        }
    }

}
