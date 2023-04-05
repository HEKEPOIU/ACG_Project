using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Floor : MonoBehaviour,IAttractAble 
{
    [SerializeField] bool m_electrode;
    [SerializeField] float force = 20;
    public bool Electrode { get { return m_electrode; } set { m_electrode = value; } }
    Rigidbody2D m_Rigidbody2D;



    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

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

    public void Attract(Transform target, float speed, bool inver)
    {
        if (inver)
        {
            Vector3 distance = transform.position - target.position;
            m_Rigidbody2D.AddRelativeForce(distance.normalized * speed, ForceMode2D.Force);
        }
        else
        {
            Vector3 distance = target.position - transform.position;
            m_Rigidbody2D.AddRelativeForce(distance.normalized * speed, ForceMode2D.Force);
        }

    }
}
