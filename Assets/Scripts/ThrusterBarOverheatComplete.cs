using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterBarOverheatComplete : MonoBehaviour
{
    
    //tell the player that they can boost again
    public void OnOverheatComplete()
    {
        UIManager.Instance.ThrusterOverheatCompleteUI();
        PlayerMovement playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        playerMovement.ResetOverheat();
    }


}
