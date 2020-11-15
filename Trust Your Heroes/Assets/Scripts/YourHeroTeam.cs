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
        heroNames[0] = "Z";
        heroNames[1] = "Xavier";
        heroNames[2] = "Hor";
        heroNames[3] = "Erasmo";
    }
}
