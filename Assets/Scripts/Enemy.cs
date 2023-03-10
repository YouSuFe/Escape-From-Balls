using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;
    protected PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if(player.isGameOver != true)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed);
        }
        StartCoroutine(DestroyGiantEnemy());
        DestroyEnemy();
    }

    void DestroyEnemy()
    {
        if(transform.position.y < -5 || player.isGameOver == true)
        {
            Destroy(gameObject);
        }       
    }

    IEnumerator DestroyGiantEnemy()
    {
        if(gameObject.CompareTag("Enemy Giant"))
        {
            yield return new WaitForSeconds(10);
            Destroy(gameObject);
        }     
    }
}
