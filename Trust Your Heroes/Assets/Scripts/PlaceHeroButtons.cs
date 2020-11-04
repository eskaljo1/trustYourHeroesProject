using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Used for the hero buttons that spawn each hero at the beginning of the game

public class PlaceHeroButtons : MonoBehaviour
{
    //Static variables
    public static int heroSelected; //Remember what button was pushed, used for disabling buttons
    public static bool spawned = false; //true if spawn happened, used for disabling buttons
    public static int heroesPlaced = 0; //Number of heroes placed

    //Buttons
    public GameObject readyButton;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    void Start()
    {
        //Set icons of chosen heroes
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
        if (spawned) //If spawn happened disable the button of the spawned hero
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

    void SetHeroIcon(GameObject button, int heroNumber) //Gets icon from resources
    {
        button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/" + YourHeroTeam.heroNames[heroNumber] + "Icon");                      
    }

    public void ClickOnHeroIcon(int heroNumber) //Click on hero icons
    {
        PlaceHero.heroIsSelected = true;
        heroSelected = heroNumber;
        //Light up every first row cell
        GameObject[] firstRowCells = GameObject.FindGameObjectsWithTag("FirstRowCell");
        for (int i = 0; i < firstRowCells.Length; i++)
        {
            firstRowCells[i].GetComponentInChildren<Light>().intensity = 15;
        }
    }

    public void Ready() //Ready button, starts game, enables movement
    {
        if (heroesPlaced == 4)
        {
            PlaceHero.heroIsSelected = false;
            PlaceHero.gameBegun = true;
            readyButton.SetActive(false);
            //Turn off lights if hero is selected
            for (int i = 0; i < SpawnGrid.cells.GetLength(0); i++)
                for (int j = 0; j < SpawnGrid.cells.GetLength(1); j++)
                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 0;
        }
    }
}
