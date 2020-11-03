using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for spawning the grid

public class SpawnGrid : MonoBehaviour
{
    public GameObject gridCellPrefab;
    public int mapNumber; //Map number, so the scripts knows where to put the obstacles
    
    public int gridX;
    public int gridZ;
    public float gridSpacingOffset = 1f;
    public Vector3 gridOrigin = Vector3.zero;

    public static GameObject[,] cells; //Created cells
    
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
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + gridOrigin; //Calculates position for new cell
                GameObject cell = Instantiate(gridCellPrefab, spawnPosition, Quaternion.identity); //Spawns cell prefab
                cell.GetComponent<PlaceHero>().SetPosition(x, z); //Sets the position of the cell in the script PlaceHero
                cells[x, z] = cell;
                if (z == 0) cell.tag = "FirstRowCell"; //Sets the tag of every cell in the first row, used for spawning heroes in PlaceHero script
            }
        }
        SetObstacles();
    }

    void SetObstacles() //function for changing the tag of every cell that needs to be an obstacle
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
                break;
        }
    }
}
