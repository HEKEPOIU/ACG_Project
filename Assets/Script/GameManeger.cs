using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameObject music;
    bool where;
    int counter = 0;

    bool isStart = true;
    // Start is called before the first frame update
    void Awake()
    {



    }
    void Start()
    {
        m_Playerinput = FindObjectsOfType<PlayerInput>();
        Time.timeScale = 0;
        for (int i = 0; i < m_Playerinput.Length; i++)
        {
            m_Playerinput[i].SwitchCurrentActionMap("UI");
            m_Playerinput[i].GetComponent<Player>().gm = this;
            int which = i;

            m_Playerinput[i].actions["Submit"].performed += e =>
            {
                if (isStart)
                {
                    tutor.SetActive(false);
                    pannl.SetActive(false);
                    setting.SetActive(true);
                    tutorText[which].text = which == 0 ? "B" : "ESC";
                    m_Playerinput[which].SwitchCurrentActionMap("Player");
                    Time.timeScale = 1;
                    isStart = false;
                }
            };
            m_Playerinput[i].actions["Submit"].performed += e =>
            {
                if (!isStart && counter>0)
                {
                    setting.SetActive(false);
                    if (where) tutor.SetActive(true);
                    else music.SetActive(true);
                }
                counter += 1;
            };
            m_Playerinput[i].actions["Navigate"].performed += e =>
            {
                if (!isStart)
                {
                    if (e.ReadValue<Vector2>().y == 1)
                    {
                        selector.transform.localPosition = new Vector3(-147.6f, 70, 0);
                        where = true;
                    }
                    else if(e.ReadValue<Vector2>().y == -1)
                    {
                        selector.transform.localPosition = new Vector3(-147.6f, -53, 0);
                        where = false;
                    }
                }
            };
            m_Playerinput[i].actions["Cancel"].performed += e =>
            {
                if (!isStart)
                {
                    music.SetActive(false);
                    tutor.SetActive(false);
                    setting.SetActive(true);
                    pannl.SetActive(false);
                    selector.transform.localPosition = new Vector3(-147.6f, 70, 0);
                    m_Playerinput[which].SwitchCurrentActionMap("Player");
                    Time.timeScale = 1;
                }
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
    }



    public void OpenUI()
    {
        for (int i = 0; i < m_Playerinput.Length; i++)
        {
            m_Playerinput[i].SwitchCurrentActionMap("UI");
        }

        Time.timeScale = 0;
        pannl.SetActive(true);
    }

}
