﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that doesn't destroy itself when scenes are changed, keeps information about the chosen team

public class YourHeroTeam : MonoBehaviour
{
    public static string[] heroNames;

    void Start()
    {
        heroNames = new string[4];
        heroNames[0] = "Xavier";
        heroNames[1] = "Nazz";
        heroNames[2] = "Frederic";
        heroNames[3] = "Charlotte";
    }
}
