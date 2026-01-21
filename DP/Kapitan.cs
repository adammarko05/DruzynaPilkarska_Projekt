using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DruzynaPilka
{
    [DataContract]
    public class Kapitan : Pilkarz
    {
        #region Właściwości
        [DataMember]
        public DateTime OtrzymanieOpaski { get; set; }
        #endregion

        #region Konstruktory
        public Kapitan()
        {
            OtrzymanieOpaski = DateTime.Now;
        }

        public Kapitan(string otrzymanie, LepszaNoga noga, Pozycja poz, double um, int numerKoszulki, string imie, string nazwisko, string dataUrodzenia, double waga, double wzrost)
            : base(noga, poz, um, numerKoszulki, imie, nazwisko, dataUrodzenia, waga, wzrost)
        {
            if (DateTime.TryParseExact(otrzymanie, new[] { "yyyy-MM-dd", "dd.MM.yyyy" }, null, DateTimeStyles.None, out DateTime o))
            {
                this.OtrzymanieOpaski = o;
            }
            else
            {
                this.OtrzymanieOpaski = DateTime.Now;
            }
        }
        #endregion

        #region Metody
        public int DoswiadczenieKaptiana()
        {
            return (int)(DateTime.Now - this.OtrzymanieOpaski).TotalDays;
        }

        public override double ObliczEnergie()
        {
            double wynik = base.ObliczEnergie() * 1.1;

            if (wynik > 100) return 100;
            return wynik;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Doświadczenie kapitańskie: {DoswiadczenieKaptiana()} dni";
        }
        #endregion
    }
}