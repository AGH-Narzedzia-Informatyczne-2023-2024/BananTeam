using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrzyciskiMenu : MonoBehaviour
{
    public void RozpocznijGre()
    {
        SceneManager.LoadScene("PoleBitwy", LoadSceneMode.Single);
    }

    public void ZarzadzajMapa()
    {
        SceneManager.LoadScene("MenuKreatora", LoadSceneMode.Single);
    }

    public void Wyjdz()
    {
        Debug.Log("Koniec");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
