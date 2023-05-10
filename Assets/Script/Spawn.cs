using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject[] tetrominoes;
    TetrisAction m_Teris;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Teris == null)
        {
            NewTetromino();
        }
    }

    public void NewTetromino()
    {
        GameObject T =Instantiate(tetrominoes[Random.Range(0, tetrominoes.Length)], transform.position, Quaternion.identity);
        m_Teris = T.GetComponent<TetrisAction>();
    }
}
