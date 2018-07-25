


using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEC
{
    
    /// <summary>
    /// Ban class
    /// </summary>
    [MetadataType(typeof(BanMetadata))]
    public  partial class Ban
    {
    
    	/// <summary>
    	/// Ban Metadata class
    	/// </summary>
    	public   class BanMetadata
    	{
    		    
    		/// <summary>
    		/// 使用者id
    		/// </summary>        
    	[DisplayName("使用者id")]
            [Required(ErrorMessage = "使用者id,未輸入")]
    		public int  user_id { get; set; }
    
    		    
    		/// <summary>
    		/// 被封原因
    		/// </summary>        
    	[DisplayName("被封原因")]
            [Required(ErrorMessage = "被封原因,未輸入")]
    		public string  ban_reason { get; set; }
    
    		    
    		/// <summary>
    		/// 封到何時
    		/// </summary>        
    	[DisplayName("封到何時")]
            [Required(ErrorMessage = "封到何時,未輸入")]
    		public System.DateTime  ban_end_data { get; set; }
    
    		    
    	}
    }
    
}
