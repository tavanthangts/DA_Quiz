using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DAO.Models;
namespace Quiz.Models
{
    public class SubjectViewModel
    {
        public SubjectViewModel()
        {
            SubjectId = 0;

        }
        public int? SubjectId { get; set; }
        public string NameSubject { get; set; }
        public int? ParentSubjectId { get; set; }
        public List<SubjectViewModel> ListChildSubject { get; set; } = new List<SubjectViewModel>();
    }
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public List<CategoryDto> Subs { get; set; } = new List<CategoryDto>();
    }
    public class PageSubject
    {
        public List<SubjectViewModel> ListSubject { get; set; }
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public int NumberPage { get; set; }
    }
}
