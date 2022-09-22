using System.Xml.Serialization;

namespace INSS.EIIR.Models.ExtractModels
{
    [XmlRoot(ElementName = "IP")]
	public class IP
	{
		public IP()
		{
		}

		[XmlElement(ElementName = "CaseNoIP")]
		public string CaseNoIP { get; set; }
		[XmlElement(ElementName = "MainIP")]
		public string MainIP { get; set; }
		[XmlElement(ElementName = "MainIPFirm")]
		public string MainIPFirm { get; set; }
		[XmlElement(ElementName = "MainIPFirmAddress")]
		public string MainIPFirmAddress { get; set; }
		[XmlElement(ElementName = "MainIPFirmPostCode")]
		public string MainIPFirmPostCode { get; set; }
		[XmlElement(ElementName = "MainIPFirmTelephone")]
		public string MainIPFirmTelephone { get; set; }
	}
}
