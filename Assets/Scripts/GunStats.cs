using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunStats : ScriptableObject
{
    [Header("---- CONSTANT VARIABLES DO NOT CHANGE! ----")]
    public int maxAmmoValue;
    public int ammoInClipValue;


    [Header("---- Gun Stats ----")]
    public int shootDamage;
    public float shootRate;
    public int shootDistance;
    public float reloadRate;
    public int ammoInClip;
    public int maxClip;
    public int maxAmmo;
    public GameObject gunModel;
    public AudioClip gunShot;


    
    public bool isRPG;
    public bool isSniper;
    public bool isShotgun;
    
    public GameObject UI;
    public int slotNumber;
    public string tag;


}



