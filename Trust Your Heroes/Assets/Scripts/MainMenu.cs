using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject heroesPanel;
    public GameObject mainPanel;
    public GameObject playPanel;
    public GameObject optionsPanel;
    public GameObject spawnTile1;
    public GameObject spawnTile2;
    public GameObject spawnTile3;
    public GameObject spawnTile4;
    public Text nameText;
    public Text descriptionText;
    public Text statsText;
    public RawImage mapImage;

    public static int teamMemberPlace = 0;

    private GameObject selectedHero;
    private GameObject[] selectedHeroes;
    private int mapNumber = 1;

    void Start()
    {
        selectedHeroes = new GameObject[4];
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void NavigatePlayPanel(bool t)
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

    public void NavigateOptions(bool t)
    {
        mainPanel.SetActive(!t);
        optionsPanel.SetActive(t);
    }

    public void NavigateMap(bool left)
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

    public void NavigateHeroesPanel(bool t)
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

    public void StartGame()
    {
        if (selectedHeroes[0] == null || selectedHeroes[1] == null || selectedHeroes[2] == null || selectedHeroes[3] == null)
            return;
        YourHeroTeam.heroNames[0] = selectedHeroes[0].name.Remove(selectedHeroes[0].name.Length - 7);
        YourHeroTeam.heroNames[1] = selectedHeroes[1].name.Remove(selectedHeroes[1].name.Length - 7);
        YourHeroTeam.heroNames[2] = selectedHeroes[2].name.Remove(selectedHeroes[2].name.Length - 7);
        YourHeroTeam.heroNames[3] = selectedHeroes[3].name.Remove(selectedHeroes[3].name.Length - 7);
        switch (mapNumber)
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

    public void SpawnHero(int heroNumber)
    {
        if (selectedHero) Destroy(selectedHero);
        selectedHero = SpawnNewHero(heroNumber, spawnTile4);
    }

    public void ChooseHero(int heroNumber)
    {
        switch(teamMemberPlace)
        {
            case 1:
                if (selectedHeroes[0]) Destroy(selectedHeroes[0]);
                selectedHeroes[0] = SpawnNewHero(heroNumber, spawnTile1);
                break;
            case 2:
                if (selectedHeroes[1]) Destroy(selectedHeroes[1]);
                selectedHeroes[1] = SpawnNewHero(heroNumber, spawnTile2);
                break;
            case 3:
                if (selectedHeroes[2]) Destroy(selectedHeroes[2]);
                selectedHeroes[2] = SpawnNewHero(heroNumber, spawnTile3);
                break;
            case 4:
                if (selectedHeroes[3]) Destroy(selectedHeroes[3]);
                selectedHeroes[3] = SpawnNewHero(heroNumber, spawnTile4);
                break;
            default:
                break;
        }
    }

    GameObject SpawnNewHero(int heroNumber, GameObject spawnTile)
    {
        GameObject hero;
        switch (heroNumber)
        {
            case 1:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Alluria/source/Alluria"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Ice Queen Alluria";
                descriptionText.text = "";
                statsText.text = "\n\n2\n\n\n\n\n\n";
                break;
            case 2:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Charlotte/source/Charlotte"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Charlotte Of The Web";
                descriptionText.text = "";
                statsText.text = "\n\n2\n\n\n\n\n\n";
                break;
            case 3:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Creek/source/Creek"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Creek The Tricky";
                descriptionText.text = "";
                statsText.text = "\n\n1\n\n\n\n\n\n";
                break;
            case 4:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Erasmo/source/Erasmo"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Erasmo The Mad";
                descriptionText.text = "";
                statsText.text = "\n\n1\n\n\n\n\n\n";
                break;
            case 5:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Frederic/source/Frederic"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Frederic The Plague";
                descriptionText.text = "";
                statsText.text = "\n\n2\n\n\n\n\n\n";
                break;
            case 6:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Hor/source/Hor"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Hor The Sun God";
                descriptionText.text = "";
                statsText.text = "\n\n2\n\n\n\n\n\n";
                break;
            case 7:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Makas/source/Makas"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "The Great Makas";
                descriptionText.text = "";
                statsText.text = "\n\n1\n\n\n\n\n\n";
                break;
            case 8:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Nazz/source/Nazz"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Nazz The Foul";
                descriptionText.text = "";
                statsText.text = "\n\n2\n\n\n\n\n\n";
                break;
            case 9:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Ohm/source/Ohm"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Ohm Steel";
                descriptionText.text = "";
                statsText.text = "\n\n2\n\n\n\n\n\n";
                break;
            case 10:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Pico/source/Pico"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Pico The Mantis";
                descriptionText.text = "";
                statsText.text = "\n\n2\n\n\n\n\n\n";
                break;
            case 11:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Ryubi/source/Ryubi"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Ryubi The Silent";
                descriptionText.text = "The most skilled sharpshooter in the realm and beyond. Ryubi empowers her arrows with her tribe's dark magic, making them capable of penetrating the toughest shields and armor.\nThey say the deadliest predators are the silent ones. Who does she have her sights on today?";
                statsText.text = "\n\n1\n\n\n\n\n\n";
                break;
            case 12:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Santino/source/Santino"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Santino Of The Forest";
                descriptionText.text = "It is said that the woods have a guardian spirit who watches over them. Santino may be a carefree forest dweller, but his love for the plants and animals is unmatched. He is incredibly skilled in healing magic, but that doesn't mean he can't fight.\nThose who wish evil upon the woods better know that Santino will stand in their way.";
                statsText.text = "\n\n2\n\n\n\n\n\n";
                break;
            case 13:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Sent/source/Sent"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Sent Steel";
                descriptionText.text = "";
                statsText.text = "\n\n2\n\n\n\n\n\n";
                break;
            case 14:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/TommyApe/source/TommyApe"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Tommy Ape";
                descriptionText.text = "Once an ordinary primate, Tommy was experimented on by his master Dr. Radon. The brutal experiments upgraded his physical and mental strength. Eventually, he turned on his former master, escaping and stealing his prized weapon, a hi-tech machine gun.\nNow, Tommy roams free, adventuring and living a life full of mischief.";
                statsText.text = "\n\n2\n\n\n\n\n\nMonkey business - able to cross jumpable obstacles (Passive ability)";
                break;
            case 15:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Xavier/source/Xavier"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Xavier The Cricket Warrior";
                descriptionText.text = "";
                statsText.text = "\n\n3\n\n\n\n\n\n";
                break;
            case 16:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Z/source/Z"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Z";
                descriptionText.text = "Not much is known about the cerebral assasin Z. Those who've seen his swordplay say he has no equal. Guided by the sword, he ventures around the world, looking for opponents worthy of tasting his blade.\nWhatever the circumstances, no matter the odds, know that Z has never lost a fight.";
                statsText.text = "\n\n2\n\n\n\n\n\n";
                break;
            default:
                hero = null;
                break;
        }
        hero.GetComponent<BoxCollider>().enabled = false;
        return hero;
    }

    public void PickTeamMemberPlace(int i)
    {
        teamMemberPlace = i;
    }
}
