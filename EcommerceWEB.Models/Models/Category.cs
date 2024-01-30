using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcommerceWEB.Models.Models;

public class Category
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(20)]
    [DisplayName("Category Name")]
    public string CategoryName { get; set; }

    [DisplayName("Display Order")]
    [Range(1,100, ErrorMessage = "Display Order must be between 1 and 100")]
    public int DisplayOrder { get; set; }
}
