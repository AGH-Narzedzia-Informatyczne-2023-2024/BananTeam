using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KolejnyRzultyPiesPosting : Doge
{
    GameObject butelkaGazu;
    float czasOstatniegoWystrzelenia;
    float czasPodniesieniaButelki = 0;

    new void Start()
    {
        base.Start();
        hp = 100;
        maksHp = 100;
        zaladowanie[0] = 100f;
        minimumDoStartuUzycia[0] = 15f;
        czasMiedzyUzyciami[0] = 0;
        wartoscUzupelniania[0] = 1;
        czasMiedzyUzupelnieniami[0] = 0.1f;
        czasOstatniegoUzycia[0] = -100f;

        zaladowanie[1] = 0f;
        minimumDoStartuUzycia[1] = 100f;
        czasMiedzyUzyciami[1] = 0f;
        wartoscUzupelniania[1] = 0.5f;
        czasMiedzyUzupelnieniami[1] = 0.1f;
        czasOstatniegoUzycia[1] = -100f;

        butelkaGazu = transform.Find("Gaz").gameObject;
    }

    new void Update()
    {
        base.Update();
        if(Input.GetMouseButton(0) && (CzyMozeUzycUmiejetnosci(0) || CzyMozeKontynuowacUzywanieUmiejetnosci(0)))
        {
            if(czyUzywa[0] == false) czyUzywa[0] = true;
            if(czasOstatniegoWystrzelenia + 0.33f <= Time.time && zaladowanie[0] >= 15)
            {
                czasOstatniegoWystrzelenia = Time.time;
                ChmuraOgnia plomien;
                if(transform.localScale.x > 0)
                {
                    plomien = Instantiate(Resources.Load<GameObject>("Prefaby/ChmuraOgnia"), new Vector2(transform.localPosition.x + 1, transform.localPosition.y + 0.2f), Quaternion.identity).GetComponent<ChmuraOgnia>();
                    plomien.przyspieszenie = new Vector3(0.06f, 0, 0);
                }
                else 
                {
                    plomien = Instantiate(Resources.Load<GameObject>("Prefaby/ChmuraOgnia"), new Vector2(transform.localPosition.x - 1, transform.localPosition.y + 0.2f), Quaternion.identity).GetComponent<ChmuraOgnia>();
                    plomien.przyspieszenie = new Vector3(-0.06f, 0, 0);
                }
                plomien.obrazenia = 35;
                plomien.tworca = this;
                Uzycie(0, 15);
            }
            //Uzycie(0, 5);
        }
        else
        {
            if(czyUzywa[0] == true) czyUzywa[0] = false;
        }

        if(Input.GetMouseButtonDown(1) && CzyMozeUzycUmiejetnosci(1))
        {
            butelkaGazu.SetActive(true);
            butelkaGazu.transform.localPosition = (Vector2)butelkaGazu.transform.localPosition + new Vector2(-1, 2.9f);
            butelkaGazu.transform.eulerAngles = new Vector3(0, 0, (transform.localScale.x > 0) ? 210 : -210);
            czasPodniesieniaButelki = Time.time;
        }

        if(Input.GetMouseButtonUp(1) && czasPodniesieniaButelki != 0)
        {
            if(Time.time - czasPodniesieniaButelki < 0.3f)
            {
                StartCoroutine(RzucButelka(0));
            }
            else
            {
                StartCoroutine(RzucButelka((transform.localScale.x > 0) ? 1 : -1));
            }
            czasPodniesieniaButelki = 0;
            zaladowanie[1] = 0;
        }

        /*if(Input.GetMouseButtonDown(1) && CzyMozeUzycUmiejetnosci(1))
        {
            Vector2 pozycjaBlokowa = Ogolne.mapa.PozycjaBlokowa((Vector2)transform.localPosition);
            //Debug.Log(pozycjaBlokowa.ToString());
            for(int x = (int)pozycjaBlokowa.x-1; x <= (int)pozycjaBlokowa.x+1; x++)
            {
                for(int y = (int)pozycjaBlokowa.y-1; y <= (int)pozycjaBlokowa.y+1; y++)
                {
                    if(Ogolne.mapa.BlokNaPozycjiJestDoPrzejscia(new Vector2(x, y)))
                    {
                        GameObject chmura = Instantiate(Resources.Load<GameObject>("Prefaby/ChmuraGazu"), Ogolne.mapa.PozycjaBlokowa(new Vector2(x, y)), Quaternion.identity);
                        ChmuraGazu skrypt = chmura.GetComponent<ChmuraGazu>();
                        skrypt.obrazenia = 10;
                        skrypt.tworca = this;
                    }
                }
            }
            zaladowanie[1] = 0;
        }*/
    }

    public IEnumerator RzucButelka(int numerRzutu)
    {
        if(numerRzutu == 0) 
        {
            int numerZwrotu = (transform.localScale.x > 0) ? 1 : -1;
            for(int i = 0; i < 10; i++)
            {
                butelkaGazu.transform.position = (Vector2)butelkaGazu.transform.position + new Vector2(numerZwrotu * 0.04f, -0.075f);
                butelkaGazu.transform.Rotate(0, 0, (-180)/10, Space.Self);
                yield return new WaitForSeconds(0.02f);
            }
            RozbijButelke(transform.localPosition);
        }
        else
        {
            Butelka skrypt = butelkaGazu.GetComponent<Butelka>();
            skrypt.Rzucona(numerRzutu);
            skrypt.wlasciciel = this;
        }
    }

    public void RozbijButelke(Vector2 miejsce)
    {
        butelkaGazu.transform.SetParent(transform);
        butelkaGazu.transform.localPosition = Vector2.zero;
        butelkaGazu.transform.eulerAngles = new Vector3(0, 0, 0);
        butelkaGazu.SetActive(false);
        Vector2 pozycjaBlokowa = Ogolne.mapa.PozycjaBlokowa(miejsce);
        for(int x = (int)pozycjaBlokowa.x-1; x <= (int)pozycjaBlokowa.x+1; x++)
        {
            for(int y = (int)pozycjaBlokowa.y-1; y <= (int)pozycjaBlokowa.y+1; y++)
            {
                if(Ogolne.mapa.BlokNaPozycjiJestDoPrzejscia(new Vector2(x, y)))
                {
                    GameObject chmura = Instantiate(Resources.Load<GameObject>("Prefaby/ChmuraGazu"), new Vector2(x, y)/*Ogolne.mapa.PozycjaBlokowa(new Vector2(x, y))*/, Quaternion.identity);
                    ChmuraGazu skrypt = chmura.GetComponent<ChmuraGazu>();
                    skrypt.obrazenia = 10;
                    skrypt.tworca = this;
                }
            }
        }
    }
}
