using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DAO.Models
{
    public partial class Resultdetails
    {
        public int ResultdetailsId { get; set; }
        public int ResultId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
    }
}
