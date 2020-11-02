﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject heroesPanel;
    public GameObject mainPanel;
    public GameObject playPanel;
    public GameObject spawnTile1;
    public GameObject spawnTile2;
    public GameObject spawnTile3;
    public GameObject spawnTile4;
    public Text nameText;
    public Text descriptionText;
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
                break;
            case 2:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Charlotte/source/Charlotte"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Charlotte Of The Web";
                descriptionText.text = "";
                break;
            case 3:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Creek/source/Creek"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Creek The Tricky";
                descriptionText.text = "";
                break;
            case 4:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Erasmo/source/Erasmo"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Erasmo The Mad";
                descriptionText.text = "";
                break;
            case 5:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Frederic/source/Frederic"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Frederic The Plague";
                descriptionText.text = "";
                break;
            case 6:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Hor/source/Hor"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Hor The Sun God";
                descriptionText.text = "";
                break;
            case 7:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Makas/source/Makas"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "The Great Makas";
                descriptionText.text = "";
                break;
            case 8:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Nazz/source/Nazz"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Nazz The Foul";
                descriptionText.text = "";
                break;
            case 9:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Ohm/source/Ohm"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Ohm Steel";
                descriptionText.text = "";
                break;
            case 10:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Pico/source/Pico"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Pico The Mantis";
                descriptionText.text = "";
                break;
            case 11:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Ryubi/source/Ryubi"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Ryubi The Silent";
                descriptionText.text = "";
                break;
            case 12:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Santino/source/Santino"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Santino Of The Forest";
                descriptionText.text = "";
                break;
            case 13:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Sent/source/Sent"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Sent Steel";
                descriptionText.text = "";
                break;
            case 14:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/TommyApe/source/TommyApe"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Tommy Ape";
                descriptionText.text = "";
                break;
            case 15:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Xavier/source/Xavier"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Xavier The Cricket Warrior";
                descriptionText.text = "";
                break;
            case 16:
                hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/Z/source/Z"), spawnTile.transform.position, Quaternion.identity);
                nameText.text = "Z";
                descriptionText.text = "";
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
