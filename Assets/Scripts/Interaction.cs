using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    [SerializeField] bool hasRequirement;
    [SerializeField] GameObject requiredObject;
    [SerializeField] Transform interactSpot;

    [SerializeField] GameObject interactIcon;
    [SerializeField] GameObject interactUI;
    [SerializeField] GameObject requiredIcon;
    [SerializeField] GameObject requiredUI;

    bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        interactIcon.SetActive(true);
        interactUI.SetActive(false);
        requiredIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            //if (hasRequirement && playerhasrequirement && Input.GetButtonDown("Interact"))
            //GameManager.instance.playerScript.Interact(interactSpot);
            if (Input.GetButtonDown("Interact"))
            {
                GameManager.instance.playerScript.Interact(interactSpot);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactIcon.SetActive(false);
            //if (hasRequirement && playerhasrequirement)
            //requiredUI.SetActive(true);
            if (hasRequirement)
            {
                requiredIcon.SetActive(true);
            }
            else
            {
                interactUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactIcon.SetActive(true);
            requiredUI.SetActive(false);
            requiredIcon.SetActive(false);
            interactUI.SetActive(false);
        }
    }
}
