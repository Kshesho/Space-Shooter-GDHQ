using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    Animator _anim;
    AudioSource _audio;
    [SerializeField] AudioClip _LaserFireClip;
    [SerializeField] float _speed = 5;
    bool _dying;

    [SerializeField] GameObject _laserPref;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }
    void Start()
    {
        StartCoroutine("FireLaserRtn");
    }
    void Update()
    {
        if (!_dying)
            Movement();   
    }

    void Movement()
    {
        transform.Translate(Vector2.down * _speed * Time.deltaTime);
        if (transform.position.y < -6)
        {
            float randomXPos = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomXPos, 8, transform.position.z);
        }
    }

    public void Damage()
    {
        _dying = true;
        StopCoroutine("FireLaserRtn");
        GameManager.Instance.UpdateScore(10);
        _anim.SetTrigger("death");
        ExplosionSound();
        Destroy(this.gameObject, 2.5f);
    }

    void ExplosionSound()
    {
        float pitch = Random.Range(0.6f, 1.3f);
        _audio.pitch = pitch;
        _audio.Play();
    }

    IEnumerator FireLaserRtn()
    {
        while(!_dying)
        {
            float waitTime = Random.Range(1f, 5f);
            yield return new WaitForSeconds(waitTime);
            Vector3 spawnPos = new Vector3(transform.position.x + 0.0135f, transform.position.y - 0.5f, 0);
            Instantiate(_laserPref, spawnPos, Quaternion.identity);
            //set audio source clip to laser clip
            //play clip at point
        }
    }


}
