using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace api.Models
{
    [Table("Comments")]
    public class Comments
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? StockId { get; set; }
        public string AppUserId {get; set;}
        public AppUser AppUser {get; set;}

    }
}