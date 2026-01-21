using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DruzynaPilka
{
    #region Komparatory
    public class GoleComparer : IComparer<Pilkarz>
    {
        public int Compare(Pilkarz? x, Pilkarz? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int result = y.Staty.LiczbaGoli.CompareTo(x.Staty.LiczbaGoli);

            if (result == 0)
            {
                return x.Nazwisko.CompareTo(y.Nazwisko);
            }
            return result;
        }
    }

    public class AsystyComparer : IComparer<Pilkarz>
    {
        public int Compare(Pilkarz? x, Pilkarz? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int result = y.Staty.LiczbaAsyst.CompareTo(x.Staty.LiczbaAsyst);

            if (result == 0)
            {
                return x.Nazwisko.CompareTo(y.Nazwisko);
            }
            return result;
        }
    }
    #endregion

    [DataContract]
    public class Druzyna
    {
        #region Właściwości
        [DataMember]
        public string Nazwa { get; set; }

        [DataMember]
        public Kapitan KapitanDruzyny { get; set; }

        [DataMember]
        public Trener TrenerZespolu { get; set; }

        [DataMember]
        public Stadion StadionDomowy { get; set; }

        [DataMember]
        public List<Pilkarz> Sklad { get; private set; } = new List<Pilkarz>();
        #endregion

        #region Konstruktory
        public Druzyna()
        {
            Nazwa = "Brak nazwy";
            KapitanDruzyny = new Kapitan();
            TrenerZespolu = new Trener();
            StadionDomowy = new Stadion();
        }

        public Druzyna(string nazwa, Kapitan kapitan, Trener trener, Stadion stadion)
        {
            this.Nazwa = nazwa;
            this.KapitanDruzyny = kapitan;
            this.TrenerZespolu = trener;
            this.StadionDomowy = stadion;

            DodajCzlonkaDruzyny(kapitan);
        }
        #endregion

        #region Zarządzanie Składem
        public void DodajCzlonkaDruzyny(Pilkarz p)
        {
            if (p == null) return;

            if (Sklad.Contains(p))
            {
                Console.WriteLine("Piłkarz już istnieje w składzie.");
                return;
            }

            bool czyNumerZajety = Sklad.Any(zawodnik => zawodnik.NumerKoszulki == p.NumerKoszulki);
            if (!czyNumerZajety)
            {
                Sklad.Add(p);
            }
            else
            {
                Console.WriteLine($"BŁĄD: Numer {p.NumerKoszulki} jest już zajęty!");
            }
        }

        public void UsunCzlonkaDruzyny(Pilkarz p)
        {
            if (p == null) return;

            if (Sklad.Contains(p))
            {
                Sklad.Remove(p);
            }
            else
            {
                Console.WriteLine("Nie ma takiego zawodnika.");
            }
        }

        public int LiczbaPilkarzy()
        {
            return Sklad.Count;
        }

        public void SortowaniePilkarzy()
        {
            Sklad.Sort();
        }

        public bool SprawdzanieTozsamowsci(Pilkarz p)
        {
            return Sklad.Contains(p);
        }
        #endregion

        #region Serializacja (Zapis/Odczyt)
        public void ZapiszDCXML(string fname)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(Druzyna));
            using XmlTextWriter writer = new XmlTextWriter(fname, Encoding.UTF8) { Formatting = Formatting.Indented };
            dcs.WriteObject(writer, this);
        }

        public static Druzyna? OdczytDCXML(string fname)
        {
            if (!File.Exists(fname)) return null;

            DataContractSerializer dcs = new DataContractSerializer(typeof(Druzyna));
            using XmlTextReader reader = new XmlTextReader(fname);
            return (Druzyna?)dcs.ReadObject(reader);
        }
        #endregion

        #region Metody Standardowe
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"--- {Nazwa} ---");

            if (TrenerZespolu != null)
            {
                sb.AppendLine($"Trener: {TrenerZespolu.Imie} {TrenerZespolu.Nazwisko} (Staż: {TrenerZespolu.LataDoswiadczenia()} lat)");
            }
            else
            {
                sb.AppendLine("Trenera brak!");
            }

            if (KapitanDruzyny != null)
            {
                sb.AppendLine($"Kapitan: {KapitanDruzyny.Imie} {KapitanDruzyny.Nazwisko} (Staż: {KapitanDruzyny.DoswiadczenieKaptiana()} dni)");
            }
            else
            {
                sb.AppendLine("Kapitan: BRAK");
            }

            if (StadionDomowy != null)
            {
                sb.AppendLine($"{StadionDomowy.ToString()}");
            }
            else
            {
                sb.AppendLine("Stadion w budowie!");
            }

            sb.AppendLine($"Liczba członków drużyny: {LiczbaPilkarzy()}");
            sb.AppendLine($"Członkowie zespołu:");

            foreach (var k in Sklad.OrderBy(p => p.NumerKoszulki))
            {
                sb.AppendLine(k.ToString());
            }

            return sb.ToString();
        }
        #endregion
    }
}