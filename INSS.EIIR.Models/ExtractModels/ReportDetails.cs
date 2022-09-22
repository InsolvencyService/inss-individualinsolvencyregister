using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace INSS.EIIR.Models.ExtractModels
{
    [XmlRoot(ElementName = "ReportDetails")]
	public class ReportDetails
	{
		public ReportDetails()
		{
		}

		[XmlElement(ElementName = "ExtractVolumes")]
		public ExtractVolumes ExtractVolumes { get; set; }

		[XmlElement(ElementName = "Disclaimer")]
		public string Disclaimer { get; set; }

		[XmlElement(ElementName = "ReportRequest")]
		public IList<ReportRequest> ReportRequest { get; set; }
	}
}
