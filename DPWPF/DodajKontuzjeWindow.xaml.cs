using DruzynaPilka;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DruzynaPilkawpf
{
    public partial class DodajKontuzjeWindow : Window
    {
        private Pilkarz pilkarz;
        private Druzyna druzyna;

        public DodajKontuzjeWindow(Pilkarz p, Druzyna d)
        {
            InitializeComponent();
            pilkarz = p;
            druzyna = d;
            TxtInfo.Text = $"Kontuzja dla: {p.Imie} {p.Nazwisko}";
            CmbTyp.SelectedIndex = 0;
        }

        private void BtnZatwierdz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string typ = "Inne";
                if (CmbTyp.SelectedItem is ComboBoxItem item)
                {
                    typ = item.Content.ToString();
                }

                int dni = int.Parse(TxtDni.Text);

                if (dni <= 0) throw new Exception("Liczba dni musi być dodatnia.");

                pilkarz.DodajKontuzje(typ, DateTime.Now, dni);

                string sciezka = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "druzyna.xml");
                druzyna.ZapiszDCXML(sciezka);

                MessageBox.Show($"Kontuzja dodana. Umiejętności zawodnika spadły.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd danych (wpisz poprawną liczbę dni): " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}