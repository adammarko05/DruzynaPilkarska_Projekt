using DruzynaPilka;
using System;
using System.IO;
using System.Windows;

namespace DruzynaPilkawpf
{
    public partial class MainWindow : Window
    {
        private Druzyna mojKlub;
        private const string PlikDanych = "druzyna.xml";

        public MainWindow()
        {
            InitializeComponent();
            WczytajDane();
        }

        private void WczytajDane()
        {
            string folderAplikacji = AppDomain.CurrentDomain.BaseDirectory;
            string sciezkaDoPliku = System.IO.Path.Combine(folderAplikacji, PlikDanych);

            if (File.Exists(sciezkaDoPliku))
            {
                try
                {
                    mojKlub = Druzyna.OdczytDCXML(sciezkaDoPliku);
                }
                catch
                {
                    MessageBox.Show("Błąd odczytu pliku danych. Utworzono nową drużynę.");
                    mojKlub = null;
                }
            }

            if (mojKlub == null)
            {
                mojKlub = new Druzyna();
                mojKlub.Nazwa = "Manchester City"; 
            }
        }

        private void ZapiszDane()
        {
            string folderAplikacji = AppDomain.CurrentDomain.BaseDirectory;
            string sciezkaDoPliku = System.IO.Path.Combine(folderAplikacji, PlikDanych);

            if (mojKlub != null)
            {
                try
                {
                    mojKlub.ZapiszDCXML(sciezkaDoPliku);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd zapisu: " + ex.Message);
                }
            }
        }

        // --- OBSŁUGA PRZYCISKÓW ---

        private void Button_Sklad_Click(object sender, RoutedEventArgs e)
        {
            SkladWindow okno = new SkladWindow(mojKlub);
            okno.ShowDialog();
            
            ZapiszDane();
        }

        private void Button_Info_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow okno = new InfoWindow(mojKlub);
            okno.ShowDialog();
        }

        private void Button_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            DodajWindow okno = new DodajWindow(mojKlub);
            okno.ShowDialog();
            ZapiszDane(); 
        }

        private void Button_Mecz_Click(object sender, RoutedEventArgs e)
        {
            MeczWindow okno = new MeczWindow(mojKlub);
            okno.ShowDialog();
            ZapiszDane(); 
        }
    }
}