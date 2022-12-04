using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField] int sensHor;
    [SerializeField] int sensVer;

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
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVer;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;

        if (invertX)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
