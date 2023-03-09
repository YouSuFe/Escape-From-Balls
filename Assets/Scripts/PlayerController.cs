using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private GameManager gameManager;
    public GameObject powerUpIndicator;
    
    [SerializeField] private float speed = 5;
    [SerializeField] private bool hasPowerUp;
    private float powerUpStrength = 15.0f;
    public bool isGameOver;
    
    private Coroutine powerUpCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);
        powerUpIndicator.transform.position = transform.position + new Vector3(0,-0.5f,0);
        DestroyIfOutOfBounds();
    }

    IEnumerator PowerUpCountDownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        powerUpIndicator.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject);
            hasPowerUp = true;
            powerUpIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerUpCountDownRoutine());

            // Check if player has power up then start coroutine again
            if (powerUpCoroutine != null)
            {
                StopCoroutine(PowerUpCountDownRoutine());
            }
            powerUpCoroutine = StartCoroutine(PowerUpCountDownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy Normal") && hasPowerUp)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            enemyRb.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
    }
    // Destroys player if it is out of bound
    private void DestroyIfOutOfBounds()
    {
        if(transform.position.y < -1)
        {
            Destroy(gameObject);
            gameManager.GameOver();
        }
    }
}
