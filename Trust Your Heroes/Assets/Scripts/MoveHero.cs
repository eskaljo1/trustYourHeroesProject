using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHero : MonoBehaviour
{
    private int x = 0;
    private int z = 0;

    public int movement = 2;
    public bool obstacleJumper = false;

    void CheckCell(int i, int j)
    {
        if (SpawnGrid.cells[i, j].tag != "OccupiedCell" && SpawnGrid.cells[i, j].GetComponent<PlaceHero>().distance == 0 && SpawnGrid.cells[i, j].GetComponent<PlaceHero>().typeOfCell != 0 && (SpawnGrid.cells[i, j].GetComponent<PlaceHero>().typeOfCell != 2 || obstacleJumper))
        {
            if ((Mathf.Abs(x - i) + Mathf.Abs(z - j)) == 1)
            {
                SpawnGrid.cells[i, j].GetComponent<PlaceHero>().ChangeDistance(1);
            }
            else
            {
                if (i - 1 >= 0)
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

    void OnMouseDown()
    {
        if (PlaceHero.heroIsSelected)
        {
            TurnOffLights();
        }
        else
            PlaceHero.heroIsSelected = true;

        PlaceHero.heroSelected = gameObject;

        if (PlaceHero.gameBegun)
        {
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
            for (int i = 0; i < SpawnGrid.cells.GetLength(0); i++)
                for (int j = 0; j < SpawnGrid.cells.GetLength(1); j++)
                    if (SpawnGrid.cells[i, j].GetComponent<PlaceHero>().distance != 0)
                    {
                        if(SpawnGrid.cells[i, j].GetComponent<PlaceHero>().typeOfCell != 2)
                            SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 15;
                        SpawnGrid.cells[i, j].GetComponent<PlaceHero>().ChangeDistance(0);
                    }
            /*for (int i = 0; i < SpawnGrid.cells.GetLength(0); i++)
                for (int j = 0; j < SpawnGrid.cells.GetLength(1); j++)
                    if (SpawnGrid.cells[i, j].tag != "OccupiedCell" && SpawnGrid.cells[i, j].tag != "Obstacle" && SpawnGrid.cells[i, j].tag != "JumpableObstacle")
                        if ((Mathf.Abs(x - i) + Mathf.Abs(z - j)) <= movement)
                            SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 15;
            for (int i = 0; i < SpawnGrid.cells.GetLength(0); i++)
                for (int j = 0; j < SpawnGrid.cells.GetLength(1); j++)
                    if (SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity == 15)
                        if (movement > 1)
                            if (!((i - 1 == x && j == z) || (i + 1 == x && j == z) || (i == x && j - 1 == z) || (i == x && j + 1 == z)))
                            {
                                bool b1 = false, b2 = false, b3 = false, b4 = false;
                                if (i - 1 >= 0)
                                {
                                    if (SpawnGrid.cells[i - 1, j].GetComponentInChildren<Light>().intensity != 15)
                                    {
                                        if (!obstacleJumper || (SpawnGrid.cells[i - 1, j].tag != "JumpableObstacle" && j == z))
                                            b1 = true;
                                    }
                                    else if (movement > 2)
                                        if (i > x)
                                        {
                                            for (int k = x; k < i; k++)
                                                if ((SpawnGrid.cells[k, j].tag == "JumpableObstacle" && !obstacleJumper) || SpawnGrid.cells[k, j].tag == "Obstacle")
                                                {
                                                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 0;
                                                    return;
                                                }
                                        }
                                        else
                                        {
                                            for (int k = i; k < x; k++)
                                                if ((SpawnGrid.cells[k, j].tag == "JumpableObstacle" && !obstacleJumper) || SpawnGrid.cells[k, j].tag == "Obstacle")
                                                {
                                                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 0;
                                                    return;
                                                }
                                        }
                                }
                                else b1 = true;
                                if (j - 1 >= 0)
                                {
                                    if (SpawnGrid.cells[i, j - 1].GetComponentInChildren<Light>().intensity != 15)
                                    {
                                        if (!obstacleJumper || (SpawnGrid.cells[i, j - 1].tag != "JumpableObstacle" && i == x))
                                            b2 = true;
                                    }
                                    else if (movement > 2)
                                        if (j > z)
                                        {
                                            for (int k = z; k < j; k++)
                                                if ((SpawnGrid.cells[i, k].tag == "JumpableObstacle" && !obstacleJumper) || SpawnGrid.cells[i, k].tag == "Obstacle")
                                                {
                                                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 0;
                                                    return;
                                                }
                                        }
                                        else
                                        {
                                            for (int k = j; k < z; k++)
                                                if ((SpawnGrid.cells[i, k].tag == "JumpableObstacle" && !obstacleJumper) || SpawnGrid.cells[i, k].tag == "Obstacle")
                                                {
                                                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 0;
                                                    return;
                                                }
                                        }
                                }
                                else b2 = true;
                                if (i + 1 < SpawnGrid.cells.GetLength(0))
                                {
                                    if (SpawnGrid.cells[i + 1, j].GetComponentInChildren<Light>().intensity != 15)
                                    {
                                        if (!obstacleJumper || (SpawnGrid.cells[i + 1, j].tag != "JumpableObstacle" && j == z))
                                            b3 = true;
                                    }
                                    else if (movement > 2)
                                        if (i > x)
                                        {
                                            for (int k = x; k < i; k++)
                                                if ((SpawnGrid.cells[k, j].tag == "JumpableObstacle" && !obstacleJumper) || SpawnGrid.cells[k, j].tag == "Obstacle")
                                                {
                                                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 0;
                                                    return;
                                                }
                                        }
                                        else
                                        {
                                            for (int k = i; k < x; k++)
                                                if ((SpawnGrid.cells[k, j].tag == "JumpableObstacle" && !obstacleJumper) || SpawnGrid.cells[k, j].tag == "Obstacle")
                                                {
                                                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 0;
                                                    return;
                                                }
                                        }
                                }
                                else b3 = true;
                                if (j + 1 < SpawnGrid.cells.GetLength(1))
                                {
                                    if (SpawnGrid.cells[i, j + 1].GetComponentInChildren<Light>().intensity != 15)
                                    {
                                        if (!obstacleJumper || (SpawnGrid.cells[i, j + 1].tag != "JumpableObstacle" && i == x))
                                            b4 = true;
                                    }
                                    else if (movement > 2)
                                        if (j > z)
                                        {
                                            for (int k = z; k < j; k++)
                                                if ((SpawnGrid.cells[i, k].tag == "JumpableObstacle" && !obstacleJumper) && SpawnGrid.cells[i, k].tag == "Obstacle")
                                                {
                                                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 0;
                                                    return;
                                                }
                                        }
                                        else
                                        {
                                            for (int k = j; k < z; k++)
                                                if ((SpawnGrid.cells[i, k].tag == "JumpableObstacle" && !obstacleJumper) && SpawnGrid.cells[i, k].tag == "Obstacle")
                                                {
                                                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 0;
                                                    return;
                                                }
                                        }
                                }
                                else b4 = true;
                                //Debug.Log(b1.ToString() + b2.ToString() + b3.ToString()+b4.ToString());
                                if (b1 && b2 && b3 && b4)
                                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 0;
                            }*/
        }
        else
        {
            GameObject[] firstRowCells = GameObject.FindGameObjectsWithTag("FirstRowCell");
            for (int i = 0; i < firstRowCells.Length; i++)
            {
                firstRowCells[i].GetComponentInChildren<Light>().intensity = 15;
            }
        }         
    }

    public void Placed(int xx, int zz)
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

    public void TurnOffLights()
    {
        for (int i = 0; i < SpawnGrid.cells.GetLength(0); i++)
            for (int j = 0; j < SpawnGrid.cells.GetLength(1); j++)
                SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 0;
    }
}
