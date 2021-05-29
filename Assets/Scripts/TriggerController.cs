using UnityEngine;

[RequireComponent(typeof(Player))]

public class TriggerController : MonoBehaviour
{
    [SerializeField, Min(0)] private int pointValue = 1;
    [SerializeField, Min(0)] private int heartValue = 1;
    [SerializeField] private AudioClip heartSound;
    [SerializeField] private AudioClip pointSound;
    [SerializeField] private AudioClip finishSound;
    [SerializeField] private AudioClip sawSound;

    [SerializeField] private GameObject heartParticlePrefab;
    [SerializeField] private GameObject bloodPrefab;

    AudioSource SoundSource;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        SoundSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var otherGameObject = other.gameObject;
        var collected = false;

        if(other.tag == "Point")
        {
            player.AddPoints(pointValue);
            SoundSource.PlayOneShot(pointSound);
            collected = true;
        }else if(other.tag == "Finish")
        {
            player.Win();
            SoundSource.PlayOneShot(finishSound);
        }
        else if(other.tag == "Heart")
        {
            player.AddHealthPoint(heartValue);
            SoundSource.PlayOneShot(heartSound);
            createHeartParticles(otherGameObject.transform.position);
            collected = true;
        }

        if (collected)
        {
            otherGameObject.SetActive(false);
            Destroy(otherGameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {

        var otherGameObject = other.gameObject;
        if (otherGameObject.tag == "Saw")
        {
            player.removeHealthPoint();
            createBloodParticles(other.transform.position);
            SoundSource.PlayOneShot(sawSound);
        }
    }

    private void createHeartParticles(Vector3 position)
    {
        Instantiate(heartParticlePrefab, position, Quaternion.identity);
    }    
    private void createBloodParticles(Vector3 position)
    {
        Instantiate(bloodPrefab, position, Quaternion.identity);
    }

}
