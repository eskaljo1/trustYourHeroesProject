using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
    public GameObject gridCellPrefab;
    public int mapNumber;
    
    public int gridX;
    public int gridZ;
    public float gridSpacingOffset = 1f;
    public Vector3 gridOrigin = Vector3.zero;

    public static GameObject[,] cells;
    
    void Start()
    {
        cells = new GameObject[gridX, gridZ];
        Spawn();
    }

    void Spawn()
    {
        for(int x = 0; x < gridX; x++)
        {
            for(int z = 0; z < gridZ; z++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + gridOrigin;
                GameObject cell = Instantiate(gridCellPrefab, spawnPosition, Quaternion.identity);
                cell.GetComponent<PlaceHero>().GetPosition(x, z);
                cells[x, z] = cell;
                if (z == 0) cell.tag = "FirstRowCell";
            }
        }
        SetObstacles();
    }

    void SetObstacles()
    {
        switch (mapNumber)
        {
            case 1:
                cells[6, 2].tag = "JumpableObstacle";
                cells[6, 5].tag = "JumpableObstacle";
                cells[3, 4].tag = "Obstacle";
                cells[2, 4].tag = "Obstacle";
                cells[3, 3].tag = "Obstacle";
                cells[2, 3].tag = "Obstacle";

                cells[6, 2].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[6, 5].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[3, 4].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[2, 4].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[3, 3].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[2, 3].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                break;
            case 2:
                cells[3, 3].tag = "Obstacle";
                cells[3, 4].tag = "Obstacle";
                cells[4, 4].tag = "Obstacle";
                cells[4, 3].tag = "Obstacle";
                cells[1, 3].tag = "JumpableObstacle";
                cells[1, 4].tag = "JumpableObstacle";
                cells[6, 4].tag = "JumpableObstacle";
                cells[6, 3].tag = "JumpableObstacle";
                cells[4, 0].tag = "Obstacle";
                cells[3, 7].tag = "Obstacle";

                cells[3, 3].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[3, 4].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[4, 4].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[4, 3].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[1, 3].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[1, 4].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[6, 4].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[6, 3].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[4, 0].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[3, 7].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                break;
            case 3:
                cells[0, 4].tag = "Obstacle";
                cells[0, 5].tag = "Obstacle";
                cells[8, 4].tag = "Obstacle";
                cells[8, 5].tag = "Obstacle";
                cells[9, 5].tag = "Obstacle";
                cells[9, 4].tag = "Obstacle";
                cells[9, 6].tag = "Obstacle";
                cells[9, 3].tag = "Obstacle";
                cells[9, 7].tag = "Obstacle";
                cells[9, 2].tag = "Obstacle";
                cells[2, 5].tag = "JumpableObstacle";
                cells[2, 4].tag = "JumpableObstacle";
                cells[3, 3].tag = "JumpableObstacle";
                cells[5, 3].tag = "JumpableObstacle";
                cells[3, 6].tag = "JumpableObstacle";
                cells[5, 6].tag = "JumpableObstacle";
                cells[6, 5].tag = "JumpableObstacle";
                cells[6, 4].tag = "JumpableObstacle";

                cells[0, 4].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[0, 5].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[8, 4].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[8, 5].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[9, 5].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[9, 4].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[9, 6].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[9, 3].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[9, 7].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[9, 2].GetComponent<PlaceHero>().ChangeTypeOfCell(0);
                cells[2, 5].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[2, 4].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[6, 5].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[6, 4].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[3, 3].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[5, 3].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[3, 6].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                cells[5, 6].GetComponent<PlaceHero>().ChangeTypeOfCell(2);
                break;
        }
    }
}
