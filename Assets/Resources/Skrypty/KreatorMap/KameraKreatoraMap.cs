using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraKreatoraMap : MonoBehaviour
{
    float predkosc = 0.01f;
    Vector2? pozycjaPrzedPrzesunieciem;
    Camera kamera;
    float pierwotnyRozmiar;
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
        if(Input.GetKey(KeyCode.W))
        {
            transform.localPosition = transform.localPosition + new Vector3(0, predkosc, 0);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.localPosition = transform.localPosition + new Vector3(-predkosc, 0, 0);
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.localPosition = transform.localPosition + new Vector3(0, -predkosc, 0);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.localPosition = transform.localPosition + new Vector3(predkosc, 0, 0);
        }
        predkosc += 0.005f;
        }
        else
        {
            predkosc = 0.01f;
        }

        if(Input.GetMouseButtonDown(2))
        {
            if(kamera == null) kamera = GetComponent<Camera>();
            pozycjaPrzedPrzesunieciem = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pierwotnyRozmiar = kamera.orthographicSize;
        }
        if(Input.GetMouseButton(2))
        {
            if(pozycjaPrzedPrzesunieciem != null)
            {
                float nowyRozmiar = pierwotnyRozmiar + (((Vector2)pozycjaPrzedPrzesunieciem).y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                if(nowyRozmiar > 30) kamera.orthographicSize = 30;
                else if(nowyRozmiar < 3) kamera.orthographicSize = 3;
                else kamera.orthographicSize = nowyRozmiar;
            }
        }
        if(Input.GetMouseButtonUp(2))
        {
            pozycjaPrzedPrzesunieciem = null;
        }
    }
}
