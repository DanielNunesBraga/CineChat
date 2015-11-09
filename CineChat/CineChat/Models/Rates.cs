using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CineChat.Models
{
    public class Rates
    {
        public int ID { get; set; }

        public double rate { get; set; }

        public virtual ApplicationUser user { get; set; }


        public virtual Movie movie { get; set; }



    }
}