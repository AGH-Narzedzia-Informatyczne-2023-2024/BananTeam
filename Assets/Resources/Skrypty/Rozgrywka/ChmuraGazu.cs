using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChmuraGazu : MonoBehaviour
{
    public int obrazenia;
    Collider2D koliderChmury;
    public Postac tworca;
    float czasStworzenia;
    SpriteRenderer sr;
    bool zapalony = false;
    void Start()
    {
        koliderChmury = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        czasStworzenia = Time.time;
    }

    void Update()
    {
        //Debug.Log(widocznosc);
        if(czasStworzenia + 9 <= Time.time) sr.color = new Color(0.4822176f, 0.6226415f, 0.03230687f, sr.color.a - 0.0012f);
        if(czasStworzenia + 12 <= Time.time)
        {
            //Debug.Log("Koniec");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D kolider)
    {
        if(kolider.gameObject.GetComponent<Postac>() != null && kolider.gameObject.GetComponent<KolejnyRzultyPiesPosting>() == null) StartCoroutine(Truj(kolider));
    }

    public IEnumerator Truj(Collider2D kolider)
    {
        if(kolider == null || koliderChmury == null) yield break;
        Postac postac = kolider.gameObject.GetComponent<Postac>();
        float ostatnieZranienie = 0;
        while(koliderChmury.IsTouching(kolider))
        {
            if(ostatnieZranienie + 2 <= Time.time)
            {
                postac.Zran(obrazenia, tworca);
                ostatnieZranienie = Time.time;
                //yield return new WaitForSeconds(0.1f);
            }
            yield return null;
            if(kolider == null || koliderChmury == null) break;
        }
        yield return null;
    }

    public void Zapal()
    {
        if(zapalony) return;
        zapalony = true;
        Ogolne.mapa.Wybuch((Vector2)transform.localPosition, 50, 4, tworca);
        Destroy(gameObject);
    } 
}
