using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceWEB.Models.Models;

public class ShoppingCart
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    [ValidateNever]
    public Product Product { get; set; }

    [Range(1, 1000, ErrorMessage = "Please enter valid value between 1 and 1000")]
    public int Count { get; set; }

    public string? ApplicationUserId { get; set; }
    [ForeignKey(nameof(ApplicationUserId))]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
    [NotMapped]
    public double Price { get; set; }
}
