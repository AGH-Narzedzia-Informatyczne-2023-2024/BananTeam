using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doge : Postac
{
    public bool czyAI = false;
    public float czasOstatniegoZranienia;
    public float czasOstatniegoUleczenia;

    [Header("Umiejetnosci")]
    public float[] zaladowanie = {100f, 0f};
    public bool[] czyUzywa = {false, false};
    public float[] minimumDoStartuUzycia = {1f, 1f};
    public float[] czasMiedzyUzyciami = {0f, 0f};
    public float[] czasOstatniegoUzycia = {0f, 0f};
    //public SposobZuzycia[] sposobZuzycia = {SposobZuzycia.rozladowywanie, SposobZuzycia.rozladowywanie};
    public float[] wartoscUzupelniania = {0f, 0f};
    public float[] czasMiedzyUzupelnieniami = {0f, 0f};
    public float[] ostatnieUzupelnienie = {0f, 0f};

    public void Start()
    {
        czasOstatniegoUleczenia = -100;
        czasOstatniegoZranienia = -100;
        Ogolne.mapa = GameObject.Find("Mapa").GetComponent<Mapa>();
        strona = Strona.Doge;
        predkosc = 2;
        predkosc_poczatkowa=predkosc;
    }

    public void Update()
    {
        if(!czyAI)
        {
            Vector2 myszka = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if((myszka.x > transform.localPosition.x && transform.localScale.x < 0) || (myszka.x < transform.localPosition.x && transform.localScale.x > 0))
            {
                transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
            }
        }

        Blok? b = Ogolne.mapa.BlokNaPozycji(Ogolne.mapa.PozycjaBlokowa(transform.localPosition));
        if(b == Blok.Pustka || b == null)
        {
            Zabij(null);
        }
    }

    public void FixedUpdate()
    {
        if(czasOstatniegoZranienia + 5 < Time.time && czasOstatniegoUleczenia + 1 < Time.time)
        {
            Ulecz(5);
            czasOstatniegoUleczenia = Time.time;
        }

        if(zaladowanie[0] < 100f)
        {
            if(Time.time >= ostatnieUzupelnienie[0] + czasMiedzyUzupelnieniami[0])
            {
                zaladowanie[0] += wartoscUzupelniania[0];
                ostatnieUzupelnienie[0] = Time.time;
                if(zaladowanie[0] > 100f) zaladowanie[0] = 100f;
            }
        }
        if(zaladowanie[1] < 100f)
        {
            if(Time.time >= ostatnieUzupelnienie[1] + czasMiedzyUzupelnieniami[1])
            {
                zaladowanie[1] += wartoscUzupelniania[1];
                ostatnieUzupelnienie[1] = Time.time;
                if(zaladowanie[1] > 100f) zaladowanie[1] = 100f;
            }
        }

        if(!czyAI)
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
            Vector2 przyspieszenie = new Vector2(0, 0);
            if(Input.GetKey(KeyCode.W))
            {
                przyspieszenie.y += predkosc;
                //IdzBezObrotu(0, predkosc);
            }
            if(Input.GetKey(KeyCode.S))
            {
                przyspieszenie.y -= predkosc;
                //IdzBezObrotu(0, -predkosc);
            }
            if(Input.GetKey(KeyCode.D))
            {
                przyspieszenie.x += predkosc;
                //IdzBezObrotu(predkosc, 0);
            }
            if(Input.GetKey(KeyCode.A))
            {
                przyspieszenie.x -= predkosc;
                //IdzBezObrotu(-predkosc, 0);
            }
            rb.MovePosition(rb.position + przyspieszenie * Time.fixedDeltaTime);
            }
        }
    }

    public bool CzyMozeUzycUmiejetnosci(int numerUmiejetnosci)
    {
        return ((zaladowanie[numerUmiejetnosci] >= minimumDoStartuUzycia[numerUmiejetnosci]) && (czasOstatniegoUzycia[numerUmiejetnosci] + czasMiedzyUzyciami[numerUmiejetnosci] <= Time.time));
    }

    public bool CzyMozeKontynuowacUzywanieUmiejetnosci(int numerUmiejetnosci)
    {
        return ((zaladowanie[numerUmiejetnosci] > 0) && (czasOstatniegoUzycia[numerUmiejetnosci] + czasMiedzyUzyciami[numerUmiejetnosci] <= Time.time) && (czyUzywa[numerUmiejetnosci]));
    }

    public void Uzycie(int numerUmiejetnosci, float zaladowanieDoUjecia)
    {
        czasOstatniegoUzycia[numerUmiejetnosci] = Time.time;
        zaladowanie[numerUmiejetnosci] -= zaladowanieDoUjecia;
        if(zaladowanie[numerUmiejetnosci] < 0) zaladowanie[numerUmiejetnosci] = 0;
    }

    public override void Zran(int obrazenia, Postac raniacy)
    {
        czasOstatniegoZranienia = Time.time;
        base.Zran(obrazenia, raniacy);
    }

    public override void Zabij(Postac zabijajacy)
    {
        Doge glownyDoge = Gracz.glownyDoge;
        Gracz.glownyDoge = null;
        if(glownyDoge == this) Ogolne.mapa.StartCoroutine(Ogolne.mapa.Odrodz(3));
        base.Zabij(zabijajacy);
    }
}

/*public enum SposobZuzycia
{
    rozladowywanie,
    odejmowanie,
}*/
