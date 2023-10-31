using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelDoga : MonoBehaviour
{
    public int numerWyswietlanegoDoga = Gracz.numerDoga;
    void Start()
    {
        Odswierz();
    }
    void Update()
    {
        if(numerWyswietlanegoDoga != Gracz.numerDoga)
        {
            numerWyswietlanegoDoga = Gracz.numerDoga;
            Odswierz();
        }
    }

    public void Odswierz()
    {
        //Debug.Log(Resources.Load<Sprite>("Grafiki/Dogi/" + Gracz.listaDogow[numerWyswietlanegoDoga]));
        Image obrazek = transform.Find("ObrazekDoga").gameObject.GetComponent<Image>();
        obrazek.sprite = Resources.Load<Sprite>("Grafiki/Dogi/" + Gracz.listaDogow[numerWyswietlanegoDoga]);
        Text tekst = transform.Find("PanelNazwy/Text").gameObject.GetComponent<Text>();
        tekst.text = Gracz.listaNazwDogow[numerWyswietlanegoDoga];
    }

    public void numerDogaPlus()
    {
        if(Gracz.numerDoga < Gracz.listaDogow.Length - 1) Gracz.numerDoga += 1;
        else Gracz.numerDoga = 0;
    }

    public void numerDogaMinus()
    {
        if(Gracz.numerDoga > 0) Gracz.numerDoga -= 1;
        else Gracz.numerDoga = Gracz.listaDogow.Length - 1;
    }
}
