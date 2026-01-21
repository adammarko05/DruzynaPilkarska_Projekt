using DruzynaPilka;
using System;
using System.Linq;
using System.Windows;

namespace DruzynaPilkawpf
{
    public partial class MeczWindow : Window
    {
        private Druzyna mojKlub;

        public MeczWindow(Druzyna d)
        {
            InitializeComponent();
            this.mojKlub = d;
            ZaladujListe();
        }

        private void ZaladujListe()
        {
            if (mojKlub != null)
            {
                CmbPilkarze.ItemsSource = mojKlub.Sklad.OfType<Pilkarz>().ToList();

                if (CmbPilkarze.Items.Count > 0)
                    CmbPilkarze.SelectedIndex = 0;
            }
        }

        private void BtnGraj_Click(object sender, RoutedEventArgs e)
        {
            if (CmbPilkarze.SelectedItem is Pilkarz wybranyGracz)
            {
                if (wybranyGracz.CzyKontuzjowany)
                {
                    MessageBox.Show($"STOP! {wybranyGracz.Nazwisko} ma aktywną kontuzję!\nNie może wybiec na boisko.\n\nWejdź w 'Skład', aby zobaczyć szczegóły.",
                                    "Kontuzja zawodnika", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int stareGole = wybranyGracz.Staty.LiczbaGoli;
                int stareAsysty = wybranyGracz.Staty.LiczbaAsyst;

                bool czyZagrał = wybranyGracz.RozegrajMecz();

                if (czyZagrał == false)
                {
                    MessageBox.Show($"Ten zawodnik jest zbyt zmęczony (Stamina < 20%)!\nWyślij go do Centrum Odnowy.",
                                    "Brak sił", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int noweGole = wybranyGracz.Staty.LiczbaGoli - stareGole;
                int noweAsysty = wybranyGracz.Staty.LiczbaAsyst - stareAsysty;

                string komunikat = $"{wybranyGracz.Imie} {wybranyGracz.Nazwisko} zakończył mecz.\n";

                if (noweGole > 0) komunikat += $"⚽ GOOOL! (+{noweGole})\n";
                if (noweAsysty > 0) komunikat += $"👟 ASYSTA! (+{noweAsysty})\n";
                if (noweGole == 0 && noweAsysty == 0) komunikat += "Dobry występ, ale bez liczb.";

                TxtWynik.Text = komunikat;
                ZapiszStan();
            }
            else
            {
                MessageBox.Show("Wybierz zawodnika z listy!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnIdzDoRegeneracji_Click(object sender, RoutedEventArgs e)
        {
            RegeneracjaWindow okno = new RegeneracjaWindow(mojKlub);
            okno.ShowDialog();
            ZapiszStan();
        }

        private void BtnZamknij_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ZapiszStan()
        {
            try
            {
                string folder = AppDomain.CurrentDomain.BaseDirectory;
                string plik = System.IO.Path.Combine(folder, "druzyna.xml");
                mojKlub.ZapiszDCXML(plik);
            }
            catch { }
        }
    }
}