using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float[] skillCD;
    [SerializeField] float skillForce = 10;
    public float powerMul = 1;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private LayerMask m_WhatIsGround;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;

    Animator m_Animator;
    AudioSource m_AudioSource;
    [SerializeField] AudioClip jumpAudio;
    [SerializeField] AudioClip ablityAudio;

    [HideInInspector] public Rigidbody2D m_Rigidbody2D;
    Vector3 m_Velocity = Vector3.zero;
    const float k_GroundedRadius = .5f;
    bool m_FacingRight = true;
    bool m_Grounded;

    // Start is called before the first frame update
    void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
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
        if (skillCD[0] >= 0f)
        {
            skillCD[0] -= Time.deltaTime;
        }
        m_Animator.SetBool("onAir", !m_Grounded);
    }
    public void Move(float axis)
    {
        Vector3 targetVelocity = new Vector2(axis * moveSpeed * 10f * Time.fixedDeltaTime,m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        if (m_Rigidbody2D.velocity.x > 0 && !m_AudioSource.isPlaying && m_Grounded)
            m_AudioSource.Play();
        else if (m_AudioSource.isPlaying && (!m_Grounded|| m_Rigidbody2D.velocity.x == 0))
            m_AudioSource.Stop();
        m_Animator.SetFloat("moveSpeed", targetVelocity.magnitude);
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
            TetrisAction.audioPlayer.PlayOneShot(jumpAudio);
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
    public void Ablity(GameObject obj,bool selfElectrode,Player who)
    {
        if (obj == null) return;
        IAttractAble target = obj.GetComponent<IAttractAble>();
        bool isSame;
        if (target != null) isSame = (target.Electrode == selfElectrode);
        else return;

        if (skillCD[0] <= 0)
        {
            m_Animator.SetBool("onCharge", true);
            TetrisAction.audioPlayer.PlayOneShot(ablityAudio);
            TetrisAction tetrisAction = obj.GetComponent<TetrisAction>();
            tetrisAction.firstTouch = tetrisAction.firstTouch==null ? who : tetrisAction.firstTouch;
            target.Attract(transform, skillForce * MathF.Min(powerMul,3), isSame, ForceMode2D.Impulse);
            skillCD[0] = 0.5f;
        }

    }
    public void OnAblityAnimationEnd()
    {
        m_Animator.SetBool("onCharge", false);
    }
}
