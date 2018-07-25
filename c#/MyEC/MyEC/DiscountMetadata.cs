


using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEC
{
    
    /// <summary>
    /// Discount class
    /// </summary>
    [MetadataType(typeof(DiscountMetadata))]
    public  partial class Discount
    {
    
    	/// <summary>
    	/// Discount Metadata class
    	/// </summary>
    	public   class DiscountMetadata
    	{
    		    
    		/// <summary>
    		/// 產品id
    		/// </summary>        
    	[DisplayName("產品id")]
            [Required(ErrorMessage = "產品id,未輸入")]
    		public int  vender_id { get; set; }
    
    		    
    		/// <summary>
    		/// 折扣為（元）
    		/// </summary>        
    	[DisplayName("折扣為（%）")]
            [Required(ErrorMessage = "折扣為（%）,未輸入")]
    		public double  discount_percent { get; set; }
    
    		    
    		/// <summary>
    		/// 啟用狀態（Y=啟用中；N=未啟用）
    		/// </summary>        
    	[DisplayName("啟用狀態（Y=啟用中；N=未啟用）")]
            [Required(ErrorMessage = "啟用狀態（Y=啟用中；N=未啟用）,未輸入")]
            [MaxLength(1, ErrorMessage = "啟用狀態（Y=啟用中；N=未啟用） 輸入不可大於 1 個字元")]
    		public string  status { get; set; }
    
    		    
    	}
    }
    
}
