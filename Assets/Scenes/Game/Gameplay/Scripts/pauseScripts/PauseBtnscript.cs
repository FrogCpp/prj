using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBtnscript : MonoBehaviour
{
    [SerializeField] private GameObject pref;
    [SerializeField] private GameObject place;
    private bool off = true;
    private GameObject menu;

    public void Press()
    {
        if (off)
        {
            menu = Instantiate(pref, place.transform);
            off = false;
        }
        else
        {
            Destroy(menu);
            off = true;
        }
    }
}
