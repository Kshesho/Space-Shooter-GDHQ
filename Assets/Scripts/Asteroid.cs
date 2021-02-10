using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDamagable
{
    [SerializeField] float _rotateSpeed;
    [SerializeField] GameObject _explosionPref;

    void Start()
    {
        
    }

    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    public void Damage()
    {
        Instantiate(_explosionPref, this.transform.position, Quaternion.identity);
        UIManager.Instance.DontTouchTheAsteroid();
        Destroy(this.gameObject);
    }


}
