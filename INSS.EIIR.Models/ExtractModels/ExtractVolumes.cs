using System.Xml.Serialization;

namespace INSS.EIIR.Models.ExtractModels
{
    [XmlRoot(ElementName = "ExtractVolumes")]
	public class ExtractVolumes
	{
		public ExtractVolumes()
		{
		}

		[XmlElement(ElementName = "TotalEntries")]
		public string TotalEntries { get; set; }

		[XmlElement(ElementName = "TotalBanks")]
		public string TotalBanks { get; set; }

		[XmlElement(ElementName = "TotalIVAs")]
		public string TotalIVAs { get; set; }

		[XmlElement(ElementName = "NewBanks")]
		public string NewBanks { get; set; }

		[XmlElement(ElementName = "TotalDros")]
		public string TotalDros { get; set; }

	}
}
