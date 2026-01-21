using DruzynaPilka;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DruzynaPilkawpf
{
    public partial class RegeneracjaWindow : Window
    {
        private Druzyna mojKlub;
        private Pilkarz wybranyPilkarz;

        public RegeneracjaWindow(Druzyna d)
        {
            InitializeComponent();
            this.mojKlub = d;
            OdswiezListe();
        }

        private void OdswiezListe()
        {
            if (mojKlub == null) return;

            var lista = mojKlub.Sklad.OfType<Pilkarz>()
                                     .OrderBy(p => p.Stamina)
                                     .ToList();

            ListaZmeczonych.ItemsSource = null;
            ListaZmeczonych.ItemsSource = lista;
        }

        private void ListaZmeczonych_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaZmeczonych.SelectedItem is Pilkarz p)
            {
                wybranyPilkarz = p;
                PanelZabiegow.IsEnabled = true;
                AktualizujPanelPrawy();
            }
        }

        private void AktualizujPanelPrawy()
        {
            if (wybranyPilkarz == null) return;

            TxtWybrany.Text = $"{wybranyPilkarz.Imie} {wybranyPilkarz.Nazwisko}";
            PasekEnergii.Value = wybranyPilkarz.Stamina;

            if (wybranyPilkarz.Stamina < 30) PasekEnergii.Foreground = Brushes.Red;
            else if (wybranyPilkarz.Stamina < 70) PasekEnergii.Foreground = Brushes.Orange;
            else PasekEnergii.Foreground = Brushes.Green;
        }

        private void BtnSauna_Click(object sender, RoutedEventArgs e)
        {
            wybranyPilkarz.RegenSauna();
            ZapiszIodswiez("Sauna zaliczona! Energia wzrosła.");
        }

        private void BtnKapiel_Click(object sender, RoutedEventArgs e)
        {
            wybranyPilkarz.RegenZimnaKapiel();
            ZapiszIodswiez("Kąpiel zaliczona! Energia wzrosła.");
        }

        private void BtnMasaz_Click(object sender, RoutedEventArgs e)
        {
            wybranyPilkarz.Masazysta();
            ZapiszIodswiez("Masaż wykonany! Energia wzrosła.");
        }

        private void ZapiszIodswiez(string komunikat)
        {
            try
            {
                string folder = AppDomain.CurrentDomain.BaseDirectory;
                string plik = System.IO.Path.Combine(folder, "druzyna.xml");
                mojKlub.ZapiszDCXML(plik);
            }
            catch { }

            MessageBox.Show(komunikat, "Regeneracja", MessageBoxButton.OK, MessageBoxImage.Information);
            OdswiezListe();
            AktualizujPanelPrawy();
        }

        private void BtnWroc_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}