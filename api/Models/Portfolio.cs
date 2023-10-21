using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public string appUserId {set; get;}
        public int stockId {set; get;}
        public AppUser appUser {set; get;}
        public Stock stock {get; set;}
    }
}