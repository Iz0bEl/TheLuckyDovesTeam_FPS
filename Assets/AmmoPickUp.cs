using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.playerScript.maxAmmo += GameManager.instance.playerScript.gunList[GameManager.instance.playerScript.selectedGun].maxClip;
            GameManager.instance.updateAmmo();
            Destroy(gameObject);
        }
        
    }

}
