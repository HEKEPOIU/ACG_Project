using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour,IAttractAble
{
    [HideInInspector] public bool inverElect;
    [SerializeField] private Sprite switchSprite;
    [SerializeField] private int life = 3;

    PlayerInput m_Playerinput;
    PlayerControl m_Playercontrol;
    float axis = 0;
    [SerializeField] bool m_electrode;
    public bool Electrode { get{ return m_electrode; } set { m_electrode = value; } }

    // Start is called before the first frame update
    void Start()
    {
        m_Playerinput = GetComponent<PlayerInput>();
        m_Playercontrol = GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Playerinput.actions["Move"].ReadValue<float>() > 0)
            axis = 1;
        else if (m_Playerinput.actions["Move"].ReadValue<float>() < 0)
            axis = -1;
        else axis = 0;
    }
    void FixedUpdate()
    {
        m_Playercontrol.Move(axis);
    }

    public void Attract(Transform target, float speed, bool inver)
    {
        if (inver)
        {
            Vector3 distance = transform.position - target.position;
            m_Playercontrol.m_Rigidbody2D.AddRelativeForce(distance.normalized * speed, ForceMode2D.Force);
        }
        else
        {
            Vector3 distance = target.position + transform.position;
            m_Playercontrol.m_Rigidbody2D.AddRelativeForce(distance.normalized * speed, ForceMode2D.Force);
        }
        
    }
}
