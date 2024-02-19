using System.ComponentModel.DataAnnotations;


namespace LinkCenter.DataModal
{
    public class tblEdnModal
    {
        [Required]
        public int? USERID { get; set; }
        [Required]
        public string? TITLE { get; set; }
        [Required]
        public string? YEAR { get; set; }
        [Required]
        public string? CLASS { get; set; }
        [Required]
        public string? INSTITUTE { get; set; }
        [Required]
        public string? PERCENTAGE { get; set; }
 
   
       
    }
}
