using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for spawning the grid

public class SpawnGrid : MonoBehaviour
{
    public int mapNumber; //Map number, so the scripts knows where to put the obstacles
    private string gridCellPrefab;

    public int gridX;
    public int gridZ;
    public float gridSpacingOffset = 1f;
    public Vector3 gridOrigin = Vector3.zero;

    public static GameObject[,] cells; //Created cells

    public GameObject loadingPanel;
    
    void Start()
    {
        cells = new GameObject[gridX, gridZ];
        if (PhotonNetwork.IsMasterClient)
        {
            switch (mapNumber)
            {
                case 1:
                    gridCellPrefab = "Models/Cells/cellitem(DarkTown)";
                    break;
                case 2:
                    gridCellPrefab = "Models/Cells/cellitem(Christmas)";
                    break;
                case 3:
                    gridCellPrefab = "Models/Cells/cellitem(Desert)";
                    break;
            }
            Spawn();
        }
    }

    [PunRPC]
    void SpawnCell(int id, int x, int z)
    {
        PhotonView p = PhotonView.Find(id);
        p.gameObject.GetComponent<PlaceHero>().SetPosition(x, z); //Sets the position of the cell in the script PlaceHero
        cells[x, z] = p.gameObject;
    }

    [PunRPC]
    void SetCellTag(int id, string t)
    {
        PhotonView p = PhotonView.Find(id);
        p.gameObject.tag = t;
    }

    [PunRPC]
    void LoadingPanel()
    {
        loadingPanel.SetActive(false);
    }

    void Spawn()
    {
        for(int x = 0; x < gridX; x++)
        {
            for(int z = 0; z < gridZ; z++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + gridOrigin; //Calculates position for new cell
                GameObject cell = PhotonNetwork.Instantiate(gridCellPrefab, spawnPosition, Quaternion.identity); //Spawns cell prefab
                PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
                photonView.RPC("SpawnCell", RpcTarget.All, cell.GetComponent<PhotonView>().ViewID, x, z);
                if (z == 0)
                    photonView.RPC("SetCellTag", RpcTarget.All, cell.GetComponent<PhotonView>().ViewID, "FirstRowCell"); //Sets the tag of every cell in the first row, used for spawning heroes in PlaceHero script
                if (z == gridZ - 1)
                    photonView.RPC("SetCellTag", RpcTarget.All, cell.GetComponent<PhotonView>().ViewID, "LastRowCell");
            }
        }
        SetObstacles();
        StartCoroutine(DeleteLoadingPanel());
    }

    IEnumerator DeleteLoadingPanel()
    {
        PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
        yield return new WaitForSeconds(1.0f);
        photonView.RPC("LoadingPanel", RpcTarget.All);
    }

    void SetObstacles() //function for changing the tag of every cell that needs to be an obstacle
    {
        PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
        switch (mapNumber)
        {
            case 1:
                photonView.RPC("SetCellTag", RpcTarget.All, cells[6, 2].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[6, 5].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[3, 4].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[2, 4].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[3, 3].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[2, 3].GetComponent<PhotonView>().ViewID, "Obstacle");
                break;
            case 2:
                photonView.RPC("SetCellTag", RpcTarget.All, cells[3, 3].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[3, 4].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[4, 4].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[4, 4].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[1, 3].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[1, 3].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[6, 4].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[6, 4].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[4, 0].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[3, 7].GetComponent<PhotonView>().ViewID, "Obstacle");
                break;
            case 3:
                photonView.RPC("SetCellTag", RpcTarget.All, cells[0, 4].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[0, 5].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[8, 4].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[8, 5].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[9, 5].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[9, 4].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[9, 6].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[9, 3].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[9, 7].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[9, 2].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[2, 5].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[2, 4].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[3, 3].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[5, 3].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[3, 6].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[5, 6].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[6, 5].GetComponent<PhotonView>().ViewID, "Obstacle");
                photonView.RPC("SetCellTag", RpcTarget.All, cells[6, 4].GetComponent<PhotonView>().ViewID, "Obstacle");
                break;
        }
    }
}
