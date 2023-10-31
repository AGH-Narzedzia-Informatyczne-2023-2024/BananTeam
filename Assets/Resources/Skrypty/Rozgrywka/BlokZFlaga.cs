using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlokZFlaga : MonoBehaviour
{
    public Strona strona;
    public Vector2 pozycjaBlokowa;
    SpriteRenderer srFlagi;
    public bool aktywny;

    void Start()
    {
        aktywny = true;
        srFlagi = transform.Find("Flaga").gameObject.GetComponent<SpriteRenderer>();
        if(strona != Strona.Niewybrana)
        {
            ZamianaBezMapy(strona);
        }
    }

    public void ZamianaBezMapy(Strona nowaStrona)
    {
        strona = nowaStrona;
        string nazwaFlagi = "flagaPolski";
        if(strona == Strona.Dziewczyna)
        {
            nazwaFlagi = "flagaDziewczyn";
        }
        srFlagi.sprite = Resources.Load<Sprite>("Grafiki/" + nazwaFlagi);
    }

    public void Zamien(Strona nowaStrona)
    {
        strona = nowaStrona;
        Blok typBloku = Blok.PiasekZFlagaDogow;
        string nazwaFlagi = "flagaPolski";
        if(strona == Strona.Dziewczyna)
        {
            typBloku = Blok.PiasekZFlagaDziewczyn;
            nazwaFlagi = "flagaDziewczyn";
        }
        Ogolne.mapa.mapa[(int)pozycjaBlokowa.x, (int)pozycjaBlokowa.y] = typBloku;
        srFlagi.sprite = Resources.Load<Sprite>("Grafiki/" + nazwaFlagi);
    }
}
