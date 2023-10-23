using MaplePoolMatch.Data;
using System.ComponentModel.DataAnnotations;

namespace MaplePoolMatch.Models
{
    public class Match
    {
        [Key]
        public int MatchId { get; private set; }

        [Required]
        public Hraci Hrac1 { get; private set; }
        [Required(ErrorMessage = "asdfbsadf")]
        
        public Hraci Hrac2 { get; private set; }

        public int Hrac1Skore { get; private set; } = 0;
        public int Hrac2Skore { get; private set; } = 0;

        public Hraci Vitez { get; set; }

        private bool probihaZapas;

        public Match(Hraci hrac1, Hraci? hrac2)
        {
            Hrac1 = hrac1;
            Hrac2 = hrac2;
            probihaZapas = true;
        }

        //public void VyberNahodneHrace()
        //{
        //    if (seznamHracu.Count < 2)
        //    {
        //        throw new InvalidOperationException("Potřebujete alespoň 2 hráče pro zápas.");
        //    }

        //    Hrac1 = seznamHracu[random.Next(seznamHracu.Count)];
        //    seznamHracu.Remove(Hrac1);
        //    Hrac2 = seznamHracu[random.Next(seznamHracu.Count)];
        //    seznamHracu.Remove(Hrac2);
        //}

        public void Hrac1VyhralKolo()
        {
            Hrac1Skore += 1;
            Hrac1.Vyhry += 1;
            Hrac2.Prohry += 1;

            if (Hrac1Skore == 2)
            {
                Vitez = Hrac1;
                probihaZapas = false;
            }
        }

        public void Hrac2VyhralKolo()
        {
            Hrac2Skore += 1;
            Hrac2.Vyhry += 1;
            Hrac1.Prohry += 1;

            if (Hrac2Skore == 2)
            {
                Vitez = Hrac2;
                probihaZapas = false;
            }
        }

        //public bool ProbihaZapas()
        //{
        //    return probihaZapas;
        //}
    }
}
