using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kamera : MonoBehaviour
{
    Image zdj;
    Text tabliczkaObecznegoWyniku;
    void Start()
    {
        zdj = GameObject.Find("Canvas/KrwaweTlo").GetComponent<Image>();
        GameObject.Find("Canvas/ObecnyWynik").SetActive(true);
        tabliczkaObecznegoWyniku = GameObject.Find("Canvas/ObecnyWynik").GetComponent<Text>();
    }
    void Update()
    {
        if(tabliczkaObecznegoWyniku.text != "" + Gracz.punkty) tabliczkaObecznegoWyniku.text = "" + Gracz.punkty;
        if(Gracz.glownyDoge != null)
        {
            Vector2 p = Gracz.glownyDoge.gameObject.transform.localPosition;
            transform.position = new Vector3(p.x, p.y, -10);
            float procent = Ogolne.Procent(Gracz.glownyDoge.hp, Gracz.glownyDoge.maksHp);
            if(procent < 40)
            {
                zdj.color = new Color(1, 0, 0, (0.4f - procent/(100/1.5f)));
            }
            else
            {
                zdj.color = new Color(1, 0, 0, 0);
            }
        }
        else
        {
            zdj.color = new Color(1, 0, 0, 0);
        }
    }
}
