using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DruzynaPilka
{
    public class WrongDataException : Exception
    {
        public WrongDataException() { }
        public WrongDataException(string? message) : base(message) { }
    }

    public enum Pozycja
    {
        Bramkarz,
        Obrońca,
        Pomocnik,
        Napastnik
    }

    public enum LepszaNoga
    {
        Prawa,
        Lewa,
        Obie
    }

    [DataContract]
    [KnownType(typeof(Kapitan))]
    public class Pilkarz : Osoba, IRegeneracja, IComparable<Pilkarz>, IEquatable<Pilkarz>, ICloneable
    {
        #region Pola
        private int numerKoszulki;
        private double umiejetnosci;
        private static Random random = new Random();
        #endregion

        #region Właściwości
        [DataMember]
        public Pozycja PozycjaNaBoisku { get; set; }

        [DataMember]
        public LepszaNoga LepszaNoga { get; set; }

        [DataMember]
        public Statystyki Staty { get; set; }

        [DataMember]
        public List<Kontuzja> HistoriaKontuzji { get; private set; } = new List<Kontuzja>();

        public bool CzyKontuzjowany => HistoriaKontuzji.Any(k => k.Aktywna);

        [DataMember]
        public int NumerKoszulki
        {
            get => numerKoszulki;
            set
            {
                if (value > 0 && value < 100)
                {
                    numerKoszulki = value;
                }
                else
                {
                    throw new WrongDataException("Numer może być tylko z przedziału od 1 do 99");
                }
            }
        }

        [DataMember]
        public double Umiejetnosci
        {
            get => umiejetnosci;
            set
            {
                if (value >= 0 && value <= 100)
                {
                    umiejetnosci = value;
                }
                else
                {
                    throw new WrongDataException("Umiejętności są w zakresie od 0 do 100");
                }
            }
        }
        #endregion

        #region Konstruktory
        public Pilkarz() : base()
        {
            numerKoszulki = 99;
            umiejetnosci = 50;
            this.Staty = new Statystyki();
            this.PozycjaNaBoisku = Pozycja.Pomocnik;
            this.LepszaNoga = LepszaNoga.Prawa;
        }

        public Pilkarz(LepszaNoga noga, Pozycja poz, double um, int numerKoszulki, string imie, string nazwisko, string dataUrodzenia, double waga, double wzrost)
            : base(imie, nazwisko, dataUrodzenia, waga, wzrost)
        {
            this.PozycjaNaBoisku = poz;
            this.NumerKoszulki = numerKoszulki;
            this.LepszaNoga = noga;
            this.Umiejetnosci = um;
            this.Stamina = ObliczEnergie();
            this.Staty = new Statystyki();
            HistoriaKontuzji = new List<Kontuzja>();
        }
        #endregion

        #region Metody Gry i Treningu
        public bool RozegrajMecz()
        {
            if (this.CzyKontuzjowany)
            {
                return false;
            }

            if (this.Stamina < 20)
            {
                return false;
            }

            Staty.RozegraneMecze++;
            this.Stamina -= 20;

            double szansaNaGola = 15 + (Umiejetnosci / 3.0);
            double szansaNaAsyste = 10 + (Umiejetnosci / 2.5);

            if (this.PozycjaNaBoisku == Pozycja.Napastnik) szansaNaGola += 20;
            if (this.PozycjaNaBoisku == Pozycja.Pomocnik) szansaNaAsyste += 20;

            int rzut = random.Next(0, 101);
            if (rzut < szansaNaGola) this.Staty.LiczbaGoli++;

            int rzutAsysta = random.Next(0, 101);
            if (rzutAsysta < szansaNaAsyste) this.Staty.LiczbaAsyst++;

            return true;
        }

        public override double ObliczEnergie()
        {
            double energiabase = base.ObliczEnergie();

            switch (PozycjaNaBoisku)
            {
                case Pozycja.Bramkarz: return energiabase * 0.7;
                case Pozycja.Obrońca: return energiabase * 0.8;
                case Pozycja.Pomocnik: return energiabase * 0.95;
                default: return energiabase * 0.90;
            }
        }

        public void TrenujPilkarza()
        {
            if (this.CzyKontuzjowany)
            {
                throw new WrongDataException("Kontuzjowany zawodnik ma zakaz treningów!");
            }

            if (this.Stamina <= 10) throw new WrongDataException("Za mało staminy na trening!");

            this.Stamina -= 10;
            if (this.Umiejetnosci < 100)
            {
                this.Umiejetnosci++;
            }
        }
        #endregion

        #region Metody Kontuzji
        public void DodajKontuzje(string typ, DateTime dataPoczatku, int czasPowrotuWDniach)
        {
            if (HistoriaKontuzji == null) HistoriaKontuzji = new List<Kontuzja>();

            Kontuzja k = new Kontuzja(typ, dataPoczatku, czasPowrotuWDniach);
            HistoriaKontuzji.Add(k);

            switch (typ.ToLower())
            {
                case "kolano": Umiejetnosci = Math.Max(0, Umiejetnosci - 5); break;
                case "bark":
                case "ścięgno": Umiejetnosci = Math.Max(0, Umiejetnosci - 3); break;
                case "mięsień":
                case "kostka": Umiejetnosci = Math.Max(0, Umiejetnosci - 2); break;
                default: Umiejetnosci = Math.Max(0, Umiejetnosci - 1); break;
            }
        }

        public void UpdateKontuzje()
        {
            if (HistoriaKontuzji != null)
            {
                foreach (var k in HistoriaKontuzji)
                {
                    k.UpdateStatus(DateTime.Now);
                }
            }
        }
        #endregion

        #region Metody Interfejsu IRegeneracja
        public void RegenSauna()
        {
            this.Stamina = Math.Min(100, this.Stamina + 10);
        }

        public void RegenZimnaKapiel()
        {
            this.Stamina = Math.Min(100, this.Stamina + 15);
        }

        public void Masazysta()
        {
            this.Stamina = Math.Min(100, this.Stamina + 20);
        }
        #endregion

        #region Metody Standardowe
        public override string ToString()
        {
            return $"{base.ToString()} | Poz: {PozycjaNaBoisku}, Nr: {NumerKoszulki}, Skill: {Umiejetnosci} | {Staty}";
        }

        public int CompareTo(Pilkarz? other)
        {
            if (other == null) return 1;
            int result = this.Imie.CompareTo(other.Imie);
            if (result == 0) return this.Nazwisko.CompareTo(other.Nazwisko);
            return result;
        }

        public bool Equals(Pilkarz? other)
        {
            if (other == null) return false;
            return this.Imie == other.Imie &&
                   this.Nazwisko == other.Nazwisko &&
                   this.DataUrodzenia == other.DataUrodzenia;
        }

        public override bool Equals(object? obj) => Equals(obj as Pilkarz);

        public override int GetHashCode() => HashCode.Combine(Imie, Nazwisko, DataUrodzenia);

        public object Clone()
        {
            Pilkarz kopia = (Pilkarz)this.MemberwiseClone();
            kopia.Staty = new Statystyki(this.Staty.LiczbaGoli, this.Staty.LiczbaAsyst, this.Staty.RozegraneMecze);
            return kopia;
        }
        #endregion
    }
}