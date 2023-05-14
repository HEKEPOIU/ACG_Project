using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    [SerializeField]AudioClip deathSound;
    [SerializeField]AudioClip blockDeath;
    [SerializeField] Vector3 spawnArea;
    [SerializeField] GameObject deathFX;
    [SerializeField] GameManeger gm;
    Player player;
    Vector3 spawnPoint;
    Trace blackHole;

    bool DamageAble = true;
    [SerializeField] float invincibilityTime = 0.5f;
    float timer;

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Rigidbody2D rb = collision.rigidbody;
    //    if (rb != null )
    //    {
    //        if (rb.velocity.magnitude > 16 && isCollisionWithWall)
    //        {
    //            OnPlayerDeath(collision.gameObject);
    //        }

    //    }
    //}

    void Update()
    {
        if (DamageAble == false)
        {
            timer += Time.deltaTime;
            if (timer > invincibilityTime)
            {
                DamageAble = true;
                timer = 0;
            }
        }
        
    }

    private void Start()
    {
        player = GetComponent<Player>();
        if (player.Electrode == true) spawnPoint = new Vector3(2, 8, 0);
        else spawnPoint = new Vector3(17, 8, 0);
        blackHole = FindObjectOfType<Trace>();
        gm = FindAnyObjectByType<GameManeger>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (DamageAble && (collision.gameObject.layer == 11 || collision.gameObject.layer == 8 || collision.gameObject.layer == 6))
        {
            TetrisAction.audioPlayer.PlayOneShot(deathSound);
            if (collision.gameObject.layer!=6)
                Destroy(collision.gameObject);
            GameObject FX = Instantiate(deathFX,gameObject.transform.position, Quaternion.identity);
            Destroy(FX,0.5f);
            GetComponent<Animator>().SetBool("onCharge", false);
            GetComponent<Animator>().SetBool("onDeath", true);
            player.enabled = false;
        }
    }


    //void OnCollisionStay2D(Collision2D collision)
    //{
    //    if ((collision.gameObject.layer == 10 || collision.gameObject.layer == 9))
    //    {
    //        isCollisionWithWall = true;
    //    }
    //}
    //void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 10 || collision.gameObject.layer == 9)
    //    {
    //        isCollisionWithWall = false;
    //    }
    //}

    public void OnPlayerDeath()
    {
        if (blackHole.currentTarget.GetComponent<IAttractAble>().Electrode == player.Electrode)
        {
            blackHole.ChangeTarget();
        }

        player.deathCount += 1;
        if (player.Electrode == true) gm.rDeathCount.text = player.deathCount.ToString();
        else gm.bDeathCount.text = player.deathCount.ToString();

        transform.position = spawnPoint;
        for (int i = 0;i<TetrisAction.tOnGround.Count;i++)
        {
            if (TetrisAction.tOnGround[i] != null)
            {
                    GameObject FXbj = Instantiate(deathFX, TetrisAction.tOnGround[i].transform.position, Quaternion.identity);
                    Destroy(FXbj, .5f);

            }
            Destroy(TetrisAction.tOnGround[i]);
        }
        TetrisAction.audioPlayer.PlayOneShot(blockDeath);
        TetrisAction.tOnGround.Clear();
        GetComponent<Animator>().SetBool("onDeath", false);
        player.enabled = true;
        DamageAble = false;
            
    }
}
