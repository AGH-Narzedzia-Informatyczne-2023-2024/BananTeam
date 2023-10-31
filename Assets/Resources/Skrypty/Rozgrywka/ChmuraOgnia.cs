using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChmuraOgnia : MonoBehaviour
{
    float czasStworzenia;
    public Vector3 przyspieszenie;
    Vector3 powiekszenie;
    public float obrazenia;
    public Postac tworca;
    SpriteRenderer sr;
    SpriteRenderer sr2;

    void Start()
    {
        powiekszenie = new Vector3(0.003f, 0.003f, 0);
        czasStworzenia = Time.time;
        sr = GetComponent<SpriteRenderer>();
        sr2 = transform.Find("Chmura").gameObject.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        transform.position = transform.position + przyspieszenie;
        transform.localScale = transform.localScale + powiekszenie;
        if(Time.time >= czasStworzenia + 1.5f)
        {
            sr.color = new Color(1, 0.4094529f, 0, sr.color.a - 0.02f);
            sr2.color = new Color(1, 0.9531983f, 0.4980392f, sr2.color.a - 0.02f);
            obrazenia -= 0.3f;
        }
        if(Time.time >= czasStworzenia + 2f) Destroy(gameObject);
        /*Doge[] dogi = mapa.DogiJakoDogi();
        foreach(Postac doge in dogi)
        {
            if(Vector2.Distance(doge.transform.localPosition, transform.position) < )
        }*/
    }

    void OnTriggerEnter2D(Collider2D kolider)
    {
        Postac postac = kolider.gameObject.GetComponent<Postac>();
        if(postac != null)
        {
            postac.Zran((int)obrazenia, tworca);
            return;
        }
        ChmuraGazu chmuraGazu = kolider.gameObject.GetComponent<ChmuraGazu>();
        if(chmuraGazu != null)
        {
            chmuraGazu.Zapal();
            return;
        }
    }
}
