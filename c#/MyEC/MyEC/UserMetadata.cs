


using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEC
{
    
    /// <summary>
    /// User class
    /// </summary>
    [MetadataType(typeof(UserMetadata))]
    public  partial class User
    {
    
    	/// <summary>
    	/// User Metadata class
    	/// </summary>
    	public   class UserMetadata
    	{
    		    
    		/// <summary>
    		/// 使用者id
    		/// </summary>        
    	[DisplayName("使用者id")]
            [Required(ErrorMessage = "使用者id,未輸入")]
    		public int  user_id { get; set; }
    
    		    
    		/// <summary>
    		/// 使用者類別
    		/// </summary>        
    	[DisplayName("使用者類別")]
            [Required(ErrorMessage = "使用者類別,未輸入")]
    		public int  user_type { get; set; }
    
    		    
    		/// <summary>
    		/// 名稱
    		/// </summary>        
    	[DisplayName("名稱")]
            [Required(ErrorMessage = "名稱,未輸入")]
            [MaxLength(50, ErrorMessage = "名稱 輸入不可大於 50 個字元")]
    		public string  name { get; set; }
    
    		    
    		/// <summary>
    		/// 帳號
    		/// </summary>        
    	[DisplayName("帳號")]
            [Required(ErrorMessage = "帳號,未輸入")]
            [MaxLength(30, ErrorMessage = "帳號 輸入不可大於 30 個字元")]
    		public string  account { get; set; }
    
    		    
    		/// <summary>
    		/// 密碼
    		/// </summary>        
    	[DisplayName("密碼")]
            [Required(ErrorMessage = "密碼,未輸入")]
            [MaxLength(30, ErrorMessage = "密碼 輸入不可大於 30 個字元")]
    		public string  password { get; set; }
    
    		    
    		/// <summary>
    		/// 電話
    		/// </summary>        
    	[DisplayName("電話")]
            [Required(ErrorMessage = "電話,未輸入")]
            [MaxLength(10, ErrorMessage = "電話 輸入不可大於 10 個字元")]
    		public string  phone { get; set; }
    
    		    
    		/// <summary>
    		/// 地址
    		/// </summary>        
    	[DisplayName("地址")]
            [Required(ErrorMessage = "地址,未輸入")]
            [MaxLength(50, ErrorMessage = "地址 輸入不可大於 50 個字元")]
    		public string  address { get; set; }
    
    		    
    		/// <summary>
    		/// email
    		/// </summary>        
    	[DisplayName("email")]
            [Required(ErrorMessage = "email,未輸入")]
            [MaxLength(30, ErrorMessage = "email 輸入不可大於 30 個字元")]
    		public string  email { get; set; }
    
    		    
    		/// <summary>
    		/// 註記
    		/// </summary>        
    	[DisplayName("註記")]
    		public string  remark { get; set; }
    
    		    
    	}
    }
    
}
