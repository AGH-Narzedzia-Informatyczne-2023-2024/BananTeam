using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PoleEdycjiMapy : MonoBehaviour, IPointerDownHandler
{
    public bool prawyKlikniety = false;
    public bool lewyKlikniety = false;

    void Start()
    {
        prawyKlikniety = false;
        lewyKlikniety = false;
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            prawyKlikniety = true;
        }
        else prawyKlikniety = false;

        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            lewyKlikniety = true;
        }
        else lewyKlikniety = false;
    }

    void Update()
    {
        if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            prawyKlikniety = false;
            lewyKlikniety = false;
        }
    }
}
