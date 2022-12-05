using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField] int sensHor;
    [SerializeField] int sensVer;

    [SerializeField] float sniperSens;

    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;

    [SerializeField] bool invertX;

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
        float mouseY;
        float mouseX;
        // Checking if rifle is scoped
        if (GameManager.instance.sniperScopeActive)
        {
            mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * (sniperSens * sensVer);
            mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * (sniperSens * sensHor);
        }
        else
        {
            mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVer;
            mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;
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
