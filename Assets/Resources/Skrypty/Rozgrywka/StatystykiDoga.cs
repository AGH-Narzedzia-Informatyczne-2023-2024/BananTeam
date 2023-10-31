using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatystykiDoga : MonoBehaviour
{
    Doge dogeSkrypt;
    GameObject zdr;
    GameObject umj1;
    GameObject umj2;
    SpriteRenderer zdrI;
    SpriteRenderer umj1I;
    SpriteRenderer umj2I;
    Sprite czerwony;
    Sprite zielony;

    void Start()
    {
        dogeSkrypt = transform.parent.gameObject.GetComponent<Doge>();
        zdr = transform.Find("Zdrowie").gameObject;
        umj1 = transform.Find("Umiejetnosc1").gameObject;
        umj2 = transform.Find("Umiejetnosc2").gameObject;
        zdrI = zdr.GetComponent<SpriteRenderer>();
        umj1I = umj1.GetComponent<SpriteRenderer>();
        umj2I = umj2.GetComponent<SpriteRenderer>();
        czerwony = Resources.Load<Sprite>("Grafiki/red_pixel");
        zielony = Resources.Load<Sprite>("Grafiki/green_pixel");
    }

    void Update()
    {
        if((dogeSkrypt.gameObject.transform.localScale.x < 0 && transform.localScale.x > 0) || (dogeSkrypt.gameObject.transform.localScale.x > 0 && transform.localScale.x < 0))
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        if(umj1.transform.localScale.x != dogeSkrypt.zaladowanie[0])
        {
            umj1.transform.localScale = new Vector2(dogeSkrypt.zaladowanie[0], umj1.transform.localScale.y);
        }
        int oczekiwanyProcentHp = (int)Ogolne.Procent(dogeSkrypt.hp, dogeSkrypt.maksHp);
        if(zdr.transform.localScale.x != oczekiwanyProcentHp)
        {
            zdr.transform.localScale = new Vector2(oczekiwanyProcentHp, zdr.transform.localScale.y);
        }
        if(dogeSkrypt.CzyMozeUzycUmiejetnosci(0))
            {
                if(umj1I.sprite != zielony)
                {
                    umj1I.sprite = zielony;
                }
            }
            else
            {
                if(umj1I.sprite != czerwony)
                {
                    umj1I.sprite = czerwony;
                }
            }
        if(umj2.transform.localScale.x != dogeSkrypt.zaladowanie[1])
        {
            umj2.transform.localScale = new Vector2(dogeSkrypt.zaladowanie[1], umj2.transform.localScale.y);
        }
        if(dogeSkrypt.CzyMozeUzycUmiejetnosci(1))
            {
                if(umj2I.sprite != zielony)
                {
                    umj2I.sprite = zielony;
                }
            }
            else
            {
                if(umj2I.sprite != czerwony)
                {
                    umj2I.sprite = czerwony;
                }
            }
    }
}
