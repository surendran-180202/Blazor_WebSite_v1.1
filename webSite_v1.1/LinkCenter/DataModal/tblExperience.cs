using System.ComponentModel.DataAnnotations;

namespace LinkCenter.DataModal
{
    public class tblExperience
    {
        [Required]
        public int USERID { get; set; }
        [Required]
        public string? YEAR { get; set; }
        [Required]
        public string? LEARING { get; set; }
        [Required]
        public string? INSTITUTE { get; set; }
    }
}
