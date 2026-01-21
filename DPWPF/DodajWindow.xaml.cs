using DruzynaPilka;
using System;
using System.Windows;

namespace DruzynaPilkawpf
{
    public partial class DodajWindow : Window
    {
        private Druzyna edytowanaDruzyna;

        public DodajWindow(Druzyna d)
        {
            InitializeComponent();
            this.edytowanaDruzyna = d;

            CmbPozycja.ItemsSource = Enum.GetValues(typeof(Pozycja));
            CmbPozycja.SelectedIndex = 0;

            CmbNoga.ItemsSource = Enum.GetValues(typeof(LepszaNoga));
            CmbNoga.SelectedIndex = 0;
        }

        private void BtnZapisz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtImie.Text) || string.IsNullOrWhiteSpace(TxtNazwisko.Text))
                {
                    MessageBox.Show("Musisz podać imię i nazwisko!", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(TxtWaga.Text, out double waga))
                    throw new FormatException("Waga musi być liczbą!");

                if (!double.TryParse(TxtWzrost.Text, out double wzrost))
                    throw new FormatException("Wzrost musi być liczbą!");

                if (!int.TryParse(TxtNumer.Text, out int numer))
                    throw new FormatException("Numer musi być liczbą całkowitą!");

                string imie = TxtImie.Text;
                string nazwisko = TxtNazwisko.Text;
                string dataUr = TxtDataUrodzenia.Text;

                Pozycja wybranaPozycja = (Pozycja)CmbPozycja.SelectedItem;
                LepszaNoga wybranaNoga = (LepszaNoga)CmbNoga.SelectedItem;
                double umiejetnosci = SliderUmiejetnosci.Value;

                Pilkarz nowyGracz = new Pilkarz(
                    wybranaNoga,
                    wybranaPozycja,
                    umiejetnosci,
                    numer,
                    imie,
                    nazwisko,
                    dataUr,
                    waga,
                    wzrost
                );

                if (edytowanaDruzyna != null)
                {
                    edytowanaDruzyna.DodajCzlonkaDruzyny(nowyGracz);
                    MessageBox.Show("Zawodnik dodany pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Błąd formatu danych: {ex.Message}\nSprawdź czy używasz przecinka/kropki poprawnie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (WrongDataException ex)
            {
                MessageBox.Show($"Błąd danych: {ex.Message}", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAnuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}