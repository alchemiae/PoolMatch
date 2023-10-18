using System.ComponentModel.DataAnnotations;

namespace MaplePoolMatch.Models
{
    public class Hraci
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Jmeno { get; set; } = "";

        [Required]
        public string Prijmeni { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        public int Vyhry { get; set; } = 0;
        public int Prohry { get; set; } = 0;

        [Display(Name = "Rating")]
        public double Uspesnost { 
            get => ((Vyhry + Prohry) != 0) ? Math.Round((double)Vyhry / (Vyhry + Prohry) * 100, 1) : 0;
            private set { } 
        }
    }
}
