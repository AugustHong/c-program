


using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial; //System.Data.Entity.Spatial.DbGeometry

namespace WebApplication1
{
    
    /// <summary>
    /// geo_test2 class
    /// </summary>
    [MetadataType(typeof(geo_test2Metadata))]
    public  partial class geo_test2
    {
    
    	/// <summary>
    	/// geo_test2 Metadata class
    	/// </summary>
    	public   class geo_test2Metadata
    	{
    		    
    		/// <summary>
    		/// id
    		/// </summary>        
    	[DisplayName("id")]
            [Required(ErrorMessage = "id,未輸入")]
    		public int  id { get; set; }
    
    		    
    		/// <summary>
    		/// graph
    		/// </summary>        
    	[DisplayName("graph")]
            [Required(ErrorMessage = "graph,未輸入")]
    		public DbGeometry  graph { get; set; }
    
    		    
    	}
    }
    
}
