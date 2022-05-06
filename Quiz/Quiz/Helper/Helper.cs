using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAO.Models;

namespace Quiz.Helper
{
    public static class Helper
    {
        public static string Paging(int currentPage, int numberPage)
        {
            string result = "";
            if (numberPage > 0)
            {
                result += "<a class=\"page-item pagination-prev disabled\" onclick=\"NextPage(" + (currentPage - 1) + ")\" href=\"javascript: void(0);\">Previous</a>";
                result += "<ul class=\"pagination listjs-pagination mb-0\">";
                for (int i = 1; i <= numberPage; i++)
                {
                    if (currentPage == i)
                    {
                        result += "<li class=\"active\"><a class=\"page\" href=\"javascript:void(0);\">" + i + "</a></li>";
                    }
                    else
                    {
                        result += "<li><a class=\"page\" onclick=\"NextPage(" + i + ")\" href=\"javascript:void(0);\">" + i + "</a></li>";
                    }
                }
                result += "</ul>";
                if (currentPage != numberPage)
                    result += "<a class=\"page-item pagination-next\" onclick=\"NextPage(" + (currentPage + 1) + ")\" href=\"javascript:void(0);\">Next</a>";
            }
            return result;
        }
        public static string Namesubject(int id)
        {
            string result = "";
            if (id != 0)
            {
                using (var _quizContext = new QuizContext())
                {
                    var subject = _quizContext.Subject.Where(x => x.SubjectId == id).FirstOrDefault();
                    result = subject.NameSubject;
                }
            }
            return result;
        }
        public static string Nametypequestion(int id)
        {
            string result = "";
            if (id != 0)
            {
                using (var _quizContext = new QuizContext())
                {
                    var typequestion = _quizContext.TypeQuestion.Where(x => x.TypeQuestionId == id).FirstOrDefault();
                    result = typequestion.NameTypeQuestion;
                }
            }
            return result;
        }
        public static string level(int level)
        {
            string result = "";
            if (level != 0)
            {
                switch (level)
                {
                    case 1:
                        result = "Dễ";
                        break;
                    case 2:
                        result = "Trung bình";
                        break;
                    case 3:
                        result = "Khó";
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
