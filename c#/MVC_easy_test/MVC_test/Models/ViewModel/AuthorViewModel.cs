﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_test.Models.ViewModel
{
    public class AuthorViewModel
    {
        //在上面加上required 和 maxlength 和 DisplayName  這些都是欄位驗證

        [Required]
        [Display(Name ="id")] // = [DisplayName("id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required]
        [Display(Name = "密碼")]
        [MaxLength(50, ErrorMessage ="長度最大50")]//最多幾個字
        [MinLength(10, ErrorMessage = "長度最小10")] //最小幾個字
        public string Pwd { get; set; }

        [Required]
        [Display(Name = "安全等級")]
        public int ILevel { get; set; }

        [DisplayName("作者圖片")]
        public string AuthorPhote { get; set; }

        [DisplayName("作者姓名")]
        [StringLength(20, MinimumLength = 0, ErrorMessage = "長度最小0最長20")] //等同最小0，最大20(同時有stringlength和maxlength會照maxlength）
        public string AuthorName { get; set; }

        [Required]
        [DisplayName("體重")]
        //呈現的格式，第一個參數都寫false，第二個是呈現的格式（0代表自己這個值，f2就是小數點第二位），第三個是空的值的時候呈現的格式
        [DisplayFormat(ApplyFormatInEditMode =false, DataFormatString= "{0:f2}", NullDisplayText ="N/A")]
        public decimal Weight { get; set; }

        //日期的格式（一樣0代表自己的值，呈現為yyyy/MM/dd）
        //[DisplayFormat(ApplyFormatInEditMode =false, DataFormatString ="{0:yyyy/MM/dd}")]

        //{0}是代表自己  {0: xxx} 是指自己要呈現什麼樣子
    }
}

/*
以上如果要分開，可以改成2個檔案

第一個檔案內容：簡單的view model資料
    public partial class AuthorViewModel
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string Pwd { get; set; }

        public int ILevel { get; set; }

        public string AuthorPhote { get; set; }

        public string AuthorName { get; set; }

        public decimal Weight { get; set; }
    }

第二個檔案內容：把有需要驗證的寫成另外一個檔案
    
    [MetadataType(typeof(AuthorMD))]   //must be need
    public partial class AuthorViewModel{   //上面的view model name

    public class AuthorMD{

        [Required]
        [Display(Name ="id")]
        public int Id { get; set; }

        
        [Required]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required]
        [Display(Name = "密碼")]
        [MaxLength(50, ErrorMessage ="長度最大50")]
        [MinLength(10, ErrorMessage = "長度最小10")] 
        public string Pwd { get; set; }

        [Required]
        [DisplayName("作者姓名")]
        [StringLength(20, MinimumLength = 0, ErrorMessage = "長度最小0最突20")] 
        public string AuthorName { get; set; }
    }
}

或者把2個檔案合併也行：
做法：
    1.把partial拿掉
    2.寫個巢狀的class即可
    3.[MetadataType(typeof(AuthorMD))]要寫在最上面

*/


/*
MVC內建驗證規則：

上面有用的到就不介紹了

1.DataTypeAttribute（不太建議直接打這個，而是直接打其子類別，因為如果不支援html5則不會驗證）
=>[DataType(DataType.Url)]

裡面可以寫很多子類別：（建議直接打以下的屬性即可，不要用上面的）
[CreditCard]
[EmailAddress]
[EnumDataType]  =>enum型別   用法：[EnumDataType(typeof(你宣告的enum型別名稱))]  ，舉例如下：

    ----------------------------------------------------------------
    //view model寫的屬性
    [EnumDataType(typeof(enumGender))]
    public enumGender Gender{get;set;}


    public enum enumGender{Male, Female}   //宣告的列舉
    ------------------------------------------------------------------

[FileExtensions]  => [FileExtensions(Extensions = "doc, docx, pdf")]  若沒指定，預設為png, jpg, jpeg, gif
[Phone]
[Url]


2.RangeAttribute   只能輸入int 和 double
[Range]

=>range可以指定type，且設定最小最大  但這個type型別要先實作ICompareable介面（跟陣列比大小一樣，要先實作出來），
再來要寫一個type的typeconverter方法來讓這一個type轉型，才能用range來比大小

//正常
[Range(typeof(int), 100, 200)]


//下方的屬性為int或double，所以可以直接寫
[Range(1, 100, ErrorMessage=""]
public int a {get;set;}

//float
[Range(typeof(float), 1, 100]

//自訂型別
[Range(typeof(你的類型), "kg10", "kg100")]



3.CompareAttribute  確認2欄相同（例如：密碼、確認密碼）
[Compare]

    ----------------------------------------
    public string Pwd{get;set;}

    [Compare("Pwd")]
    public string confirmPwd{get;set;}
    -------------------------------------------


4.RegularExpressionAttribute 正規表達式
[RegularExpression]

=>[RegularExpression("^[A-Z | a-z][1-2][0-9]{8}$")]  //詳情正規表達式請自行去找


5.自訂驗證CustomValidationAttribute

    寫一個靜態的mathod，再引用即可。例如：
    ------------------------------------------------------------------------------------------------

    [CustomValidation(typeof(StringHelper), "MyCheck")]
    public string Name{get;set;}


    public class StringHelper{
        public static ValidationResult MyCheck(ojbect value, ValidationContext context){
            
            if(value == null){return ValidationResult.Success;}

            if(value.toString().length == 3){return ValidationResult.Success;}
            else{ return new Validation("name must be length is 3", new string[]{context.MemberName});
        }
    }
    --------------------------------------------------------------------------------------------------


6.MembershipPasswordAttribute（設定密碼強度） => 不建議用


---------------------------------------------------------------------------------------------------

7.[Remote(函式名, Controller名, HttpMathod="POST", ErrorMessage="輸入錯誤")]

    ex:[Remote("test", "Test", HttpMathod="POST", ErrorMessage="輸入錯誤")]

    當然test函式要在TestController裡寫好方法，並且回傳JSON(true) 或 JSON(false)
    public ActionResult test(){ 判斷 ...   成功：return JSON(true)   失敗：return JSON(false);}

---------------------------------------------------------------------------------------------------

8.自已寫驗證屬性 （要繼承ValidatonAttribute )

    ex: public xxxx TestValidationAttribute : ValidationAttribute{}

    ps:這個在聽時不太清楚，所以有些有錯，要再實測才知道

----------------------------------------------------------------------------------------------------

在view model 可以實作IValidationObject 後寫一個mathod 名叫 Validate() 裡面可以寫驗證
而在（前+後）全部都驗證完後，確認全部皆正確後，才會執行這段mathod，然後會回傳true 和 false。但不管結果如何都會end。（繼續執行）


*/
