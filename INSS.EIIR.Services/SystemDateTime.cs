using INSS.EIIR.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.Services
{
    public class SystemDateTime : ISystemDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
