using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DAO.Models
{
    public partial class QuestionInExam
    {
        public int QuestionInExamId { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
    }
}
