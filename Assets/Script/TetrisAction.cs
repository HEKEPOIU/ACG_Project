
using System.Collections.Generic;
using UnityEngine;

public class TetrisAction : MonoBehaviour, IAttractAble
{

    public static int height = 40;
    public static int width = 20;
    static Transform[,] grid = new Transform[width, height];
    static public Spawn spawn;

    float currentTime;
    float nextFallTime;

    [SerializeField] float fallTime;
    [SerializeField] Vector3 rotatePoint;

    [HideInInspector] public Rigidbody2D rb;

    bool isAiControl = true;
    [SerializeField] bool electrode = true;

    public static AudioSource audioPlayer;
    [SerializeField] AudioClip clearLine;
    [SerializeField] AudioClip hit;

    static public GameObject tetrisOnStage;

    public bool Electrode { get { return electrode; } set { electrode = value; } }

    float maxPosY;

    public Player firstTouch;

    public static List<GameObject> tOnGround = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spawn == null)
        {
            spawn = FindAnyObjectByType<Spawn>();
            audioPlayer = spawn.GetComponent<AudioSource>();
        }
        tetrisOnStage = gameObject;
    }
    void Update()
    {
        currentTime += Time.deltaTime;
        if (isAiControl)
        {
            Drop();
        }


    }

    void FixedUpdate()
    {
        if (!isAiControl)
        {
            //if (rb.velocity.x <=0.5f)
            //{
            //    rb.velocity = Vector2.zero;
            //    int roundedX = Mathf.RoundToInt(transform.position.x);
            //    int roundedY = Mathf.RoundToInt(transform.position.y);
            //    rb.MovePosition(new Vector2(roundedX, roundedY));
            //    rb.gravityScale = 17;

            //}

            if (!ValidMove(rb.velocity.normalized))
            {

                if (ValidMove(Vector2.down))
                {
                    rb.gravityScale = 17;

                }

                else
                {
                    rb.velocity = Vector2.zero;
                    rb.gravityScale = 0;
                    int roundedX = Mathf.RoundToInt(transform.position.x);
                    int roundedY = Mathf.RoundToInt(transform.position.y);
                    rb.MovePosition(new Vector2(roundedX, roundedY));

                    nextTetris();

                }
            }

           else if (rb.velocity.magnitude == 0 || !ValidMove(Vector2.down))
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                int roundedX = Mathf.RoundToInt(transform.position.x);
                int roundedY = Mathf.RoundToInt(transform.position.y);
                rb.MovePosition(new Vector2(roundedX, roundedY));

                nextTetris();
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(this.enabled ==true)
            audioPlayer.PlayOneShot(hit);
    }


    bool IsReachMaxY()
    {
        bool reachedMax = false;
        if (!reachedMax && rb.velocity.y > 0)
        {
            float currentPosY = rb.position.y + rb.velocity.normalized.y;

            if (currentPosY > maxPosY)
            {
                maxPosY = currentPosY;
            }
            else
            {
                reachedMax = true;
            }
        }
        return reachedMax;
    }
    void CheckForLines()
    {
        for (int i = height-1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }


    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }
        audioPlayer.PlayOneShot(clearLine);
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for(int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y-1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y-1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }


    void Drop()
    {
        if (currentTime > nextFallTime)
        {
            if (ValidMove(Vector2.down))
            {
                rb.MovePosition((Vector2)transform.position + Vector2.down);
            }
            else
            {
                nextTetris();
            }

            nextFallTime += fallTime;
        }
    }


    void nextTetris()
    {
        AddToGrid();
        CheckForLines();
        gameObject.layer = 11;
        audioPlayer.PlayOneShot(hit);
        tOnGround.Add(gameObject);
        Destroy(this);
    }



    bool ValidMove(Vector2 moveDir)
    {
        foreach (Transform children in transform)
        {
            float roundedX = Mathf.RoundToInt(children.transform.position.x);
            float roundedY = Mathf.RoundToInt(children.transform.position.y);

            roundedX += moveDir.x;
            roundedY += moveDir.y;

            if (roundedX < 0 || roundedX > width || roundedY < 0 || roundedY > height)
                return false;
            if (Mathf.RoundToInt(roundedX)>=width)
                return false;
            if (grid[Mathf.RoundToInt(roundedX), Mathf.RoundToInt(roundedY)] != null)
                return false;

        }


        return true;
    }


    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;
            gameObject.layer = 11;
        }
    }


    public void Rotate()
    {
        if (isAiControl)
        {
            transform.RotateAround(transform.TransformPoint(rotatePoint), Vector3.forward, 90);

            if(!ValidMove(Vector2.zero))
            {
                transform.RotateAround(transform.TransformPoint(rotatePoint), Vector3.forward, -90);
            }
        }
    }

    public void Attract(Transform target, float speed, bool inver = false, ForceMode2D forceMode2D = ForceMode2D.Force)
    {
        if (inver)
        {
            Vector3 distance = transform.position - target.position;
            rb.AddRelativeForce(distance.normalized * speed, forceMode2D);
        }
        else
        {
            Vector3 distance = target.position - transform.position;
            rb.AddRelativeForce(distance.normalized * speed, forceMode2D);
        }

        isAiControl = false;
        rb.gravityScale = 1;
        maxPosY = transform.position.y;
    }
}
