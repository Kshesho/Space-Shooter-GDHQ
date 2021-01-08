using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    AudioSource _audio; //laser fire sound effect

    [SerializeField] GameObject _laserPref, _tripleShotLaserPref, _shields, _speedBoostTrail, _tripleShotIndicator;
    float _cooldownTimer;
    [SerializeField] float _standardCooldownTime = 0.5f, _tripleShotCooldownTime = 0.25f;
    bool _shieldsActive;

    int _lives = 3;
    [SerializeField] List<GameObject> _damagedIndicators;
    [SerializeField] GameObject _explosionPref;


    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }


    void Shoot()
    {
        if (WeaponCooledDown())
        {
            if (GameManager.Instance.TripleShotActive)
            {
                FireTripleShotLaser();
            }
            else
            {
                FireStandardLaser();
            }

            _audio.Play();
        }
    } 
    void FireStandardLaser()
    {
        _cooldownTimer = Time.time + _standardCooldownTime;
        Instantiate(_laserPref, transform.position, Quaternion.identity);
    }
    void FireTripleShotLaser()
    {
        _cooldownTimer = Time.time + _tripleShotCooldownTime;
        Instantiate(_tripleShotLaserPref, transform.position, Quaternion.identity);
    }

    bool WeaponCooledDown()
    {
        if (_cooldownTimer > Time.time)
            return false;

        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.Damage();
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        if (_shieldsActive)
        {
            DeactivateShields();
            return;
        }

        _lives --;
        ShowDamagedIndicator();
        UIManager.Instance.UpdateLivesImage(_lives);

        if (_lives < 1)
        {
            Death();
        }
    }
    void ShowDamagedIndicator()
    {
        int index = Random.Range(0, _damagedIndicators.Count);
        _damagedIndicators[index].SetActive(true);
        _damagedIndicators.Remove(_damagedIndicators[index]);
    }

    void Death()
    {
        GameManager.Instance.PlayerDied();
        Instantiate(_explosionPref, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void ActivateShield()
    {
        _shieldsActive = true;
        _shields.SetActive(true);
    }
    void DeactivateShields()
    {
        _shieldsActive = false;
        _shields.SetActive(false);
    }

    public void ActivateSpeedBoostTrail()
    {
        _speedBoostTrail.SetActive(true);
        StopCoroutine("DeactivateSpeedBoostTrailRtn");
        StartCoroutine("DeactivateSpeedBoostTrailRtn");
    }
    IEnumerator DeactivateSpeedBoostTrailRtn()
    {
        yield return new WaitForSeconds(GameManager.Instance.SpeedBoostActiveTime);
        _speedBoostTrail.SetActive(false);
    }

    public void ActivateTripleShotIndicator()
    {
        _tripleShotIndicator.SetActive(true);
        StopCoroutine("DeactivateTripleShotIndicatorRtn");
        StartCoroutine("DeactivateTripleShotIndicatorRtn");
    }
    IEnumerator DeactivateTripleShotIndicatorRtn()
    {
        yield return new WaitForSeconds(GameManager.Instance.TripleShotActiveTime);
        _tripleShotIndicator.SetActive(false);
    }


}
