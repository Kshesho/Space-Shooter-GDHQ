using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationCleanup : MonoBehaviour
{
    [SerializeField] float _timeBeforeDestroy = 5f;
    
    void Start()
    {
        Destroy(this.gameObject, _timeBeforeDestroy);
    }

    
}
