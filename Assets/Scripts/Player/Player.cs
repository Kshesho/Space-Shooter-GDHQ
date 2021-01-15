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
    [SerializeField] List<GameObject> _inactiveDamagedIndicators, _activeDamagedIndicators;
    [SerializeField] GameObject _explosionPref, _playerDamagedAudioPref;

    //----------------------------------------------------------------------------------------------------------------------
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
    //----------------------------------------------------------------------------------------------------------------------

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
        Instantiate(_playerDamagedAudioPref, transform.position, Quaternion.identity);
        UIManager.Instance.UpdateLivesImage(_lives);

        if (_lives < 1)
        {
            Death();
        }
    }
    void ShowDamagedIndicator()
    {
        int index = Random.Range(0, _inactiveDamagedIndicators.Count);
        GameObject indicator = _inactiveDamagedIndicators[index];
        indicator.SetActive(true);

        _activeDamagedIndicators.Add(indicator);
        _inactiveDamagedIndicators.Remove(indicator);

        GameManager.Instance.ShakeTheCamera();
    }
    void RemoveDamageIndicator()
    {
        int index = Random.Range(0, _activeDamagedIndicators.Count);
        GameObject indicator = _activeDamagedIndicators[index];
        indicator.SetActive(false);

        _inactiveDamagedIndicators.Add(indicator);
        _activeDamagedIndicators.Remove(indicator);
    }

    void Death()
    {
        GameManager.Instance.PlayerDied();
        Instantiate(_explosionPref, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void GainOneLife()
    {
        if (_lives < 3)
        {
            _lives++;
            RemoveDamageIndicator();
            UIManager.Instance.UpdateLivesImage(_lives);
        }
        else
        {
            ActivateShield();
        }
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
