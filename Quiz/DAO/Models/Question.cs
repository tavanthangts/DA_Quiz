using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DAO.Models
{
    public partial class Question
    {
        public int QuestionId { get; set; }
        public string NameQuestion { get; set; }
        public int TypeQuestionId { get; set; }
        public int Level { get; set; }
        public int SubjectId { get; set; }
    }
}
