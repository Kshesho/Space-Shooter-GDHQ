using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    AudioSource _audio;//laser fire sound

    // TODO: give the player infinite ammo until the starting asteroid is blown up
    int _singleShotAmmoCount = 30, _tripleShotAmmoCount = 8;
    bool _tripleShotActive;
    [SerializeField] GameObject _laserPref, _tripleShotLaserPref, _speedBoostTrail, _tripleShotIndicator;
    float _cooldownTimer;
    [SerializeField] float _standardCooldownTime = 0.5f, _tripleShotCooldownTime = 0.6f;

    [SerializeField] GameObject _shields;
    SpriteRenderer _shieldsSpriteRend;
    [SerializeField] Color[] _shieldStrengthColors;
    bool _shieldsActive;
    int _shieldStrength , _maxShieldStrength = 3;
    [SerializeField] AudioSource _shieldPowerUpAudio, _shieldPowerDownAudio, _shieldDamagedAudio;

    int _lives = 3;
    [SerializeField] List<GameObject> _inactiveDamagedIndicators, _activeDamagedIndicators;
    [SerializeField] GameObject _explosionPref, _playerDamagedAudioPref;

    //----------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _shieldsSpriteRend = _shields.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && !GameManager.Instance.GamePaused)
        {
            if (WeaponCooledDown())
                Shoot();
        }
    }
    //----------------------------------------------------------------------------------------------------------------------

    void Shoot()
    {
        if (_tripleShotActive)
        {
            if (_tripleShotAmmoCount > 1)
            {
                FireTripleShotLaser();
                UIManager.Instance.UpdateAmmoDisplay_TripleShot(_tripleShotAmmoCount);
            }
            else
            {
                DeactivateTripleShot();
                FireTripleShotLaser();
            }
        }
        else
        {
            if (_singleShotAmmoCount > 0)
            {
                FireStandardLaser();
                UIManager.Instance.UpdateAmmoDisplay_SingleShot(_singleShotAmmoCount);
            }
            else
            {
                UIManager.Instance.OutOfAmmoFlash();
            }
        }
    } 
    void FireStandardLaser()
    {
        _cooldownTimer = Time.time + _standardCooldownTime;

        Instantiate(_laserPref, transform.position, Quaternion.identity);
        _singleShotAmmoCount--;

        _audio.pitch = 1.2f;
        _audio.volume = 0.3f;
        _audio.Play();
    }
    void FireTripleShotLaser()
    {
        _cooldownTimer = Time.time + _tripleShotCooldownTime;

        Instantiate(_tripleShotLaserPref, transform.position, Quaternion.identity);
        _tripleShotAmmoCount--;

        _audio.pitch = 1f;
        _audio.volume = 0.35f;
        _audio.Play();
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
            WeakenShield();
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



    #region Powerups

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

    public void GainAmmo()
    {
        _singleShotAmmoCount = 30;
        UIManager.Instance.UpdateAmmoDisplay_SingleShot(_singleShotAmmoCount);

        if (_tripleShotActive)
            DeactivateTripleShot();
    }

    public void ActivateShield()
    {
        _shieldsActive = true;
        _shieldStrength = _maxShieldStrength;
        _shieldsSpriteRend.color = _shieldStrengthColors[_shieldStrength];
        _shields.SetActive(true);

        _shieldPowerUpAudio.Play();
    }
    void WeakenShield()
    {
        _shieldStrength--;
        _shieldsSpriteRend.color = _shieldStrengthColors[_shieldStrength];

        if (_shieldStrength < 1)
        {
            _shieldsActive = false;
            _shields.SetActive(false);

            _shieldPowerDownAudio.Play();
            return;
        }

        _shieldDamagedAudio.Play();
    }

    public void ActivateTripleShot()
    {
        _tripleShotAmmoCount = 8;
        UIManager.Instance.UpdateAmmoDisplay_TripleShot(_tripleShotAmmoCount);
        _tripleShotActive = true;
        _tripleShotIndicator.SetActive(true);
        UIManager.Instance.TripleShotUI();

        //update UI and ammo for when player loses triple shot
        _singleShotAmmoCount = 30;
        UIManager.Instance.UpdateAmmoDisplay_SingleShot(_singleShotAmmoCount);
    }
    void DeactivateTripleShot()
    {
        _tripleShotActive = false;
        _tripleShotIndicator.SetActive(false);
        UIManager.Instance.SingleShotUI();
    }

    #endregion




}
