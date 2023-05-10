using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour,IAttractAble
{

    [HideInInspector] public bool inverElect;
    [SerializeField] private Sprite switchSprite;
    [SerializeField] public int life = 3;
    [SerializeField] float skillRange = 1.0f;


    [SerializeField] GameObject rotateChild;
    [SerializeField] SpriteRenderer arrow;

    [Header("¿ï¾ÜAttractable¹Ï¼h")]
    [SerializeField] LayerMask layerMask;
    float chargeTime =.2f;

    PlayerInput m_Playerinput;
    PlayerControl m_Playercontrol;
    float axis = 0;
    bool jump = false;
    [SerializeField] bool m_electrode;
    public bool Electrode { get{ return m_electrode; } set { m_electrode = value; } }
    Vector3 _FaceDir;




    GameObject obj;
    GameObject isClickedOn()
    {
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position,_FaceDir,skillRange, layerMask);
            Debug.DrawRay(gameObject.transform.position, _FaceDir* skillRange, Color.red);
            if (hit.collider != null && !hit.collider.CompareTag("Player"))
            {
                return hit.collider.gameObject;
            }
            return null;
    }




    // Start is called before the first frame update
    void Start()
    {
        m_Playerinput = GetComponent<PlayerInput>();
        m_Playercontrol = GetComponent<PlayerControl>();
        m_Playerinput.actions["Select"].performed += x =>
        {
            obj = isClickedOn();
        };

        m_Playerinput.actions["Select"].canceled += x =>
        {
            m_Playercontrol.powerMul += (float)x.duration;
            m_Playercontrol.Ablity(obj, Electrode);
            m_Playercontrol.powerMul = 1;
        };
        m_Playerinput.actions["look"].performed += x =>
        {
            _FaceDir = x.ReadValue<Vector2>();
            if (m_Playerinput.currentControlScheme == "Keyboard&Mouse")
                _FaceDir = (Camera.main.ScreenToWorldPoint(_FaceDir) - gameObject.transform.position).normalized;
            _FaceDir.z = 0;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Playerinput.actions["Move"].ReadValue<float>() > 0) axis = 1;
        else if (m_Playerinput.actions["Move"].ReadValue<float>() < 0) axis = -1;
        else axis = 0;

        if(m_Playerinput.actions["Select"].inProgress)
        {
            chargeTime += Time.deltaTime / 3;
            arrow.color = new Color(1,1,1, chargeTime);
        }
        else
        {
            arrow.color = new Color(1, 1, 1, .2f);
            chargeTime = .2f;
        }
        rotateChild.transform.up = _FaceDir;

        if (m_Playerinput.actions["Jump"].WasPressedThisFrame())
            jump = true;
    }
    //void LateUpdate()
    //{
    //    Vector3 viewPosition = transform.position;
    //    viewPosition.x = Mathf.Clamp(viewPosition.x,_screenBounds.x * -1 ,_screenBounds.x);
    //    viewPosition.y = Mathf.Clamp(viewPosition.y,_screenBounds.y * -1 ,_screenBounds.y);
    //    transform.position = viewPosition;
    //}


    void FixedUpdate()
    {
        m_Playercontrol.Move(axis);
        m_Playercontrol.Jump(jump);
        jump = false;
        
    }

    public void Attract(Transform target, float speed, bool inver,ForceMode2D forceMode2D)
    {
        if (inver)
        {
            Vector3 distance = transform.position - target.position;
            m_Playercontrol.m_Rigidbody2D.AddRelativeForce(distance.normalized * speed, forceMode2D);
        }
        else
        {
            Vector3 distance = target.position - transform.position;
            m_Playercontrol.m_Rigidbody2D.AddRelativeForce(distance.normalized * speed, forceMode2D);
        }
        
    }
    


}
