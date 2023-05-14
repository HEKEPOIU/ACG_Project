using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartManeger : MonoBehaviour
{
    PlayerInput m_Playerinput;
    [SerializeField] AudioClip startAudio;
    AudioSource audioPlayer;
    [SerializeField] GameObject ROBJ;
    [SerializeField] GameObject BOBJ;
    

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        m_Playerinput = GetComponent<PlayerInput>();
        m_Playerinput.SwitchCurrentActionMap("UI");



        m_Playerinput.actions["Submit"].performed += async(e) =>
        {
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                audioPlayer.PlayOneShot(startAudio);
                await Task.Delay(1000);
                m_Playerinput.SwitchCurrentActionMap("Player");
                SceneManager.LoadScene(1);

            }
        };

    }

    // Update is called once per frame
    void Update()
    {

        ROBJ.transform.Translate(Vector3.left*Time.deltaTime);
        BOBJ.transform.Translate(Vector3.right*Time.deltaTime);

        if (ROBJ.transform.position.x < -9.5f)
        {
            ROBJ.transform.position = new Vector3(11.17f, -2.4f, 0);
        }
        if (BOBJ.transform.position.x > 11.17f)
        {
            BOBJ.transform.position = new Vector3(-9.5f, -2.4f, 0);
        }
    }

}
