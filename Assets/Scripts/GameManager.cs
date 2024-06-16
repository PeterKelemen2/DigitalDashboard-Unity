using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI StatusText; 
    void Start()
    {
        changeStatusText("Waiting for input...");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeStatusText(String status)
    {
        StatusText.SetText("Status: " + status);
    }
}
