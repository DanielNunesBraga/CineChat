using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CineChat.DAL;

namespace CineChat.ViewModels
{
    public class AssignedCategoryData
    {
        public int CategoryID { get; set; }
        public string Description { get; set; }
        public bool Assigned { get; set; }
    }

}
