using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] int sensHor;
    [SerializeField] int sensVer;
    [SerializeField][Range(0.01f,0.5f)] float sniperSens;

    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;

    [SerializeField] bool invertX;

    float mouseY;
    float mouseX;
    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //checking if scoped
        if (GameManager.instance.sniperScopeActive)
        {
            mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * (sensVer * sniperSens);
            mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * (sensHor * sniperSens);
        }
        else
        {
            mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVer;
            mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;
        }

        //fixing sens as time scale changing sens
        if (Time.timeScale != 1)
        {
            mouseY *= (2 - Time.timeScale);
            mouseX *= (2 - Time.timeScale);
        }

        if (invertX)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
    }

   
}
