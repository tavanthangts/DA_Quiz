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
    public class QuestionController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Danh sách câu hỏi";
            using (var _quizContext = new QuizContext())
            {
                List<Subject> lstsubject = _quizContext.Subject.ToList();
                ViewBag.LstSubject = lstsubject.Select(i => new SelectListItem
                {
                    Value = i.SubjectId.ToString(),
                    Text = i.NameSubject
                }).ToList();
                List<TypeQuestion> lsttypeQuestion = _quizContext.TypeQuestion.ToList();
                ViewBag.LstTypeQuestion = lsttypeQuestion.Select(i => new SelectListItem
                {
                    Value = i.TypeQuestionId.ToString(),
                    Text = i.NameTypeQuestion
                }).ToList();
            }
            return View();
        }
        /// <summary>
        /// Create question url
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Tạo mới câu hỏi";
            using (var _quizContext = new QuizContext())
            {
                List<Subject> lstsubject = _quizContext.Subject.ToList();
                ViewBag.LstSubject = lstsubject.Select(i => new SelectListItem
                {
                    Value = i.SubjectId.ToString(),
                    Text = i.NameSubject
                }).ToList();
                List<TypeQuestion> lsttypeQuestion = _quizContext.TypeQuestion.ToList();
                ViewBag.LstTypeQuestion = lsttypeQuestion.Select(i => new SelectListItem
                {
                    Value = i.TypeQuestionId.ToString(),
                    Text = i.NameTypeQuestion
                }).ToList();
            }
            return View("Edit");
        }
        /// <summary>
        /// Edit question url
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Chỉnh sửa câu hỏi";
            using (var _quizContext = new QuizContext())
            {
                List<Subject> lstsubject = _quizContext.Subject.ToList();
                ViewBag.LstSubject = lstsubject.Select(i => new SelectListItem
                {
                    Value = i.SubjectId.ToString(),
                    Text = i.NameSubject
                }).ToList();
                List<TypeQuestion> lsttypeQuestion = _quizContext.TypeQuestion.ToList();
                ViewBag.LstTypeQuestion = lsttypeQuestion.Select(i => new SelectListItem
                {
                    Value = i.TypeQuestionId.ToString(),
                    Text = i.NameTypeQuestion
                }).ToList();
                var question = _quizContext.Question.Where(x => x.QuestionId == id).FirstOrDefault();
                QuestionViewModel data = new QuestionViewModel();
                data.QuestionId = question.QuestionId;
                data.NameQuestion = question.NameQuestion;
                data.TypeQuestionId = question.TypeQuestionId;
                data.Level = question.Level;
                data.SubjectId = question.SubjectId;
                return View(data);
            }
        }
        /// <summary>
        /// Create question post or edit
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(QuestionViewModel questionData)
        {
            using (var _quizContext = new QuizContext())
            {
                if (ModelState.IsValid)
                {
                    // Edit
                    if (questionData.QuestionId != null)
                    {
                        var questionEdit = _quizContext.Question.Where(q => q.QuestionId == questionData.QuestionId).FirstOrDefault();
                        questionEdit.NameQuestion = questionData.NameQuestion;
                        questionEdit.TypeQuestionId = questionData.TypeQuestionId;
                        questionEdit.Level = questionData.Level;
                        questionEdit.SubjectId = questionData.SubjectId;
                        _quizContext.Update(questionEdit);
                        TempData["Message"] = "Dữ liệu đã được cập nhật";
                    }
                    else // Create
                    {
                        Question questionCreate = new Question();
                        questionCreate.NameQuestion = questionData.NameQuestion;
                        questionCreate.TypeQuestionId = questionData.TypeQuestionId;
                        questionCreate.Level = questionData.Level;
                        questionCreate.SubjectId = questionData.SubjectId;
                        _quizContext.Add(questionCreate);
                        TempData["Message"] = "Dữ liệu đã được thêm mới";
                    }
                    _quizContext.SaveChanges();

                }
            }

            return RedirectToAction("Index");
        }
        /// <summary>
        /// Delete question  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Delete(int id)
        {
            using (var _quizContext = new QuizContext())
            {
                var questionDelete = _quizContext.Question.FirstOrDefault(t => t.QuestionId == id);
                if (questionDelete != null)
                {
                    _quizContext.Question.Remove(questionDelete);
                    _quizContext.SaveChanges();
                }

                return Json(true);
            }
        }
        /// <summary>
        /// Get list question
        /// </summary>
        /// <param name="search">search key word</param>
        /// <param name="typequestion">Choose typequestion</param>
        /// <param name="subject">Choose subject</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IActionResult ListQuestion(string search, int? typequestion, int? subject, int? page)
        {
            using (var _quizContext = new QuizContext())
            {
                //  get all question
                List<QuestionViewModel> lstQuestionViewModel = _quizContext.Question.Select(q => new QuestionViewModel
                {
                    QuestionId = q.QuestionId,
                    NameQuestion = q.NameQuestion,
                    TypeQuestionId = q.TypeQuestionId,
                    Level = q.Level,
                    SubjectId = q.SubjectId
                }).Where(q => ((q.TypeQuestionId == typequestion || typequestion == null) && (q.SubjectId == subject || subject == null))).ToList();
                // search by name
                if (!string.IsNullOrEmpty(search))
                {
                    lstQuestionViewModel = lstQuestionViewModel.Where(s => s.NameQuestion!.Contains(search)).ToList();
                }
                var pageIndex = page ?? 1;
                var totalPage = lstQuestionViewModel.Count;
                var numberPage = Math.Ceiling((float)totalPage / Constant.pageSize);
                var start = (pageIndex - 1) * Constant.pageSize;
                lstQuestionViewModel = lstQuestionViewModel.Skip(start).Take(Constant.pageSize).ToList();
                PageQuestion objPageQuestion = new PageQuestion();
                objPageQuestion.ListQuestion = lstQuestionViewModel;
                objPageQuestion.TotalPage = totalPage;
                objPageQuestion.PageIndex = pageIndex;
                objPageQuestion.NumberPage = (int)numberPage;
                return ViewComponent("ViewQuestion", objPageQuestion);
            }
        }
    }
}
