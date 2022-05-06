using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Views.Shared.Components.ViewQuestion
{
    [ViewComponent]
    public class ViewQuestion:ViewComponent
    {
        public ViewQuestion()
        {
        }
        public IViewComponentResult Invoke(PageQuestion objPageQuestion)
        {
            return View(objPageQuestion);
        }
    }
}
