using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class SubjectViewModel
    {
        public int? SubjectId { get; set; }
        public string NameSubject { get; set; }
        public int? ParentSubjectId { get; set; }
    }
    public class PageSubject
    {
       public List<SubjectViewModel> ListSubject { get; set;}
       public int TotalPage { get; set; } 
       public int PageIndex { get; set; }
       public int NumberPage { get; set; }
    }
}
