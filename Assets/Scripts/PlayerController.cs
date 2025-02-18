using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public bool hasPowerup = false;
    private float powerUpStrength = 50.0f;
    private LifeManager lifeManager;

    public GameObject powerupIndicator;
    private Vector3 indicatorOffset = new Vector3(0, -0.5f, 0);

    // Propriedades da ShockWave
    public float shockWaveForce = 10f;
    public float shockWaveRadius = 5f;
    public float shockWaveCooldown = 60f;
    private float shockWaveTimer = 60f;

    // Variáveis para travar
    private bool isBraking = false;
    public float brakeDeceleration = 10f;

    // Variáveis para o salto
    public float jumpForce = 10f;
    private bool isGrounded = true;

    // Sistema de Vidas
    public int lives = 3;
    public float deathY = -10f;
    private Vector3 spawnPosition;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
        spawnPosition = transform.position; 
        lifeManager = GameObject.Find("LifeManager").GetComponent<LifeManager>(); 
    }

    void Update()
    {
        if (transform.position.y < deathY)
        {
            HandleFallOff();
        }

        if (Input.GetKeyDown(KeyCode.G) && isGrounded)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            isBraking = true;
        }
        else
        {
            isBraking = false;
        }

        
        if (!isBraking)
        {
            float forwardInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");

            playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);
            playerRb.AddForce(focalPoint.transform.right * horizontalInput * speed);
        }
        else
        {
            playerRb.velocity = Vector3.Lerp(playerRb.velocity, Vector3.zero, brakeDeceleration * Time.deltaTime);

            if (playerRb.velocity.magnitude < 0.1f)
            {
                playerRb.velocity = Vector3.zero;
            }
        }

        if (hasPowerup)
        {
            powerupIndicator.transform.position = transform.position + indicatorOffset;
        }

        shockWaveTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J) && shockWaveTimer >= shockWaveCooldown)
        {
            DoShockWave();
            shockWaveTimer = 0f;
        }
    }

    void DoShockWave()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, shockWaveRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                Rigidbody enemyRb = nearbyObject.GetComponent<Rigidbody>();
                if (enemyRb != null)
                {
                    Vector3 awayFromPlayer = (nearbyObject.transform.position - transform.position).normalized;
                    enemyRb.AddForce(awayFromPlayer * shockWaveForce, ForceMode.Impulse);
                }
            }
        }

        Debug.Log("ShockWave ativada!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            Debug.Log("Colidiu com Powerup");
            hasPowerup = true;
            Destroy(other.gameObject);

            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        Debug.Log("Powerup Ativo");
        yield return new WaitForSeconds(7.0f);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
        Debug.Log("Powerup Desativado");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Island"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody rbEnemy = collision.gameObject.GetComponent<Rigidbody>();
            if (rbEnemy != null)
            {
                Vector3 awayFromEnemy = (collision.gameObject.transform.position - transform.position).normalized;
                rbEnemy.AddForce(awayFromEnemy * powerUpStrength, ForceMode.Impulse);
                Debug.Log("Colidiu com: " + collision.gameObject.name + " com Powerup ativo.");
            }
        }
    }

    void HandleFallOff()
    {
        
        lifeManager.DecreaseLives();

        if (lifeManager.GetLives() > 0)
        {
            transform.position = spawnPosition;
            playerRb.velocity = Vector3.zero;
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
       
        gameObject.SetActive(false);
    }
}
