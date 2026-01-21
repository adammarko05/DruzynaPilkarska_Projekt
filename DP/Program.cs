using System;

namespace DruzynaPilka
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== 1 TWORZENIE FUNDAMENTÓW MANCHESTER CITY ===");

                
                Stadion stadion = new Stadion("Etihad Stadium", 53400, 130.00);
                Console.WriteLine(stadion);

               
                Trener trener = new Trener(Taktyka.Ofensywna, "2016-07-01", "Pep", "Guardiola", "1971-01-18", 76, 1.80);
                Console.WriteLine(trener);

                
                Kapitan kapitan = new Kapitan("2023-08-01", LepszaNoga.Prawa, Pozycja.Obrońca, 87, 2, "Bernardo", "Silva", "1990-05-28", 83, 1.83);
                Console.WriteLine($"KAPITAN: {kapitan}");

                
                Druzyna city = new Druzyna("Manchester City", kapitan, trener, stadion);
                Console.WriteLine("\n");


                Console.WriteLine("=== 2 DODAWANIE ZAWODNIKÓW ===");

                
                Pilkarz p1 = new Pilkarz(LepszaNoga.Lewa, Pozycja.Bramkarz, 91, 31, "Gianluigi", "Donnaruma", "1993-08-17", 89, 1.88);
                Pilkarz p2 = new Pilkarz(LepszaNoga.Prawa, Pozycja.Obrońca, 93, 3, "Ruben", "Dias", "1997-05-14", 83, 1.87);
                Pilkarz p3 = new Pilkarz(LepszaNoga.Lewa, Pozycja.Pomocnik, 94, 47, "Phil", "Foden", "2000-05-28", 70, 1.71);
                Pilkarz p4 = new Pilkarz(LepszaNoga.Lewa, Pozycja.Napastnik, 99, 9, "Erling", "Haaland", "2000-07-21", 88, 1.95);
                Pilkarz p5 = new Pilkarz(LepszaNoga.Prawa, Pozycja.Pomocnik, 85, 11, "Jeremy", "Doku", "2002-05-27", 75, 1.73);

                city.DodajCzlonkaDruzyny(p1);
                city.DodajCzlonkaDruzyny(p2);
                city.DodajCzlonkaDruzyny(p3);
                city.DodajCzlonkaDruzyny(p4);
                city.DodajCzlonkaDruzyny(p5);

                Console.WriteLine(p1.ToString());
                Console.WriteLine(p2.ToString());
                Console.WriteLine(p3.ToString());
                Console.WriteLine(p4.ToString());


                Console.Write("Test duplikatu:");
                city.DodajCzlonkaDruzyny(p4);

                
                Console.Write("Test zajętego numeru: ");
                Pilkarz pFail = new Pilkarz(LepszaNoga.Prawa, Pozycja.Napastnik, 70, 9, "xxx", "xxx", "2000-01-01", 80, 1.80);
                city.DodajCzlonkaDruzyny(pFail);

                Console.WriteLine($"Aktualna liczba piłkarzy: {city.LiczbaPilkarzy()}\n");


                Console.WriteLine("=== 3 TRENING ===");

                Console.WriteLine($"Przed treningiem Haaland: Skill {p4.Umiejetnosci}, Stamina {p4.Stamina}%");

               
                p4.TrenujPilkarza();
                

                
                trener.ZmotywujPilkarza(p4);

                Console.WriteLine($"Po treningu i motywacji: Skill {p4.Umiejetnosci}, Stamina {p4.Stamina}%\n");


                Console.WriteLine("=== 4. SYMULACJA MECZU ===");

                
                
                for (int i = 0; i < 5; i++)
                {
                    
                    p4.RegenSauna();
                    if (p4.RozegrajMecz())
                    {
                        Console.Write(" [Mecz] ");
                    }
                }
                Console.WriteLine("\nWyniki Haalanda: " + p4.Staty.ToString() + "\n");


                Console.WriteLine("=== 5. SYSTEM KONTUZJI ===");

                
                
                p5.DodajKontuzje("Mięsień", DateTime.Now, 21); 

                
                Console.WriteLine($"Czy Doku kontuzjowany? {p5.CzyKontuzjowany}");
                Console.WriteLine($"Skill po kontuzji : {p5.Umiejetnosci}");

                
                Console.Write("Próba rozegrania meczu przez Doku: ");
                bool czyZagral = p5.RozegrajMecz();
                if (!czyZagral) Console.WriteLine("BLOKADA ZADZIAŁAŁA");

                
                try
                {
                    Console.Write("Próba treningu Doku: ");
                    p5.TrenujPilkarza();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ZŁAPANO BŁĄD: {ex.Message}");
                }
                Console.WriteLine();


                


                Console.WriteLine("=== 6 ZAPIS I ODCZYT ===");

                string plik = "city_data.xml";

                
                city.ZapiszDCXML(plik);
                Console.WriteLine("Dane Manchesteru City zapisane do pliku.");

                
                Druzyna? wczytana = Druzyna.OdczytDCXML(plik);

                if (wczytana != null)
                {
                    Console.WriteLine("\n--- RAPORT Z PLIKU XML ---");
                    Console.WriteLine($"Klub: {wczytana.Nazwa}");
                    Console.WriteLine($"Trener: {wczytana.TrenerZespolu.Nazwisko}");
                    Console.WriteLine($"Liczba graczy: {wczytana.LiczbaPilkarzy()}");

                    
                }
                else
                {
                    Console.WriteLine("Błąd wczytywania!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n!!! WYSTĄPIŁ NIEOCZEKIWANY BŁĄD: {ex.Message} !!!");
            }

            Console.WriteLine("\n--- BLUE MOON ---");
            Console.ReadKey();
        }
    }
}