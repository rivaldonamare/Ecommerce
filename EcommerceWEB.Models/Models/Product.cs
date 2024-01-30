﻿using System.ComponentModel.DataAnnotations;

namespace EcommerceWEB.Models.Models;

public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(50)]
    public string Title { get; set; }
    public string Description { get; set; }

    [Required]
    public string ISBN { get; set; }
    [Required]
    public string Author { get; set; }

    [Required]
    [Display(Name = "List Price")]
    [Range(1, 1000000)]
    public double ListPrice { get; set; }

    [Required]
    [Display(Name = "Price for 1-50")]
    [Range(1, 1000000)]
    public double Price { get; set; }

    [Required]
    [Display(Name = "Price for 50+")]
    [Range(1, 1000000)]
    public double Price50 { get; set; }

    [Required]
    [Display(Name = "Price for 100+")]
    [Range(1, 1000000)]
    public double Price100 { get; set; }
}
