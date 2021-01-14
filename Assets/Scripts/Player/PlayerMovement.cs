using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator _anim;

    [SerializeField] float _hSpeed = 6f, _vSpeed = 4f;
    [SerializeField] float _hBoostSpeed = 8f, _vBoostSpeed = 5.5f;
    float _hInput, _vInput;


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
        Move();
    }

    void Move()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        _vInput = Input.GetAxisRaw("Vertical");

        PlayerTurnAnimations();

        if (GameManager.Instance.SpeedBoostActive)
        {
            transform.Translate(new Vector3(_hInput * _hBoostSpeed, _vInput * _vBoostSpeed) * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(_hInput * _hSpeed, _vInput * _vSpeed) * Time.deltaTime);
        }

        float yClamp = Mathf.Clamp(transform.position.y, -4, 6);
        transform.position = new Vector3(transform.position.x, yClamp, transform.position.z);

        HorizontalScreenWrap();
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
