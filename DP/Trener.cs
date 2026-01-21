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
    public enum Taktyka
    {
        Ofensywna,
        Rownowaga,
        Defensywna,
        Autobus
    }

    [DataContract]
    public class Trener : Osoba
    {
        #region Właściwości
        [DataMember]
        public Taktyka UlubionaTaktyka { get; set; }

        [DataMember]
        public DateTime DataObjeciaStanowiska { get; private set; }
        #endregion

        #region Konstruktory
        public Trener() : base()
        {
            UlubionaTaktyka = Taktyka.Rownowaga;
            DataObjeciaStanowiska = DateTime.Now;
        }

        public Trener(Taktyka taktyka, string dataObjeciaStanowiska, string imie, string nazwisko, string dataUrodzenia, double waga, double wzrost)
            : base(imie, nazwisko, dataUrodzenia, waga, wzrost)
        {
            this.UlubionaTaktyka = taktyka;

            if (DateTime.TryParseExact(dataObjeciaStanowiska, new[] { "yyyy-MM-dd", "dd.MM.yyyy" }, null, DateTimeStyles.None, out DateTime date))
            {
                this.DataObjeciaStanowiska = date;
            }
            else
            {
                throw new WrongDataException("Zly format daty");
            }
        }
        #endregion

        #region Metody
        public void ZmotywujPilkarza(Pilkarz p)
        {
            if (p == null) return;
            p.Stamina = Math.Min(100, p.Stamina + 5);
        }

        public int LataDoswiadczenia()
        {
            return (int)(DateTime.Now - DataObjeciaStanowiska).TotalDays / 365;
        }

        public override string ToString()
        {
            return base.ToString() + $" Trenerem jest od: {LataDoswiadczenia()} lat, ULUBIONA TAKTYKA: {UlubionaTaktyka}";
        }
        #endregion
    }
}