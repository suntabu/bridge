using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelScript : MonoBehaviour
{
    public Text Title1;

    public Text Title2;

    public Text Title3;

    public void SetText(string t1, string t2, string t3)
    {
        Title1.text = t1;
        Title2.text = t2;
        Title3.text = t3;
    }
    
    
    void Start()
    {
    }

    void Update()
    {
    }
}