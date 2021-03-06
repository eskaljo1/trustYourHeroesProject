﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script attached to every hero, used for movement

public class MoveHero : MonoBehaviour
{
    //Coordinates
    private int x = 0;
    private int z = 0;

    public int movement = 2; //Movement distance
    public bool obstacleJumper = false; //Can he jump over obstacles

    public static bool player1Move = true;
    public static bool performing = false;

    //Erasmo's grass needs to dissapear below
    void Start()
    {
        performing = false;
        player1Move = true;
        if (name == "Erasmo(Clone)")
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.045f, transform.position.z);
    }

    void CheckCell(int i, int j, int heroMovement) //Check if cell is available to move to
    {
        if (NetworkManager.firstPlayer)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player2");
            for (int e = 0; e < enemies.Length; e++)
                if (enemies[e].name == "Creek(Clone)")
                    if ((Mathf.Abs(x - enemies[e].GetComponent<MoveHero>().GetX()) + Mathf.Abs(z - enemies[e].GetComponent<MoveHero>().GetZ())) == 1)
                        heroMovement--;
        }
        else
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
            for (int e = 0; e < enemies.Length; e++)
                if (enemies[e].name == "Creek(Clone)")
                    if ((Mathf.Abs(x - enemies[e].GetComponent<MoveHero>().GetX()) + Mathf.Abs(z - enemies[e].GetComponent<MoveHero>().GetZ())) == 1)
                        heroMovement--;
        }
        if (GetComponent<Hero>().slow) heroMovement--;
        if (GetComponent<Hero>().movement != 0) heroMovement += GetComponent<Hero>().movement;
        if (heroMovement < 1) return;
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
                        (SpawnGrid.cells[i - 1, j].GetComponent<PlaceHero>().distance + 1 <= heroMovement))
                        SpawnGrid.cells[i, j].GetComponent<PlaceHero>().ChangeDistance(SpawnGrid.cells[i - 1, j].GetComponent<PlaceHero>().distance + 1);
                }
                if (j - 1 >= 0)
                {
                    if (SpawnGrid.cells[i, j - 1].GetComponent<PlaceHero>().distance != 0 &&
                        (SpawnGrid.cells[i, j - 1].GetComponent<PlaceHero>().distance + 1 <= heroMovement))
                        SpawnGrid.cells[i, j].GetComponent<PlaceHero>().ChangeDistance(SpawnGrid.cells[i, j - 1].GetComponent<PlaceHero>().distance + 1);
                }
                if (i + 1 < SpawnGrid.cells.GetLength(0))
                {
                    if (SpawnGrid.cells[i + 1, j].GetComponent<PlaceHero>().distance != 0 &&
                        (SpawnGrid.cells[i + 1, j].GetComponent<PlaceHero>().distance + 1 <= heroMovement))
                        SpawnGrid.cells[i, j].GetComponent<PlaceHero>().ChangeDistance(SpawnGrid.cells[i + 1, j].GetComponent<PlaceHero>().distance + 1);
                }
                if (j + 1 < SpawnGrid.cells.GetLength(1))
                {
                    if (SpawnGrid.cells[i, j + 1].GetComponent<PlaceHero>().distance != 0 &&
                        (SpawnGrid.cells[i, j + 1].GetComponent<PlaceHero>().distance + 1 <= heroMovement))
                        SpawnGrid.cells[i, j].GetComponent<PlaceHero>().ChangeDistance(SpawnGrid.cells[i, j + 1].GetComponent<PlaceHero>().distance + 1);
                }
            }
        }
    }

    void OnMouseDown() //When hero is clicked, check all available cells and light their lights on
    {
        if (!performing)
        {
            if ((!PlaceHero.gameBegun && ((!PlaceHeroButtons.ready1 && NetworkManager.firstPlayer) || (!PlaceHeroButtons.ready2 && !NetworkManager.firstPlayer))) || (PlaceHero.gameBegun && (player1Move && NetworkManager.firstPlayer) || (!player1Move && !NetworkManager.firstPlayer)))
            {
                if ((NetworkManager.firstPlayer && gameObject.tag == "Player") || (!NetworkManager.firstPlayer && gameObject.tag == "Player2"))
                {
                    PlaceHero.movement = true;
                    if (PlaceHero.heroIsSelected) //If there was a selected hero, turn off lights of his available cells
                    {
                        TurnOffLights();
                    }
                    else
                        PlaceHero.heroIsSelected = true;

                    PlaceHero.heroSelected = gameObject;
                    if (!PlaceHero.heroSelected.GetComponent<Hero>().stun)
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
                                    CheckCell(i, j, movement);
                                }
                            }
                            //+-
                            for (int i = x; i < SpawnGrid.cells.GetLength(0); i++)
                            {
                                for (int j = z; j >= 0; j--)
                                {
                                    if (i == x && j == z) continue;
                                    CheckCell(i, j, movement);
                                }
                            }
                            //-+
                            for (int i = x; i >= 0; i--)
                            {
                                for (int j = z; j < SpawnGrid.cells.GetLength(1); j++)
                                {
                                    if (i == x && j == z) continue;
                                    CheckCell(i, j, movement);
                                }
                            }
                            //--
                            for (int i = x; i >= 0; i--)
                            {
                                for (int j = z; j >= 0; j--)
                                {
                                    if (i == x && j == z) continue;
                                    CheckCell(i, j, movement);
                                }
                            }
                            //Does the code above again, doesn't work if it doesn't repeat it, some cells get left out
                            for (int i = x; i < SpawnGrid.cells.GetLength(0); i++)
                            {
                                for (int j = z; j < SpawnGrid.cells.GetLength(1); j++)
                                {
                                    if (i == x && j == z) continue;
                                    CheckCell(i, j, movement);
                                }
                            }
                            for (int i = x; i < SpawnGrid.cells.GetLength(0); i++)
                            {
                                for (int j = z; j >= 0; j--)
                                {
                                    if (i == x && j == z) continue;
                                    CheckCell(i, j, movement);
                                }
                            }
                            for (int i = x; i >= 0; i--)
                            {
                                for (int j = z; j < SpawnGrid.cells.GetLength(1); j++)
                                {
                                    if (i == x && j == z) continue;
                                    CheckCell(i, j, movement);
                                }
                            }
                            for (int i = x; i >= 0; i--)
                            {
                                for (int j = z; j >= 0; j--)
                                {
                                    if (i == x && j == z) continue;
                                    CheckCell(i, j, movement);
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
                    }
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

    [PunRPC]
    void ChangePerforming()
    {
        performing = !performing;
    }
}
