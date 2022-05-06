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
    public class SubjectController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Danh sách chủ đề";
            return View();
        }

        /// <summary>
        ///  create subject url
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Tạo mới chủ đề";
            using (var _quizContext = new QuizContext())
            {
                List<Subject> lstsubject = _quizContext.Subject.ToList();
                ViewBag.LstSubject = lstsubject.Select(i => new SelectListItem
                {
                    Value = i.SubjectId.ToString(),
                    Text = i.NameSubject
                }).ToList();
            }
            return View("Edit");
        }

        /// <summary>
        /// Create Subject post and edit
        /// </summary>
        /// <param name="subjectData">data form</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(SubjectViewModel subjectData)
        {
            using (var _quizContext = new QuizContext())
            {
                if (ModelState.IsValid)
                {
                    // Edit
                    if (subjectData.SubjectId != null)
                    {
                        var subjectEdit = _quizContext.Subject.Where(s => s.SubjectId == subjectData.SubjectId).FirstOrDefault();
                        subjectEdit.NameSubject = subjectData.NameSubject;
                        subjectEdit.ParentSubjectId = subjectData.ParentSubjectId;
                        _quizContext.Update(subjectEdit);
                        TempData["Message"] = "Dữ liệu đã được cập nhật";
                    }
                    else // Create
                    {
                        Subject subjectCreate = new Subject();
                        subjectCreate.NameSubject = subjectData.NameSubject;
                        subjectCreate.ParentSubjectId = subjectData.ParentSubjectId;
                        _quizContext.Add(subjectCreate);
                        TempData["Message"] = "Dữ liệu đã được thêm mới";
                    }
                    _quizContext.SaveChanges();

                }
            }

            return RedirectToAction("Index");

        }

        /// <summary>
        /// Edit subject url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Chỉnh sửa chủ đề";
            using (var _quizContext = new QuizContext())
            {
                List<Subject> lstsubject = _quizContext.Subject.ToList();
                ViewBag.LstSubject = lstsubject.Select(i => new SelectListItem
                {
                    Value = i.SubjectId.ToString(),
                    Text = i.NameSubject
                }).ToList();

                var subject = _quizContext.Subject.Where(x => x.SubjectId == id).FirstOrDefault();
                SubjectViewModel data = new SubjectViewModel();
                data.SubjectId = subject.SubjectId;
                data.NameSubject = subject.NameSubject;
                data.ParentSubjectId = subject.ParentSubjectId;
                return View(data);
            }
        }

        /// <summary>
        /// Delete Subject  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Delete(int id)
        {
            using (var _quizContext = new QuizContext())
            {
                var subjectDelete = _quizContext.Subject.FirstOrDefault(p => p.SubjectId == id);
                if (subjectDelete != null)
                {
                    _quizContext.Subject.Remove(subjectDelete);
                    _quizContext.SaveChanges();
                }

                return Json(true);
            }
        }

        /// <summary>
        /// Get list subject
        /// </summary>
        /// <param name="search">search key word</param>
        /// <returns></returns>
        public IActionResult ListSubject(string search, int? page)
        {
            using (var _quizContext = new QuizContext())
            {
                //  get all subject
                List<SubjectViewModel> lstSubjectViewmodel = _quizContext.Subject.Select(p => new SubjectViewModel
                {
                    SubjectId = p.SubjectId,
                    NameSubject = p.NameSubject,
                    ParentSubjectId = p.ParentSubjectId
                }).ToList();

                 

                // search by name
                if (!string.IsNullOrEmpty(search))
                {
                    lstSubjectViewmodel = lstSubjectViewmodel.Where(s => s.NameSubject!.Contains(search)).ToList();
                }
                var pageIndex = page ?? 1;
                var totalPage = lstSubjectViewmodel.Count;
                var numberPage = Math.Ceiling((float)totalPage / Constant.pageSize);
                var start = (pageIndex - 1) * Constant.pageSize;
                lstSubjectViewmodel = lstSubjectViewmodel.Skip(start).Take(Constant.pageSize).ToList();
                PageSubject objPageSubject = new PageSubject();
                objPageSubject.ListSubject = lstSubjectViewmodel;
                objPageSubject.TotalPage = totalPage;
                objPageSubject.PageIndex = pageIndex;
                objPageSubject.NumberPage = (int)numberPage;
                return ViewComponent("ViewSubject", objPageSubject);
                //return ViewComponent("ViewSubject", lstSubjectViewmodel);
            }
        }
    }
}
