using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulka : Postac
{
    public int obrazenia;
    public Postac wlasciciel;
    GameObject cel;
    Vector2 punktDoZblizania;

    void Start()
    {
        nazwa = "Bulka";
        hp = 50;
        maksHp = 50;
        predkosc = 2.5f;
    }

    void FixedUpdate()
    {
        if(cel == null)
        {
            Postac najblizszyWrog = NajblizszyWrog();
            if(najblizszyWrog != null) cel = najblizszyWrog.gameObject;
        }
        else
        {
            punktDoZblizania = (Vector2)cel.transform.localPosition;
            if(wlasciciel != null)
            {
            if((Vector2.Distance(punktDoZblizania, transform.localPosition) > 0.5f && !cel.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>())) || Vector2.Distance(wlasciciel.transform.localPosition, transform.localPosition) < 3)
            {
                PoruszajSieWKierunku(predkosc, punktDoZblizania - (Vector2)transform.localPosition);
                //PoruszajSieWKierunku(predkosc, punktDoZblizania - (Vector2)transform.localPosition);
                /*if((punktDoZblizania.x > transform.localPosition.x && transform.localScale.x < 0) || (punktDoZblizania.x < transform.localPosition.x && transform.localScale.x > 0)) 
                transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);*/
            }
            else
            {
                Zabij(null);
            }
            }
            else
            {
            if(Vector2.Distance(punktDoZblizania, transform.localPosition) > 0.5f)
            {
                PoruszajSieWKierunku(predkosc, punktDoZblizania - (Vector2)transform.localPosition);
            }
            else
            {
                Zabij(null);
            }
            }
        }
    }

    /*public override void Zran(int obrazenia, Postac postac)
    {
        if(hp <= 0) return;
        hp -= 
        Zabij(postac);
    }*/

    public override void Zabij(Postac postac)
    {
        if(zabity) return;
        zabity = true;
        Ogolne.mapa.Wybuch((Vector2)transform.localPosition, obrazenia, 3, wlasciciel);
        Destroy(gameObject);
    }
}
