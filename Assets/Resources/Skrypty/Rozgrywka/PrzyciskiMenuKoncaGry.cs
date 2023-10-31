using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrzyciskiMenuKoncaGry : MonoBehaviour
{
    public void JeszczeRaz()
    {
        SceneManager.LoadScene("PoleBitwy", LoadSceneMode.Single);
    }

    public void WrocDoMenu()
    {
        SceneManager.LoadScene("MenuStart", LoadSceneMode.Single);
    }
}
