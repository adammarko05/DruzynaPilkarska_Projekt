using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DruzynaPilka
{
    #region Wyjątki
    public class WrongStaminaException : Exception
    {
        public WrongStaminaException() { }
        public WrongStaminaException(string? message) : base(message) { }
    }

    public class WrongDaneException : Exception
    {
        public WrongDaneException() { }
        public WrongDaneException(string? message) : base(message) { }
    }
    #endregion

    [DataContract]
    [KnownType(typeof(Pilkarz))]
    [KnownType(typeof(Trener))]
    [KnownType(typeof(Kapitan))]
    public abstract class Osoba
    {
        #region Pola
        double waga;
        double wzrost;
        protected double stamina;
        #endregion

        #region Właściwości
        [DataMember]
        public string Imie { get; set; }

        [DataMember]
        public string Nazwisko { get; set; }

        [DataMember]
        public DateTime DataUrodzenia { get; set; }

        [DataMember]
        public double Waga
        {
            get => waga;
            set
            {
                if (value <= 0) throw new WrongDaneException("Dane biometryczne musza byc wieksze od 0!!!");
                waga = value;
            }
        }

        [DataMember]
        public double Wzrost
        {
            get => wzrost;
            set
            {
                if (value <= 0) throw new WrongDaneException("Dane biometryczne musza byc wieksze od 0!!!");
                wzrost = value;
            }
        }

        [DataMember]
        public double Stamina
        {
            get => stamina;
            set
            {
                if (!(value >= 0 && value <= 100)) throw new WrongStaminaException("Stamina musi zawierać sie w przedziale od 0 do 100");
                stamina = value;
            }
        }
        #endregion

        #region Konstruktory
        public Osoba()
        {
            Imie = string.Empty;
            Nazwisko = string.Empty;
            DataUrodzenia = DateTime.Now;
            waga = 1;
            wzrost = 1;
            stamina = ObliczEnergie();
        }

        public Osoba(string imie, string nazwisko, string dataUrodzenia, double waga, double wzrost)
        {
            this.Imie = imie;
            this.Nazwisko = nazwisko;
            this.Waga = waga;
            this.Wzrost = wzrost;

            if (DateTime.TryParseExact(dataUrodzenia, new[] { "yyyy-MM-dd", "dd.MM.yyyy" }, null, DateTimeStyles.None, out DateTime date))
            {
                this.DataUrodzenia = date;
            }
            else
            {
                throw new WrongDataException("Zly format daty");
            }

            stamina = ObliczEnergie();
        }
        #endregion

        #region Metody
        public int WiekTotal()
        {
            return (int)(DateTime.Now - DataUrodzenia).TotalDays / 365;
        }

        public virtual double ObliczEnergie()
        {
            double wynik = 100 - (this.waga * 0.5);

            if (wynik < 20) return 20;
            if (wynik > 100) return 100;

            return wynik;
        }

        public override string ToString()
        {
            return $"{Imie} {Nazwisko} lat: {WiekTotal()}, BMI: {Bmi():F2}, Energia:{Stamina:F2}% ";
        }

        public double Bmi()
        {
            if (wzrost == 0) return 0;
            return waga / (wzrost * wzrost);
        }
        #endregion
    }
}