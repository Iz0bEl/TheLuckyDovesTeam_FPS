using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donotdestroy : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
}
