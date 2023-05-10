using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour
{
    public float changeTargetInterval = 10f;  // 定義更換目標的時間間隔

    private Rigidbody2D rb;
    private GameObject currentTarget;
    private float changeTargetTimer = 0f;
    GameObject[] targets;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targets = GameObject.FindGameObjectsWithTag("Player");
        currentTarget = targets[Random.Range(0,targets.Length)];
    }

    void Update()
    {
        Vector2 direction = currentTarget.transform.position - transform.position;
        rb.velocity = direction.normalized * 1;

        changeTargetTimer += Time.deltaTime;
        if (changeTargetTimer >= changeTargetInterval)
        {
            ChangeTarget();
            changeTargetTimer = 0;
        }
    }

    void ChangeTarget()
    {

        if (currentTarget == targets[0]) currentTarget = targets[ 1%targets.Length];
        else currentTarget = targets[0];
    }
}
