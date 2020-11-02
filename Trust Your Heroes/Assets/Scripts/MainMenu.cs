using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject heroesPanel;
    public GameObject mainPanel;
    public GameObject playPanel;
    public GameObject spawnTile;
    public Text nameText;
    public Text descriptionText;

    private GameObject selectedHero;
    
    public void Exit()
    {
        Application.Quit();
    }

    public void NavigatePlayPanel(bool t)
    {
        mainPanel.SetActive(!t);
        playPanel.SetActive(t);
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
        }
    }

    public void SpawnHero(int heroNumber)
    {
        if (selectedHero) Destroy(selectedHero);
        switch(heroNumber)
        {
            case 1:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Alluria/source/Alluria"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Ice Queen Alluria";
                descriptionText.text = "";
                break;
            case 2:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Charlotte/source/Charlotte"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Charlotte Of The Web";
                descriptionText.text = "";
                break;
            case 3:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Creek/source/Creek"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Creek The Tricky";
                descriptionText.text = "";
                break;
            case 4:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Erasmo/source/Erasmo"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Erasmo The Mad";
                descriptionText.text = "";
                break;
            case 5:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Frederic/source/Frederic"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Frederic The Plague";
                descriptionText.text = "";
                break;
            case 6:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Hor/source/Hor"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Hor The Sun God";
                descriptionText.text = "";
                break;
            case 7:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Makas/source/Makas"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "The Great Makas";
                descriptionText.text = "";
                break;
            case 8:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Nazz/source/Nazz"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Nazz The Foul";
                descriptionText.text = "";
                break;
            case 9:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Ohm/source/Ohm"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Ohm Steel";
                descriptionText.text = "";
                break;
            case 10:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Pico/source/Pico"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Pico The Mantis";
                descriptionText.text = "";
                break;
            case 11:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Ryubi/source/Ryubi"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Ryubi The Silent";
                descriptionText.text = "";
                break;
            case 12:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Santino/source/Santino"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Santino Of The Forest";
                descriptionText.text = "";
                break;
            case 13:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Sent/source/Sent"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Sent Steel";
                descriptionText.text = "";
                break;
            case 14:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Tommy Ape/source/TommyApe"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Tommy Ape";
                descriptionText.text = "";
                break;
            case 15:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Xavier/source/Xavier"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Xavier The Cricket Warrior";
                descriptionText.text = "";
                break;
            case 16:
                selectedHero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Z/source/Z"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Z";
                descriptionText.text = "";
                break;
        }
        selectedHero.GetComponent<BoxCollider>().enabled = false;
    }
}
