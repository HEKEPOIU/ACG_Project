using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BlackHole : MonoBehaviour
{
    [SerializeField] float blackHoleForce; 
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        Vector3 distance = collision.transform.position - transform.position;
        collision.GetComponent<Rigidbody2D>().AddRelativeForce(distance.normalized * blackHoleForce, ForceMode2D.Force);
        if (distance.magnitude < 3 && collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }


}
