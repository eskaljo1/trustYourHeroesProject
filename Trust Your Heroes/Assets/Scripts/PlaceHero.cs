using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that is connected to every gridcell, when clicked on cell selected hero moves

public class PlaceHero : MonoBehaviour
{
    //Coordinates
    private int x = 0;
    private int z = 0;

    //For movement
    public int distance = 0;

    //Static variables
    public static bool gameBegun = false; //When Ready button is clicked the game starts
    public static bool heroIsSelected = false; //true if a hero is selected
    public static GameObject heroSelected = null; //The selected hero

    void Start()
    {
        heroSelected = null;
        gameBegun = false;
        heroIsSelected = false;
    }

    void Update()
    {
        if (gameBegun) //When game starts there is no need for FirstRowCell tag
        {
            if(gameObject.tag == "FirstRowCell")
                gameObject.tag = "Cell";
        }
    }

    void OnMouseDown()
    {
        if (heroIsSelected)
        {
            if (gameObject.tag == "FirstRowCell") //If the selected cell has FirstRowCell tag the game hasn't started yet, spawns hero on cell
            {
                heroIsSelected = false;
                if (heroSelected) //If a hero is selected on the board, move him from his current cell
                {
                    //Change tag from OccupiedCell to FirstRowCell
                    SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].tag = "FirstRowCell";
                    //Move
                    MoveSelectedHero();
                }
                else //else spawn hero of the selected hero icon
                {
                    GameObject hero = Instantiate(Resources.Load<GameObject>("Models/Heroes/" + YourHeroTeam.heroNames[PlaceHeroButtons.heroSelected - 1] + "/source/" + YourHeroTeam.heroNames[PlaceHeroButtons.heroSelected - 1]), transform.position, Quaternion.identity);
                    hero.GetComponent<MoveHero>().SetCoordinates(x, z);
                    PlaceHeroButtons.spawned = true;
                    PlaceHeroButtons.heroesPlaced++;
                }
                //Turn off lights
                GameObject[] firstRowCells = GameObject.FindGameObjectsWithTag("FirstRowCell");
                for (int i = 0; i < firstRowCells.Length; i++)
                {
                    firstRowCells[i].GetComponentInChildren<Light>().intensity = 0;
                }
                tag = "OccupiedCell";
            }
            if (gameBegun) //If game has begun, move hero to cell if it is available
            {
                if (GetComponentInChildren<Light>().intensity == 15) //Checks if cell is available
                {
                    MoveSelectedHero();
                    tag = "OccupiedCell";
                }
            }
        }
    }

    void MoveSelectedHero() //Move hero
    {
        //Changes the tag of the cell the hero is currently on
        if(SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].tag == "OccupiedCell")
        {
            SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].tag = "Cell";
        }
        //Moves hero and changes the coordinates in MoveHero
        heroSelected.transform.position = transform.position;
        heroSelected.GetComponent<MoveHero>().SetCoordinates(x, z);
        //Turns off all the cell lights
        heroSelected.GetComponent<MoveHero>().TurnOffLights();
        heroSelected = null;
    }

    public void SetPosition(int xx, int zz) //Set coordinates
    {
        x = xx;
        z = zz;
    }
    
    public void ChangeDistance(int d) //Set distance
    {
        distance = d;
    }
}
