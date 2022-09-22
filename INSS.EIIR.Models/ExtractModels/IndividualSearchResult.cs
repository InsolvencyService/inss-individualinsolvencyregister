using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.Models.ExtractModels
{
    public class IndividualSearchResult
    {
        [JsonProperty("@search.score")]
        public int SearchScore { get; set; }
        public string CaseNumber { get; set; }
        public string FirstName { get; set; }
        public object MiddleName { get; set; }
        public string FamilyName { get; set; }
        public object AlternativeNames { get; set; }
        public object Gender { get; set; }
        public object Occupation { get; set; }
        public object LastKnownAddress { get; set; }
        public object LastKnownPostcode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public object CaseName { get; set; }
        public string Court { get; set; }
        public int CourtNumber { get; set; }
        public object InsolvencyType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public object CaseStatus { get; set; }
        public object CaseDescription { get; set; }
        public object TradingName { get; set; }
        public object TradingAddress { get; set; }
        public object TradingPostcode { get; set; }
        public object InsolvencyServiceOffice { get; set; }
        public object InsolvencyServiceContact { get; set; }
        public object InsolvencyServiceAddress { get; set; }
        public object InsolvencyServicePostcode { get; set; }
        public object InsolvencyServiceTelephone { get; set; }
    }
}
