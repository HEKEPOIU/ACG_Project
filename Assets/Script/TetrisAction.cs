
using DG.Tweening;
using UnityEngine;
using static Unity.VisualScripting.Metadata;
using static UnityEngine.GraphicsBuffer;

public class TetrisAction : MonoBehaviour, IAttractAble
{

    public static int height = 40;
    public static int width = 20;
    static Transform[,] grid = new Transform[width, height];
    static Spawn spawn;


    float currentTime;
    float nextFallTime;

    [SerializeField] float fallTime;
    [SerializeField] Vector3 rotatePoint;

    [HideInInspector] public Rigidbody2D rb;

    bool isAiControl = true;

    [SerializeField] bool electrode = true;

    public bool Electrode { get { return electrode; } set { electrode = value; } }

    float maxPosY;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawn = FindAnyObjectByType<Spawn>();
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
            //if (IsReachMaxY())
            //{
            //        rb.velocity = Vector2.zero;
            //        int roundedX = Mathf.RoundToInt(transform.position.x) ;
            //        int roundedY = Mathf.RoundToInt(transform.position.y);
            //        rb.MovePosition(new Vector2(roundedX, roundedY));
            //        rb.gravityScale = 10;

            //}

            if (!ValidMove(rb.velocity.normalized))
            {

                if (ValidMove(Vector2.down))
                {
                    rb.gravityScale = 10;
                }

                else
                {
                    rb.velocity = Vector2.zero;
                    rb.gravityScale = 0;
                    int roundedX = Mathf.RoundToInt(transform.position.x);
                    int roundedY = Mathf.RoundToInt(transform.position.y);
                    rb.MovePosition(new Vector2(roundedX, roundedY));


                    spawn.NewTetromino();
                    AddToGrid();
                    CheckForLines();
                    gameObject.layer = 9;
                    this.enabled = false;
                    
                }
            }
        }
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

    void Move(Vector2 dir)
    {
        if (ValidMove(dir) && isAiControl)
        {
            rb.MovePosition((Vector2)transform.position + dir);
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
                spawn.NewTetromino();
                AddToGrid();
                CheckForLines();
                rb.bodyType = RigidbodyType2D.Static;
                this.enabled = false;
            }

            nextFallTime += fallTime;
        }
    }

    float easeInQuint(float x)
    {
        return x * (x*4);
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
