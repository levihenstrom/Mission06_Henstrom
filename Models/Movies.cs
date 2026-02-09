using System.ComponentModel.DataAnnotations;

namespace Mission06_Henstrom.Models;

public class Movie
{
    // Category	    Title	Year	Director	Rating	Edited	Lent To:	Notes
    [Key]
    [Required]
    public int MovieID { get; set; }
    [Required]
    public string Category { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public string Director { get; set; }
    [Required]
    public string Rating { get; set; }
    public bool? Edited { get; set; }
    public string? Lent { get; set; }
    [StringLength(25)]
    public string? Notes { get; set; }
    
}
