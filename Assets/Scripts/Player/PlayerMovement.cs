using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator _anim;

    bool _boosting, _overheated, _speedBoostActive;
    float _timeWhenBoostingStarted, _timeWhenBoostingStopped, _overheatedTimerValueWhenBoostingStarted, _overheatedTimerValueWhenBoostingStopped;
    [SerializeField] float _overheatTimer, _timeBeforeOverheatStarts = 4f, _speedBoostActiveTime = 5;
    [SerializeField] float _hSpeed, _vSpeed;
    [SerializeField] float _hBaseSpeed = 6f, _hBoostSpeed = 8f, _vBaseSpeed = 3.5f, _vBoostSpeed = 5.5f;
    float _hInput, _vInput;
    [SerializeField] float _lerpSpeed = 1;

    [SerializeField] SpriteRenderer _thrusterSpriteRend;
    [SerializeField] Color _thrusterBaseColor, _thrusterBoostColor;

    [SerializeField] GameObject _speedBoostThruster;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        transform.position = Vector2.zero;
    }

    void Update()
    {
        if (!_overheated && !_speedBoostActive)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _boosting = true;
                _timeWhenBoostingStarted = Time.time;
                _overheatedTimerValueWhenBoostingStarted = _overheatTimer;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _boosting = false;
                _timeWhenBoostingStopped = Time.time;
                _overheatedTimerValueWhenBoostingStopped = _overheatTimer;
            }
        }
        
        Move();
    }

    void Move()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        _vInput = Input.GetAxisRaw("Vertical");

        PlayerTurnAnimations();

        if (!_overheated && !_speedBoostActive)
            ThrusterSystem();

        //vv Actually Move vv
        transform.Translate(new Vector3(_hInput * _hSpeed, _vInput * _vSpeed) * Time.deltaTime);

        float yClamp = Mathf.Clamp(transform.position.y, -4, 6);
        transform.position = new Vector3(transform.position.x, yClamp, transform.position.z);

        HorizontalScreenWrap();
    }
    void ThrusterSystem()
    {
        //'snap' the lerp value to its end value once it gets within 0.1 of it so it doesn't take forever to lerp
        if (_boosting)
        {
            if (_hSpeed < _hBoostSpeed - 0.1f)
                _hSpeed = Mathf.Lerp(_hSpeed, _hBoostSpeed, _lerpSpeed * Time.deltaTime);
            else
                _hSpeed = _hBoostSpeed;

            if (_vSpeed < _vBoostSpeed - 0.1f)
                _vSpeed = Mathf.Lerp(_vSpeed, _vBoostSpeed, _lerpSpeed * Time.deltaTime);
            else
                _vSpeed = _vBoostSpeed;

            _thrusterSpriteRend.color = Color.Lerp(_thrusterSpriteRend.color, _thrusterBoostColor, _lerpSpeed * Time.deltaTime);
            //enable thruster particle effect v/
            //change speed boost powerup to give infinite thruster (without having to hold shift)
            //balance player speed, thrust speed, thrust overheat time, and overheat cooldown time
            //add thrusting and overheat sound effects
        }
        else
        {
            if (_hSpeed > _hBaseSpeed + 0.1f)
                _hSpeed = Mathf.Lerp(_hSpeed, _hBaseSpeed, _lerpSpeed * Time.deltaTime);
            else
                _hSpeed = _hBaseSpeed;

            if (_vSpeed > _vBaseSpeed + 0.1f)
                _vSpeed = Mathf.Lerp(_vSpeed, _vBaseSpeed, _lerpSpeed * Time.deltaTime);
            else
                _vSpeed = _vBaseSpeed;

            _thrusterSpriteRend.color = Color.Lerp(_thrusterSpriteRend.color, _thrusterBaseColor, _lerpSpeed * Time.deltaTime);
        }
        OverheatTimer();
        float overheatPercentage = (1 / GameManager.Instance.ThrusterOverheatTimer) * _overheatTimer;
        UIManager.Instance.UpdateThrusterUI(_boosting, overheatPercentage);
    }
    void OverheatTimer()
    {
        if (_boosting)
        {
            if (_overheatTimer < GameManager.Instance.ThrusterOverheatTimer)
            {
                //add time since boosting started
                _overheatTimer = _overheatedTimerValueWhenBoostingStarted + (Time.time - _timeWhenBoostingStarted);
            }
            else
            {
                TriggerOverheat();
            }
        }
        else
        {
            if (_overheatTimer > 0)
            {
                //remove time since boosting stopped
                _overheatTimer = _overheatedTimerValueWhenBoostingStopped - (Time.time - _timeWhenBoostingStopped);
            }
        }
    }
    void TriggerOverheat()
    {
        _overheated = true;
        _thrusterSpriteRend.color = _thrusterBaseColor;
        _hSpeed = _hBaseSpeed;
        _vSpeed = _vBaseSpeed;
        _boosting = false;
        _overheatTimer = 0;
        UIManager.Instance.ThrusterOverheatedUI();
    }
    public void ResetOverheat()
    {
        _overheated = false;
    }

    public void ActivateSpeedBoost()
    {
        _speedBoostActive = true;
        _hSpeed = _hBoostSpeed;
        _vSpeed = _vBoostSpeed;

        _thrusterSpriteRend.gameObject.SetActive(false);
        _speedBoostThruster.SetActive(true);
        UIManager.Instance.ThrusterSpeedBoostUI();

        StopCoroutine("DeactivateSpeedBoostRtn");
        StartCoroutine("DeactivateSpeedBoostRtn");
    }
    IEnumerator DeactivateSpeedBoostRtn()
    {
        yield return new WaitForSeconds(_speedBoostActiveTime);
        _speedBoostActive = false;
        _thrusterSpriteRend.gameObject.SetActive(true);
        _speedBoostThruster.SetActive(false);
        TriggerOverheat();
    }

    void HorizontalScreenWrap()
    {
        if (transform.position.x > 10.5f)
        {
            transform.position = new Vector3(-10.5f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -10.5f)
        {
            transform.position = new Vector3(10.5f, transform.position.y, transform.position.z);
        }
    }

    void PlayerTurnAnimations()
    {
        if (_hInput > 0)
        {
            _anim.SetBool("movingLeft", false);
            _anim.SetBool("movingRight", true);
        }
        else if (_hInput < 0)
        {
            _anim.SetBool("movingRight", false);
            _anim.SetBool("movingLeft", true);
        }
        else
        {
            _anim.SetBool("movingRight", false);
            _anim.SetBool("movingLeft", false);
        }
    }


}
