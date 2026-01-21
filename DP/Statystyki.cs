using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

namespace DruzynaPilka
{
    [DataContract]
    public class Statystyki
    {
        #region Pola
        int liczbaGoli;
        int liczbaAsyst;
        int rozegraneMecze;
        #endregion

        #region Właściwości
        [DataMember]
        public int LiczbaGoli
        {
            get => liczbaGoli;
            set
            {
                if (value < 0) throw new WrongDataException("Gole musza byc na plusie!!!");
                liczbaGoli = value;
            }
        }

        [DataMember]
        public int LiczbaAsyst
        {
            get => liczbaAsyst;
            set
            {
                if (value < 0) throw new WrongDataException("Asysty musza byc na plusie!!!");
                liczbaAsyst = value;
            }
        }

        [DataMember]
        public int RozegraneMecze
        {
            get => rozegraneMecze;
            set
            {
                if (value < 0) throw new WrongDataException("Rozgrane mecze musza byc na plusie!!!");
                rozegraneMecze = value;
            }
        }
        #endregion

        #region Konstruktory
        public Statystyki()
        {
            liczbaGoli = 0;
            liczbaAsyst = 0;
            rozegraneMecze = 0;
        }

        public Statystyki(int liczbaGoli, int liczbaAsyst, int rozegraneMecze)
        {
            this.LiczbaGoli = liczbaGoli;
            this.LiczbaAsyst = liczbaAsyst;
            this.RozegraneMecze = rozegraneMecze;
        }
        #endregion

        #region Metody
        public override string ToString()
        {
            return $"Gole: {liczbaGoli}, Asysty: {liczbaAsyst}, Rozegrane Mecze: {rozegraneMecze}";
        }
        #endregion
    }
}