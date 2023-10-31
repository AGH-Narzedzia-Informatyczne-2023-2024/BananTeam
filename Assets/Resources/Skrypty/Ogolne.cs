using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogolne : MonoBehaviour
{
    public static Blok[] bloki = {Blok.Piasek, Blok.Kamien, Blok.Pustka, Blok.PiasekZFlagaDogow, Blok.PiasekZFlagaDziewczyn};
    public static string[] kodyBlokow = {"WPI", "WKM", "WPS", "WFD", "WFZ"};
    public static string[] sciezki = {"Grafiki/yellow_pixel", "Grafiki/grey_pixel", "Grafiki/black_pixel", "Grafiki/flagaPolski", "Grafiki/flagaDziewczyn"};

    public static Mapa mapa;
    public static bool Okolo(float pierwsza, float druga, float przyblizenie)
    {
        return (Math.Abs(pierwsza - druga) < przyblizenie);
    }

    public static float Procent(float pierwsza, float druga)
    {
        return pierwsza / druga * 100;
    }

    public static Sprite BlokDoObrazka(Blok blok)
    {
        for(int i = 0; i < bloki.Length; i++)
        {
            if(bloki[i] == blok) 
            {
                return Resources.Load<Sprite>(sciezki[i]);
            }
        }
        return null;
    }

    public static Blok Nastepny(Blok poprzedni)
    {
        for(int i = 0; i < bloki.Length; i++)
        {
            if(bloki[i] == poprzedni)
            {
                if(i == bloki.Length - 1) return bloki[0];
                else return bloki[i+1];
            }
        }
        return Blok.Pustka;
    }

    public static Blok Poprzedni(Blok nastepny)
    {
        for(int i = 0; i < bloki.Length; i++)
        {
            if(bloki[i] == nastepny)
            {
                if(i == 0) return bloki[bloki.Length - 1];
                else return bloki[i-1];
            }
        }
        return Blok.Pustka;
    }

    public static bool BlokDoPrzejscia(Blok? blok)
    {
        return !(blok == Blok.Kamien || blok == Blok.Pustka || blok == null);
    }

    /*public static Vector2[] NajkrotrzaDrogaDoPunktu(Vector2 punkt)
    {

    }*/
}
