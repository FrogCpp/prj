using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrolPanControl : MonoBehaviour
{
    [SerializeField] private TMP_Text Place;

    public void Write(string text)
    {
        Place.text += "\n\n" + text;
    }
}
