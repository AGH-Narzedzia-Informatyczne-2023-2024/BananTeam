using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class DaneDoMap : MonoBehaviour
{
    public static DaneMapy daneUstawionejMapy;
    public static DaneMapy daneZarzadzanejMapy;
    public static string uzywanaSciezka;
    public static string problemWczytanejMapy;
    public static string sugerowaneRozwiazanieProblemu;

    public static string NazwaPlikuZeSciezki(string sciezka)
    {
        //Debug.Log(sciezka);
        if(sciezka != null)
        {
        string wynik = "";
        if(sciezka.Length == 0) return "";
        for(int i = sciezka.Length-1; i >= 0; i--)
        {
            if(sciezka[i] != '/' && sciezka[i] != '\\') wynik = sciezka[i] + wynik;
            else break;
        }
        return wynik;
        }
        return "";
    }

    /*public static Blok? KodBlokuDoBloku(string kod)
    {
        switch(kod)
        {
            case "WPS":
                return Blok.Pustka;
            case "WPI":
                return Blok.Piasek;
            case "WKM":
                return Blok.Kamien;
            case "WFD":
                return Blok.PiasekZFlagaDogow;
            case "WFZ":
                return Blok.PiasekZFlagaDziewczyn;
            case "WPD":
                return Blok.PortalDogow;
            case "WPZ":
                return Blok.PortalDziewczyn;
            default:
                return null;
        }
    }*/

    public static Blok? KodBlokuDoBloku(string kod)
    {
        for(int i = 0; i < Ogolne.kodyBlokow.Length; i++)
        {
            if(Ogolne.kodyBlokow[i] == kod) return Ogolne.bloki[i];
        }
        return null;
    }

    public static string BlokDoKoduBloku(Blok blok)
    {
        for(int i = 0; i < Ogolne.bloki.Length; i++)
        {
            if(Ogolne.bloki[i] == blok) return Ogolne.kodyBlokow[i];
        }
        return null;
    }

    public static DaneMapy LinijkiDoMapy(string[] linijki)
    {
        try
        {
        if(linijki[0] != "[BoD-KM]") 
        {
            problemWczytanejMapy = "Nie wykryto zwrotu kluczowego '[BoD-KM]' w początkowej linijce skryptu";
            sugerowaneRozwiazanieProblemu = "Ustaw tekst pierwszej linijki skryptu na '[BoD-KM]' (bez znaków ' ')";
            return null;
        }
        if(linijki.Length < 5)
        {
            problemWczytanejMapy = "Podany plik nie spełnia wymagań długości";
            sugerowaneRozwiazanieProblemu = "Uzupełnij podstawowe dane pierwszych 5 linijek skryptu"; //(zwrot kloczowy, komentarz (opcjonalny, linijka moze zostac pusta), szerokość mapy (liczba naturalna z przedzialu <1; 1000>), wysokość mapy (liczba naturalna z przedzialu <1; 1000>), przynajmniej jedna linijka kodu mapy)
            return null;
        }
        int parsX = int.Parse(linijki[2]);
        if(parsX < 1 || parsX > 100 || Single.IsNaN(parsX))
        {
            problemWczytanejMapy = "Podana szerokość mapy nie spełnia wymagań";
            sugerowaneRozwiazanieProblemu = "Ustaw szerokość mapy (trzecia linijka) na liczbę naturalną z przedziału <1; 100>";
            return null;
        }
        int parsY = int.Parse(linijki[3]);
        if(parsY < 1 || parsY > 100 || Single.IsNaN(parsY))
        {
            problemWczytanejMapy = "Podana wysokość mapy nie spełnia wymagań";
            sugerowaneRozwiazanieProblemu = "Ustaw wysokość mapy (czwarta linijka) na liczbę naturalną z przedziału <1; 100>";
            return null;
        }
        DaneMapy daneMapy = new DaneMapy();
        daneMapy.dlugoscX = parsX;
        daneMapy.dlugoscY = parsY;
        if(daneMapy.dlugoscY != linijki.Length - 4)
        {
            problemWczytanejMapy = "Wartość wykrytej wysokości mapy (Y: " + daneMapy.dlugoscY + ") różni się od liczby pozostałych wykrytych linijek skryptu (LPWLK: " + (linijki.Length - 4) + ")";
            sugerowaneRozwiazanieProblemu = "Zmień wartość wysokości mapy lub dodaj/odejmij linijki skryptu tak aby wartość wysokości mapy była równa liczbie końcowych linijek skryptu odpowiedzialnych za generowanie bloków";
            return null;
        }

        Blok[,] mapa = new Blok[daneMapy.dlugoscX, daneMapy.dlugoscY];
        int dlugoscKoduPrzedKodemMapy = 4;
        int liczbaPunktowOdrodzeniaGracza = 0;
        for(int y = 0; y < daneMapy.dlugoscY; y++)
        {
            string linijka = linijki[y + dlugoscKoduPrzedKodemMapy];
            //Debug.Log(linijka);
            if(((float)linijka.Length) % 3 != 0)
            {
                problemWczytanejMapy = "Linijka nr. " + (y + dlugoscKoduPrzedKodemMapy + 1) + " nie składa się wyłącznie z 3-elementowych kodów";
                sugerowaneRozwiazanieProblemu = "Popraw zawartość linijki nr. " + (y + dlugoscKoduPrzedKodemMapy + 1) + " tak aby składała się ona wyłącznie z 3-elementowych kodów (bez spacji, przecinków i tym podobnych)";
                return null; 
            }
            if(linijka.Length / 3 != daneMapy.dlugoscX)
            {
                problemWczytanejMapy = "Liczba kodów w linijce nr. " + (y + dlugoscKoduPrzedKodemMapy + 1) + " nie odpowiada wykrytej wartości szerokości mapy (X: " + daneMapy.dlugoscX + ")";
                sugerowaneRozwiazanieProblemu = "Popraw zawartość linijki nr. " + (y + dlugoscKoduPrzedKodemMapy + 1) + " tak aby liczba 3-elementowych kodów była równa wartości szerokości mapy";
                return null; 
            }
            for(int x = 0; x < daneMapy.dlugoscX; x++)
            {
                //Debug.Log(x);
                string kod = "" + linijka[x * 3] + linijka[(x * 3) + 1] + linijka[(x * 3) + 2];
                Blok? blok = KodBlokuDoBloku(kod);
                if(blok == null)
                {
                    problemWczytanejMapy = "W linijce nr. " + (y + dlugoscKoduPrzedKodemMapy + 1) + " znalazł się nierozpoznany kod '" + kod + "'";
                    sugerowaneRozwiazanieProblemu = "Popraw nierozpoznany kod na rozpoznawalny";
                    return null; 
                }
                else
                {
                    mapa[x, (daneMapy.dlugoscY-1) - y] = (Blok)blok;
                    if(blok == Blok.PiasekZFlagaDogow || blok == Blok.PortalDogow) liczbaPunktowOdrodzeniaGracza += 1;
                }
            }
        }

        if(liczbaPunktowOdrodzeniaGracza < 1)
        {
            problemWczytanejMapy = "Mapa nie zawiera żadnego punktu odrodzenia dla gracza";
            sugerowaneRozwiazanieProblemu = "Dodaj przynajmniej jedną flagę dogów lub portal";
            return null; 
        }

        daneMapy.mapa = mapa;

        //Debug.Log("Pomyslnie wczytano mape!");
        return daneMapy;
        }
        catch
        {
            problemWczytanejMapy = "Wystapil nierozpoznany blad systemu podczas generowania tej mapy";
            sugerowaneRozwiazanieProblemu = "Zglos ten problem do administracji gry";
            return null;
        }
    }

    public static string[] MapaDoLinijek(DaneMapy daneMapy)
    {
        string[] linijki = new string[4 + daneMapy.dlugoscY];
        linijki[0] = "[BoD-KM]";
        linijki[1] = "";
        linijki[2] = "" + daneMapy.dlugoscX;
        linijki[3] = "" + daneMapy.dlugoscY;
        for(int y = daneMapy.dlugoscY-1; y >= 0; y--)
        {
            string linijka = "";
            for(int x = 0; x < daneMapy.dlugoscX; x++)
            {
                linijka += BlokDoKoduBloku(daneMapy.mapa[x, y]);
            }
            linijki[(3 + daneMapy.dlugoscY) - y] = linijka;
        }
        return linijki;
    }

    public static void WczytajMapeZPliku()
    {
        //uzywanaSciezka = null;
        problemWczytanejMapy = null;
        sugerowaneRozwiazanieProblemu = null;
        //string sciezka = EditorUtility.OpenFilePanel("Wczytaj kod txt", "", "txt");
        //string sciezka = WczytajPlik.WczytajSciezkePliku();
        string sciezka = DaneDoMap.uzywanaSciezka;
        if(sciezka != "" && sciezka != null)
        {
            uzywanaSciezka = sciezka;
            try
            {
                string[] linijki = System.IO.File.ReadAllLines(@"" + sciezka);
                DaneMapy dane = LinijkiDoMapy(linijki);
                if(dane != null)
                {
                    daneZarzadzanejMapy = dane;
                }
            }
            catch
            {
                problemWczytanejMapy = "Wystapil nieoczekiwany blad podczas wczytywania pliku z wybranej sciezki";
                sugerowaneRozwiazanieProblemu = "Sproboj ponownie a jesli blad bedzie sie powtarzal zglos go do administracji gry";
            }
        }
        else
        {
            problemWczytanejMapy = "Nie wykryto zadnej sciezki lub wprowadzona sciezka jest nieprawidlowa";
            sugerowaneRozwiazanieProblemu = "Sproboj ponownie i upewnij sie ze wybrales odpowiedni plik";
        }
    }
}

public class DaneMapy
{
    public int dlugoscX;
    public int dlugoscY;
    public Blok[,] mapa;
}

public enum Blok
{
Pustka,
Piasek,
Kamien,
PiasekZFlagaDogow,
PiasekZFlagaDziewczyn,
PortalDogow,
PortalDziewczyn,
}