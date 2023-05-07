using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    bool isCollisionWithWall;



    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.rigidbody;
        if (rb != null )
        {
            if (rb.velocity.magnitude > 20 && isCollisionWithWall)
            {
                print(rb.velocity.magnitude);
                Destroy(rb.gameObject);
                Destroy(gameObject);
            }

        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 9)
        {
            isCollisionWithWall = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 9)
        {
            isCollisionWithWall = false;
        }
    }
}
