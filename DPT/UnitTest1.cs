using Microsoft.VisualStudio.TestTools.UnitTesting;
using DruzynaPilka;
using System;
using System.Collections.Generic;

namespace DruzynaPilka.Tests
{
    [TestClass]
    public class UnitTest1
    {
        #region Testy Klasy Druzyna

        [TestMethod]
        public void Druzyna_DodajCzlonka_PowinienDodacGdyNumerJestWolny()
        {
            var druzyna = new Druzyna();
            var p1 = new Pilkarz { NumerKoszulki = 10, Imie = "Jan", Nazwisko = "Kowalski" };

            druzyna.DodajCzlonkaDruzyny(p1);

            CollectionAssert.Contains(druzyna.Sklad, p1);
            Assert.AreEqual(1, druzyna.LiczbaPilkarzy());
        }

        [TestMethod]
        public void Druzyna_DodajCzlonka_NiePowinienDodacGdyNumerZajety()
        {
            var druzyna = new Druzyna();
            var p1 = new Pilkarz { NumerKoszulki = 7, Imie = "Cristiano" };
            var p2 = new Pilkarz { NumerKoszulki = 7, Imie = "Kamil" };

            druzyna.DodajCzlonkaDruzyny(p1);
            druzyna.DodajCzlonkaDruzyny(p2);

            Assert.AreEqual(1, druzyna.LiczbaPilkarzy());
            CollectionAssert.DoesNotContain(druzyna.Sklad, p2);
        }

        #endregion

        #region Testy Klasy Pilkarz i Statystyki

        [TestMethod]
        public void Pilkarz_Trening_PrzyKontuzji_PowinienRzucicWyjatek()
        {
            var p = new Pilkarz();
            p.DodajKontuzje("Kolano", DateTime.Now, 10);

            var ex = Assert.ThrowsException<WrongDataException>(() => p.TrenujPilkarza());
            Assert.AreEqual("Kontuzjowany zawodnik ma zakaz treningów!", ex.Message);
        }

        [TestMethod]
        public void Statystyki_Gole_Ujemne_PowinnyRzucicWyjatek()
        {
            var s = new Statystyki();

            Assert.ThrowsException<WrongDataException>(() => s.LiczbaGoli = -5);
        }

        #endregion

        #region Testy Klasy Stadion

        [TestMethod]
        public void Stadion_ObliczZysk_ZwracaPoprawnaKwote()
        {
            var stadion = new Stadion("Test", 1000, 50.0);

            var zysk = stadion.ObliczZysk(0.5);

            Assert.AreEqual(25000.0, zysk);
        }

        [DataTestMethod]
        [DataRow(-0.1)]
        [DataRow(1.1)]
        public void Stadion_ObliczZysk_BlednyProcent_RzucaWyjatek(double procent)
        {
            var stadion = new Stadion();

            Assert.ThrowsException<WrongDataException>(() => stadion.ObliczZysk(procent));
        }

        #endregion

        #region Testy Klasy Trener i Kontuzje

        [TestMethod]
        public void Trener_Zmotywuj_ZwiekszaStamineMaxDo100()
        {
            var trener = new Trener();
            var p = new Pilkarz { Stamina = 98 };

            trener.ZmotywujPilkarza(p);

            Assert.AreEqual(100, p.Stamina);
        }

        [TestMethod]
        public void Kontuzja_UpdateStatus_PowinnaSieZakonczycPoCzasie()
        {
            var k = new Kontuzja("Kostka", DateTime.Now.AddDays(-5), 3);

            k.UpdateStatus(DateTime.Now);

            Assert.IsFalse(k.Aktywna);
        }

        #endregion

        #region Testy Porównywania

        [TestMethod]
        public void GoleComparer_PowinienSortowacMalejaco()
        {
            var p1 = new Pilkarz { Imie = "A", Staty = new Statystyki { LiczbaGoli = 5 } };
            var p2 = new Pilkarz { Imie = "B", Staty = new Statystyki { LiczbaGoli = 10 } };
            var comparer = new GoleComparer();

            int result = comparer.Compare(p1, p2);

            Assert.IsTrue(result > 0);
        }

        #endregion
    }
}