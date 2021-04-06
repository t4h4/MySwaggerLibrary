using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MySwaggerLibrary.Models
{
    /// <summary>
    /// Urun nesnesi
    /// </summary>
    public partial class Product
    {
        /// <summary>
        /// urun id'si
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// urun ismi
        /// </summary>
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public DateTime? Date { get; set; }
        public string Category { get; set; }
    }
}
