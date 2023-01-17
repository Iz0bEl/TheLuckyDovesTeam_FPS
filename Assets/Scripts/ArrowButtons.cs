using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButtons : MonoBehaviour
{
    float Selection;

    [Space(10)]
    [Header("Resume")]
    public GameObject _resume;
    public GameObject _resumeSel;

    [Space(10)]
    [Header("Settings")]
    public GameObject _settings;
    public GameObject _settingsSel;

    [Space(10)]
    [Header("Restart")]
    public GameObject _restart;
    public GameObject _restartSel;

    [Space(10)]
    [Header("Quit")]
    public GameObject _Quit;
    public GameObject _QuitSel;




    // Start is called before the first frame update
    void Start()
    {
        Selection = 1;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Selection <=4)
            {
                Selection++;
            }
            if (Selection > 4)
            {
                Selection = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Selection >= 1)
            {
                Selection--;
            }
            if (Selection < 1)
            {
                Selection = 4;
            }
        }

        if (Selection == 1)
        {
            _resume.SetActive(false);
            _resumeSel.SetActive(true);

            _settings.SetActive(true);
            _settingsSel.SetActive(false);

            _restart.SetActive(true);
            _restartSel.SetActive(false);

            _Quit.SetActive(true);
            _QuitSel.SetActive(false);
        }

        if (Selection == 2)
        {
            _resume.SetActive(true);
            _resumeSel.SetActive(false);

            _settings.SetActive(false);
            _settingsSel.SetActive(true);

            _restart.SetActive(true);
            _restartSel.SetActive(false);

            _Quit.SetActive(true);
            _QuitSel.SetActive(false);
        }

        if (Selection == 3)
        {
            _resume.SetActive(true);
            _resumeSel.SetActive(false);

            _settings.SetActive(true);
            _settingsSel.SetActive(false);

            _restart.SetActive(false);
            _restartSel.SetActive(true);

            _Quit.SetActive(true);
            _QuitSel.SetActive(false);
        }

        if (Selection == 4)
        {
            _resume.SetActive(true);
            _resumeSel.SetActive(false);

            _settings.SetActive(true);
            _settingsSel.SetActive(false);

            _restart.SetActive(true);
            _restartSel.SetActive(false);

            _Quit.SetActive(false);
            _QuitSel.SetActive(true);
        }
    }
}
