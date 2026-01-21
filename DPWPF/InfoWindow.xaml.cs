using DruzynaPilka;
using System;
using System.Windows;

namespace DruzynaPilkawpf
{
    public partial class InfoWindow : Window
    {
        public InfoWindow(Druzyna d)
        {
            InitializeComponent();

            if (d != null)
            {
                TxtNazwa.Text = d.Nazwa.ToUpper();

                if (d.StadionDomowy != null)
                {
                    TxtStadionNazwa.Text = d.StadionDomowy.Nazwa;
                    TxtStadionPojemnosc.Text = $"Pojemność: {d.StadionDomowy.Pojemnosc:N0} miejsc";
                }
                else
                {
                    TxtStadionNazwa.Text = "Brak stadionu";
                    TxtStadionPojemnosc.Text = "";
                }

                if (d.TrenerZespolu != null)
                {
                    TxtTrener.Text = $"{d.TrenerZespolu.Imie} {d.TrenerZespolu.Nazwisko}";
                    TxtTaktyka.Text = $"Staż: {d.TrenerZespolu.LataDoswiadczenia()} lat";
                }
                else
                {
                    TxtTrener.Text = "Wakat (Brak trenera)";
                    TxtTaktyka.Text = "";
                }

                if (d.KapitanDruzyny != null)
                {
                    TxtKapitan.Text = $"{d.KapitanDruzyny.Imie} {d.KapitanDruzyny.Nazwisko}";
                }
                else
                {
                    TxtKapitan.Text = "Nie wybrano kapitana";
                }
            }
        }

        private void BtnZamknij_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}