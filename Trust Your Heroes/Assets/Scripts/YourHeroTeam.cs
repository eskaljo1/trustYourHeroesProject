using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that doesn't destroy itself when scenes are changed, keeps information about the chosen team

public class YourHeroTeam : MonoBehaviour
{
    public static string[] heroNames;

    void Start()
    {
        heroNames = new string[4];
        heroNames[0] = "Charlotte";
        heroNames[1] = "Xavier";
        heroNames[2] = "TommyApe";
        heroNames[3] = "Makas";
    }
}
