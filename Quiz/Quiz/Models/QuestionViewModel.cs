using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class QuestionViewModel
    {
        public int ?QuestionId { get; set; }
        public string NameQuestion { get; set; }
        public int TypeQuestionId { get; set; }
        public int Level { get; set; }
        public int SubjectId { get; set; }
    }
    public class PageQuestion
    {
        public List<QuestionViewModel> ListQuestion { get; set; }
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public int NumberPage { get; set; }
    }
}
