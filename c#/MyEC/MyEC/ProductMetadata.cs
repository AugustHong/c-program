


using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEC
{
    
    /// <summary>
    /// Product class
    /// </summary>
    [MetadataType(typeof(ProductMetadata))]
    public  partial class Product
    {
    
    	/// <summary>
    	/// Product Metadata class
    	/// </summary>
    	public   class ProductMetadata
    	{
    		    
    		/// <summary>
    		/// 產品id
    		/// </summary>        
    	[DisplayName("產品id")]
            [Required(ErrorMessage = "產品id,未輸入")]
    		public int  product_id { get; set; }
    
    		    
    		/// <summary>
    		/// 產品名稱
    		/// </summary>        
    	[DisplayName("產品名稱")]
            [Required(ErrorMessage = "產品名稱,未輸入")]
            [MaxLength(50, ErrorMessage = "產品名稱 輸入不可大於 50 個字元")]
    		public string  pruduct_name { get; set; }
    
    		    
    		/// <summary>
    		/// 供應商id
    		/// </summary>        
    	[DisplayName("供應商id")]
            [Required(ErrorMessage = "供應商id,未輸入")]
    		public int  vendor_id { get; set; }
    
    		    
    		/// <summary>
    		/// 類型
    		/// </summary>        
    	[DisplayName("類型")]
            [Required(ErrorMessage = "類型,未輸入")]
            [MaxLength(20, ErrorMessage = "類型 輸入不可大於 20 個字元")]
    		public string  type { get; set; }
    
    		    
    		/// <summary>
    		/// 價錢
    		/// </summary>        
    	[DisplayName("價錢")]
            [Required(ErrorMessage = "價錢,未輸入")]
    		public int  price { get; set; }
    
    		    
    		/// <summary>
    		/// 圖片路徑
    		/// </summary>        
    	[DisplayName("圖片路徑")]
            [Required(ErrorMessage = "圖片路徑,未輸入")]
            [MaxLength(100, ErrorMessage = "圖片路徑 輸入不可大於 100 個字元")]
    		public string  pic_path { get; set; }
    
    		    
    		/// <summary>
    		/// 數量
    		/// </summary>        
    	[DisplayName("數量")]
            [Required(ErrorMessage = "數量,未輸入")]
    		public int  amount { get; set; }
    
    		    
    		/// <summary>
    		/// 發佈日
    		/// </summary>        
    	[DisplayName("發佈日")]
            [Required(ErrorMessage = "發佈日,未輸入")]
    		public System.DateTime  push_date { get; set; }
    
    		    
    		/// <summary>
    		/// 產品描述
    		/// </summary>        
    	[DisplayName("產品描述")]
            [Required(ErrorMessage = "產品描述,未輸入")]
    		public string  description { get; set; }
    
    		    
    	}
    }

    public enum p_type
    {
        家電,
        生活用品,
        傢俱,
        食品,
        娛樂,
        其他
    }
    
}
