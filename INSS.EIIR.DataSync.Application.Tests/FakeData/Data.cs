using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.Tests.FakeData
{
    public class ValidData
    {
        public static InsolventIndividualRegisterModel Standard() 
        {
            Random r = new Random();

            return new InsolventIndividualRegisterModel()
            {
                caseNo = r.Next(100000000, 800000000),
                individualForenames = "Carl",
                individualSurname = "McIntyre"
            };
        }
    }

    public class InvalidData
    {
        public static InsolventIndividualRegisterModel NegativeId()
        {
            return new InsolventIndividualRegisterModel()
            {
                caseNo = -1,
                individualForenames = "Carl",
                individualSurname = "McIntyre"
            };
        }

        public static InsolventIndividualRegisterModel NoData()
        {
            return new InsolventIndividualRegisterModel();
        }
    }
}
