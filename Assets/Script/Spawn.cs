using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject[] tetrominoes;
    TetrisAction m_Teris;
    bool isSpawn = false;
    int dir = -2;
    bool whichColor;
    Player first;

    // Start is called before the first frame update
    void Start()
    {
        NewTetromino();
        dir *= Random.Range(0,2) == 0 ? 1:-1;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Teris == null)
        {
            NewTetromino();
        }
        if (isSpawn)
        {
            if (transform.position.x >= 16 || transform.position.x <= 3) dir *= -1;
            transform.position = transform.position + new Vector3(dir, 0, 0);
            isSpawn = false;
        }
        if (m_Teris != null)
        {
            first = m_Teris.firstTouch;
        }


    }

    public void NewTetromino()
    {
        GameObject T;
        if (first != null)
        {
            T = first.Electrode ? Instantiate(tetrominoes[Random.Range(0, 7)], transform.position, Quaternion.identity) : Instantiate(tetrominoes[Random.Range(7, 14)], transform.position, Quaternion.identity);
        }
        else
        {
            T =whichColor ? Instantiate(tetrominoes[Random.Range(0, 7)], transform.position, Quaternion.identity) : Instantiate(tetrominoes[Random.Range(7, 14)], transform.position, Quaternion.identity);
        }
        m_Teris = T.GetComponent<TetrisAction>();
        whichColor = !whichColor;
        isSpawn =true;
    }
}
