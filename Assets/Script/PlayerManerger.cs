using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManerger : MonoBehaviour
{
    PlayerInputManager m_PlayerInputManager;
    
    // Start is called before the first frame update
    void Awake()
    {
        m_PlayerInputManager = GetComponent<PlayerInputManager>();
        m_PlayerInputManager.JoinPlayer();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initPlayer()
    {

    }
}
