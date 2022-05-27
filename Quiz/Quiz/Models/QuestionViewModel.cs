using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class QuestionViewModel
    {
        public QuestionViewModel()
        {
            ListAnswer = new List<AnswerViewModel>();
        }

        public int? QuestionId { get; set; }
        public string NameQuestion { get; set; }
        public int TypeQuestionId { get; set; }
        public string CodeTypeQuestion { get; set; }
        public int Level { get; set; }
        public int SubjectId { get; set; }
        public string NameTypeQuestion { get; set; }
        public string NameSubject { get; set; }
        public List<AnswerViewModel> ListAnswer { get; set; }
        public string ArrDelete { get; set; }
    }
    public class PageQuestion
    {
        public List<QuestionViewModel> ListQuestion { get; set; }
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public int NumberPage { get; set; }
    }
}
