


using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{

    /// <summary>
    /// employee class
    /// </summary>
    [MetadataType(typeof(employeeMetadata))]
    public partial class employee
    {

        /// <summary>
        /// employee Metadata class
        /// </summary>
        public class employeeMetadata
        {

            /// <summary>
            /// 編號
            /// </summary>        
            [DisplayName("編號")]
            [Required(ErrorMessage = "編號,未輸入")]
            public int id { get; set; }


            /// <summary>
            /// 姓名
            /// </summary>        
            [DisplayName("姓名")]
            [Required(ErrorMessage = "姓名,未輸入")]
            [MaxLength(30, ErrorMessage = "姓名 輸入不可大於 30 個字元")]
            public string name { get; set; }


            /// <summary>
            /// 生日
            /// </summary>        
            [DisplayName("生日")]
            [Required(ErrorMessage = "生日,未輸入")]
            public System.DateTime birthday { get; set; }


            /// <summary>
            /// 身高
            /// </summary>        
            [DisplayName("身高")]
            [Required(ErrorMessage = "身高,未輸入")]
            public double height { get; set; }


            /// <summary>
            /// 體重
            /// </summary>        
            [DisplayName("體重")]
            [Required(ErrorMessage = "體重,未輸入")]
            public double weight { get; set; }


            /// <summary>
            /// 薪水
            /// </summary>        
            [DisplayName("薪水")]
            [Required(ErrorMessage = "薪水,未輸入")]
            public decimal salary { get; set; }


            /// <summary>
            /// 部門
            /// </summary>        
            [DisplayName("部門")]
            [Required(ErrorMessage = "部門,未輸入")]
            [MaxLength(30, ErrorMessage = "部門 輸入不可大於 30 個字元")]
            public string department { get; set; }


            /// <summary>
            /// 信箱
            /// </summary>        
            [DisplayName("信箱")]
            [MaxLength(30, ErrorMessage = "信箱 輸入不可大於 30 個字元")]
            public string mail { get; set; }


        }
    }

}
