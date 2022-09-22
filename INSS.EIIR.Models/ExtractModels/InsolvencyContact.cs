using System.Xml.Serialization;

namespace INSS.EIIR.Models.ExtractModels
{
    [XmlRoot(ElementName = "InsolvencyContact")]
	public class InsolvencyContact
	{
		public InsolvencyContact()
		{
		}

		[XmlElement(ElementName = "CaseNoContact")]
		public string CaseNoContact { get; set; }
		[XmlElement(ElementName = "InsolvencyServiceOffice")]
		public string InsolvencyServiceOffice { get; set; }
		[XmlElement(ElementName = "Contact")]
		public string Contact { get; set; }
		[XmlElement(ElementName = "ContactAddress")]
		public string ContactAddress { get; set; }
		[XmlElement(ElementName = "ContactPostCode")]
		public string ContactPostCode { get; set; }
		[XmlElement(ElementName = "ContactTelephone")]
		public string ContactTelephone { get; set; }
	}
}
