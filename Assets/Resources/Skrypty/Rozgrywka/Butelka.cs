using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butelka : MonoBehaviour
{
    public int kierunek;
    bool lec = false;
    public KolejnyRzultyPiesPosting wlasciciel;

    public void Rzucona(int nowyKierunek)
    {
        transform.SetParent(null);
        kierunek = nowyKierunek;
        lec = true;
        gameObject.AddComponent<Rigidbody2D>();
        //GetComponent<Collider2D>().isTrigger = false;
    }

    void FixedUpdate()
    {
        if(lec)
        {
            transform.position = (Vector2)transform.position + new Vector2(((float)kierunek)/10, 0);
            transform.Rotate(0, 0, kierunek * -18, Space.Self);
        }
    }

    void OnTriggerEnter2D(Collider2D kolider)
    {
        //Debug.Log("Kolizja");
        if(lec && kolider.gameObject.GetComponent<KolejnyRzultyPiesPosting>() != (KolejnyRzultyPiesPosting)wlasciciel) 
        {
            Destroy(GetComponent<Rigidbody2D>());
            lec = false;
            kierunek = 0;
            wlasciciel.RozbijButelke(transform.localPosition);
        }
    }
}
