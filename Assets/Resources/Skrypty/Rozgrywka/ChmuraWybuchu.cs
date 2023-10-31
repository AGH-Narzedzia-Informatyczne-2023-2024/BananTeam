using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChmuraWybuchu : MonoBehaviour
{
    Vector2 powiekszenie;
    public float maksymalnyRozmiar;
    SpriteRenderer sr;
    SpriteRenderer sr2;
    void Start()
    {
        powiekszenie = new Vector2(0.04f, 0.04f);
        sr = GetComponent<SpriteRenderer>();
        sr2 = transform.Find("Chmura").gameObject.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(transform.localScale.x >= maksymalnyRozmiar) Destroy(gameObject);
        transform.localScale = (Vector2)transform.localScale + powiekszenie;
        sr.color = new Color(1, 0.4094529f, 0, sr.color.a - 0.01f);
        sr2.color = new Color(1, 0.9531983f, 0.4980392f, sr2.color.a - 0.01f);
    }
}
