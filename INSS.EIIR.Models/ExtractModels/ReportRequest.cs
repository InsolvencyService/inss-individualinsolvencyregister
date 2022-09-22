using System.Xml.Serialization;

namespace INSS.EIIR.Models.ExtractModels
{
    [XmlRoot(ElementName = "ReportRequest")]
	public class ReportRequest
	{
		[XmlElement(ElementName = "ExtractDate")]
		public string ExtractDate { get; set; }
		[XmlElement(ElementName = "CaseNoReportRequest")]
		public string CaseNoReportRequest { get; set; }
		[XmlElement(ElementName = "IndividualDetailsText")]
		public string IndividualDetailsText { get; set; }
		[XmlElement(ElementName = "IndividualDetails")]
		public IndividualDetails IndividualDetails { get; set; }
		[XmlElement(ElementName = "CaseDetailsText")]
		public string CaseDetailsText { get; set; }
		[XmlElement(ElementName = "CaseDetails")]
		public CaseDetails CaseDetails { get; set; }
		[XmlElement(ElementName = "InsolvencyPractitionerText")]
		public string InsolvencyPractitionerText { get; set; }

		[XmlElement(ElementName = "IP")]
		public IP IP { get; set; }

		[XmlElement(ElementName = "InsolvencyContactText")]
		public string InsolvencyContactText { get; set; }

		[XmlElement(ElementName = "InsolvencyContact")]
		public InsolvencyContact InsolvencyContact { get; set; }
	}
}
