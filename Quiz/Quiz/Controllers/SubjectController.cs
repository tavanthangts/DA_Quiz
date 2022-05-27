using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quiz.Helper;
using DAO.Models;
using Quiz.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

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
            SubjectViewModel data = new SubjectViewModel();
            return View("Edit", data);
        }

        /// <summary>
        /// Create Subject post and edit
        /// </summary>
        /// <param name="subjectData">data form</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(SubjectViewModel subjectData, int chooseTheme)
        {
            using (var _quizContext = new QuizContext())
            {
                if (ModelState.IsValid)
                {
                    // Edit
                    if (subjectData.SubjectId != 0)
                    {
                        var subjectEdit = _quizContext.Subject.Where(s => s.SubjectId == subjectData.SubjectId).FirstOrDefault();
                        if (subjectEdit != null)
                        {
                            subjectEdit.NameSubject = subjectData.NameSubject;
                            if (chooseTheme == (int)ChooseTheme.OriginalTheme)
                            {
                                subjectEdit.ParentSubjectId = null;
                            }
                            else
                            {
                                subjectEdit.ParentSubjectId = subjectData.ParentSubjectId;
                            }
                            _quizContext.Update(subjectEdit);
                            TempData["Message"] = "Dữ liệu đã được cập nhật";
                        }
                        
                    }
                    else // Create
                    {
                        Subject subjectCreate = new Subject();
                        subjectCreate.NameSubject = subjectData.NameSubject;
                        subjectCreate.ParentSubjectId = subjectData.ParentSubjectId;
                        if (chooseTheme == (int)ChooseTheme.OriginalTheme)
                        {
                            subjectCreate.ParentSubjectId = null;
                        }
                        else
                        {
                            subjectCreate.ParentSubjectId = subjectData.ParentSubjectId;
                        }
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
            SubjectViewModel data = new SubjectViewModel();
            using (var _quizContext = new QuizContext())
            {
                var subject = _quizContext.Subject.Where(x => x.SubjectId == id).FirstOrDefault();
                if (subject != null)
                {
                    
                    data.SubjectId = subject.SubjectId;
                    data.NameSubject = subject.NameSubject;
                    data.ParentSubjectId = subject.ParentSubjectId;
                    
                }
                
            }
            return View(data);

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
                var lstSubject = _quizContext.Subject.Select(p => new SubjectViewModel
                {
                    SubjectId = p.SubjectId,
                    NameSubject = p.NameSubject,
                    ParentSubjectId = p.ParentSubjectId
                });



                // search by name
                if (!string.IsNullOrEmpty(search))
                {
                    lstSubject = lstSubject.Where(s => s.NameSubject!.Contains(search));
                }

                List<SubjectViewModel> lstSubjectViewmodel = lstSubject.ToList();

                var pageIndex = page ?? 1;


                var totalPage = lstSubjectViewmodel.Where(s => s.ParentSubjectId == null).ToList().Count;
                var numberPage = Math.Ceiling((float)totalPage / Constant.pageSize);
                var start = (pageIndex - 1) * Constant.pageSize;
                var lstSubjectParent = lstSubject.Where(l => l.ParentSubjectId == null).Skip(start).Take(Constant.pageSize).ToList();
                var lstSubjectId = lstSubjectParent.Select(x => x.SubjectId).ToArray();
                var lstSubjectChild = lstSubject.Where(x => lstSubjectId.Contains(x.ParentSubjectId)).ToList();
                //lstSubjectChild = BindTree(lstSubjectViewmodel, lstSubjectChild);
                //var result = lstSubjectParent.Union(lstSubjectChild).ToList();
                var result = lstSubjectParent.Union(BindTree(lstSubjectViewmodel, lstSubjectChild)).ToList();
                //var result = lstSubjectParent.Union(lstSubjectChild).ToList();
                PageSubject objPageSubject = new PageSubject();
                objPageSubject.ListSubject = result;
                objPageSubject.TotalPage = totalPage;
                objPageSubject.PageIndex = pageIndex;
                objPageSubject.NumberPage = (int)numberPage;
                return ViewComponent("ViewSubject", objPageSubject);
                //return ViewComponent("ViewSubject", lstSubjectViewmodel);
            }
        }
        private List<SubjectViewModel> BindTree(List<SubjectViewModel> listAllSubject, List<SubjectViewModel> listChild)
        {
            List<SubjectViewModel> result = new List<SubjectViewModel>();
            result.AddRange(listChild);

            foreach (var item in listChild)
            {
                var grandchildTree = listAllSubject.Where(p => p.ParentSubjectId == item.SubjectId).ToList();
                if (grandchildTree.Count > 0)
                {
                    result.AddRange(grandchildTree);
                    result.AddRange(BindTree(listAllSubject, grandchildTree));

                }
            }

            return result;
        }
        public JsonResult DropDownSubject(int id)
        {
            List<CategoryDto> lstCategoryDto = new List<CategoryDto>();
            using (var _quizContext = new QuizContext())
            {
                if (id == 0)
                {
                    lstCategoryDto = _quizContext.Subject.Select(p => new CategoryDto
                    {
                        Id = p.SubjectId,
                        Title = p.NameSubject,
                        ParentId = p.ParentSubjectId
                    }).ToList();
                }
                else
                {
                    lstCategoryDto = _quizContext.Subject.Select(p => new CategoryDto
                    {
                        Id = p.SubjectId,
                        Title = p.NameSubject,
                        ParentId = p.ParentSubjectId
                    }).Where(p => p.Id != id).ToList();
                }
                var tree = BuildTrees(lstCategoryDto, null);
                return Json(tree);
            }

        }
        private List<CategoryDto> BuildTrees(List<CategoryDto> list, CategoryDto parentNode)
        {
            List<CategoryDto> result = new List<CategoryDto>();
            var nodes = list.Where(x => (parentNode == null ? x.ParentId == null : x.ParentId == parentNode.Id)).ToList();
            foreach (var node in nodes)
            {
                CategoryDto newNode = new CategoryDto
                {
                    Id = node.Id,
                    Title = node.Title
                };
                if (parentNode == null)
                {
                    result.Add(newNode);
                }
                else
                {
                    parentNode.Subs.Add(newNode);
                }
                BuildTrees(list, newNode);
            }
            return result;
        }


    }
}

