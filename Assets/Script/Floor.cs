using UnityEngine;

public class Floor : MonoBehaviour,IAttractAble 
{
    [SerializeField] bool m_electrode;
    [SerializeField] float force = 20;
    //[SerializeField] Transform blackHole;
    public bool Electrode { get { return m_electrode; } set { m_electrode = value; } }
    Rigidbody2D m_Rigidbody2D;

    Vector2 _screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        //Attract(blackHole, 1000,false);
        //_screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

    }

    //void LateUpdate()
    //{
    //    Vector3 viewPosition = transform.position;
    //    viewPosition.x = Mathf.Clamp(viewPosition.x, _screenBounds.x * -1, _screenBounds.x);
    //    viewPosition.y = Mathf.Clamp(viewPosition.y, _screenBounds.y * -1, _screenBounds.y);
    //    transform.position = viewPosition;
    //}
    void OnTriggerStay2D(Collider2D collision)
    {
        IAttractAble attractAbleObj = collision.GetComponent<IAttractAble>();
        if (attractAbleObj != null )
        {
            if (attractAbleObj.Electrode == Electrode)
            {
                attractAbleObj.Attract(gameObject.transform, force *1.2f, true);
            }
            else if(collision.tag !="Player")
            {
                attractAbleObj.Attract(gameObject.transform, force, false);
            }
        }
    }
    public void Attract(Transform target, float speed, bool inver, ForceMode2D forceMode2D)
    {
        if (inver)
        {
            Vector3 distance = transform.position - target.position;
            m_Rigidbody2D.AddRelativeForce(distance.normalized * speed, forceMode2D);
        }
        else
        {
            Vector3 distance = target.position - transform.position;
            m_Rigidbody2D.AddRelativeForce(distance.normalized * speed, forceMode2D);
        }

    }
}
