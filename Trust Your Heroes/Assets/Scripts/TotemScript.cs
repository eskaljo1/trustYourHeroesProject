using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemScript : MonoBehaviour
{
    //1 for heal, 0 for trap
    public bool type;

    [PunRPC]
    void SetTag(bool t)
    {
        if (t)
            gameObject.tag = "PlayerTotem";
        else
            gameObject.tag = "Player2Totem";
    }

    [PunRPC]
    void DeathHeal(bool p, int t)
    {
        if (p)
        {
            PhotonView pv = PhotonView.Find(t);
            pv.gameObject.GetComponent<Hero>().health += 30;
            GetComponent<ParticleSystem>().Play();
        }
        StartCoroutine(Die());
    }

    [PunRPC]
    void DeathTrap(bool p, int t)
    {
        if (p)
        {
            PhotonView pv = PhotonView.Find(t);
            pv.gameObject.GetComponent<Hero>().health -= 30;
            GetComponent<ParticleSystem>().Play();
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.8f);
        if (GetComponent<PhotonView>().IsMine) PhotonNetwork.Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (gameObject.tag == "PlayerTotem" && NetworkManager.firstPlayer)
        {
            if (collider.gameObject.tag == "Player")
            {
                if (type)
                    GetComponent<PhotonView>().RPC("DeathHeal", RpcTarget.All, true, collider.gameObject.GetComponent<PhotonView>().ViewID);
                else
                    GetComponent<PhotonView>().RPC("DeathTrap", RpcTarget.All, false, 0);
            }
            else if (collider.gameObject.tag == "Player2")
            {
                if (type)
                    GetComponent<PhotonView>().RPC("DeathHeal", RpcTarget.All, false, 0);
                else
                    GetComponent<PhotonView>().RPC("DeathTrap", RpcTarget.All, true, collider.gameObject.GetComponent<PhotonView>().ViewID);
            }
        }
        else if (gameObject.tag == "Player2Totem" && !NetworkManager.firstPlayer)
        {
            if (collider.gameObject.tag == "Player")
            {
                if (type)
                    GetComponent<PhotonView>().RPC("DeathHeal", RpcTarget.All, false, 0);
                else
                    GetComponent<PhotonView>().RPC("DeathTrap", RpcTarget.All, true, collider.gameObject.GetComponent<PhotonView>().ViewID);
            }
            else if(collider.gameObject.tag == "Player2")
            {
                if (type)
                    GetComponent<PhotonView>().RPC("DeathHeal", RpcTarget.All, true, collider.gameObject.GetComponent<PhotonView>().ViewID);
                else
                    GetComponent<PhotonView>().RPC("DeathTrap", RpcTarget.All, false, 0);
            }
        }
    }
}
