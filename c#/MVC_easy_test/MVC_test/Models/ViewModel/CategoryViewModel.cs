using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations; //欄位驗證要加的
using System.Linq;
using System.Web;

namespace MVC_test.Models.ViewModel
{
    public class CategoryViewModel
    {
        [Required] //欄位驗證
        [DisplayName("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "分類名稱,未輸入")]
        [MaxLength(50, ErrorMessage = "長度需小於50字")]
        [DisplayName("分類名稱")]
        public string CategoryName { get; set; }
    }
}