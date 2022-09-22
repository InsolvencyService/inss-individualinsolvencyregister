using System.Xml.Serialization;

namespace INSS.EIIR.Models.ExtractModels
{
    [XmlRoot(ElementName = "IndividualDetails")]
	public class IndividualDetails
	{
		public IndividualDetails()
		{
		}

		[XmlElement(ElementName = "CaseNoIndividual")]
		public string CaseNoIndividual { get; set; }
		[XmlElement(ElementName = "Title")]
		public string Title { get; set; }
		[XmlElement(ElementName = "Gender")]
		public string Gender { get; set; }
		[XmlElement(ElementName = "FirstName")]
		public string FirstName { get; set; }
		[XmlElement(ElementName = "Surname")]
		public string Surname { get; set; }
		[XmlElement(ElementName = "Occupation")]
		public string Occupation { get; set; }
		[XmlElement(ElementName = "DateofBirth")]
		public DateTime DateofBirth { get; set; }
		[XmlElement(ElementName = "LastKnownAddress")]
		public string LastKnownAddress { get; set; }
		[XmlElement(ElementName = "LastKnownPostCode")]
		public string LastKnownPostCode { get; set; }
		[XmlElement(ElementName = "OtherNames")]
		public string OtherNames { get; set; }
	}
}
