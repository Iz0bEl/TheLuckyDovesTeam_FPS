using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunStats : ScriptableObject
{
    public int shootDamage;
    public float shootRate;
    public int shootDistance;
    public float reloadRate;
    public int ammoInClip;
    public int maxClip;
    public int maxAmmo;
    public GameObject gunModel;
    public AudioClip gunShot;
    public bool isSniper;
    public bool isShotgun;
    //public string gunName; I don't think we need this anymore? - Josh
    public GameObject UI;
    public int slotNumber;
    public string tag;


}



