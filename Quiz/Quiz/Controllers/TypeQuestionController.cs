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
    public class TypeQuestionController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Danh sách loại câu hỏi";
            return View();
        }
        /// <summary>
        /// create type question url
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Tạo mới loại câu hỏi";
            return View("Edit");
        }
        /// <summary>
        /// Edit type question url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Chỉnh sửa loại câu hỏi";
            using (var _quizContext = new QuizContext())
            {
                var typequestion = _quizContext.TypeQuestion.Where(x => x.TypeQuestionId == id).FirstOrDefault();
                TypeQuestionViewModel data = new TypeQuestionViewModel();
                data.TypeQuestionId = typequestion.TypeQuestionId;
                data.NameTypeQuestion = typequestion.NameTypeQuestion;
                return View(data);
            }
        }
        /// <summary>
        /// Create type question post and edit
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(TypeQuestionViewModel typeQuestiondata)
        {
            using (var _quizContext = new QuizContext())
            {
                if (ModelState.IsValid)
                {
                    // Edit
                    if (typeQuestiondata.TypeQuestionId != null)
                    {
                        var typeQuestionEdit = _quizContext.TypeQuestion.Where(t => t.TypeQuestionId == typeQuestiondata.TypeQuestionId).FirstOrDefault();
                        typeQuestionEdit.NameTypeQuestion = typeQuestiondata.NameTypeQuestion;
                        _quizContext.Update(typeQuestionEdit);
                        TempData["Message"] = "Dữ liệu đã được cập nhật";
                    }
                    else // Create
                    {
                        TypeQuestion typequestionCreate = new TypeQuestion();
                        typequestionCreate.NameTypeQuestion = typeQuestiondata.NameTypeQuestion;
                        _quizContext.Add(typequestionCreate);
                        TempData["Message"] = "Dữ liệu đã được thêm mới";
                    }
                    _quizContext.SaveChanges();

                }
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Delete type question  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Delete(int id)
        {
            using (var _quizContext = new QuizContext())
            {
                var typeQuestionDelete = _quizContext.TypeQuestion.FirstOrDefault(t => t.TypeQuestionId == id);
                if (typeQuestionDelete != null)
                {
                    _quizContext.TypeQuestion.Remove(typeQuestionDelete);
                    _quizContext.SaveChanges();
                }

                return Json(true);
            }
        }

        /// <summary>
        /// Get list type question
        /// </summary>
        /// <param name="search">search key word</param>
        /// <returns></returns>
        public IActionResult GetListTypeQuestion(string search, int? page)
        {
            using (var _quizContext = new QuizContext())
            {
                //  get all subject
                List<TypeQuestionViewModel> lstTypeQuestionViewModel = _quizContext.TypeQuestion.Select(p => new TypeQuestionViewModel
                {
                    TypeQuestionId = p.TypeQuestionId,
                    NameTypeQuestion = p.NameTypeQuestion
                }).ToList();


                // search by name
                if (!string.IsNullOrEmpty(search))
                {
                    lstTypeQuestionViewModel = lstTypeQuestionViewModel.Where(t => t.NameTypeQuestion!.Contains(search)).ToList();
                }
                var pageIndex = page ?? 1;
                var totalPage = lstTypeQuestionViewModel.Count;
                var numberPage = Math.Ceiling((float)totalPage / Constant.pageSize);
                var start = (pageIndex - 1) * Constant.pageSize;
                lstTypeQuestionViewModel = lstTypeQuestionViewModel.Skip(start).Take(Constant.pageSize).ToList();
                PageTypeQuestion objPageTypeQuestion = new PageTypeQuestion();
                objPageTypeQuestion.ListTypeQuestion = lstTypeQuestionViewModel;
                objPageTypeQuestion.TotalPage = totalPage;
                objPageTypeQuestion.PageIndex = pageIndex;
                objPageTypeQuestion.NumberPage = (int)numberPage;
                return ViewComponent("ViewTypeQuestion", objPageTypeQuestion);
            }
        }
    }
}
