using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Mapa : MonoBehaviour
{
    public int dlugoscX;
    public int dlugoscY;
    public GameObject poleDogow;
    public GameObject poleDziewczyn;
    public GameObject menuKoncaGry;
    public Blok[,] mapa;

    public int[,] mapaRegionow;
    public int liczbaRegionow = 0;
    public List<int>[] sasiednieRegiony;
    public Vector2[] poczatkiRegionow;
    public Vector2[] konceRegionow;
    public List<Vector2[]> zapisaneSciezki;

    public int dziewczynyDoPrzyzwania = 10;
    public float czasMiedzyPrzyzwaniami = 4;

    void Start()
    {
        menuKoncaGry.SetActive(false);
        Time.timeScale = 1;
        Physics2D.gravity = Vector2.zero;
        if(DaneDoMap.daneUstawionejMapy != null)
        {
        dlugoscX = DaneDoMap.daneUstawionejMapy.dlugoscX;
        dlugoscY = DaneDoMap.daneUstawionejMapy.dlugoscY;
        mapa = new Blok[dlugoscX, dlugoscY];
        for(int x = 0; x < dlugoscX; x++)
        {
            for (int y = 0; y < dlugoscY; y++)
            {
                mapa[x, y] = DaneDoMap.daneUstawionejMapy.mapa[x, y];
            }
        }
        //Debug.Log(DaneDoMap.daneUstawionejMapy.mapa);
        /*for(int y = 0; y < dlugoscY; y++)
        {
            for(int x = 0; x < dlugoscX; x++)
            {
                Debug.Log(mapa[x,y]);
            }
        }*/
        }
        else
        {
        dlugoscX = 14;
        dlugoscY = 7;
        mapa = new Blok[dlugoscX, dlugoscY];
        for(int y = 0; y < dlugoscY; y++)
        {
            for(int x = 0; x < dlugoscX; x++)
            {
                if(x == 0 || y == 0 || x == dlugoscX-1 || y == dlugoscY-1) mapa[x, y] = Blok.Kamien;
                else mapa[x, y] = Blok.Piasek;
            }
        }
        mapa[1, dlugoscY/2] = Blok.PiasekZFlagaDogow;
        mapa[dlugoscX-2, 1] = Blok.PiasekZFlagaDziewczyn;
        mapa[dlugoscX-2, dlugoscY-2] = Blok.PiasekZFlagaDziewczyn;
        }

        GenerujMape();
        Ogolne.mapa = this;
        Gracz.punkty = 0;
        StartCoroutine(Odrodz(0));
        GenerujMapeRegionow();
        StartCoroutine(Fala(1));

        /*List<Vector2>[] pkt = WszystkiePunktyGraniczneRegionow(8, 10);
        foreach (Vector2 punkt in pkt[0])
        {
            Debug.Log(punkt.ToString());
        }
        Debug.Log("Nastepny");
        foreach (Vector2 punkt in pkt[1])
        {
            Debug.Log(punkt.ToString());
        }*/

        List<int[]> listaSciezek = WszystkieSciezkiRegionoweDoPozycji(1, 12);
        if(listaSciezek != null)
        {
            foreach (int[] sciezka in listaSciezek)
            {
                Debug.Log("Sciezka: " + TablicaNaString(sciezka));
            }
            //Debug.Log("Sciezka: " + TablicaNaString(listaSciezek[0]));
            foreach (var point in SciezkaRegionalnaNaWektorowa(listaSciezek[1]))
            {
                Debug.Log(point.ToString());
            }
            /*string drogaJakoString = "Droga: ";
            for (int i = 0; i < droga.Length; i++)
            {
                drogaJakoString += droga[i] + ", ";
            }
            Debug.Log(drogaJakoString);*/
        }
        //else Debug.Log("Brak drogi!");*/
    }

    public void GenerujMape()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for(int y = 0; y < dlugoscY; y++)
        {
            for(int x = 0; x < dlugoscX; x++)
            {
                GameObject prefab;
                Strona stronaDoUstawienia = Strona.Niewybrana;
                //Debug.Log(mapa[x, y]);
                if(mapa[x, y] == Blok.Kamien) prefab = Resources.Load("Prefaby/Kamien 1") as GameObject;
                else if(mapa[x, y] == Blok.PiasekZFlagaDogow) 
                {
                    prefab = Resources.Load("Prefaby/PiasekZFlaga 1") as GameObject;
                    stronaDoUstawienia = Strona.Doge;
                }
                else if(mapa[x, y] == Blok.PiasekZFlagaDziewczyn) 
                {
                    prefab = Resources.Load("Prefaby/PiasekZFlaga 1") as GameObject;
                    stronaDoUstawienia = Strona.Dziewczyna;
                }
                else if(mapa[x, y] == Blok.Piasek) prefab = Resources.Load("Prefaby/Piasek 1") as GameObject;
                else prefab = Resources.Load("Prefaby/Pustka") as GameObject;
                GameObject blok = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
                if(stronaDoUstawienia != Strona.Niewybrana)
                {
                    BlokZFlaga skrypt = blok.GetComponent<BlokZFlaga>();
                    skrypt.strona = stronaDoUstawienia;
                    skrypt.pozycjaBlokowa = new Vector2(x, y);
                    blok.GetComponent<BlokWGrze>().rodzajBloku = mapa[x, y];
                }
                blok.name = "Blok(" + x + ";" + y + ")";
                blok.transform.SetParent(transform); 
            }
        }
    }

    private void AnektujRegion(int anektujacy, int anektowany)
    {
        //Debug.Log(anektujacy + " anektuje " + anektowany);
        for(int y = 0; y < dlugoscY; y++)
        {
            for(int x = 0; x < dlugoscX; x++)
            {
                if(mapaRegionow[x, y] == anektowany) mapaRegionow[x, y] = anektujacy;
            }
        }
    }

    public void GenerujMapeRegionow()
    {
        mapaRegionow = new int[dlugoscX, dlugoscY];
        int obecnyRegion = 0;
        int poprzedniRegion = 0;
        for(int y = 0; y < dlugoscY; y++)
        {
            for(int x = 0; x < dlugoscX; x++)
            {
                if(Ogolne.BlokDoPrzejscia(mapa[x, y]))
                {
                    //bool wyregionowano = false;
                    if(obecnyRegion == 0)
                    {
                        obecnyRegion = poprzedniRegion + 1;
                        //wyregionowano = true;
                    }
                    mapaRegionow[x, y] = obecnyRegion;
                    if(x == dlugoscX - 1 && obecnyRegion != 0 && y + 1 < dlugoscY) //&& !wyregionowano)
                    {
                        if(Ogolne.BlokDoPrzejscia(mapa[0, y+1]))
                        {
                            poprzedniRegion = obecnyRegion;
                            obecnyRegion = obecnyRegion + 1;
                        }
                    }
                }
                else
                {
                    mapaRegionow[x, y] = 0;
                    if(obecnyRegion != 0)
                    {
                        poprzedniRegion = obecnyRegion;
                        obecnyRegion = 0;
                    }
                }
            }
        }

        /*for(int y = 0; y < dlugoscY; y++)
        {
            string wyn = "Rzadek " + y + ": ";
            for(int x = 0; x < dlugoscX; x++)
            {
                wyn += mapaRegionow[x, y];
            }
            Debug.Log(wyn);
        }*/

        for(int y = 1; y < dlugoscY; y++)
        {
            bool pasuje = false;
            bool minieto = false;
            int poprzedni = 0;
            int podlaczany = -1;
            for(int x = 0; x < dlugoscX; x++)
            {
                if(mapaRegionow[x, y] > 1 && y > 0)
                {
                    minieto = true;
                }
                if(minieto)
                {
                    if(x < dlugoscX - 1)
                    {
                    if(poprzedni != mapaRegionow[x, y])
                    {
                        if(pasuje && podlaczany != mapaRegionow[x, y - 1])
                        {
                            if(poprzedni != 0) AnektujRegion(podlaczany, poprzedni);
                            pasuje = false;
                            podlaczany = -1;
                        }
                        if(mapaRegionow[x, y - 1] != 0 && !pasuje)
                        {
                            if(x == 0)
                            {
                                pasuje = true;
                                podlaczany = mapaRegionow[x, y - 1];
                            }
                            else
                            {
                                if(mapaRegionow[x - 1, y - 1] != mapaRegionow[x, y - 1])
                                {
                                    pasuje = true;
                                    podlaczany = mapaRegionow[x, y - 1];
                                }
                            }
                        }
                        else
                        {
                            pasuje = false;
                            podlaczany = -1;
                        }
                    }
                    else
                    {
                        if(pasuje)
                        {
                            if(mapaRegionow[x, y - 1] != podlaczany)
                            {
                                pasuje = false;
                                podlaczany = -1;
                            }
                        }
                    }
                    }
                    else if(x > 0)
                    {
                        if((pasuje && podlaczany == mapaRegionow[x, y - 1] && poprzedni == mapaRegionow[x, y]) || (pasuje && podlaczany != mapaRegionow[x, y - 1] && poprzedni != mapaRegionow[x, y]))
                        {
                            if(poprzedni != 0) AnektujRegion(podlaczany, poprzedni);
                            pasuje = false;
                            podlaczany = -1;
                        }
                    }
                }
                else
                {
                    pasuje = false;
                }

                poprzedni = mapaRegionow[x, y];
            }
        }

        int id = 0;
        for(int y = 0; y < dlugoscY; y++)
        {
            for(int x = 0; x < dlugoscX; x++)
            {
                if(mapaRegionow[x, y] > id)
                {
                    id += 1;
                    if(id != mapaRegionow[x, y]) AnektujRegion(id, mapaRegionow[x, y]);
                }
            }
        }

        liczbaRegionow = id;

        sasiednieRegiony = new List<int>[liczbaRegionow];

        for (int i = 0; i < liczbaRegionow; i++)
        {
            sasiednieRegiony[i] = new List<int>();
        }
        for(int i = 0; i < liczbaRegionow; i++)
        {
            for(int j = 0; j < liczbaRegionow; j++)
            {
                if(i != j && !sasiednieRegiony[i].Contains(j+1))
                {
                    if(RegionyStykajaSie(i+1, j+1))
                    {
                        sasiednieRegiony[i].Add(j+1);
                        sasiednieRegiony[j].Add(i+1);
                    }
                }
            }

        }

        poczatkiRegionow = new Vector2[liczbaRegionow];
        konceRegionow = new Vector2[liczbaRegionow];
        int numerRegionu = 0;
        for(int y = 0; y < dlugoscY; y++)
        {
            for(int x = 0; x < dlugoscX; x++)
            {
                if(mapaRegionow[x, y] != 0)
                {
                    if(numerRegionu + 1 == mapaRegionow[x, y])
                    {
                        poczatkiRegionow[numerRegionu] = new Vector2(x, y);
                        numerRegionu++;
                    }
                    konceRegionow[mapaRegionow[x, y]-1] = new Vector2(x, y);
                }
            }
        }

        /*for (int i = 0; i < liczbaRegionow; i++)
        {
            Debug.Log("Region " + (i+1) + ": Poczatek: " + poczatkiRegionow[i].ToString() + ", Koniec: " + konceRegionow[i].ToString());
        }*/


        /*for(int y = 0; y < dlugoscY; y++)
        {
            string wyn = "Rzadek " + y + ": ";
            for(int x = 0; x < dlugoscX; x++)
            {
                wyn += mapaRegionow[x, y];
            }
            Debug.Log(wyn);
        }*/

        /*for(int i = 0; i < liczbaRegionow; i++)
        {
            string wyn = (i+1) + ": ";
            foreach (int item in sasiednieRegiony[i])
            {
                wyn += item + ", ";
            }
            Debug.Log(wyn);
        }*/
    }

    public bool RegionyStykajaSie(int region1, int region2)
    {
        for(int y = 0; y < dlugoscY; y++)
        {
            for(int x = 0; x < dlugoscX; x++)
            {
                if(mapaRegionow[x, y] == region1)
                {
                    if(x > 0)
                    {
                        if(mapaRegionow[x - 1, y] == region2) return true;
                    }
                    if(y > 0)
                    {
                        if(mapaRegionow[x, y - 1] == region2) return true;
                    }
                    if(x < dlugoscX - 1)
                    {
                        if(mapaRegionow[x + 1, y] == region2) return true;
                    }
                    if(y < dlugoscY - 1)
                    {
                        if(mapaRegionow[x, y + 1] == region2) return true;
                    }
                }
            }
        }
        return false;
    }

    public int[] NajkrotrzaRegionowaDrogaDoPozycji(int pozycjaStartowa, int pozycjaKoncowa) //(Vector2 pozycjaStartowa, Vector2 pozycjaKoncowa)
    {
        int regionStartowy = pozycjaStartowa; //mapaRegionow[(int)pozycjaStartowa.x, (int)pozycjaStartowa.y];
        int regionKoncowy = pozycjaKoncowa; //mapaRegionow[(int)pozycjaKoncowa.x, (int)pozycjaKoncowa.y];
        List<int> sciezkaStartowa = new List<int>();
        sciezkaStartowa.Add(regionStartowy);
        if(regionStartowy == regionKoncowy) return sciezkaStartowa.ToArray();
        List<int> lista = SprawdzajRegiony(regionKoncowy, sciezkaStartowa);
        if(lista != null) return lista.ToArray();
        else return null;
    }

    public List<int[]> WszystkieSciezkiRegionoweDoPozycji(int pozycjaStartowa, int pozycjaKoncowa) //(Vector2 pozycjaStartowa, Vector2 pozycjaKoncowa)
    {
        int regionStartowy = pozycjaStartowa; //mapaRegionow[(int)pozycjaStartowa.x, (int)pozycjaStartowa.y];
        int regionKoncowy = pozycjaKoncowa; //mapaRegionow[(int)pozycjaKoncowa.x, (int)pozycjaKoncowa.y];
        List<int[]> zbiorSciezek = new List<int[]>();
        List<int> sciezkaStartowa = new List<int>();
        sciezkaStartowa.Add(regionStartowy);
        if(regionStartowy == regionKoncowy) 
        {
            zbiorSciezek.Add(sciezkaStartowa.ToArray());
            return zbiorSciezek;
        }
        zbiorSciezek = WydajWszystkieRegiony(regionKoncowy, sciezkaStartowa);
        zbiorSciezek.RemoveAll(s => s == null);
        if(zbiorSciezek.Count > 0) return zbiorSciezek;
        else return null;
    }

    public List<int> SprawdzajRegiony(int cel, List<int> obecnaSciezka) // Wyspecjalizowna dla NajkrotrzaRegionowaDrogaDoPozycji?
    {
        //if(obecnaSciezka[obecnaSciezka.Count-1] == cel) return obecnaSciezka;
        List<int> wyn = null;
        //Debug.Log("Wywolac (razy): " + sasiednieRegiony[obecnaSciezka[obecnaSciezka.Count-1] - 1].Count);
        int powtorki = sasiednieRegiony[obecnaSciezka[obecnaSciezka.Count-1] - 1].Count;
        //Debug.Log(powtorki);
        for(int i = 0; i < powtorki; i++)
        {
            //Debug.Log("I: " + i);
            //Debug.Log("Liczba sasiadow dla " + obecnaSciezka[obecnaSciezka.Count-1] + ": " + sasiednieRegiony[obecnaSciezka[obecnaSciezka.Count-1] - 1].Count);
            //Debug.Log("Sasiedzi: " + (ListaNaString(sasiednieRegiony[obecnaSciezka[obecnaSciezka.Count-1] - 1])));
            int region = (sasiednieRegiony[obecnaSciezka[obecnaSciezka.Count-1] - 1])[i];
            if(!obecnaSciezka.Contains(region))
            {
                //Debug.Log("Region dla " + obecnaSciezka[obecnaSciezka.Count-1] + ": " + region);
                List<int> nowaSciezka = new List<int>();
                for (int j = 0; j < obecnaSciezka.Count; j++)
                {
                    nowaSciezka.Add(obecnaSciezka[j]);
                }
                nowaSciezka.Add(region);
                //Debug.Log("Sciezka: " + ListaNaString(nowaSciezka));
                if(region == cel) return nowaSciezka;
                List<int> lista = SprawdzajRegiony(cel, nowaSciezka);
                //Debug.Log("Dotarlem! Obecna lista: " + );
                if(lista != null && wyn == null) wyn = lista;
                else if(lista != null && wyn != null)
                {
                    if(lista.Count < wyn.Count) wyn = lista;
                }
            }
        }
        return wyn;
    }

    public List<int[]> WydajWszystkieRegiony(int cel, List<int> obecnaSciezka) // Wyspecjalizowna dla WszystkieSciezkiRegionoweDoPozycji?
    {
        List<int[]> wyn = new List<int[]>();
        int powtorki = sasiednieRegiony[obecnaSciezka[obecnaSciezka.Count-1] - 1].Count;
        for(int i = 0; i < powtorki; i++)
        {
            int region = (sasiednieRegiony[obecnaSciezka[obecnaSciezka.Count-1] - 1])[i];
            if(!obecnaSciezka.Contains(region))
            {
                List<int> nowaSciezka = new List<int>();
                for (int j = 0; j < obecnaSciezka.Count; j++)
                {
                    nowaSciezka.Add(obecnaSciezka[j]);
                }
                nowaSciezka.Add(region);
                if(region == cel) 
                {
                    wyn.Add(nowaSciezka.ToArray());
                    return wyn;
                }
                List<int[]> listaSciezek = WydajWszystkieRegiony(cel, nowaSciezka);
                foreach (int[] sciezka in listaSciezek)
                {
                    wyn.Add(sciezka);
                }
            }
        }
        if(wyn.Count > 0) return wyn;
        else 
        {
            wyn.Add(null);
            return wyn;
        }
    }

    public List<Vector2>[] WszystkiePunktyGraniczneRegionow(int region1, int region2)
    {
        if(!sasiednieRegiony[region1-1].Contains(region2)) return null;
        List<Vector2>[] wyn = new List<Vector2>[2];
        List<Vector2> punktyPierwszego = new List<Vector2>();
        List<Vector2> punktyDrugiego = new List<Vector2>();
        for (int y = 0; y < dlugoscY; y++)
        {
            for (int x = 0; x < dlugoscX; x++)
            {
                if(x > 0)
                {
                    if(mapaRegionow[x, y] == region1 && mapaRegionow[x-1, y] == region2) 
                    {
                        punktyPierwszego.Add(new Vector2(x, y));
                        punktyDrugiego.Add(new Vector2(x-1, y));
                    }
                }
                if(y > 0)
                {
                    if(mapaRegionow[x, y] == region1 && mapaRegionow[x, y-1] == region2) 
                    {
                        punktyPierwszego.Add(new Vector2(x, y));
                        punktyDrugiego.Add(new Vector2(x, y-1));
                    }
                }
                if(x < dlugoscX-1)
                {
                    if(mapaRegionow[x, y] == region1 && mapaRegionow[x+1, y] == region2) 
                    {
                        punktyPierwszego.Add(new Vector2(x, y));
                        punktyDrugiego.Add(new Vector2(x+1, y));
                    }
                }
                if(y < dlugoscY-1)
                {
                    if(mapaRegionow[x, y] == region1 && mapaRegionow[x, y+1] == region2) 
                    {
                        punktyPierwszego.Add(new Vector2(x, y));
                        punktyDrugiego.Add(new Vector2(x, y+1));
                    }
                }
            }
        }
        wyn[0] = punktyPierwszego;
        wyn[1] = punktyDrugiego;
        return wyn;
    }

    public Vector2[] SciezkaRegionalnaNaWektorowa(int[] sciezkaRegionalna)
    {
        List<Vector2> wyn = new List<Vector2>();
        Vector2 srodekPoczatkowegoRegionu = new Vector2((poczatkiRegionow[sciezkaRegionalna[0]-1].x + konceRegionow[sciezkaRegionalna[0]-1].x)/2, (poczatkiRegionow[sciezkaRegionalna[0]-1].y + konceRegionow[sciezkaRegionalna[0]-1].y)/2);
        int ostatni = sciezkaRegionalna[sciezkaRegionalna.Length-1]-1;
        Vector2 srodekKoncowegoRegionu = new Vector2((poczatkiRegionow[ostatni].x + konceRegionow[ostatni].x)/2, (poczatkiRegionow[ostatni].y + konceRegionow[ostatni].y)/2);
        //Debug.Log("Środek 1: " + srodekPoczatkowegoRegionu.ToString());
        //Debug.Log("Środek 2: " + srodekKoncowegoRegionu.ToString());
        wyn.Add(srodekPoczatkowegoRegionu);
        for(int i = 0; i < sciezkaRegionalna.Length-1; i++)
        {
            List<Vector2>[] punktyGraniczne = WszystkiePunktyGraniczneRegionow(sciezkaRegionalna[i], sciezkaRegionalna[i+1]);
            List<Vector2> pierwsze = punktyGraniczne[0];
            List<Vector2> drugie = punktyGraniczne[1];
            //Debug.Log(pierwsze.Count);
            //Debug.Log(drugie.Count);
            string pierw = "";
            string drug = "";
            for(int j = 0; j < pierwsze.Count; j++)
            {
                pierw += pierwsze[j].ToString() + " ";
                drug += drugie[j].ToString() + " ";
            }
            //Debug.Log("Regiony " + sciezkaRegionalna[i] + " i " + sciezkaRegionalna[i+1] + ": " + pierw + " : " + drug);
            if(pierwsze.Count == 1)
            {
                wyn.Add(pierwsze[0]);
                wyn.Add(drugie[0]);
            }
            else
            {
                Vector2 pierwszy = pierwsze[0];
                Vector2 pierwszyPorownawczy = new Vector2((poczatkiRegionow[sciezkaRegionalna[i]-1].x + konceRegionow[sciezkaRegionalna[i]-1].x)/2, (poczatkiRegionow[sciezkaRegionalna[i]-1].y + konceRegionow[sciezkaRegionalna[i]-1].y)/2);
                Vector2 drugiPorownawczy;
                if(i == sciezkaRegionalna.Length-2) drugiPorownawczy = srodekKoncowegoRegionu;
                else drugiPorownawczy = new Vector2((poczatkiRegionow[sciezkaRegionalna[i+1]-1].x + konceRegionow[sciezkaRegionalna[i+1]-1].x)/2, (poczatkiRegionow[sciezkaRegionalna[i+1]-1].y + konceRegionow[sciezkaRegionalna[i+1]-1].y)/2);
                float dystansPierwszegoOdPoczatkowego = Vector2.Distance(pierwszyPorownawczy, pierwszy);
                Vector2 drugi = drugie[0];
                float dystansDrugiegoOdKoncowego = Vector2.Distance(drugiPorownawczy, drugi);
                for(int j = 1; j < pierwsze.Count; j++)
                {
                    float nowyDystansPierwszegoOdPoczatkowego = Vector2.Distance(pierwszyPorownawczy, pierwsze[j]);
                    float nowyDystansDrugiegoOdKoncowego = Vector2.Distance(drugiPorownawczy, drugie[j]);
                    if(dystansPierwszegoOdPoczatkowego + dystansDrugiegoOdKoncowego > nowyDystansPierwszegoOdPoczatkowego + nowyDystansDrugiegoOdKoncowego)
                    {
                        pierwszy = pierwsze[j];
                        dystansPierwszegoOdPoczatkowego = nowyDystansPierwszegoOdPoczatkowego;
                        drugi = drugie[j];
                        dystansDrugiegoOdKoncowego = nowyDystansDrugiegoOdKoncowego;
                    }
                }
                wyn.Add(pierwszy);
                wyn.Add(drugi);
            }
        }
        wyn.Add(srodekKoncowegoRegionu);
        return wyn.ToArray();
    }

    public string ListaNaString(List<int> lista)
    {
        string wyn = "[";
        foreach (int punkt in lista)
        {
            wyn += punkt + ", ";
        }
        return wyn + "]";
    } 

    public string TablicaNaString(int[] tablica)
    {
        string wyn = "[";
        foreach (int punkt in tablica)
        {
            wyn += punkt + ", ";
        }
        return wyn + "]";
    } 

    public GameObject BlokNaPozycjiJakoObiekt(Vector2 pozycjaBlokowa)
    {
        Transform tr = transform.Find("Blok(" + (int)pozycjaBlokowa.x + ";" + (int)pozycjaBlokowa.y + ")");
        if(tr != null) return tr.gameObject;
        else return null;
    }

    public GameObject[] DogiJakoObiekty()
    {
        List<GameObject> dzieci = new List<GameObject>();
        foreach (Transform t in poleDogow.transform)
        {
            dzieci.Add(t.gameObject);
        }
        return dzieci.ToArray();
    }

    public Doge[] DogiJakoDogi()
    {
        List<Doge> dzieci = new List<Doge>();
        foreach (Transform t in poleDogow.transform)
        {
            dzieci.Add(t.gameObject.GetComponent<Doge>());
        }
        return dzieci.ToArray();
    }

    public Dziewczyna[] DziewczynyJakoDziewczyny()
    {
        List<Dziewczyna> dzieci = new List<Dziewczyna>();
        foreach (Transform t in poleDziewczyn.transform)
        {
            dzieci.Add(t.gameObject.GetComponent<Dziewczyna>());
        }
        return dzieci.ToArray();
    }

    public GameObject NajblizszaFlagaWroga(Strona strona)
    {
        GameObject[] flagi = GameObject.FindGameObjectsWithTag("BlokZFlaga");
        //List<GameObject> flagiL = new List<GameObject>();
        /*foreach (Transform child in transform)
        {
            if()
        }*/
        if(flagi.Length > 0)
        {
            GameObject najblizszaFlaga = null;
            float dystans = 0;
            foreach(GameObject flaga in flagi)
            {
                if(najblizszaFlaga == null && flaga.GetComponent<BlokZFlaga>().strona != strona)
                {
                    najblizszaFlaga = flaga;
                    dystans = Vector2.Distance(flaga.transform.localPosition, transform.localPosition);
                }
                else
                {
                    float nowyDystans = Vector2.Distance(flaga.transform.localPosition, transform.localPosition);
                    if(nowyDystans < dystans && flaga.GetComponent<BlokZFlaga>().strona != strona)
                    {
                        najblizszaFlaga = flaga;
                        dystans = nowyDystans;
                    }
                }
            }
            return najblizszaFlaga;
        }
        return null;
    }

    Postac Przyzwij(GameObject postac, Vector2 pozycja, bool czyAI)
    {
        //GameObject prefab = Resources.Load<GameObject>("Prefaby/Dogi/" + postac);
        //if(prefab == null) prefab = Resources.Load<GameObject>("Prefaby/" + postac);
        GameObject przyzwany = Instantiate(postac, pozycja, Quaternion.identity);
        Postac skrypt = przyzwany.GetComponent<Doge>();
        if(skrypt != null) 
        {
            Doge d = (Doge)skrypt;
            przyzwany.transform.SetParent(poleDogow.transform);
            d.czyAI = czyAI;
        }
        else
        {
            skrypt = przyzwany.GetComponent<Dziewczyna>();
            if(skrypt != null)
            {
                przyzwany.transform.SetParent(poleDziewczyn.transform);
            }
        }
        skrypt.nazwa = postac.name;
        return przyzwany.GetComponent<Postac>();
    }

    Postac PostawWPrzypadkowymMiejscu(GameObject postac, bool czyAI)
    {
        List<Vector2> miejscaStawiania = new List<Vector2>();
        if(postac.GetComponent<Doge>() != null)
        {
            for(int y = 0; y < dlugoscY; y++)
            {
            for(int x = 0; x < dlugoscX; x++)
            {
                if(mapa[x, y] == Blok.PortalDogow || (mapa[x, y] == Blok.PiasekZFlagaDogow && BlokNaPozycjiJakoObiekt(new Vector2(x, y)).GetComponent<BlokZFlaga>().aktywny)) 
                //miejscaStawiania.Add(new Vector2(0.5f + (float)x - ((float)dlugoscX)/2, 0.5f + (float)y - ((float)dlugoscY)/2));
                miejscaStawiania.Add(new Vector2(x, y));
            }
            }
        }
        else
        {
            for(int y = 0; y < dlugoscY; y++)
            {
            for(int x = 0; x < dlugoscX; x++)
            {
                if(mapa[x, y] == Blok.PortalDziewczyn || (mapa[x, y] == Blok.PiasekZFlagaDziewczyn && BlokNaPozycjiJakoObiekt(new Vector2(x, y)).GetComponent<BlokZFlaga>().aktywny)) 
                //miejscaStawiania.Add(new Vector2(0.5f + (float)x - ((float)dlugoscX)/2, 0.5f + (float)y - ((float)dlugoscY)/2));
                miejscaStawiania.Add(new Vector2(x, y));
            }
            }
        }
        if(miejscaStawiania.Count < 1) return null;
        Vector2 przypadkoweMiejsce = miejscaStawiania[Random.Range(0, miejscaStawiania.Count)];
        //Debug.Log(przypadkoweMiejsce.ToString());
        return Przyzwij(postac, przypadkoweMiejsce, czyAI);
    }

    public IEnumerator Odrodz(float czasDoOdrodzenia)
    {
        yield return new WaitForSeconds(czasDoOdrodzenia);
        Gracz.glownyDoge = PostawWPrzypadkowymMiejscu(Resources.Load<GameObject>("Prefaby/Dogi/" + Gracz.listaDogow[Gracz.numerDoga]), false) as Doge;
        if(Gracz.glownyDoge == null) 
        {
            foreach(GameObject blok in GameObject.FindGameObjectsWithTag("BlokZFlaga"))
            {
                blok.GetComponent<BlokZFlaga>().aktywny = false;
            }
            KoniecGry();
            Time.timeScale = 0;
        }
    }

    public Vector2 PozycjaBlokowa(Vector2 pozycjaRealna)
    {
        return new Vector2((int)(pozycjaRealna.x + 0.5f - ((pozycjaRealna.x < 0) ? 1 : 0)), (int)(pozycjaRealna.y + 0.5f - ((pozycjaRealna.y < 0) ? 1 : 0)));
    }

    /*public Vector2 PozycjaRealna(Vector2 pozycjaBlokowa)
    {
        return new Vector2((int)(pozycjaBlokowa.x - 0.5f + ((pozycjaBlokowa.x < 0) ? 1 : 0)), (int)(pozycjaBlokowa.y - 0.5f + ((pozycjaBlokowa.y < 0) ? 1 : 0)));
    }*/

    public bool BlokNaPozycjiJestDoPrzejscia(Vector2 pozycjaBlokowa)
    {
        Blok? blok = BlokNaPozycji(pozycjaBlokowa);
        return Ogolne.BlokDoPrzejscia(blok);
    }

    public Blok? BlokNaPozycji(Vector2 pozycjaBlokowa)
    {
        if(pozycjaBlokowa.x < 0 || pozycjaBlokowa.x >= dlugoscX || pozycjaBlokowa.y < 0 || pozycjaBlokowa.y >= dlugoscY) return null;
        return mapa[(int)pozycjaBlokowa.x, (int)pozycjaBlokowa.y];
    }

    /*public bool ZawieraPozycjeBlokowa(Vector2 pozycja)
    {
        return (pozycja.x >= 0 && pozycja.y >= 0 && pozycja.x < dlugoscX && pozycja.y < dlugoscY);
    }*/

    public void Wybuch(Vector2 zrodlo, int maksymalneObrazenia, float rozmiar, Postac wysadzajacy)
    {
        Instantiate(Resources.Load<GameObject>("Prefaby/ChmuraWybuchu"), zrodlo, Quaternion.identity).GetComponent<ChmuraWybuchu>().maksymalnyRozmiar = 1.5f;
        GameObject[] postacie = GameObject.FindGameObjectsWithTag("Postac");
        foreach(GameObject postac in postacie)
        {
            float odleglosc = Vector2.Distance((Vector2)postac.transform.localPosition, zrodlo);
            if(odleglosc < rozmiar)
            {
                Postac postacSkrypt = null;
                if(postac != null) postacSkrypt = postac.GetComponent<Postac>();
                if(postacSkrypt != null) postacSkrypt.Zran((int)((float)maksymalneObrazenia * (rozmiar - odleglosc) / rozmiar), wysadzajacy);
            }
        }
        /*foreach(Dziewczyna dziewczyna in dziewczyny)
        {
            float odleglosc = Vector2.Distance((Vector2)dziewczyna.transform.localPosition, zrodlo);
            if(odleglosc < rozmiar)
            {
                dziewczyna.Zran((int)((float)maksymalneObrazenia * (rozmiar - odleglosc) / rozmiar));
            }
        }*/
        GameObject[] chmury = GameObject.FindGameObjectsWithTag("ChmuraGazu");
        foreach(GameObject chmura in chmury)
        {
            if(chmura != null)
            {
                float odleglosc = Vector2.Distance((Vector2)chmura.transform.localPosition, zrodlo);
                if(odleglosc < rozmiar)
                {
                    if(chmura != null) chmura.GetComponent<ChmuraGazu>().Zapal();
                }
            }
        }
    }

    public IEnumerator Fala(int dziewczyny)
    {
        GameObject dziewczyna = Resources.Load<GameObject>("Prefaby/DoomerBaba");
        //Debug.Log(dziewczyny);
        while(dziewczyny > 0)
        {
            //Debug.Log("Baba");
            yield return new WaitForSeconds(czasMiedzyPrzyzwaniami);
            PostawWPrzypadkowymMiejscu(dziewczyna, false);
            if(czasMiedzyPrzyzwaniami > 1.3f) czasMiedzyPrzyzwaniami -= 0.3f;
        }
    }

    public void KoniecGry()
    {
        menuKoncaGry.SetActive(true);
        GameObject.Find("Canvas/ObecnyWynik").SetActive(false);
        menuKoncaGry.transform.Find("Wynik").gameObject.GetComponent<Text>().text = "Wynik: " + Gracz.punkty;
    }
}