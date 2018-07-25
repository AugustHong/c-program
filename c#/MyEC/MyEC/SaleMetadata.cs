


using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEC
{
    
    /// <summary>
    /// Sale class
    /// </summary>
    [MetadataType(typeof(SaleMetadata))]
    public  partial class Sale
    {
    
    	/// <summary>
    	/// Sale Metadata class
    	/// </summary>
    	public   class SaleMetadata
    	{
    		    
    		/// <summary>
    		/// 銷售id
    		/// </summary>        
    	[DisplayName("銷售id")]
            [Required(ErrorMessage = "銷售id,未輸入")]
    		public int  sale_id { get; set; }
    
    		    
    		/// <summary>
    		/// 買方id
    		/// </summary>        
    	[DisplayName("買方id")]
            [Required(ErrorMessage = "買方id,未輸入")]
    		public int  buyer_id { get; set; }
    
    		    
    		/// <summary>
    		/// 產品id
    		/// </summary>        
    	[DisplayName("產品id")]
            [Required(ErrorMessage = "產品id,未輸入")]
    		public int  product_id { get; set; }
    
    		    
    		/// <summary>
    		/// 銷售日
    		/// </summary>        
    	[DisplayName("銷售日")]
            [Required(ErrorMessage = "銷售日,未輸入")]
    		public System.DateTime  sale_date { get; set; }
    
    		    
    		/// <summary>
    		/// 銷售價格
    		/// </summary>        
    	[DisplayName("銷售價格")]
            [Required(ErrorMessage = "銷售價格,未輸入")]
    		public decimal  sale_price { get; set; }
    
    		    
    		/// <summary>
    		/// 出貨狀態
    		/// </summary>        
    	[DisplayName("出貨狀態")]
            [Required(ErrorMessage = "出貨狀態,未輸入")]
            [MaxLength(10, ErrorMessage = "出貨狀態 輸入不可大於 10 個字元")]
    		public string  goods_status { get; set; }
    
    		    
    		/// <summary>
    		/// 購買數量
    		/// </summary>        
    	[DisplayName("購買數量")]
            [Required(ErrorMessage = "購買數量,未輸入")]
    		public int  buy_amount { get; set; }
    
    		    
    	}
    }
    
}
