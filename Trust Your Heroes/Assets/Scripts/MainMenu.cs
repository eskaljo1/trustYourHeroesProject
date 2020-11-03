using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Script for everything that happens in the main menu scene

public class MainMenu : MonoBehaviour
{
    //Panels
    public GameObject heroesPanel;
    public GameObject mainPanel;
    public GameObject playPanel;
    public GameObject optionsPanel;
    
    //Tiles for spawning heroes
    public GameObject spawnTile1;
    public GameObject spawnTile2;
    public GameObject spawnTile3;
    public GameObject spawnTile4;

    //UI elements
    //*Heroes Panel
    public Text nameText;
    public Text descriptionText;
    public Text statsText;
    //*Play Panel
    public RawImage mapImage;

    //Info on what spawntile is selected
    public static int teamMemberPlace = 0;

    //Info on the spawned heroes
    //*Heroes Panel
    private GameObject selectedHero;
    //*Play Panel
    private GameObject[] selectedHeroes;

    //Selected map in the Play Panel
    private int mapNumber = 1;

    void Start()
    {
        selectedHeroes = new GameObject[4];
    }

    public void Exit() //Exit button
    {
        Application.Quit();
    }

    public void NavigatePlayPanel(bool t) //Play button in Main Panel, Back button in Play Panel
    {
        mainPanel.SetActive(!t);
        playPanel.SetActive(t);
        if (!t)
        {
            Destroy(selectedHeroes[0]);
            Destroy(selectedHeroes[1]);
            Destroy(selectedHeroes[2]);
            Destroy(selectedHeroes[3]);
        }
    }

    public void NavigateOptions(bool t) //Options button in Main Panel, Back button in Options Panel
    {
        mainPanel.SetActive(!t);
        optionsPanel.SetActive(t);
    }

    public void NavigateMap(bool left) //Left and Right arrows for choosing map
    {
        switch (mapNumber)
        {
            case 1:
                if (left)
                {
                    mapNumber = 3;
                    mapImage.texture = Resources.Load<Texture>("Icons/Maps/deserticon");
                }
                else
                {
                    mapNumber = 2;
                    mapImage.texture = Resources.Load<Texture>("Icons/Maps/christmasicon");
                }
                break;
            case 2:
                if (left)
                {
                    mapNumber = 1;
                    mapImage.texture = Resources.Load<Texture>("Icons/Maps/darktownicon");
                }
                else
                {
                    mapNumber = 3;
                    mapImage.texture = Resources.Load<Texture>("Icons/Maps/deserticon");
                }
                break;
            case 3:
                if (left)
                {
                    mapNumber = 2;
                    mapImage.texture = Resources.Load<Texture>("Icons/Maps/christmasicon");
                }
                else
                {
                    mapNumber = 1;
                    mapImage.texture = Resources.Load<Texture>("Icons/Maps/darktownicon");
                }
                break;
        }
    }

    public void NavigateHeroesPanel(bool t) //Heroes button in Main Panel, Back button in Heroes Panel
    {
        mainPanel.SetActive(!t);
        heroesPanel.SetActive(t);
        if (!t)
        {
            Destroy(selectedHero);
            nameText.text = "";
            descriptionText.text = "";
            statsText.text = "";
        }
    }

    public void StartGame() //Play button in Play Panel
    {
        //If 4 heroes are not chosen return
        if (selectedHeroes[0] == null || selectedHeroes[1] == null || selectedHeroes[2] == null || selectedHeroes[3] == null)
            return;
        //Stores hero information in YourHeroTeam script
        YourHeroTeam.heroNames[0] = selectedHeroes[0].name.Remove(selectedHeroes[0].name.Length - 7);
        YourHeroTeam.heroNames[1] = selectedHeroes[1].name.Remove(selectedHeroes[1].name.Length - 7);
        YourHeroTeam.heroNames[2] = selectedHeroes[2].name.Remove(selectedHeroes[2].name.Length - 7);
        YourHeroTeam.heroNames[3] = selectedHeroes[3].name.Remove(selectedHeroes[3].name.Length - 7);
        switch (mapNumber) //Opens scene depending on the chosen map
        {
            case 1:
                SceneManager.LoadScene("DarkTownScene");
                break;
            case 2:
                SceneManager.LoadScene("ChristmasScene");
                break;
            case 3:
                SceneManager.LoadScene("DesertScene");
                break;
        }
    }

    public void SpawnHero(int heroNumber) //Spawns hero prefab, Heroes Panel
    {
        if (selectedHero) Destroy(selectedHero);
        selectedHero = SpawnNewHero(heroNumber, spawnTile4, true);
    }

    public void ChooseHero(int heroNumber) //Spawns hero prefab, Play Panel
    {
        switch(teamMemberPlace) //Checks which spawn tile is chosen
        {
            case 1:
                if (selectedHeroes[0]) Destroy(selectedHeroes[0]);
                selectedHeroes[0] = SpawnNewHero(heroNumber, spawnTile1, false);
                break;
            case 2:
                if (selectedHeroes[1]) Destroy(selectedHeroes[1]);
                selectedHeroes[1] = SpawnNewHero(heroNumber, spawnTile2, false);
                break;
            case 3:
                if (selectedHeroes[2]) Destroy(selectedHeroes[2]);
                selectedHeroes[2] = SpawnNewHero(heroNumber, spawnTile3, false);
                break;
            case 4:
                if (selectedHeroes[3]) Destroy(selectedHeroes[3]);
                selectedHeroes[3] = SpawnNewHero(heroNumber, spawnTile4, false);
                break;
            default:
                break;
        }
    }

    GameObject SpawnNewHero(int heroNumber, GameObject spawnTile, bool heroesPanel) //Spawns hero prefab
    {
        GameObject hero;
        switch (heroNumber)
        {
            case 1:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Alluria/source/Alluria"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 2:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Charlotte/source/Charlotte"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 3:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Creek/source/Creek"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 4:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Erasmo/source/Erasmo"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 5:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Frederic/source/Frederic"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 6:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Hor/source/Hor"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 7:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Makas/source/Makas"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 8:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Nazz/source/Nazz"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 9:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Ohm/source/Ohm"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 10:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Pico/source/Pico"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 11:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Ryubi/source/Ryubi"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 12:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Santino/source/Santino"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 13:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Sent/source/Sent"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 14:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/TommyApe/source/TommyApe"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 15:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Xavier/source/Xavier"), spawnTile.transform.position, Quaternion.identity);
                break;
            case 16:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Z/source/Z"), spawnTile.transform.position, Quaternion.identity);
                break;
            default:
                hero = null;
                break;
        }
        if (heroesPanel) //If spawning in heroes panel also edit the description and stats
        {
            nameText.text = hero.GetComponent<Hero>().heroName;
            descriptionText.text = hero.GetComponent<Hero>().description;
            statsText.text = hero.GetComponent<Hero>().health.ToString() + "\n\n" + hero.GetComponent<MoveHero>().movement.ToString() + "\n\n" + hero.GetComponent<Hero>().mainAttack + "\n\n" + hero.GetComponent<Hero>().firstAbility + "\n\n" + hero.GetComponent<Hero>().secondAbility;
        }
        hero.GetComponent<BoxCollider>().enabled = false; //Disable click on hero
        return hero;
    }

    public void PickTeamMemberPlace(int i) //Switch spawn tile in Play Panel
    {
        teamMemberPlace = i;
    }
}
