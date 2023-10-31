using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapaKreatoraMap : MonoBehaviour
{
    public DaneMapy daneMapy;
    public GameObject obszarDoEdycji;
    PoleEdycjiMapy poleEdycjiMapy;
    
    public Blok blokLewy = Blok.Piasek;
    public Blok blokPrawy = Blok.Kamien;
    public int rozmiarLewegoOlowka = 1;
    public int rozmiarPrawegoOlowka = 1;

    bool prawyKliknietyWPolu;
    bool lewyKliknietyWPolu;

    void Awake()
    {
        blokLewy = Blok.Piasek;
        blokPrawy = Blok.Kamien;
    }

    void Start()
    {
        obszarDoEdycji = GameObject.Find("ObszarDoEdycji");
        poleEdycjiMapy = GameObject.Find("Canvas/PoleEdycjiMapy").GetComponent<PoleEdycjiMapy>();
        if(daneMapy == null) 
        {
            daneMapy = new DaneMapy();
            daneMapy.dlugoscX = 1;
            daneMapy.dlugoscY = 1;
            daneMapy.mapa = new Blok[daneMapy.dlugoscX, daneMapy.dlugoscY];
        }
        GenerujMape();
    }

    void Update()
    {
        prawyKliknietyWPolu = poleEdycjiMapy.prawyKlikniety;
        lewyKliknietyWPolu = poleEdycjiMapy.lewyKlikniety;

        if(Input.GetMouseButton(0) && lewyKliknietyWPolu)
        {
            Vector2 pozycjaBlokowaMyszki = PozycjaBlokowa(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Blok? blok = BlokNaPozycji(pozycjaBlokowaMyszki);
            if(blok != null)
            {
                Koloruj(new Vector2((int)pozycjaBlokowaMyszki.x, (int)pozycjaBlokowaMyszki.y), blokLewy, rozmiarLewegoOlowka);
            }
        }

        if(Input.GetMouseButton(1) && prawyKliknietyWPolu)
        {
            Vector2 pozycjaBlokowaMyszki = PozycjaBlokowa(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Blok? blok = BlokNaPozycji(pozycjaBlokowaMyszki);
            //Debug.Log(pozycjaBlokowaMyszki);
            if(blok != null)
            {
                Koloruj(new Vector2((int)pozycjaBlokowaMyszki.x, (int)pozycjaBlokowaMyszki.y), blokPrawy, rozmiarPrawegoOlowka);
            }
        }
    }

    public void Koloruj(Vector2 pozycjaBlokowa, Blok blok, int rozmiarOlowka)
    {
        if(rozmiarOlowka < 1) return;
        int oX = (int)pozycjaBlokowa.x;
        int oY = (int)pozycjaBlokowa.y;
        for(int y = 0; y < rozmiarOlowka; y++)
        {
            for(int x = 0; x < rozmiarOlowka; x++)
            {
                if(ZawieraPozycjeBlokowa(new Vector2(x + oX - (rozmiarOlowka/2), y + oY - (rozmiarOlowka/2)))) 
                daneMapy.mapa[x + oX - (rozmiarOlowka/2), y + oY - (rozmiarOlowka/2)] = blok;
            }
        }
        OdswierzMape();
    }

    public void GenerujMape()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for(int y = 0; y < daneMapy.dlugoscY; y++)
        {
            for(int x = 0; x < daneMapy.dlugoscX; x++)
            {
                UstawBlokNaPozycji(new Vector2(x, y), daneMapy.mapa[x, y]);
            }
        }
    }

    public void OdswierzMape()
    {
        for(int y = 0; y < daneMapy.dlugoscY; y++)
        {
            for(int x = 0; x < daneMapy.dlugoscX; x++)
            {
                Vector2 pozycja = new Vector2(x, y);
                if(BlokNaPozycjiJakoObiekt(pozycja).GetComponent<BlokWGrze>().rodzajBloku != daneMapy.mapa[x, y]) 
                {
                    Destroy(BlokNaPozycjiJakoObiekt(pozycja));
                    UstawBlokNaPozycji(pozycja, daneMapy.mapa[x, y]);
                }
            }
        }
    }

    /*public void Wypelnij(Vector2 pozycjaBlokowa, Blok blok)
    {

    }*/

    public void UstawBlokNaPozycji(Vector2 pozycjaBlokowa, Blok rodzajBloku)
    {
        GameObject prefab;
        int x = (int)pozycjaBlokowa.x;
        int y = (int)pozycjaBlokowa.y;
        daneMapy.mapa[x, y] = rodzajBloku;
        Strona stronaDoUstawienia = Strona.Niewybrana;
        if(rodzajBloku == Blok.Kamien) prefab = Resources.Load("Prefaby/Kamien") as GameObject;
        else if(rodzajBloku == Blok.PiasekZFlagaDogow) 
        {
            prefab = Resources.Load("Prefaby/PiasekZFlaga") as GameObject;
            stronaDoUstawienia = Strona.Doge;
        }
        else if(rodzajBloku == Blok.PiasekZFlagaDziewczyn) 
        {
            prefab = Resources.Load("Prefaby/PiasekZFlaga") as GameObject;
            stronaDoUstawienia = Strona.Dziewczyna;
        }
        else if(rodzajBloku == Blok.Piasek) prefab = Resources.Load("Prefaby/Piasek") as GameObject;
        else prefab = Resources.Load("Prefaby/Pustka") as GameObject;
        GameObject blok = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
        if(stronaDoUstawienia != Strona.Niewybrana)
        {
            BlokZFlaga skrypt = blok.GetComponent<BlokZFlaga>();
            skrypt.strona = stronaDoUstawienia;
            skrypt.pozycjaBlokowa = new Vector2(x, y);
            blok.GetComponent<BlokWGrze>().rodzajBloku = daneMapy.mapa[x, y];
        }
        blok.name = "Blok(" + x + ";" + y + ")";
        blok.transform.SetParent(transform);
    }

    public void Przeskaluj(Vector2 nowyRozmiar)
    {
        int nDlugoscX = (int)nowyRozmiar.x;
        int nDlugoscY = (int)nowyRozmiar.y;
        int krotszyX = (nDlugoscX > daneMapy.dlugoscX) ? daneMapy.dlugoscX : nDlugoscX;
        int krotszyY = (nDlugoscY > daneMapy.dlugoscY) ? daneMapy.dlugoscY : nDlugoscY;
        Blok[,] nowaMapa = new Blok[nDlugoscX, nDlugoscY];
        for(int y = 0; y < krotszyY; y++)
        {
            for(int x = 0; x < krotszyX; x++)
            {
                nowaMapa[x,y] = daneMapy.mapa[x,y];
            }
        }
        /*for(int y = 0; y < nDlugoscY; y++)
        {
            for(int x = 0; x < nDlugoscX; x++)
            {
                Debug.Log(nowaMapa[x, y]);
                //if(nowaMapa[x, y] == null) nowaMapa[x, y] = Blok.Pustka;
            }
        }*/
        daneMapy.dlugoscX = nDlugoscX;
        daneMapy.dlugoscY = nDlugoscY;
        daneMapy.mapa = nowaMapa;
        obszarDoEdycji.transform.localScale = new Vector2(20 + (100 * nDlugoscX), 20 + (100 * nDlugoscY));
        GenerujMape();
    }

    public Vector2 PozycjaBlokowa(Vector2 pozycjaRealna)
    {
        return new Vector2((int)(pozycjaRealna.x + 0.5f - ((pozycjaRealna.x < 0) ? 1 : 0)), (int)(pozycjaRealna.y + 0.5f - ((pozycjaRealna.y < 0) ? 1 : 0)));
    }

    public GameObject BlokNaPozycjiJakoObiekt(Vector2 pozycjaBlokowa)
    {
        Transform tr = transform.Find("Blok(" + (int)pozycjaBlokowa.x + ";" + (int)pozycjaBlokowa.y + ")");
        if(tr != null) return tr.gameObject;
        else return null;
    }

    public Blok? BlokNaPozycji(Vector2 pozycjaBlokowa)
    {
        if(pozycjaBlokowa.x < 0 || pozycjaBlokowa.x >= daneMapy.dlugoscX || pozycjaBlokowa.y < 0 || pozycjaBlokowa.y >= daneMapy.dlugoscY) return null;
        return daneMapy.mapa[(int)pozycjaBlokowa.x, (int)pozycjaBlokowa.y];
    }

    public bool ZawieraPozycjeBlokowa(Vector2 pozycja)
    {
        return (pozycja.x >= 0 && pozycja.y >= 0 && pozycja.x < daneMapy.dlugoscX && pozycja.y < daneMapy.dlugoscY);
    }
}
