using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public void FinishDialogue()
    {
        SpawnManager.Instance.StartSpawning();
        this.gameObject.SetActive(false);
    }


}
