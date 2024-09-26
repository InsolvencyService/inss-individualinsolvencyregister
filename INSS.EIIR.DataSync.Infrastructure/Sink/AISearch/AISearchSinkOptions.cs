using AutoMapper;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.AISearch
{
    public class AISearchSinkOptions
    {
        public string AISearchEndpoint { get; set; }
        public string AISearchKey { get; set; }
        public int BatchLimit { get; set; }
        public IMapper Mapper { get; set; }
    }
}
