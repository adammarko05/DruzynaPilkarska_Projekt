using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DruzynaPilka
{
    [DataContract]
    public class Stadion
    {
        #region Pola
        private string nazwa;
        private int pojemnosc;
        private double cenaBiletu;
        #endregion

        #region Właściwości
        [DataMember]
        public string Nazwa
        {
            get => nazwa;
            set => nazwa = value;
        }

        [DataMember]
        public int Pojemnosc
        {
            get => pojemnosc;
            set
            {
                if (value < 0) throw new WrongDataException("Pojemność nie może być mniejsza od 0");
                pojemnosc = value;
            }
        }

        [DataMember]
        public double CenaBiletu
        {
            get => cenaBiletu;
            set
            {
                if (value < 0) throw new WrongDataException("Cena nie może być mniejsza od 0");
                cenaBiletu = value;
            }
        }
        #endregion

        #region Konstruktory
        public Stadion()
        {
            nazwa = string.Empty;
            pojemnosc = 0;
            cenaBiletu = 0;
        }

        public Stadion(string nazwa, int pojemnosc, double cenaBiletu)
        {
            this.Nazwa = nazwa;
            this.Pojemnosc = pojemnosc;
            this.CenaBiletu = cenaBiletu;
        }
        #endregion

        #region Metody
        public double ObliczZysk(double procentZapelnienia)
        {
            if (procentZapelnienia < 0 || procentZapelnienia > 1)
                throw new WrongDataException("Zapełnienie musi być od 0 do 1");

            int widzowie = (int)(pojemnosc * procentZapelnienia);
            return widzowie * cenaBiletu;
        }

        public override string ToString()
        {
            return $"Stadion {Nazwa}, pojemność: {Pojemnosc}, cena wejścia: {CenaBiletu}";
        }
        #endregion
    }
}