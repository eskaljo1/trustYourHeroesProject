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
    
    public Button hero1Button1;
    public Button hero1Button2;
    public Button hero1Button3;

    public Button hero2Button1;
    public Button hero2Button2;
    public Button hero2Button3;

    public Button hero3Button1;
    public Button hero3Button2;
    public Button hero3Button3;

    public Button hero4Button1;
    public Button hero4Button2;
    public Button hero4Button3;

    void Start()
    {
        //Set icons of chosen heroes and abilities
        SetHeroIcon(button1.gameObject, 0);
        SetAbilityButtons(hero1Button1.gameObject, hero1Button2.gameObject, hero1Button3.gameObject, 0);
        SetHeroIcon(button2.gameObject, 1);
        SetAbilityButtons(hero2Button1.gameObject, hero2Button2.gameObject, hero2Button3.gameObject, 1);
        SetHeroIcon(button3.gameObject, 2);
        SetAbilityButtons(hero3Button1.gameObject, hero3Button2.gameObject, hero3Button3.gameObject, 2);
        SetHeroIcon(button4.gameObject, 3);
        SetAbilityButtons(hero4Button1.gameObject, hero4Button2.gameObject, hero4Button3.gameObject, 3);

        heroesPlaced = 0;
        spawned = false;
        heroSelected = -1;
    }

    void Update()
    {
        if (spawned) //If spawn happened disable the button of the spawned hero
        {
            //Add onclick to ability buttons
            switch (heroSelected)
            {
                case 1:
                    button1.interactable = false;
                    button1.enabled = false;
                    hero1Button1.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().MainAttack);
                    hero1Button2.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().Ability1);
                    hero1Button1.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    hero1Button2.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    if (PlaceHero.heroSelected.GetComponent<Hero>().ability2Passive)
                        hero1Button3.interactable = false;
                    else
                        hero1Button3.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().MainAttack);
                    hero1Button3.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    break;
                case 2:
                    button2.interactable = false;
                    button2.enabled = false;
                    hero2Button1.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().MainAttack);
                    hero2Button2.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().Ability1);
                    hero2Button1.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    hero2Button2.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    if (PlaceHero.heroSelected.GetComponent<Hero>().ability2Passive)
                        hero2Button3.interactable = false;
                    else
                        hero2Button3.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().MainAttack);
                    hero2Button3.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    break;
                case 3:
                    button3.interactable = false;
                    button3.enabled = false;
                    hero3Button1.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().MainAttack);
                    hero3Button2.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().Ability1);
                    hero3Button1.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    hero3Button2.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    if (PlaceHero.heroSelected.GetComponent<Hero>().ability2Passive)
                        hero3Button3.interactable = false;
                    else
                        hero3Button3.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().MainAttack);
                    hero3Button3.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    break;
                case 4:
                    button4.interactable = false;
                    button4.enabled = false;
                    hero4Button1.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().MainAttack);
                    hero4Button2.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().Ability1);
                    hero4Button1.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    hero4Button2.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    if (PlaceHero.heroSelected.GetComponent<Hero>().ability2Passive)
                        hero4Button3.interactable = false;
                    else
                        hero4Button3.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().MainAttack); hero1Button1.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    hero4Button3.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    break;
            }
            PlaceHero.heroSelected = null;
            spawned = false;
            heroSelected = -1;
        }
    }

    void SetHeroIcon(GameObject button, int heroNumber) //Gets icon from resources
    {
        button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/" + YourHeroTeam.heroNames[heroNumber] + "Icon");                      
    }

    void SetAbilityButtons(GameObject button1, GameObject button2, GameObject button3, int heroNumber)
    {
        button1.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Abilities/" + YourHeroTeam.heroNames[heroNumber] + "MainAttack");
        button2.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Abilities/" + YourHeroTeam.heroNames[heroNumber] + "Ability1");
        button3.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Abilities/" + YourHeroTeam.heroNames[heroNumber] + "Ability2");
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
            
            hero1Button1.interactable = true;
            hero1Button2.interactable = true;
            hero1Button3.interactable = true;
            hero2Button1.interactable = true;
            hero2Button2.interactable = true;
            hero2Button3.interactable = true;
            hero3Button1.interactable = true;
            hero3Button2.interactable = true;
            hero3Button3.interactable = true;
            hero4Button1.interactable = true;
            hero4Button2.interactable = true;
            hero4Button3.interactable = true;
        }
    }
}
