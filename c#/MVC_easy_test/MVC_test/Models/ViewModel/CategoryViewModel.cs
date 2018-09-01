using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_test.Models.ViewModel
{
    public class CategoryViewModel
    {
        [Required]
        [DisplayName("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("分類名稱")]
        public string CategoryName { get; set; }
    }
}