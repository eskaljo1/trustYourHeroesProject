using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHero : MonoBehaviour
{
    private int x = 0;
    private int z = 0;

    public int typeOfCell = 1;
    public int distance = 0;

    public static bool gameBegun = false;
    public static bool heroIsSelected = false;
    public static GameObject heroSelected = null;

    void Start()
    {
        heroSelected = null;
        gameBegun = false;
        heroIsSelected = false;
    }

    void Update()
    {
        if (gameBegun)
        {
            if(gameObject.tag == "FirstRowCell")
                gameObject.tag = "Cell";
        }
    }

    void OnMouseDown()
    {
        if (heroIsSelected)
        {
            if (gameObject.tag == "FirstRowCell")
            {
                heroIsSelected = false;
                if (heroSelected)
                {
                    SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].tag = "FirstRowCell";
                    MoveSelectedHero();
                }
                else
                {
                    GameObject hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/" + YourHeroTeam.heroNames[PlaceHeroButtons.heroSelected - 1] + "/source/" + YourHeroTeam.heroNames[PlaceHeroButtons.heroSelected - 1]), transform.position, Quaternion.identity);
                    hero.GetComponent<MoveHero>().Placed(x, z);
                    PlaceHeroButtons.spawned = true;
                    PlaceHeroButtons.heroesPlaced++;
                }
                GameObject[] firstRowCells = GameObject.FindGameObjectsWithTag("FirstRowCell");
                for (int i = 0; i < firstRowCells.Length; i++)
                {
                    firstRowCells[i].GetComponentInChildren<Light>().intensity = 0;
                }
                gameObject.tag = "OccupiedCell";
            }
            if (gameBegun)
            {
                if (GetComponentInChildren<Light>().intensity == 15)
                {
                    MoveSelectedHero();
                    if (tag != "JumpableObstacle") tag = "OccupiedCell";
                }
            }
        }
    }

    void MoveSelectedHero()
    {
        if(SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].tag == "OccupiedCell")
        {
            SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].tag = "Cell";
        }
        heroSelected.transform.position = transform.position;
        heroSelected.GetComponent<MoveHero>().Placed(x, z);
        heroSelected.GetComponent<MoveHero>().TurnOffLights();
        heroSelected = null;
    }

    public void GetPosition(int xx, int zz)
    {
        x = xx;
        z = zz;
    }

    public void ChangeTypeOfCell(int t)
    {
        typeOfCell = t;
    }
    public void ChangeDistance(int d)
    {
        distance = d;
    }
}
