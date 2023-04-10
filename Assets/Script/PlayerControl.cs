using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float[] skillCD;
    [SerializeField] float skillForce = 10;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private LayerMask m_WhatIsGround;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;

    [HideInInspector] public Rigidbody2D m_Rigidbody2D;
    Vector3 m_Velocity = Vector3.zero;
    const float k_GroundedRadius = .2f;
    bool m_FacingRight = true;
    bool m_Grounded;

    // Start is called before the first frame update
    void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        

    }

    void Update()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;

            }
        }
    }
    public void Move(float axis)
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
    }
    public void Jump(bool jump)
    {
        if (jump && m_Grounded)
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
    public void Ablity(IAttractAble obj,bool isSame)
    {

        if (isSame)
            obj.Attract(transform, skillForce, true, ForceMode2D.Impulse);
        else
            obj.Attract(transform, skillForce, false,ForceMode2D.Impulse);
        obj = null;

    }
}
