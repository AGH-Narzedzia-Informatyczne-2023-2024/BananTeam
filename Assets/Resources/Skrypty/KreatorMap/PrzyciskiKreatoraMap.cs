using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using System.Windows.Forms;
//using UnityEditor;

public class PrzyciskiKreatoraMap : MonoBehaviour
{
    Text tekstPolaSzerokosciMapy;
    Text tekstPolaWysokosciMapy;
    MapaKreatoraMap mapaKreatoraMap;

    void Start()
    {
        mapaKreatoraMap = GameObject.Find("Mapa").GetComponent<MapaKreatoraMap>();
        tekstPolaSzerokosciMapy = transform.Find("PanelUstawien/SzerokoscMapy/Text").GetComponent<Text>();
        tekstPolaSzerokosciMapy.text = "" + 1;
        tekstPolaWysokosciMapy = transform.Find("PanelUstawien/WysokoscMapy/Text").GetComponent<Text>();
        tekstPolaWysokosciMapy.text = "" + 1;
    }

    public void Odswierz()
    {
        int parsX;
        int parsY;
        if(tekstPolaSzerokosciMapy.text == "")
        {
            parsX = 1;
        }
        else
        {
            parsX = int.Parse(tekstPolaSzerokosciMapy.text);
        }
        if(tekstPolaWysokosciMapy.text == "")
        {
            parsY = 1;
        }
        else
        {
            parsY = int.Parse(tekstPolaWysokosciMapy.text);
        }

        if(!Single.IsNaN(parsX) && !Single.IsNaN(parsY))
        {
            if(parsX > 100) parsX = 100;
            if(parsY > 100) parsY = 100;
            if(parsX < 1) parsX = 1;
            if(parsY < 1) parsY = 1;
            if(mapaKreatoraMap.daneMapy.dlugoscX != parsX || mapaKreatoraMap.daneMapy.dlugoscY != parsY) 
            {
                mapaKreatoraMap.Przeskaluj(new Vector2(parsX, parsY));
            }
        }
    }

    public void Zapisz()
    {
        string[] linijki = DaneDoMap.MapaDoLinijek(mapaKreatoraMap.daneMapy);
        string polaczaneLinijki = "";
        foreach(string linijka in linijki) polaczaneLinijki += linijka + "\n";
        string sciezka = DaneDoMap.uzywanaSciezka;
        if(sciezka == null || !File.Exists(sciezka))
        {
            //Process.Start("explorer.exe" , @"C:\Users");
            //OpenFileDialog open = new OpenFileDialog();
            //var proces = Process.Start(@"c:\");
            //UnityEngine.Debug.Log(proces);
            //sciezka = EditorUtility.SaveFolderPanel("Zapis mapy", "", "");
            //Debug.Log("A");
            //GetOpenFileName();
            //OpenFileName ofn = new OpenFileName();
            //string fullPath = Path.GetFullPath("C:/");
            string nazwa = "/mapa.txt";
            if(File.Exists(sciezka + nazwa))
            {
                nazwa = "/mapa1.txt";
                int n = 1;
                while(File.Exists(sciezka + nazwa))
                {
                    n += 1;
                    nazwa = "/mapa" + n + ".txt";
                }
            }
            File.WriteAllText(sciezka + nazwa, polaczaneLinijki);
        }
        //foreach(string linijka in linijki) Debug.Log(linijka);
    }

    public void Powrot()
    {
        SceneManager.LoadScene("MenuKreatora", LoadSceneMode.Single);
    }
}
