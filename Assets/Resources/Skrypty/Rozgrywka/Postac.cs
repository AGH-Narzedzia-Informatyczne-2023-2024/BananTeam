using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Postac : MonoBehaviour
{
    public string nazwa;
    public int hp;
    public int maksHp;
    public float predkosc;
    public float predkoscPoczatkowa;
    public Strona strona;
    public int pokonania = 0;
    public bool zabity;

    public Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        zabity = false;
    }

    public virtual void Zran(int obrazenia, Postac raniacy)
    {
        if(zabity) return;
        hp -= obrazenia;
        if(hp <= 0){
            Zabij(raniacy);
        }
        else{
            predkosc=predkoscPoczatkowa*((float)(hp)/(float)(maksHp));
        }
    }

    public virtual void Zabij(Postac zabijajacy)
    {
        if(zabity) return;
        zabity = true;
        if(zabijajacy != null) 
        {
            if(zabijajacy.strona != strona) 
            {
                zabijajacy.pokonania += 1;
                if(zabijajacy == (Postac)Gracz.glownyDoge) Gracz.punkty += 1;
            }
        }
        Destroy(gameObject);
    }

    public bool Przed(GameObject obiekt)
    {
        return ((obiekt.transform.localPosition.x > transform.localPosition.x && transform.localScale.x > 0) || (obiekt.transform.localPosition.x < transform.localPosition.x && transform.localScale.x < 0));
    }

    public void Idz(float x, float y)
    {
        transform.localPosition = new Vector2(transform.localPosition.x + x, transform.localPosition.y + y);
        if((x > 0 && transform.localScale.x < 0) || (x < 0 && transform.localScale.x > 0)) transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
    }

    public void IdzBezObrotu(float x, float y)
    {
        transform.localPosition = new Vector2(transform.localPosition.x + x, transform.localPosition.y + y);
        //if((x > 0 && transform.localScale.x < 0) || (x < 0 && transform.localScale.x > 0)) transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
    }

    public Vector2 PoruszajSieWKierunku(float predkosc, Vector2 kierunek)
    {
        if(kierunek.x == 0 && kierunek.y == 0) return (Vector2)transform.localPosition;
        float x;
        float y;
        if(kierunek.y == 0 && Math.Abs(kierunek.x) < predkosc)
        {
            y = 0;
            x = Math.Abs(kierunek.x);
        }
        else if(kierunek.y == 0 && Math.Abs(kierunek.x) > predkosc)
        {
            y = 0;
            x = predkosc;
        }
        else if(kierunek.x == 0 && Math.Abs(kierunek.y) < predkosc)
        {
            x = 0;
            y = Math.Abs(kierunek.y);
        }
        else if(kierunek.x == 0 && Math.Abs(kierunek.y) > predkosc)
        {
            x = 0;
            y = predkosc;
        }
        else 
        {
            float minX = Math.Abs(kierunek.x) / Math.Abs(kierunek.y);
            float sumY = (float)Math.Pow(1 + Math.Pow(minX, 2), 0.5);
            y = predkosc / sumY;
            x = minX * y;

        if(Single.IsNaN(x))
        {
            Debug.Log("Postac, PoruszajSieWKierunku, X rowny NaN, awaryjne ustawianie na 0, wywolany przez '" + nazwa + "'. Informacje o danych ponizej");
            Debug.Log("Wprowadzone: Predkosc: " + predkosc + "; Kierunek: (" + kierunek.x + "; " + kierunek.y + ")");
            Debug.Log("Wyniki: MinX: " + minX + "; SumY: " + sumY + "; Y: " + y + "; X: " + x);
            x = 0;
        } 
        if(Single.IsNaN(y))
        {
            Debug.Log("Postac, PoruszajSieWKierunku, Y rowny NaN, awaryjne ustawianie na 0, wywolany przez '" + nazwa + "'. Informacje o danych ponizej");
            Debug.Log("Wprowadzone: Predkosc: " + predkosc + "; Kierunek: (" + kierunek.x + "; " + kierunek.y + ")");
            Debug.Log("Wyniki: MinX: " + minX + "; SumY: " + sumY + "; Y: " + y + "; X: " + x);
            y = 0;
        }
        }
        if(kierunek.x < 0) x *= -1;
        if(kierunek.y < 0) y *= -1;
        Vector2 miejsceDocelowe = new Vector2(x, y);
        //Debug.Log(miejsceDocelowe.ToString());
        rb.MovePosition(rb.position + miejsceDocelowe * Time.fixedDeltaTime);
        return miejsceDocelowe;
    }

    public Postac NajblizszyWrog()
    {
        Postac[] wrogowie = (strona != Strona.Doge) ? (Postac[])Ogolne.mapa.DogiJakoDogi() : (Postac[])Ogolne.mapa.DziewczynyJakoDziewczyny();
        if(wrogowie.Length > 0)
        {
            Postac najblizszyWrog = wrogowie[0];
            if(najblizszyWrog != null)
            {
            float dystans = Vector2.Distance(wrogowie[0].transform.localPosition, transform.localPosition);
            foreach(Postac wrog in wrogowie)
            {
                if(wrog)
                {
                float nowyDystans = Vector2.Distance(wrog.transform.localPosition, transform.localPosition);
                if(nowyDystans < dystans)
                {
                    najblizszyWrog = wrog;
                    dystans = nowyDystans;
                }
                }
            }
            }
            return najblizszyWrog;
        }
        return null;
    }

    public void Ulecz(int uleczenie)
    {
        hp += uleczenie;
        if(hp > maksHp) hp = maksHp;
    }
}

public enum Strona
{
    Doge,
    Dziewczyna,
    Niewybrana,
} 