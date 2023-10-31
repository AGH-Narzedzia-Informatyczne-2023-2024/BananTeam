using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chlebel : Doge
{
    GameObject prefabBulki;
    GameObject warstwa;
    bool niezniszczalny = false;
    new void Start()
    {
        base.Start();
        hp = 150;
        maksHp = 150;
        predkosc = 1.5f;

        zaladowanie[0] = 100f;
        minimumDoStartuUzycia[0] = 35f;
        czasMiedzyUzyciami[0] = 1;
        wartoscUzupelniania[0] = 1;
        czasMiedzyUzupelnieniami[0] = 0.1f;
        czasOstatniegoUzycia[0] = -100f;

        zaladowanie[1] = 0f;
        minimumDoStartuUzycia[1] = 100f;
        czasMiedzyUzyciami[1] = 1f;
        wartoscUzupelniania[1] = 0.8f;
        czasMiedzyUzupelnieniami[1] = 0.1f;
        czasOstatniegoUzycia[1] = -100f;

        prefabBulki = Resources.Load<GameObject>("Prefaby/Bulka");
        warstwa = transform.Find("StwardnialaWarstwa").gameObject;

        niezniszczalny = false;
    }

    new void Update()
    {
        base.Update();
        if(Input.GetMouseButton(0) && (CzyMozeUzycUmiejetnosci(0)) && !czyUzywa[1])
        {
            Uzycie(0, 35);
            GameObject bulka = Instantiate(prefabBulki, new Vector2(transform.localPosition.x + ((transform.localScale.x > 0) ? 1 : -1), transform.localPosition.y), Quaternion.identity);
            bulka.transform.SetParent(Ogolne.mapa.poleDogow.transform);
            Bulka skryptBulki = bulka.GetComponent<Bulka>();
            skryptBulki.wlasciciel = this;
            skryptBulki.obrazenia = 150;
            skryptBulki.strona = Strona.Doge;
        }
        if(Input.GetMouseButton(1) && (CzyMozeUzycUmiejetnosci(1)) && !czyUzywa[1])
        {
            czyUzywa[1] = true;
            warstwa.SetActive(true);
            wartoscUzupelniania[1] = 0;
            czasMiedzyUzupelnieniami[1] = 10;
            StartCoroutine(Wysusz());
        } 
    }

    public IEnumerator Wysusz()
    {
        SpriteRenderer sr = warstwa.GetComponent<SpriteRenderer>();
        niezniszczalny = true;
        for(int i = 0; i < 15; i++)
        {
            transform.localScale = (Vector2)transform.localScale + new Vector2(((transform.localScale.x > 0) ? 0.01f : -0.01f), 0.01f);
            sr.color = sr.color + new Color(-(1 - 0.7075472f)/30, -(1 - 0.651556f)/30, -(1 - 0.4171858f)/30, 0);
            predkosc -= 0.031f;
            yield return new WaitForSeconds(0.05f);
        }
        niezniszczalny = false;
    }

    public void ZniszczWarstwe()
    {
        wartoscUzupelniania[1] = 0.8f;
        czasMiedzyUzupelnieniami[1] = 0.1f;
        warstwa.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        warstwa.SetActive(false);
        niezniszczalny = true;
        transform.localScale = new Vector2(0.25f, 0.25f);
        Ogolne.mapa.Wybuch(transform.localPosition, 200, 5, this);
        Ulecz(maksHp);
        Uzycie(1, 0);
        predkosc = 1.5f;
        niezniszczalny = false;
        czyUzywa[1] = false;
    }

    public override void Zran(int obrazenia, Postac raniacy)
    {
        if(!niezniszczalny)
        {
        if(!czyUzywa[1]) base.Zran(obrazenia, raniacy);
        else
        {
            zaladowanie[1] -= ((float)obrazenia)/5;
            if(zaladowanie[1] <= 0)
            {
                zaladowanie[1] = 0;
                ZniszczWarstwe();
            }
            if(raniacy != null && raniacy != this) raniacy.Zran(20, this);
        }
        }
    }
}
