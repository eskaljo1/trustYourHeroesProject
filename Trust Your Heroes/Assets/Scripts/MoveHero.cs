using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script attached to every hero, used for movement

public class MoveHero : MonoBehaviour
{
    //Coordinates
    private int x = 0;
    private int z = 0;

    public int movement = 2; //Movement distance
    public bool obstacleJumper = false; //Can he jump over obstacles

    //Erasmo's grass needs to dissapear below
    void Start()
    {
        //for testing
        if (gameObject.name == "Nazz" && transform.position.x < 0 && transform.position.z > -3.5)
        {
            SetCoordinates(0, 1);
        }
        if(gameObject.name == "Charlotte" && transform.position.x > 0 && transform.position.z > -3.5)
        {
            SetCoordinates(4, 2);
        }
        //
        if (name == "Erasmo(Clone)")
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.045f, transform.position.z);
    }

    void CheckCell(int i, int j) //Check if cell is available to move to
    {
        //True if the cell is not occupied and not a obstacle unless the hero can jump over them
        if (SpawnGrid.cells[i, j].tag != "EnemyCell" && SpawnGrid.cells[i, j].tag != "OccupiedCell" && SpawnGrid.cells[i, j].GetComponent<PlaceHero>().distance == 0  && (SpawnGrid.cells[i, j].tag != "Obstacle" || obstacleJumper))
        { 
            //Checks if the distance of the cell is 1
            if ((Mathf.Abs(x - i) + Mathf.Abs(z - j)) == 1)
            {
                SpawnGrid.cells[i, j].GetComponent<PlaceHero>().ChangeDistance(1);
            }
            else
            {
                //Checks if any adjacent cell is marked as available, if so marks the current cell as available if the distance is within movement 
                if (i - 1 >= 0) //used so no index exceptions happen
                {
                    if (SpawnGrid.cells[i - 1, j].GetComponent<PlaceHero>().distance != 0 &&
                        (SpawnGrid.cells[i - 1, j].GetComponent<PlaceHero>().distance + 1 <= movement))
                        SpawnGrid.cells[i, j].GetComponent<PlaceHero>().ChangeDistance(SpawnGrid.cells[i - 1, j].GetComponent<PlaceHero>().distance + 1);
                }
                if (j - 1 >= 0)
                {
                    if (SpawnGrid.cells[i, j - 1].GetComponent<PlaceHero>().distance != 0 &&
                        (SpawnGrid.cells[i, j - 1].GetComponent<PlaceHero>().distance + 1 <= movement))
                        SpawnGrid.cells[i, j].GetComponent<PlaceHero>().ChangeDistance(SpawnGrid.cells[i, j - 1].GetComponent<PlaceHero>().distance + 1);
                }
                if (i + 1 < SpawnGrid.cells.GetLength(0))
                {
                    if (SpawnGrid.cells[i + 1, j].GetComponent<PlaceHero>().distance != 0 &&
                        (SpawnGrid.cells[i + 1, j].GetComponent<PlaceHero>().distance + 1 <= movement))
                        SpawnGrid.cells[i, j].GetComponent<PlaceHero>().ChangeDistance(SpawnGrid.cells[i + 1, j].GetComponent<PlaceHero>().distance + 1);
                }
                if (j + 1 < SpawnGrid.cells.GetLength(1))
                {
                    if (SpawnGrid.cells[i, j + 1].GetComponent<PlaceHero>().distance != 0 &&
                        (SpawnGrid.cells[i, j + 1].GetComponent<PlaceHero>().distance + 1 <= movement))
                        SpawnGrid.cells[i, j].GetComponent<PlaceHero>().ChangeDistance(SpawnGrid.cells[i, j + 1].GetComponent<PlaceHero>().distance + 1);
                }
            }
        }
    }

    void OnMouseDown() //When hero is clicked, check all available cells and light their lights on
    {
        PlaceHero.movement = true;
        if (PlaceHero.heroIsSelected) //If there was a selected hero, turn off lights of his available cells
        {
            TurnOffLights();
        }
        else
            PlaceHero.heroIsSelected = true;

        PlaceHero.heroSelected = gameObject;
        if (PlaceHero.heroSelected.GetComponent<Hero>().status == "")
        {
            if (PlaceHero.gameBegun) //If game has begun, lights up every available cell
            {
                //Checks four quadrants from the hero for available cells
                //++
                for (int i = x; i < SpawnGrid.cells.GetLength(0); i++)
                {
                    for (int j = z; j < SpawnGrid.cells.GetLength(1); j++)
                    {
                        if (i == x && j == z) continue;
                        CheckCell(i, j);
                    }
                }
                //+-
                for (int i = x; i < SpawnGrid.cells.GetLength(0); i++)
                {
                    for (int j = z; j >= 0; j--)
                    {
                        if (i == x && j == z) continue;
                        CheckCell(i, j);
                    }
                }
                //-+
                for (int i = x; i >= 0; i--)
                {
                    for (int j = z; j < SpawnGrid.cells.GetLength(1); j++)
                    {
                        if (i == x && j == z) continue;
                        CheckCell(i, j);
                    }
                }
                //--
                for (int i = x; i >= 0; i--)
                {
                    for (int j = z; j >= 0; j--)
                    {
                        if (i == x && j == z) continue;
                        CheckCell(i, j);
                    }
                }
                //Does the code above again, doesn't work if it doesn't repeat it, some cells get left out
                for (int i = x; i < SpawnGrid.cells.GetLength(0); i++)
                {
                    for (int j = z; j < SpawnGrid.cells.GetLength(1); j++)
                    {
                        if (i == x && j == z) continue;
                        CheckCell(i, j);
                    }
                }
                for (int i = x; i < SpawnGrid.cells.GetLength(0); i++)
                {
                    for (int j = z; j >= 0; j--)
                    {
                        if (i == x && j == z) continue;
                        CheckCell(i, j);
                    }
                }
                for (int i = x; i >= 0; i--)
                {
                    for (int j = z; j < SpawnGrid.cells.GetLength(1); j++)
                    {
                        if (i == x && j == z) continue;
                        CheckCell(i, j);
                    }
                }
                for (int i = x; i >= 0; i--)
                {
                    for (int j = z; j >= 0; j--)
                    {
                        if (i == x && j == z) continue;
                        CheckCell(i, j);
                    }
                }
                //Light up all cells that have distance != 0 and are not JumpableObstacles
                for (int i = 0; i < SpawnGrid.cells.GetLength(0); i++)
                    for (int j = 0; j < SpawnGrid.cells.GetLength(1); j++)
                        if (SpawnGrid.cells[i, j].GetComponent<PlaceHero>().distance != 0)
                        {
                            if (SpawnGrid.cells[i, j].tag != "Obstacle")
                                SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 15;
                            //Reset distance for next click
                            SpawnGrid.cells[i, j].GetComponent<PlaceHero>().ChangeDistance(0);
                        }
            }
            else //else turn on first row cell lights
            {
                GameObject[] firstRowCells = GameObject.FindGameObjectsWithTag("FirstRowCell");
                for (int i = 0; i < firstRowCells.Length; i++)
                {
                    firstRowCells[i].GetComponentInChildren<Light>().intensity = 15;
                }
            }
        }
    }

    public void SetCoordinates(int xx, int zz) //Change coordinates
    {
        x = xx;
        z = zz;
    }

    public int GetX()
    {
        return x;
    }

    public int GetZ()
    {
        return z;
    }

    public void TurnOffLights() //Turns off all cell lights
    {
        for (int i = 0; i < SpawnGrid.cells.GetLength(0); i++)
            for (int j = 0; j < SpawnGrid.cells.GetLength(1); j++)
                SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 0;
    }
}
