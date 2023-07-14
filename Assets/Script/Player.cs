using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Scripting;

public class Player : MonoBehaviour,IAttractAble
{

    [HideInInspector] public bool inverElect;
    [SerializeField] private Sprite switchSprite;
    public int deathCount = 0;
    [SerializeField] float skillRange = 1.0f;


    [SerializeField] GameObject rotateChild;
    [SerializeField] SpriteRenderer arrow;
    Light2D _light;

    [Header("選擇Attractable圖層")]
    [SerializeField] LayerMask layerMask;
    float chargeTime = 0f;

    PlayerInput m_Playerinput;
    PlayerControl m_Playercontrol;
    float axis = 0;
    bool jump = false;
    [SerializeField] bool m_electrode;
    public bool Electrode { get{ return m_electrode; } set { m_electrode = value; } }
    Vector3 _FaceDir;

    public GameManeger gm;


    [SerializeField] Color chargeColor;
    [SerializeField] Color originColor;
    [SerializeField] Color arrowColor;



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

        if (Electrode) gameObject.transform.position = new Vector3(2, 8, 0);
        else gameObject.transform.position = new Vector3(17, 8, 0);
        m_Playerinput = GetComponent<PlayerInput>();
        m_Playercontrol = GetComponent<PlayerControl>();
        m_Playerinput.actions["Select"].performed += x =>
        {
            obj = isClickedOn();
        };

        m_Playerinput.actions["Select"].canceled += x =>
        {
            m_Playercontrol.powerMul += (float)x.duration;
            m_Playercontrol.Ablity(obj, Electrode,this);
            m_Playercontrol.powerMul = 0.3f;
        };

        m_Playerinput.actions["UI"].performed += e =>
        {
            gm.OpenUI();
        };

        _light = GetComponentInChildren<Light2D>();
        arrowColor = arrow.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(TetrisAction.tetrisOnStage != null)
        {
            Vector3 vec = TetrisAction.tetrisOnStage.transform.position - gameObject.transform.position;
            if (vec.magnitude <= skillRange)
            {
                _FaceDir = (vec).normalized;
                
            }

        }
        if (m_Playerinput.actions["Move"].ReadValue<float>() > 0) axis = 1;
        else if (m_Playerinput.actions["Move"].ReadValue<float>() < 0) axis = -1;
        else axis = 0;

        if(m_Playerinput.actions["Select"].inProgress)
        {
            chargeTime += Time.deltaTime / 3;
            Color lerpColor = Color.Lerp(originColor,chargeColor, chargeTime);
            _light.intensity = 3 + chargeTime*4;
            _light.color = lerpColor;
            arrow.color = lerpColor;
        }
        else
        {
            arrow.color = arrowColor;
            _light.intensity = 0;
            chargeTime = 0f;
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
