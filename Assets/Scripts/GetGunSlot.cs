using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetGunSlot : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = GameManager.instance.playerScript.gunList[GameManager.instance.playerScript.selectedGun].slotNumber.ToString();
    }

   


}
