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
    [SerializeField] private float powerUpStrength = 700.0f;
    public int numberOfStopChange = 5;
    public int numberOfBoostChange = 5;
    public bool isStopChangeFinished;
    public bool isBoostChangeFinished;
    public bool isGameOver;
    public bool powerUpControl
    {
        get { return hasPowerUp; }
    }
    
    
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
        StopPlayer();
        BoostPlayer();
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
                StopCoroutine(powerUpCoroutine);
            }
            powerUpCoroutine = StartCoroutine(PowerUpCountDownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if((collision.gameObject.CompareTag("Enemy Normal") || 
            collision.gameObject.CompareTag("Enemy Fast") ||
            collision.gameObject.CompareTag("Enemy Giant")) && hasPowerUp)
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

    void StopPlayer()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isStopChangeFinished == false)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            numberOfStopChange -= 1;
            gameManager.UpdateStop();
            if(numberOfStopChange == 0)
            {
                isStopChangeFinished = true;
            }
        }
    }
    
    void BoostPlayer()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isBoostChangeFinished == false)
        {
            playerRb.AddForce(focalPoint.transform.forward * 100, ForceMode.Impulse);
            numberOfBoostChange -= 1;

            if(numberOfBoostChange == 0)
            {
                isBoostChangeFinished = true;
            }
        }
    }
}
