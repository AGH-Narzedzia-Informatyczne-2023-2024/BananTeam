using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrzyciskiMenuKreatora : MonoBehaviour
{
    public void WczytajMape()
    {
        GetComponent<WczytywanieSciezki>().WczytajSciezke();
        /*string nowaSciezka = DaneDoMap.uzywanaSciezka;
        for (int i = 0; i < nowaSciezka.Length; i++)
        {
            if(nowaSciezka[i] == "\\") nowaSciezka[i] = "/";
        }
        DaneDoMap.uzywanaSciezka = nowaSciezka;*/
        DaneDoMap.WczytajMapeZPliku();
        StartCoroutine(Analiza());
    }

    public void UstawMape()
    {
        if(DaneDoMap.daneZarzadzanejMapy != null)
        {
            DaneDoMap.daneUstawionejMapy = DaneDoMap.daneZarzadzanejMapy;
        }
        else
        {
            Debug.Log("Brak mapy!");
        }
    }

    public void Powrot()
    {
        SceneManager.LoadScene("MenuStart", LoadSceneMode.Single);
    }

    public void StworzLubEdytujMape()
    {
        SceneManager.LoadScene("KreatorMap", LoadSceneMode.Single);
    }

    public IEnumerator Analiza()
    {
        yield return null;
        //Debug.Log(DaneDoMap.uzywanaSciezka);
        GameObject.Find("Canvas/PanelNazwyPliku/Text").GetComponent<Text>().text = DaneDoMap.NazwaPlikuZeSciezki(DaneDoMap.uzywanaSciezka);
        if(DaneDoMap.problemWczytanejMapy == null) 
        {
            GameObject.Find("Canvas/PanelWyniku/Problem").GetComponent<Text>().text = "Mapa wczytana poprawnie";
            GameObject.Find("Canvas/PanelWyniku/Sugestia").GetComponent<Text>().text = "";
        }
        else
        {
            GameObject.Find("Canvas/PanelWyniku/Problem").GetComponent<Text>().text = "Problem:\n" + DaneDoMap.problemWczytanejMapy;
            GameObject.Find("Canvas/PanelWyniku/Sugestia").GetComponent<Text>().text = "Sugerowane rozwiązanie:\n" + DaneDoMap.sugerowaneRozwiazanieProblemu;
        }
    }
}
