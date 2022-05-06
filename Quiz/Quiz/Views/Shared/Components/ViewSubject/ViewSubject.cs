using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Quiz.Views.Shared.Components.ViewSubject
{
    [ViewComponent]
    public class ViewSubject : ViewComponent
    {

        public ViewSubject()
        {
        }

        //public IViewComponentResult Invoke(IEnumerable<SubjectViewModel> subject)
        //{
        //    return View(subject);
        //}
        public IViewComponentResult Invoke(PageSubject opbjPageSubject)
        {
            return View(opbjPageSubject);
        }

    }
}
