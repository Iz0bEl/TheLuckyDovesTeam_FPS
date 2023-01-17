using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraBetter : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
