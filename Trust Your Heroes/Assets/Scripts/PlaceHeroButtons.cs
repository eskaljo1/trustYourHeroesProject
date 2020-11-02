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
        switch (YourHeroTeam.heroNames[heroNumber])
        {
            case "Alluria":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/alluriaIcon");
                break;
            case "Charlotte":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/charlotteIcon");
                break;
            case "Creek":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/creekIcon");
                break;
            case "Erasmo":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/erasmoIcon");
                break;
            case "Frederic":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/fredericIcon");
                break;
            case "Hor":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/horIcon");
                break;
            case "Makas":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/makasIcon");
                break;
            case "Nazz":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/nazzIcon");
                break;
            case "Ohm":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/oh,Icon");
                break;
            case "Pico":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/picoIcon");
                break;
            case "Ryubi":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/ryubiIcon");
                break;
            case "Santino":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/santinoIcon");
                break;
            case "Sent":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/sentIcon");
                break;
            case "TommyApe":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/tommyApeIcon");
                break;
            case "Xavier":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/xavierIcon");
                break;
            case "Z":
                button.GetComponent<RawImage>().texture = Resources.Load<Texture>("Icons/Heroes/zIcon");
                break;
        }
        
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
