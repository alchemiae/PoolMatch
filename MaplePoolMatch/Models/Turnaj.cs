using MaplePoolMatch.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace MaplePoolMatch.Models
{
    public class Turnaj
    {
        [Key]
        public int TurnajId { get; set; }
        [Required]
        [Display(Name = "Název turnaje")]
        public string NazevTurnaje { get; set; } = string.Empty;
        [Display(Name = "Datum turnaje")]
        public DateTime Datum { get; set; } = DateTime.Now.Date;

        [Display(Name = "Typ Turnaje (počet losů)")]
        public int PocetLosu { get; private set; } = 16; // Turnaj na způsob pavouka může mít 16, 32, 64, 128 hráčů

        public Hraci VitezTurnaje { get; private set; }

        private bool probihaTurnaj = false;
        private int aktualniKolo = 1;

        protected List<Hraci> seznamHracu;
        protected List<Hraci> seznamHracuSZolikem;
        protected List<Match> matches;
        protected Dictionary<int, Match> kola;


        /// <summary>
        /// Vybírá před spuštěním turnaje jeho typ (2, 4, 8, 16, 32, 64, 128 nebo 256 losů)
        /// </summary>
        /// <param name="vyber">Předává metodě počet losů</param>
        public void VyberPocetLosu(int vyber) // jak bude fungovat vyber
        {
            PocetLosu = vyber;
        }

        public Turnaj()
        {
            seznamHracu = new List<Hraci>();
            seznamHracuSZolikem = new List<Hraci>();
            matches = new List<Match>();
            kola = new Dictionary<int, Match>();
        }

        public void VyberHracuDoTurnaje(List<Hraci> vybraniHraci)
        {
            foreach (Hraci hrac in vybraniHraci)
            {
                seznamHracu.Add(hrac);
            }
        }

        /// <summary>
        /// Pokud je menší počet hráčů, než požadovaný počet (což se bude stávat většinu času), doplní se do požadovaného počtu
        /// žolíci. Žolíci se následně přiřadí k hráčům, kteří se proto v prvních zápasech turnaje stanou automaticky vítězi.
        /// </summary>
        public void DoplnZolikyDoTurnaje()
        {
            if (seznamHracu.Count < PocetLosu && seznamHracu.Count > (PocetLosu / 2))
            {
                seznamHracu.OrderByDescending(hrac => hrac.Uspesnost).ToList();
                int pocetZoliku = PocetLosu - seznamHracu.Count;
                seznamHracuSZolikem.AddRange(seznamHracu.Take(pocetZoliku)); // zde nemusím od pocetZoliku odecitat jedna, ze?
            }
        }

        public void SpustitTurnaj()
        {
            if (probihaTurnaj)
            {
                throw new Exception("Turnaj právě probíhá."); // hledat vice zpusobu vyjimek
            }
            else
            {
                if (seznamHracu.Count < PocetLosu / 2)
                {
                    int chybiHracu = (PocetLosu / 2) - seznamHracu.Count;
                    throw new Exception($"Nedostatek hráčů pro tento turnaj. Chybí {chybiHracu} hráčů. Zvažte nižší počet losů.");
                }
                else
                {
                    probihaTurnaj = true;
                    matches.Clear();

                    VytvorPavouka(seznamHracu);
                }

                probihaTurnaj = false;
            }
        }

        private void VytvorPavouka(List<Hraci> hraci) // hraci vymazani z tohoto parametrickeho seznamu hracu se vymazou celkove, nebo jen z tohoto param
        {
            int pocetMatchu = PocetLosu / 2;

            // Náhodný výběr hráčů do prvního kola, vyřazení hráčů s žolíky z losování
            if (aktualniKolo == 1)
            {
                List<Hraci> vitezove = new List<Hraci>();

                foreach (Hraci hracSeZolikem in seznamHracuSZolikem)
                {
                    Match match = new Match(hracSeZolikem, null);
                    match.Vitez = hracSeZolikem;
                    matches.Add(match);
                    hraci.Remove(hracSeZolikem);
                    vitezove.Add(hracSeZolikem);
                }

                List<int> indexyHracu = Enumerable.Range(0, hraci.Count).ToList();
                Random random = new Random();

                for (int i = 0; i < pocetMatchu - matches.Count; i++)
                {
                    int indexHrace1 = indexyHracu[random.Next(indexyHracu.Count)]; 
                    indexyHracu.Remove(indexHrace1);
                    int indexHrace2 = indexyHracu[random.Next(indexyHracu.Count)];
                    indexyHracu.Remove(indexHrace2);

                    Hraci hrac1 = hraci[indexHrace1];
                    Hraci hrac2 = hraci[indexHrace2];

                    Match match = new Match(hrac1, hrac2);
                    vitezove.Add(match.Vitez); // zde potřebuju počkat na dokončení matchů
                    kola.Add(aktualniKolo, match);
                    matches.Add(match);
                }

                aktualniKolo++;
                VytvorPavouka(vitezove);
            }
            else if (aktualniKolo > 1 && matches.Count % 2 != 1) 
            {
                List<Hraci> vitezoveAktualnihoKola = new List<Hraci>();

                for (int i = 2; i <= hraci.Count; i += 2)
                {
                    Match match = new Match(hraci[i - 2], hraci[i - 1]);
                    vitezoveAktualnihoKola.Add(match.Vitez);
                    kola.Add(aktualniKolo, match);
                    matches.Add(match);
                }

                aktualniKolo++;
                VytvorPavouka(vitezoveAktualnihoKola);
            }
            else
            {
                VitezTurnaje = matches[matches.Count - 1].Vitez;
            }
        }
    }
}
