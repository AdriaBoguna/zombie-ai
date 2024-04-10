using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeController : MonoBehaviour
{
    public bool throwing = false;

    public float throwDelayTime = 0f;
    public float time = 0f;

    public GameObject itemPrefab;
    public GameObject theGranade;

    public Sprite weaponIcon;

    //Extern
    PlayerController player;
    WeaponSlots slots;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        slots = GetComponentInParent<WeaponSlots>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            throwing = true;
        }

        if (throwing == true)
        {
            Throw();
        }
    }

    public void Throw()
    {
        time += Time.deltaTime;
        Debug.Log(time);

        player.playerAnim.Play("Final Granade");

        if (time >= throwDelayTime)
        {
            Instantiate(theGranade, player.spawnGranade.position, player.spawnGranade.rotation);
            throwing = false;
            Destroy(this.gameObject);

            player.throwableWeapon = null;
            player.weapons--;

            player.hasGranade = false;
            player.playerAnim.SetLayerWeight(1, 1);
            player.playerAnim.SetLayerWeight(2, 0);

            player.throwableWeaponIcon.color = Color.white;
            player.throwableWeaponIcon.gameObject.SetActive(false);
        }

        if (player.secondaryWeapons != null && player.throwableWeapon == null)
        {
            player.playerAnim.SetLayerWeight(1, 1);
            player.hasRifle = true;
            slots.ToggleSlot(player.secondarySlot);
            player.primaryWeaponIcon.color = Color.white;
            player.secondaryWeaponIcon.color = Color.red;
        }
        else if (player.primaryWeapons !=null && player.secondaryWeapons == null && player.throwableWeapon == null)
        {
            player.playerAnim.SetLayerWeight(1, 1);
            player.hasGun = true;
            slots.ToggleSlot(player.primarySlot);
            player.primaryWeaponIcon.color = Color.red;
            player.secondaryWeaponIcon.color = Color.white;
        }
    }
}
