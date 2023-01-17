using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadMainMenu : MonoBehaviour
{

    [SerializeField] GameObject parent;

    public void OnCLick_LoadMenu()
    {
        SceneManager.LoadScene(0);
        parent.SetActive(false);

        Donotdestroy temp = GameObject.FindGameObjectWithTag("Player").GetComponent<Donotdestroy>();
        Destroy(temp);
        //beta
    }
}
