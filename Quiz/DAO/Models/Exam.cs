using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DAO.Models
{
    public partial class Exam
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public int? UserId { get; set; }
        public int NumberofQuestions { get; set; }
        public int? Level { get; set; }
    }
}
