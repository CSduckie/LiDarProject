using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadKey : MonoBehaviour
{
    public string key;
    [SerializeField] private Material normalMat;
    [SerializeField] private Material highlightMat;
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }
    
    public void SendKey()
    {
        this.transform.GetComponentInParent<KeypadController>().PasswordEntry(key);
    }

    public void SetHighlight(bool isHighlighted)
    {
        rend.material = isHighlighted ? highlightMat : normalMat;
    }
}
