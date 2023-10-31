using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelPrzyborow : MonoBehaviour
{
    MapaKreatoraMap mapaKreatoraMap;
    Image obrazekLewego;
    Image obrazekPrawego;
    Text tekstRozmiaruPrawegoOlowka;
    Text tekstRozmiaruLewegoOlowka;
    Scrollbar miarkaPrawegoOlowka;
    Scrollbar miarkaLewegoOlowka;

    void Start()
    {
        mapaKreatoraMap = GameObject.Find("Mapa").GetComponent<MapaKreatoraMap>();
        obrazekLewego = transform.Find("BlokLewy/Obrazek").GetComponent<Image>();
        obrazekPrawego = transform.Find("BlokPrawy/Obrazek").GetComponent<Image>();
        tekstRozmiaruLewegoOlowka = transform.Find("BlokLewy/RozmiarLewegoOlowka/SlidingArea/Handle/Text").GetComponent<Text>();
        tekstRozmiaruPrawegoOlowka = transform.Find("BlokPrawy/RozmiarPrawegoOlowka/SlidingArea/Handle/Text").GetComponent<Text>();
        miarkaLewegoOlowka = transform.Find("BlokLewy/RozmiarLewegoOlowka").GetComponent<Scrollbar>();
        miarkaPrawegoOlowka = transform.Find("BlokPrawy/RozmiarPrawegoOlowka").GetComponent<Scrollbar>();
        obrazekLewego.sprite = Ogolne.BlokDoObrazka(mapaKreatoraMap.blokLewy);
        obrazekPrawego.sprite = Ogolne.BlokDoObrazka(mapaKreatoraMap.blokPrawy);
    }

    void FixedUpdate()
    {
        tekstRozmiaruLewegoOlowka.text = "" + mapaKreatoraMap.rozmiarLewegoOlowka;
    }

    public void PrawyWPrawo()
    {
        mapaKreatoraMap.blokPrawy = Ogolne.Nastepny(mapaKreatoraMap.blokPrawy);
        obrazekPrawego.sprite = Ogolne.BlokDoObrazka(mapaKreatoraMap.blokPrawy);
    }
    public void LewyWPrawo()
    {
        mapaKreatoraMap.blokLewy = Ogolne.Nastepny(mapaKreatoraMap.blokLewy);
        obrazekLewego.sprite = Ogolne.BlokDoObrazka(mapaKreatoraMap.blokLewy);
    }
    public void PrawyWLewo()
    {
        mapaKreatoraMap.blokPrawy = Ogolne.Poprzedni(mapaKreatoraMap.blokPrawy);
        obrazekPrawego.sprite = Ogolne.BlokDoObrazka(mapaKreatoraMap.blokPrawy);
    }
    public void LewyWLewo()
    {
        mapaKreatoraMap.blokLewy = Ogolne.Poprzedni(mapaKreatoraMap.blokLewy);
        obrazekLewego.sprite = Ogolne.BlokDoObrazka(mapaKreatoraMap.blokLewy);
    }

    public void PrawyOlowek()
    {
        int wartosc = 0;
        if(miarkaPrawegoOlowka.value <= 0.125f) wartosc = 1;
        else if(miarkaPrawegoOlowka.value <= 0.374f) wartosc = 3;
        else if(miarkaPrawegoOlowka.value <= 0.625f) wartosc = 5;
        else if(miarkaPrawegoOlowka.value <= 0.874f) wartosc = 7;
        else wartosc = 9;
        mapaKreatoraMap.rozmiarPrawegoOlowka = wartosc;
        tekstRozmiaruPrawegoOlowka.text = "" + wartosc;
    }

    public void LewyOlowek()
    {
        int wartosc = 0;
        if(miarkaLewegoOlowka.value <= 0.125f) wartosc = 1;
        else if(miarkaLewegoOlowka.value <= 0.374f) wartosc = 3;
        else if(miarkaLewegoOlowka.value <= 0.625f) wartosc = 5;
        else if(miarkaLewegoOlowka.value <= 0.874f) wartosc = 7;
        else wartosc = 9;
        mapaKreatoraMap.rozmiarLewegoOlowka = wartosc;
        tekstRozmiaruLewegoOlowka.text = "" + wartosc;
    }
}
