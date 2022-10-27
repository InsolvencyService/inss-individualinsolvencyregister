using System.Runtime.Serialization;

namespace INSS.EIIR.Models.FeedbackModels
{
    public class FeedbackFilterModel
    {
        public string Status { get; set; }
        public string Organisation { get; set; }    
        public string InsolvencyType { get; set; }
    }

    public enum ViewFilter
    {
        [EnumMember(Value = "Unviewed")]
        Unviewed,
        [EnumMember(Value = "Viewed")]
        Viewed,
        [EnumMember(Value = "All")]
        All
    }
}
