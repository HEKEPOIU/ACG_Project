using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float[] skillCD;
    [SerializeField] float abiltyRange;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;

    [HideInInspector] public Rigidbody2D m_Rigidbody2D;
    Vector3 m_Velocity = Vector3.zero;
    bool m_FacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        

    }
    public void Move(float axis,bool jump)
    {
        
        Vector3 targetVelocity = new Vector2(axis * moveSpeed * 10f * Time.fixedDeltaTime,m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        // If the input is moving the player right and the player is facing left...
        if (axis > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (axis < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        if (jump)
        {
            // Add a vertical force to the player.
            m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
    }
    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
