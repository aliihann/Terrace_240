using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrace_240.DAL.Model
{
    public class Movie
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        public int Year { get; set; }

        [Required, MaxLength(50)]
        public string Genre { get; set; }

        [Range(0, 10)]
        public double Rating { get; set; }

        // URL or relative path to poster
        public string PosterUrl { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }  

        // Comma-separated times like "19:30,21:30,00:30"
        public string ScheduleTimes { get; set; }  

        public int Price { get; set; } = 300;
    }
}
