using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoNotifications : MonoBehaviour
{
    Animator _outOfAmmoAnim;
    AudioSource _outOfAmmoAudio;
    bool _canPlay = true;

    void Awake()
    {
        _outOfAmmoAnim = GetComponent<Animator>();
        _outOfAmmoAudio = GetComponent<AudioSource>();
    }


    public void OutOfAmmoIndicator()
    {
        if (_canPlay)
        {
            _canPlay = false;
            _outOfAmmoAnim.SetTrigger("flash");
            _outOfAmmoAudio.Play();
        }
    }

    public void OutOfAmmo_AnimationFinished()
    {
        _canPlay = true;
    }


}
