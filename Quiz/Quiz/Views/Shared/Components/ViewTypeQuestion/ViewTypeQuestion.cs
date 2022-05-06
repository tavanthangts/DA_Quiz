using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Views.Shared.Components.ViewTypeQuestion
{
    [ViewComponent]
    public class ViewTypeQuestion:ViewComponent
    {
        public ViewTypeQuestion()
        {
        }
        public IViewComponentResult Invoke(PageTypeQuestion objTypeQuestion)
        {
            return View(objTypeQuestion);
        }
    }
}
