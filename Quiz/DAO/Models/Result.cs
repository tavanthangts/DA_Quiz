using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DAO.Models
{
    public partial class Result
    {
        public int ResultId { get; set; }
        public int? UserId { get; set; }
        public int? ExamId { get; set; }
        public DateTime TestDay { get; set; }
        public TimeSpan ExamTime { get; set; }
        public int? Testscore { get; set; }
        public bool? Status { get; set; }
    }
}
