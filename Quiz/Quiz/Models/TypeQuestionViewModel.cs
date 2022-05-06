using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class TypeQuestionViewModel
    {
        public int? TypeQuestionId { get; set; }
        public string NameTypeQuestion { get; set; }
    }
    public class PageTypeQuestion
    {
        public List<TypeQuestionViewModel> ListTypeQuestion { get; set; }
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public int NumberPage { get; set; }
    }
}
