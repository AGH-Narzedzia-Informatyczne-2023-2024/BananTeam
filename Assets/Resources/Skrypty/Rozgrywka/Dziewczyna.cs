using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dziewczyna : Postac
{
    public GameObject cel;
    public Vector2 punktDoZblizania;
    public bool atakuje;
    public Collider2D koliderCelu;
    GameObject cialo;
    void Start()
    {
        strona = Strona.Dziewczyna;
        predkosc = 1.5f;
        predkoscPoczatkowa=predkosc;
        atakuje = false;
        maksHp=100;
        cialo = transform.Find("CialoDziewczyny").gameObject;
        //punktDoZblizania = new Vector2(0, 0);
    }

    void FixedUpdate()
    {
        if(cel == null)
        {
            koliderCelu = null;
            cel = Ogolne.mapa.NajblizszaFlagaWroga(strona);
            if(Random.Range(0, 2) == 1 || cel == null) 
            {
                Postac wrog = NajblizszyWrog();
                if(wrog != null) 
                {
                    cel = wrog.gameObject;
                    koliderCelu = cel.GetComponent<Collider2D>();
                }
            }
        }
        else
        {
            punktDoZblizania = (Vector2)cel.transform.localPosition;
            if(Vector2.Distance(punktDoZblizania, transform.localPosition) > 1 && ((koliderCelu != null) ? !koliderCelu.IsTouching(GetComponent<Collider2D>()) : true))
            {
                PoruszajSieWKierunku(predkosc, punktDoZblizania - (Vector2)transform.localPosition);
                if((punktDoZblizania.x > transform.localPosition.x && transform.localScale.x < 0) || (punktDoZblizania.x < transform.localPosition.x && transform.localScale.x > 0)) 
                transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
            }
            else
            {
                StartCoroutine(ProstyAtak());
            }

            BlokZFlaga blok = cel.GetComponent<BlokZFlaga>();
            if(blok != null)
            {
                if(blok.strona == strona) cel = null;
            }
            /*if(!Ogolne.Okolo(punktDoZblizania.x, transform.localPosition.x, 1))
            {
                if(punktDoZblizania.x > transform.localPosition.x) Idz(predkosc, 0);
                else if(punktDoZblizania.x < transform.localPosition.x) Idz(-predkosc, 0);
            }
            if(!Ogolne.Okolo(punktDoZblizania.y, transform.localPosition.y, 1))
            {
                if(punktDoZblizania.y > transform.localPosition.y) Idz(0, predkosc);
                else if(punktDoZblizania.y < transform.localPosition.y) Idz(0, -predkosc);
            }*/
        }
    }

    public override void Zran(int obrazenia, Postac raniacy)
    {
        base.Zran(obrazenia, raniacy);
        if(raniacy != null && cel != null && !zabity)
        {
            if(raniacy.gameObject != null && raniacy.gameObject != cel)
            {
            if(Vector2.Distance(raniacy.gameObject.transform.localPosition, transform.localPosition) <= Vector2.Distance(cel.transform.localPosition, transform.localPosition))
            {
                cel = raniacy.gameObject;
            }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D kolizja)
    {
        Postac postac = kolizja.gameObject.GetComponent<Postac>();
        if(postac != null) 
        {
            if(postac.strona != strona)
            {
                cel = kolizja.gameObject;
                koliderCelu = kolizja.collider;
            }
        }
    }

    public IEnumerator ProstyAtak()
    {
        if(!atakuje)
        {
        atakuje = true;
        cialo.transform.localRotation = Quaternion.identity;
        float obrot = -3.5f; //((transform.localScale.x < 0) ? 3.5f : -3.5f);
        for(int i = 0; i < 10; i++)
        {
            cialo.transform.Rotate(0, 0, -(obrot/7), Space.Self);
            yield return new WaitForSeconds(0.01f);
        }
        for(int i = 0; i < 5; i++)
        {
            cialo.transform.Rotate(0, 0, 2*(obrot + (obrot/7)), Space.Self);
            yield return new WaitForSeconds(0.01f);
        }
        if(cel != null)
        {
        if(Vector2.Distance(cel.transform.localPosition, transform.localPosition) <= 1)
        {
            Postac postac = cel.GetComponent<Postac>();
            if(postac != null) postac.Zran(20, this);
            else
            {
                BlokZFlaga blok = cel.GetComponent<BlokZFlaga>();
                if(blok != null) 
                {
                    blok.Zamien(strona);
                    cel = null;
                }
            }
        }
        else if(cel.GetComponent<Collider2D>() != null)
        {
            if(cel.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
            {
                Postac postac = cel.GetComponent<Postac>();
                if(postac != null) postac.Zran(20, this);
            }
        }
        }
        for(int i = 0; i < 10; i++)
        {
            cialo.transform.Rotate(0, 0, -obrot, Space.Self);
            yield return new WaitForSeconds(0.02f);
        }
        cialo.transform.localRotation = Quaternion.identity;
        atakuje = false;
        }
        yield return null;
    }
}
