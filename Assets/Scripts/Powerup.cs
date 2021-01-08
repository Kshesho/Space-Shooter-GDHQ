using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    enum TypeOfPowerup
    {
        TripleShot,
        Shield,
        SpeedBoost
    }
    [SerializeField] TypeOfPowerup _type;
    [SerializeField] float _moveSpeed = 2f;

    [SerializeField] GameObject _powerupCollectedAudioPref;


    void Update()
    {
        transform.Translate(Vector2.down * _moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            

            switch (_type)
            {
                case TypeOfPowerup.TripleShot:
                    SpawnCollectedAudio(0.7f);
                    player.ActivateTripleShotIndicator();
                    GameManager.Instance.ActivateTripleShot();
                    break;
                case TypeOfPowerup.Shield:
                    SpawnCollectedAudio(1f);
                    player.ActivateShield();
                    break;
                case TypeOfPowerup.SpeedBoost:
                    SpawnCollectedAudio(1.4f);
                    player.ActivateSpeedBoostTrail();
                    GameManager.Instance.ActivateSpeedBoost();
                    break;
            }
            Destroy(this.gameObject);
        }
    }

    void SpawnCollectedAudio(float audioPitch)
    {
        GameObject powerupCollectedAudio = Instantiate(_powerupCollectedAudioPref, this.transform.position, Quaternion.identity);
        AudioSource source = powerupCollectedAudio.GetComponent<AudioSource>();
        
        source.pitch = audioPitch;
    }


}
