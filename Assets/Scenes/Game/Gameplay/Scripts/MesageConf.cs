using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MesageConf : MonoBehaviour
{
    [SerializeField] private TMP_Text _msgText;
    [SerializeField] private TMP_Text _msgAutor;

    public void Init(string text, string autor)
    {
        _msgAutor.text = autor;
        _msgText.text = text;
        _msgText.ForceMeshUpdate();
    }
}
