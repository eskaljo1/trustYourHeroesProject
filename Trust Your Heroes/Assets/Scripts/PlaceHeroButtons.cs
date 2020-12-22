using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

//Used for the hero buttons that spawn each hero at the beginning of the game

public class PlaceHeroButtons : MonoBehaviour
{
    //Dead heroes
    public static int player1DeadHeroes = 0;
    public static int player2DeadHeroes = 0;

    //Static variables
    public static int heroSelected; //Remember what button was pushed, used for disabling buttons
    public static bool spawned = false; //true if spawn happened, used for disabling buttons
    public static int heroesPlaced = 0; //Number of heroes placed
    public static bool ready1 = false; //When Ready button is clicked the game starts
    public static bool ready2 = false;

    public GameObject exitMenu;

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

    //Panels
    public GameObject winPanel;
    public GameObject losePanel;

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
        ready1 = false;
        ready2 = false;
        player1DeadHeroes = 0;
        player2DeadHeroes = 0;
    }

    void Update()
    {
        if (player1DeadHeroes != 4 && player2DeadHeroes != 4)
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
                if (NetworkManager.firstPlayer)
                    photonView.RPC("Player1Wins", RpcTarget.All);
                else
                    photonView.RPC("Player2Wins", RpcTarget.All);
            }
        if (player1DeadHeroes == 4)
        {
            PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
            photonView.RPC("Player2Wins", RpcTarget.All);
        }
        else if (player2DeadHeroes == 4)
        {
            PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
            photonView.RPC("Player1Wins", RpcTarget.All);
        }
        if(ready1 && ready2)
        {
            ready1 = false;
            ready2 = false;
            PlaceHero.gameBegun = true;
            SwitchCamera.moveText.SetActive(true);
            if (MoveHero.player1Move)
            {
                if (NetworkManager.firstPlayer)
                {
                    SwitchCamera.moveText.GetComponent<Text>().text = "Your move";
                }
                else
                {
                    SwitchCamera.moveText.GetComponent<Text>().text = "Opponent's move";
                }
            }
            else
            {
                if (!NetworkManager.firstPlayer)
                {
                    SwitchCamera.moveText.GetComponent<Text>().text = "Your move";
                }
                else
                {
                    SwitchCamera.moveText.GetComponent<Text>().text = "Opponent's move";
                }
            }
        }
        if (spawned) //If spawn happened disable the button of the spawned hero
        {
            spawned = false;
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
                            hero1Button3.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().Ability2);
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
                            hero2Button3.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().Ability2);
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
                            hero3Button3.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().Ability2);
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
                            hero4Button3.onClick.AddListener(PlaceHero.heroSelected.GetComponent<Hero>().Ability2); hero1Button1.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                        hero4Button3.gameObject.GetComponent<HeroAbilityButtonsShowPanel>().UpdateText(PlaceHero.heroSelected);
                    break;
            }
            PlaceHero.heroSelected = null;
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
        if (NetworkManager.firstPlayer)
        {
            GameObject[] firstRowCells = GameObject.FindGameObjectsWithTag("FirstRowCell");
            for (int i = 0; i < firstRowCells.Length; i++)
            {
                firstRowCells[i].GetComponentInChildren<Light>().intensity = 15;
            }
        }
        else
        {
            GameObject[] lastRowCells = GameObject.FindGameObjectsWithTag("LastRowCell");
            for (int i = 0; i < lastRowCells.Length; i++)
            {
                lastRowCells[i].GetComponentInChildren<Light>().intensity = 15;
            }
        }
    }

    [PunRPC]
    void Ready1Set()
    {
        ready1 = true;
    }

    [PunRPC]
    void Ready2Set()
    {
        ready2 = true;
    }

    [PunRPC]
    void Player1Wins()
    {
        if(NetworkManager.firstPlayer)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
    }

    [PunRPC]
    void Player2Wins()
    {
        if (!NetworkManager.firstPlayer)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
    }

    public void Ready() //Ready button, starts game, enables movement
    {
        if (heroesPlaced == 4)
        {
            PlaceHero.heroIsSelected = false;
            PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
            if (NetworkManager.firstPlayer)
                photonView.RPC("Ready1Set", RpcTarget.All);
            else
                photonView.RPC("Ready2Set", RpcTarget.All);
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

    public void ExitMenu(bool open)
    {
        exitMenu.SetActive(open);
    }

    public void Exit()
    {
        PhotonNetwork.LeaveRoom();
    }
}
