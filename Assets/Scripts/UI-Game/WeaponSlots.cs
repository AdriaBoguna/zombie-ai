using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlots : MonoBehaviour
{

    public Transform primarySlot;
    public Transform secondarySlot;
    public Transform throwableSlot;

    private Transform lastActivateSlot;

    //Externals
    PlayerController player;
        
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.weapons < 1) return;
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            if (player.primaryWeapons == null) return;

            ToggleSlot(primarySlot);

            player.hasGun = true;

            player.hasRifle = false;
            player.hasGranade = false;

            player.primaryWeaponIcon.color = Color.red;
            player.secondaryWeaponIcon.color = Color.white;
            player.throwableWeaponIcon.color = Color.white;

            player.actualWeaponActive = player.primaryWeapons;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (player.secondaryWeapons == null) return;

            ToggleSlot(secondarySlot);

            player.hasRifle = true;

            player.hasGun = false;
            player.hasGranade = false;

            player.primaryWeaponIcon.color = Color.white;
            player.secondaryWeaponIcon.color = Color.red;
            player.throwableWeaponIcon.color = Color.white;

            player.actualWeaponActive = player.secondaryWeapons;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (player.throwableWeapon == null) return;

            ToggleSlot(throwableSlot);

            player.hasGranade = true;

            player.hasGun = false;
            player.hasRifle = false;

            player.primaryWeaponIcon.color = Color.white;
            player.secondaryWeaponIcon.color = Color.white;
            player.throwableWeaponIcon.color = Color.red;

            player.actualWeaponActive = player.throwableWeapon;
        }
    }

    public void ToggleSlot(Transform slot)
    {
        if (slot == lastActivateSlot)
        {
            return;
        }
        DesactivateAllSlots();

        bool isActivate = slot.gameObject.activeSelf;
        slot.gameObject.SetActive(!isActivate);

        lastActivateSlot = isActivate ? null : slot;
    }

    public void DesactivateAllSlots()
    {
        primarySlot.gameObject.SetActive(false);
        secondarySlot.gameObject.SetActive(false);
        throwableSlot.gameObject.SetActive(false);
    }
}
