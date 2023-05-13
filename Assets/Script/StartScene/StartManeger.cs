using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartManeger : MonoBehaviour
{
    PlayerInput m_Playerinput;
    [SerializeField] AudioClip startAudio;
    AudioSource audioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        m_Playerinput = GetComponent<PlayerInput>();
        m_Playerinput.SwitchCurrentActionMap("UI");



        m_Playerinput.actions["Submit"].performed += async(e) =>
        {
            audioPlayer.PlayOneShot(startAudio);
            await Task.Delay(1000);
            SceneManager.LoadScene(1);
        };

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
