using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class BookEditViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Publisher { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Condition { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string AuthorFullName { get; set; }
        public IFormFile Photo { get; set; }

        public string oldPhotoPath { get; set; }
    }
}
