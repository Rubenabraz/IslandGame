using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f;
    private Rigidbody enemyRb;
    private GameObject player;

    [Header("Pontuação")]
    [Tooltip("Pontos concedidos quando este inimigo é morto.")]
    public int scoreValue = 10;

    private bool hasScored = false;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
       
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

        if (transform.position.y < -10 && !hasScored)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(scoreValue);
                hasScored = true;
            }

            
            Destroy(gameObject);
        }
    }

    public void KillEnemy()
    {
        
        if (ScoreManager.Instance != null && !hasScored)
        {
            ScoreManager.Instance.AddScore(scoreValue);
            hasScored = true;
        }

      
        Destroy(gameObject);
    }
}
