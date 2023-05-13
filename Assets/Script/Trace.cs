using UnityEngine;

public class Trace : MonoBehaviour
{
    public float changeTargetInterval = 10f;  // 定義更換目標的時間間隔

    Rigidbody2D rb;
    [HideInInspector] public GameObject currentTarget;
    float changeTargetTimer = 0f;
    [SerializeField] GameObject FX;
    [SerializeField] AudioClip blockDeath;

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
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            TetrisAction.tOnGround.Remove(collision.gameObject);
            TetrisAction.audioPlayer.PlayOneShot(blockDeath);
            GameObject FXbj = Instantiate(FX, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(FXbj, .5f);
            Destroy(collision.gameObject);
            
        }
    }

    public void ChangeTarget()
    {

        if (currentTarget == targets[0]) currentTarget = targets[ 1%targets.Length];
        else currentTarget = targets[0];
        changeTargetTimer = 0;
    }
}
