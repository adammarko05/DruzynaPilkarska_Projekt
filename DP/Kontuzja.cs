using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DruzynaPilka
{
    [DataContract]
    public class Kontuzja
    {
        #region Właściwości
        [DataMember]
        public string Typ { get; set; }

        [DataMember]
        public DateTime DataPoczatku { get; set; }

        [DataMember]
        public int LiczbaDniNieobecnosci { get; set; }

        [DataMember]
        public bool Aktywna { get; set; } = true;
        #endregion

        #region Konstruktory
        public Kontuzja() { }

        public Kontuzja(string typ, DateTime start, int dni)
        {
            Typ = typ;
            DataPoczatku = start;
            LiczbaDniNieobecnosci = dni;
            Aktywna = true;
        }
        #endregion

        #region Metody
        public void UpdateStatus(DateTime aktualnaData)
        {
            if (Aktywna && (aktualnaData - DataPoczatku).TotalDays >= LiczbaDniNieobecnosci)
            {
                Aktywna = false;
            }
        }

        public override string ToString()
        {
            string status = Aktywna ? "Aktywna" : "Zakończona";
            return $"{Typ} ({LiczbaDniNieobecnosci} dni) - {status}";
        }
        #endregion
    }
}