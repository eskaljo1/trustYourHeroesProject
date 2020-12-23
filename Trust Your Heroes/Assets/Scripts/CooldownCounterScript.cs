using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownCounterScript : MonoBehaviour
{
   [PunRPC]
   void SetCooldownCounter(int id, bool one)
    {
        PhotonView p = PhotonView.Find(id);
        if (one)
            p.gameObject.GetComponent<Hero>().ability1CooldownCounter = GetComponent<Text>();
        else
            p.gameObject.GetComponent<Hero>().ability2CooldownCounter = GetComponent<Text>();
    }
}
