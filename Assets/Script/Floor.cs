using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Floor : MonoBehaviour,IAttractAble 
{
    [SerializeField] bool m_electrode;
    public bool Electrode { get { return m_electrode; } set { m_electrode = value; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        print(Electrode);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        IAttractAble attractAbleObj = collision.GetComponent<IAttractAble>();
        if (attractAbleObj != null )
        {
            if (attractAbleObj.Electrode == Electrode)
            {
                attractAbleObj.Attract(gameObject.transform, 50,true);
            }
            else
            {
                attractAbleObj.Attract(gameObject.transform, 50, false);
            }
        }
    }

    public void Attract(Transform target, float speed, bool inver)
    {
        
    }
}
