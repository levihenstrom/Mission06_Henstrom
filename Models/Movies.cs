using System.ComponentModel.DataAnnotations;

namespace Mission06_Henstrom.Models;

public class Movie
{
    [Key]
    [Required]
    public int MovieId { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.ForeignKey("CategoryId")]
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Range(1888, int.MaxValue, ErrorMessage = "Year must be 1888 or later.")]
    public int Year { get; set; }

    public string? Director { get; set; }

    public string? Rating { get; set; }

    [Required]
    public bool Edited { get; set; }

    public string? LentTo { get; set; }

    [Required]
    public bool CopiedToPlex { get; set; }

    [StringLength(25)]
    public string? Notes { get; set; }
}
