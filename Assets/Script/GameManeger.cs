using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManeger : MonoBehaviour
{
    PlayerInput[] m_Playerinput = new PlayerInput[2];
    [SerializeField] GameObject pannl;
    [SerializeField] GameObject tutor;
    [SerializeField] Text[] tutorText;
    [SerializeField] GameObject setting;
    [SerializeField] GameObject selector;
    [SerializeField] GameObject endSelector;
    [SerializeField] GameObject music;
    [SerializeField] GameObject end;
    [SerializeField] Text winnerText;
    [SerializeField] Text timerText;
    public Text rDeathCount;
    public Text bDeathCount;
    [SerializeField] Slider volume;


    [SerializeField] AudioClip uiUpAudio;
    [SerializeField] AudioClip uiDownAudio;
    AudioSource m_AudioPlayer;
    bool where;
    int counter = 0;

    bool isEnd;

    Vector3[] targetPos = new Vector3[2];

    [SerializeField] float endTime;

    bool isStart = true;
    // Start is called before the first frame update
    void Awake()
    {

        targetPos[0] = new Vector3(-147.6f, 70, 0);
        targetPos[1] = new Vector3(-147.6f, -53, 0);

    }
    void Start()
    {
        m_AudioPlayer = GetComponent<AudioSource>();
        m_Playerinput = FindObjectsOfType<PlayerInput>();
        Time.timeScale = 0;
        for (int i = 0; i < m_Playerinput.Length; i++)
        {
            m_Playerinput[i].SwitchCurrentActionMap("UI");
            m_Playerinput[i].GetComponent<Player>().gm = this;
            int which = i;
            volume.value = AudioListener.volume;
            m_Playerinput[i].actions["Submit"].performed += e =>
            {
                if (isStart)
                {
                    tutor.SetActive(false);
                    pannl.SetActive(false);
                    setting.SetActive(true);
                    tutorText[which].text = which == 0 ? "B" : "ESC";
                    m_Playerinput[0].SwitchCurrentActionMap("Player");
                    m_Playerinput[1].SwitchCurrentActionMap("Player");
                    m_AudioPlayer.PlayOneShot(uiUpAudio);
                    Time.timeScale = 1;
                    isStart = false;
                }
            };
            m_Playerinput[i].actions["Submit"].performed += e =>
            {
                if (!isStart && counter>0)
                {
                    if (isEnd)
                    {
                        if (where) SceneManager.LoadScene(1); 
                        else Application.Quit();
                        m_AudioPlayer.PlayOneShot(uiUpAudio);
                    }

                    setting.SetActive(false);
                    if (where) tutor.SetActive(true);
                    else music.SetActive(true);
                    m_AudioPlayer.PlayOneShot(uiUpAudio);
                }
                counter += 1;
            };
            m_Playerinput[i].actions["Navigate"].performed += e =>
            {
                if (!isStart)
                {
                    if (e.ReadValue<Vector2>().y == 1)
                    {
                        selector.transform.localPosition = targetPos[0];
                        where = true;
                        m_AudioPlayer.PlayOneShot(uiUpAudio);
                    }
                    else if(e.ReadValue<Vector2>().y == -1)
                    {
                        selector.transform.localPosition = targetPos[1];
                        where = false;
                        m_AudioPlayer.PlayOneShot(uiUpAudio);
                    }
                    if (music.activeInHierarchy == true && e.ReadValue<Vector2>().x == 1)
                    {
                        AudioListener.volume += .1f;
                        if (AudioListener.volume >1)
                        {
                            AudioListener.volume = 1;
                        }
                        volume.value = AudioListener.volume;
                        m_AudioPlayer.PlayOneShot(uiUpAudio);
                    }
                    else if (music.activeInHierarchy == true && e.ReadValue<Vector2>().x == -1)
                    {
                        AudioListener.volume -= .1f;

                        volume.value = AudioListener.volume;
                        m_AudioPlayer.PlayOneShot(uiDownAudio);
                    }
                }
            };
            m_Playerinput[i].actions["Cancel"].performed += e =>
            {
                if (!isStart && !isEnd)
                {
                    music.SetActive(false);
                    tutor.SetActive(false);
                    setting.SetActive(true);
                    pannl.SetActive(false);
                    selector.transform.localPosition = targetPos[0];
                    m_Playerinput[0].SwitchCurrentActionMap("Player");
                    m_Playerinput[1].SwitchCurrentActionMap("Player");
                    m_AudioPlayer.PlayOneShot(uiUpAudio);
                    Time.timeScale = 1;
                }
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart)
        {
            timerText.text = Mathf.FloorToInt(endTime / 60).ToString() + " : " + Mathf.FloorToInt(endTime % 60);
            endTime -= Time.deltaTime;
            if (endTime <= 0 && !isEnd)
            {
                EndGame();
                isEnd = true;
                targetPos[0] = new Vector3(-147.6f, -104, 0);
                targetPos[1] = new Vector3(-147.6f, -227, 0);
                selector = endSelector;
            }
            
        }
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        setting.SetActive(false);
        pannl.SetActive(true);
        end.SetActive(true);
        Player[] players = new Player[2];
        for (int i = 0; i < m_Playerinput.Length; i++)
        {
            m_Playerinput[i].SwitchCurrentActionMap("UI");
            players[i] = m_Playerinput[i].GetComponent<Player>();
        }
        if (players[0].deathCount > players[1].deathCount)
        {
            winnerText.text = players[1].Electrode ? "Red is Winner!" : "Blue is Winner!";
        }
        else if (players[0].deathCount < players[1].deathCount)
        {
            winnerText.text = players[0].Electrode ? "Red is Winner!" : "Blue is Winner!";
        }
        else
        {
            winnerText.text = "Nobody is Winner...";
        }

    }



    public void OpenUI()
    {
        for (int i = 0; i < m_Playerinput.Length; i++)
        {
            m_Playerinput[i].SwitchCurrentActionMap("UI");
        }

        Time.timeScale = 0;
        m_AudioPlayer.PlayOneShot(uiUpAudio);
        pannl.SetActive(true);
    }

}
