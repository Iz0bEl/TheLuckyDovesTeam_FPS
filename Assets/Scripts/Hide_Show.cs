using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Hide_Show : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    bool A;

    private void Update()
    {
        if (Input.GetButtonDown("Help"))
        {
            A = !A;
            Panel.SetActive(A);
        }

    }

}
