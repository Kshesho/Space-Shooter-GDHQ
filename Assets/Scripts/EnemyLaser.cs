using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour, IDamagable
{
    [SerializeField] float _speed = 2;

    void Update()
    {
        transform.Translate(Vector2.down * _speed * Time.deltaTime);

        if (transform.position.y < -20)
        {
            Destroy(this.gameObject);
        }
    }

    public void Damage()
    {
        Destroy(this.gameObject);
    }


}
