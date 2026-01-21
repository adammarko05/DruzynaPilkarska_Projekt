using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DruzynaPilka;

namespace DruzynaPilkawpf
{
    public partial class SkladWindow : Window
    {
        private Druzyna wyswietlanaDruzyna;

        public SkladWindow(Druzyna d)
        {
            InitializeComponent();
            this.wyswietlanaDruzyna = d;
            OdswiezWidok();
        }

        private void OdswiezWidok()
        {
            if (wyswietlanaDruzyna != null)
            {
                ListaGraczy.ItemsSource = null;
                ListaGraczy.ItemsSource = wyswietlanaDruzyna.Sklad;

                if (wyswietlanaDruzyna.Sklad != null)
                {
                    TxtLicznik.Text = $"Aktywni: {wyswietlanaDruzyna.Sklad.Count}";
                }
            }
        }

        private void BtnSortNazwisko_Click(object sender, RoutedEventArgs e)
        {
            if (wyswietlanaDruzyna == null) return;
            wyswietlanaDruzyna.Sklad.Sort();
            OdswiezWidok();
        }

        private void BtnSortNumer_Click(object sender, RoutedEventArgs e)
        {
            if (wyswietlanaDruzyna == null) return;
            wyswietlanaDruzyna.Sklad.Sort((x, y) => x.NumerKoszulki.CompareTo(y.NumerKoszulki));
            OdswiezWidok();
        }

        private void BtnSortGole_Click(object sender, RoutedEventArgs e)
        {
            if (wyswietlanaDruzyna == null) return;
            wyswietlanaDruzyna.Sklad.Sort(new GoleComparer());
            OdswiezWidok();
        }

        private void BtnSortAsysty_Click(object sender, RoutedEventArgs e)
        {
            if (wyswietlanaDruzyna == null) return;
            wyswietlanaDruzyna.Sklad.Sort(new AsystyComparer());
            OdswiezWidok();
        }

        private void BtnKontuzja_Click(object sender, RoutedEventArgs e)
        {
            if (ListaGraczy.SelectedItem is Pilkarz p)
            {
                DodajKontuzjeWindow okno = new DodajKontuzjeWindow(p, wyswietlanaDruzyna);
                okno.ShowDialog();
                OdswiezWidok();
            }
            else
            {
                MessageBox.Show("Zaznacz piłkarza z listy, aby zgłosić kontuzję.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnZamknij_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}