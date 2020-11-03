using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceHeroButtons : MonoBehaviour
{
    public static int heroSelected;
    public static bool spawned = false;
    public static int heroesPlaced = 0;

    public GameObject readyButton;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    void Start()
    {
        SetHeroIcon(button1.gameObject, 0);
        SetHeroIcon(button2.gameObject, 1);
        SetHeroIcon(button3.gameObject, 2);
        SetHeroIcon(button4.gameObject, 3);

        heroesPlaced = 0;
        spawned = false;
        heroSelected = -1;
    }

    void Update()
    {
        if (spawned)
        {
            switch (heroSelected)
            {
                case 1:
                    button1.interactable = false;
                    button1.enabled = false;
                    break;
                case 2:
                    button2.interactable = false;
                    button2.enabled = false;
                    break;
                case 3:
                    button3.interactable = false;
                    button3.enabled = false;
                    break;
                case 4:
                    button4.interactable = false;
                    button4.enabled = false;
                    break;
            }
            spawned = false;
            heroSelected = -1;
        }
    }

    void SetHeroIcon(GameObject button, int heroNumber)
    {
        button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/" + YourHeroTeam.heroNames[heroNumber] + "Icon");                      
    }

    public void ClickOnHeroIcon(int heroNumber)
    {
        PlaceHero.heroIsSelected = true;
        GameObject[] firstRowCells = GameObject.FindGameObjectsWithTag("FirstRowCell");
        heroSelected = heroNumber;
        for (int i = 0; i < firstRowCells.Length; i++)
        {
            firstRowCells[i].GetComponentInChildren<Light>().intensity = 15;
        }
    }

    public void Ready()
    {
        if (heroesPlaced == 4)
        {
            PlaceHero.gameBegun = true;
            readyButton.SetActive(false);
        }
    }
}
