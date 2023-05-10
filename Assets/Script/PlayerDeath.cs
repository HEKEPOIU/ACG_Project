using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    [SerializeField]AudioClip deathSound;
    [SerializeField] Vector3 spawnArea;
    Player player;

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Rigidbody2D rb = collision.rigidbody;
    //    if (rb != null )
    //    {
    //        if (rb.velocity.magnitude > 16 && isCollisionWithWall)
    //        {
    //            OnPlayerDeath(collision.gameObject);
    //        }

    //    }
    //}

    private void Start()
    {
        player = GetComponent<Player>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 8 || collision.gameObject.layer == 6)
        {
            TetrisAction.audioPlayer.PlayOneShot(deathSound);
            if (collision.gameObject.layer!=6)
                Destroy(collision.gameObject);
            GetComponent<Animator>().SetBool("onCharge", false);
            GetComponent<Animator>().SetBool("onDeath", true);
            player.enabled = false;
        }
    }


    //void OnCollisionStay2D(Collision2D collision)
    //{
    //    if ((collision.gameObject.layer == 10 || collision.gameObject.layer == 9))
    //    {
    //        isCollisionWithWall = true;
    //    }
    //}
    //void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 10 || collision.gameObject.layer == 9)
    //    {
    //        isCollisionWithWall = false;
    //    }
    //}

    public void OnPlayerDeath()
    {
        if (player.life != 0)
        {
            player.life -= 1;
            Collider[] colliders;
            do
            {
                Vector3 randomPosition = new Vector3(Random.Range(0, spawnArea.x), Random.Range(0, spawnArea.y), 0);
                colliders = Physics.OverlapSphere(randomPosition, 5f);

                if (colliders.Length == 0)
                {
                    transform.position = randomPosition;
                    GetComponent<Animator>().SetBool("onDeath", false);
                    player.enabled = true;
                }
            }
            while (colliders.Length != 0);
        }
        else
            Destroy(gameObject);
    }
}
