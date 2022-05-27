using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quiz.Helper;
using DAO.Models;
using Quiz.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Quiz.Controllers
{
    public class BaseController : Controller
    {
        public void DropDownListQuestion(string parameter)
        {
            using (var _quizContext = new QuizContext())
            {
                List<TypeQuestion> lstTypeQuestion = _quizContext.TypeQuestion.ToList();
                if (parameter == "Index")
                {
                    ViewBag.LstTypeQuestion = lstTypeQuestion.Select(i => new SelectListItem
                    {
                        Value = i.TypeQuestionId.ToString(),
                        Text = i.NameTypeQuestion
                    }).ToList();
                }
                else
                {

                    ViewBag.LstTypeQuestion = lstTypeQuestion.Select(i => new SelectListItem
                    {
                        Value = i.CodeTypeQuestion,
                        Text = i.NameTypeQuestion
                    }).ToList();
                }
            }
        }
        public void DropDownListSubject()
        {
            using (var _quizContext = new QuizContext())
            {
                List<Subject> lstSubject = _quizContext.Subject.ToList();
                ViewBag.LstSubject = lstSubject.Select(i => new SelectListItem
                {
                    Value = i.SubjectId.ToString(),
                    Text = i.NameSubject
                }).ToList();
            }

        }
    }
}
