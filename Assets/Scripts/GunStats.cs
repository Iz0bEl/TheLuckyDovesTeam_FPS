using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunStats : ScriptableObject
{
    public int shootDamage;
    public float shootRate;
    public int shootDistance;
    public GameObject gunModel;
    public AudioClip gunShot;
    public bool isSniper;
    public bool isShotgun;
    public string gunName;
    public GameObject UI;
    public int slotNumber;
    public string tag;

}
