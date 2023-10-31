using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WislanskiDoge : Doge
{
    public Animator animator;
    GameObject siekiera;
    GameObject tarcza;
    new void Start()
    {
        base.Start();
        hp = 100;
        maksHp = 100;
        zaladowanie[0] = 100f;
        minimumDoStartuUzycia[0] = 0f;
        czasMiedzyUzyciami[0] = 0.7f;
        wartoscUzupelniania[0] = 100f;
        czasMiedzyUzupelnieniami[0] = 0f;
        czasOstatniegoUzycia[0] = -1f;

        zaladowanie[1] = 0f;
        minimumDoStartuUzycia[1] = 10f;
        czasMiedzyUzyciami[1] = 0f;
        wartoscUzupelniania[1] = 0.5f;
        czasMiedzyUzupelnieniami[1] = 0.1f;
        czasOstatniegoUzycia[1] = -1f;
        czyUzywa[1] = false;

        animator = GetComponent<Animator>();

        siekiera = transform.Find("Sprzet/Siekiera").gameObject;
        tarcza = transform.Find("Sprzet/Tarcza").gameObject;
    }
    new void Update()
    {
        base.Update();
        if(Input.GetMouseButtonDown(0) && CzyMozeUzycUmiejetnosci(0))
        {
            Uzycie(0, 0f);
            StartCoroutine(AtakSiekiera());
        }
        if(Input.GetMouseButton(1) && (CzyMozeUzycUmiejetnosci(1) || CzyMozeKontynuowacUzywanieUmiejetnosci(1)))
        {
            Uzycie(1, 0f);
            if(!czyUzywa[1])
            {
                czyUzywa[1] = true;
            }
        }
        else
        {
            if(czyUzywa[1])
            {
                czyUzywa[1] = false;
            }
        }

        if(czyUzywa[1] && !tarcza.activeSelf)
        {
            //Debug.Log(Ogolne.mapa.PozycjaBlokowa(transform.localPosition).ToString());
            tarcza.SetActive(true);
            predkosc /= 2;
        }
        else if(!czyUzywa[1] && tarcza.activeSelf)
        {
            tarcza.SetActive(false);
            predkosc *= 2;
        }
    }

    public override void Zran(int obrazenia, Postac raniacy)
    {
        if(Przed(raniacy.gameObject) && czyUzywa[1])
        {
            zaladowanie[1] -= obrazenia/2;
            if(zaladowanie[1] < 0) zaladowanie[1] = 0;
        }
        else
        {
            base.Zran(obrazenia, raniacy);
        }
    }

    IEnumerator AtakSiekiera()
    {
        siekiera.SetActive(true);
        animator.SetBool("CzyAtakuje", true);
        yield return new WaitForSeconds(0.35f);
        Dziewczyna[] dziewczyny = Ogolne.mapa.DziewczynyJakoDziewczyny();
        if(dziewczyny.Length > 0)
        {
            foreach(Dziewczyna dziewczyna in dziewczyny)
            {
                Vector2 pDziewczyny = dziewczyna.transform.localPosition;
                Vector2 pDoga = transform.localPosition;
                Vector2 sDoga = transform.localScale;
                if(Vector2.Distance(pDziewczyny, pDoga) < 2.2f && ((pDziewczyny.x > pDoga.x && sDoga.x > 0) || (pDziewczyny.x < pDoga.x && sDoga.x < 0)) && Ogolne.Okolo(pDziewczyny.y, pDoga.y, 2))
                {
                    dziewczyna.Zran(50, this);
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("CzyAtakuje", false);
        siekiera.SetActive(false);
    }
}
