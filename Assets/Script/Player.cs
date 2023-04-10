using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour,IAttractAble
{
    [HideInInspector] public bool inverElect;
    [SerializeField] private Sprite switchSprite;
    [SerializeField] private int life = 3;
    [SerializeField] float skillRange = 1.0f;
    [SerializeField] GameObject child;
    [Header("選擇Attractable圖層")]
    [SerializeField] LayerMask layerMask;


    PlayerInput m_Playerinput;
    PlayerControl m_Playercontrol;
    float axis = 0;
    bool jump = false;
    [SerializeField] bool m_electrode;
    public bool Electrode { get{ return m_electrode; } set { m_electrode = value; } }
    private Vector2 _screenBounds;
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

        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3 ( Screen.width, Screen.height, Camera.main.transform.position.z ));
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Playerinput.actions["Move"].ReadValue<float>() > 0) axis = 1;
        else if (m_Playerinput.actions["Move"].ReadValue<float>() < 0) axis = -1;
        else axis = 0;

        m_Playerinput.actions["look"].performed += x => { _FaceDir = x.ReadValue<Vector2>(); };
        if (m_Playerinput.currentControlScheme == "Keyboard&Mouse")
            _FaceDir = (Camera.main.ScreenToWorldPoint(_FaceDir)-gameObject.transform.position).normalized;
        child.transform.up = _FaceDir;


        m_Playerinput.actions["Select"].performed += x => {print("click"); obj = isClickedOn();};
        
        m_Playerinput.actions["Select"].canceled += x => 
        {
            if (obj==null)
            {
                return;
            }
            IAttractAble target = obj.GetComponent<IAttractAble>();
            bool isSame = false;
            if (target != null)
            {
                isSame = (target.Electrode == this.Electrode);
            }
            m_Playercontrol.Ablity(target, isSame); 
        };


        if (m_Playerinput.actions["Jump"].WasPressedThisFrame())
        {
            jump = true;
        }
    }
    void LateUpdate()
    {
        Vector3 viewPosition = transform.position;
        viewPosition.x = Mathf.Clamp(viewPosition.x,_screenBounds.x * -1 ,_screenBounds.x);
        viewPosition.y = Mathf.Clamp(viewPosition.y,_screenBounds.y * -1 ,_screenBounds.y);
        transform.position = viewPosition;
    }
    //void OnBecameInvisible() //相機再也看不到他，我用來倒捲的。
    //{
    //    // 0,1,0在viewport空間是上緣，0,0,0是下緣。
    //    Vector3 newPosition = transform.position;
    //    if (newPosition.y<=0) newPosition.y = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + transform.localScale.y;
    //    else newPosition.y = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - transform.localScale.y;

    //    // Set the new position of the object
    //    transform.position = newPosition;
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
