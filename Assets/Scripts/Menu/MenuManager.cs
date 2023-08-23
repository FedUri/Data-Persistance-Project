using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField newPlayer;
    public static string playerString;

    public static MenuManager Instance;
    
    // Start is called before the first frame update
    public void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    public void Update()
    {
        NewPlayerName();
    }
    public void NewPlayerName()
    {
        playerString = newPlayer.text;
    }

    }


    


