using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourHeroTeam : MonoBehaviour
{
    public static GameObject[] yourHeroes;

    void Start()
    {
        yourHeroes = new GameObject[4];
        yourHeroes[0] = Resources.Load<GameObject>("Models/Heroes/Xavier/source/Xavier");
        yourHeroes[1] = Resources.Load<GameObject>("Models/Heroes/Tommy Ape/source/TommyApe");
        yourHeroes[2] = Resources.Load<GameObject>("Models/Heroes/Charlotte/source/Charlotte");
        yourHeroes[3] = Resources.Load<GameObject>("Models/Heroes/Creek/source/Creek");
    }
}
