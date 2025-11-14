using System.Runtime.Serialization;

namespace INSS.EIIR.Models.FeedbackModels
{
    public class FeedbackFilterModel
    {
        private int _softDeleteViewedAfterDays = 30;

        public string Status { get; set; }

        public string Organisation { get; set; }   
        
        public string InsolvencyType { get; set; }

        public int SoftDeleteViewedRecordsAfterDays
        {
            get
            { 
                return _softDeleteViewedAfterDays;
            }

            set 
            {
                _softDeleteViewedAfterDays = value;
            } 
        }
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