﻿namespace INSS.EIIR.Data.Models
{
    public class CiCaseFeedback
    {
        public int FeedbackId { get; set; }
        public DateTime FeedbackDate { get; set; }
        public string Message { get; set; } = null!;
        public string ReporterFullname { get; set; } = null!;
        public string ReporterEmailAddress { get; set; } = null!;
        public string ReporterOrganisation { get; set; } = null!;
        public bool Viewed { get; set; }    
        public DateTime? ViewedDate { get; set; }
        public int CaseId { get; set; }
    }
}
