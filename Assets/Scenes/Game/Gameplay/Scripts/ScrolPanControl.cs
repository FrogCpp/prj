using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrolPanControl : MonoBehaviour
{
    [SerializeField] private GameObject msgPref;
    [SerializeField] private GameObject Place;

    public void Write(string text)
    {
        var a = Instantiate(msgPref, Place.transform);
        a.GetComponent<MesageConf>().Init(text);
    }
}
