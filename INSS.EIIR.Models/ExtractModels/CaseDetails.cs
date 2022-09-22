using System.Xml.Serialization;

namespace INSS.EIIR.Models.ExtractModels
{
    [XmlRoot(ElementName = "CaseDetails")]
	public class CaseDetails
	{
		public CaseDetails()
		{
		}

		[XmlElement(ElementName = "CaseNoCase")]
		public string CaseNoCase { get; set; }
		[XmlElement(ElementName = "CaseName")]
		public string CaseName { get; set; }
		[XmlElement(ElementName = "Court")]
		public string Court { get; set; }
		[XmlElement(ElementName = "CaseType")]
		public string CaseType { get; set; }
		[XmlElement(ElementName = "CourtNumber")]
		public string CourtNumber { get; set; }
		[XmlElement(ElementName = "CaseYear")]
		public string CaseYear { get; set; }
		[XmlElement(ElementName = "StartDate")]
		public string StartDate { get; set; }
		[XmlElement(ElementName = "Status")]
		public string Status { get; set; }
		[XmlElement(ElementName = "CaseDescription")]
		public string CaseDescription { get; set; }
		[XmlElement(ElementName = "TradingNames")]
		public string TradingNames { get; set; }
	}
}
