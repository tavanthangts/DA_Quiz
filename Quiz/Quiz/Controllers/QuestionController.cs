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
    public class QuestionController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Danh sách câu hỏi";
            DropDownListSubject();
            DropDownListQuestion(Constant.parameter);
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
            DropDownListSubject();
            DropDownListQuestion(null);
            QuestionViewModel data = new QuestionViewModel();
            return View("Edit", data);
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
                List<AnswerViewModel> lstAnswerViewModel = new List<AnswerViewModel>();
                DropDownListSubject();
                DropDownListQuestion(null);
                var question = _quizContext.Question.Join(_quizContext.TypeQuestion, x => x.TypeQuestionId, c => c.TypeQuestionId, (x, c) => new
                {
                    QuestionId = x.QuestionId,
                    NameQuestion = x.NameQuestion,
                    TypeQuestionId = x.TypeQuestionId,
                    CodeTypeQuestion = c.CodeTypeQuestion,
                    Level = x.Level,
                    SubjectId = x.SubjectId
                }).Where(x => x.QuestionId == id).FirstOrDefault();
                if (question != null)
                {
                    var lstAnswer = _quizContext.Answer.Select(q => new AnswerViewModel
                    {
                        AnswerId = q.AnswerId,
                        Content = q.Content,
                        IsCorrect = q.IsCorrect,
                        QuestionId = q.QuestionId
                    }).Where(q => q.QuestionId == question.QuestionId);
                    lstAnswerViewModel = lstAnswer.ToList();
                } 
                QuestionViewModel data = new QuestionViewModel();
                data.QuestionId = question.QuestionId;
                data.NameQuestion = question.NameQuestion;
                data.CodeTypeQuestion = question.CodeTypeQuestion;
                data.Level = question.Level;
                data.SubjectId = question.SubjectId;
                data.ListAnswer = lstAnswerViewModel;
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
            int index = Convert.ToInt32(Request.Form["IsCorrect"]);
            if (questionData.CodeTypeQuestion == (string)Constant.radio)
            {
                questionData.ListAnswer[index].IsCorrect = true;
            }
            using (var _quizContext = new QuizContext())
            {

                // Edit
                if (questionData.QuestionId != null)
                {

                    var questionEdit = _quizContext.Question.Where(q => q.QuestionId == questionData.QuestionId).FirstOrDefault();
                    if (questionEdit != null)
                    {
                        questionEdit.NameQuestion = questionData.NameQuestion;
                        switch (questionData.CodeTypeQuestion)
                        {
                            case (string)Constant.text:
                                questionEdit.TypeQuestionId = 1;
                                break;
                            case Constant.radio:
                                questionEdit.TypeQuestionId = 2;
                                break;
                            case Constant.checkbox:
                                questionEdit.TypeQuestionId = 3;
                                break;
                        }

                        questionEdit.Level = questionData.Level;
                        questionEdit.SubjectId = questionData.SubjectId;
                        _quizContext.Update(questionEdit);
                        foreach (var item in questionData.ListAnswer)
                        {
                            Answer answerUpdate = new Answer();
                            answerUpdate.AnswerId = item.AnswerId;
                            answerUpdate.Content = item.Content;
                            answerUpdate.IsCorrect = item.IsCorrect;
                            answerUpdate.QuestionId = questionEdit.QuestionId;
                            _quizContext.Answer.Update(answerUpdate);
                        }
                        _quizContext.SaveChanges();
                        TempData["Message"] = "Dữ liệu đã được cập nhật";
                    }
                }
                else // Create
                {
                    Question questionCreate = new Question();
                    questionCreate.NameQuestion = questionData.NameQuestion;
                    switch (questionData.CodeTypeQuestion)
                    {
                        case Constant.text:
                            questionCreate.TypeQuestionId = 1;
                            break;
                        case Constant.radio:
                            questionCreate.TypeQuestionId = 2;
                            break;
                        case Constant.checkbox:
                            questionCreate.TypeQuestionId = 3;
                            break;
                    }
                    questionCreate.Level = questionData.Level;
                    questionCreate.SubjectId = questionData.SubjectId;
                    _quizContext.Add(questionCreate);
                    _quizContext.SaveChanges();
                    var questionDB = _quizContext.Question.ToList().Last();
                    if (questionDB != null)
                    {
                        List<Answer> answers = new List<Answer>();
                        foreach (var item in questionData.ListAnswer)
                        {
                            Answer answerCreate = new Answer();
                            answerCreate.Content = item.Content;
                            answerCreate.IsCorrect = item.IsCorrect;
                            answerCreate.QuestionId = questionDB.QuestionId;
                            answers.Add(answerCreate);
                        }
                        _quizContext.Answer.AddRange(answers);
                        _quizContext.SaveChanges();
                    }
                    TempData["Message"] = "Dữ liệu đã được thêm mới";
                }
                if (questionData.ArrDelete != null)
                {
                    List<string> AnswerID = questionData.ArrDelete.Split(',').ToList();

                    foreach (string item in AnswerID)
                    {
                        var AnswerDelete = _quizContext.Answer.FirstOrDefault(t => t.AnswerId == int.Parse(item));
                        if (AnswerDelete != null)
                        {
                            _quizContext.Answer.Remove(AnswerDelete);
                            _quizContext.SaveChanges();
                        }
                    }
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
                var lstQuestion = from q in _quizContext.Question
                                  join t in _quizContext.TypeQuestion
                                  on q.TypeQuestionId equals t.TypeQuestionId
                                  join s in _quizContext.Subject
                                  on q.SubjectId equals s.SubjectId
                                  where (q.TypeQuestionId == typequestion || typequestion == null) && (q.SubjectId == subject || subject == null)
                                  select new QuestionViewModel
                                  {
                                      QuestionId = q.QuestionId,
                                      NameQuestion = q.NameQuestion,
                                      TypeQuestionId = q.TypeQuestionId,
                                      Level = q.Level,
                                      SubjectId = q.SubjectId,
                                      NameTypeQuestion = t.NameTypeQuestion,
                                      NameSubject = s.NameSubject

                                  };
                // search by name
                if (!string.IsNullOrEmpty(search))
                {
                    lstQuestion = lstQuestion.Where(s => s.NameQuestion!.Contains(search));
                }
                List<QuestionViewModel> lstQuestionViewModel = lstQuestion.ToList();
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
