using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool Player = true;
    public bool Active = true;

    //Player
    Transform playerTr;
    Rigidbody playerRb;
    internal Animator playerAnim;
    RagdollController playerRagdoll;

    public float maxHealth = 100f;
    public float currentHealth;

    public float playerSpeed = 0f;

    public bool hasGun = false;
    public bool hasRifle = false;
    public bool hasGranade = false;

    private Vector2 newDirection;

    //Actions
    public bool inventoryOpen = false;
    public GameObject[] droppedItems;
    public GameObject actualWeaponActive;
    public GameObject dropThisWeapon;
    public bool dropWeapon = false;

    //Cámara
    public Transform cameraAxis;
    public Transform cameraTrack;
    public Transform cameraWeaponTrack;
    private Transform theCamera;

    private float rotY = 0f;
    private float rotX = 0f;

    public float camRotSpeed = 200f;
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float cameraSpeed = 200f;

    //Items
    public GameObject nearItem;
    public GameObject[] itemPrefabs;
    public Transform itemSlot;

    public GameObject crosshair;

    //Weapons
    public int weapons;
    public GameObject primaryWeapons;
    public GameObject secondaryWeapons;
    public GameObject throwableWeapon;

    public Transform primarySlot;
    public Transform secondarySlot;
    public Transform throwableSlot;

    public Transform spawnGranade;

    //UI
    public Canvas playerUI;
    public Image primaryWeaponIcon;
    public Image secondaryWeaponIcon;
    public Image throwableWeaponIcon;

    public GameObject inventoryController;

    //Externals
    WeaponSlots weaponSlots;

    //Sound
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    public bool isMoving;

    //Animations
    private readonly int dance = Animator.StringToHash("Dance");

    // Start is called before the first frame update
    void Start()
    {
        playerTr = this.transform;
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        playerRagdoll = GetComponentInChildren<RagdollController>();
        weaponSlots = GetComponentInChildren<WeaponSlots>();

        theCamera = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;

        currentHealth = maxHealth;
        Active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player)
        {
            MoveLogic();
            CameraLogic();
            Dance();
        }

        if (!Active) return;

        ActionsLogic();
        
        ItemLogic();
        AnimLogic();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(10f);
        }
    }

    private void Dance()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            playerAnim.Play(dance);
        }
    }

    public void MoveLogic()
    {
        if (inventoryOpen == true)
        {
            return;
        }

        Vector3 direction = playerRb.velocity;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float theTime = Time.deltaTime;

        newDirection = new Vector2 (moveX, moveZ);

        Vector3 side = playerSpeed * moveX * theTime * playerTr.right;
        Vector3 forward = playerSpeed * moveZ * theTime * playerTr.forward;

        Vector3 endDirection = side + forward;

        playerRb.velocity = endDirection + playerRb.velocity.y * Vector3.up;

        
        if (lastPosition != gameObject.transform.position)
        {
            isMoving = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
        }
        else
        {
            isMoving = false;
            SoundManager.Instance.grassWalkSound.Stop();
        }
        lastPosition = gameObject.transform.position;

    }

    public void CameraLogic()
    {
        if (inventoryOpen == true)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float theTime = Time.deltaTime;

        rotY += mouseY * theTime * camRotSpeed;
        rotX = mouseX * theTime * camRotSpeed;

        playerTr.Rotate(0, rotX, 0);

        rotY = Mathf.Clamp(rotY, minAngle, maxAngle);

        Quaternion localRotation = Quaternion.Euler(-rotY, 0, 0);
        cameraAxis.localRotation = localRotation;

        if (hasGun || hasRifle || hasGranade)
        {
            cameraTrack.gameObject.SetActive(false);
            cameraWeaponTrack.gameObject.SetActive(true);

            crosshair.gameObject.SetActive(true);

            theCamera.position = Vector3.Lerp(theCamera.position, cameraWeaponTrack.position, cameraSpeed * theTime);
            theCamera.rotation = Quaternion.Lerp(theCamera.rotation, cameraWeaponTrack.rotation, camRotSpeed * theTime);
        }
        else
        {
            cameraTrack.gameObject.SetActive(true);
            cameraWeaponTrack.gameObject.SetActive(false);

            theCamera.position = Vector3.Lerp(theCamera.position, cameraTrack.position, cameraSpeed * theTime);
            theCamera.rotation = Quaternion.Lerp(theCamera.rotation, cameraTrack.rotation, camRotSpeed * theTime);
        }
    }

    public void ActionsLogic()
    {
        //Inventary
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryOpen = !inventoryOpen;

            Cursor.lockState = inventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = inventoryOpen;
        }

        if (inventoryOpen == false)
        {
            inventoryController.gameObject.SetActive(false);
        }
        else
        {
            inventoryController.gameObject.SetActive(true);
        }

        //Dropear arma seleccionada

        if (Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }
    }

    public void Drop()
    {
        if (hasGun && primaryWeapons !=null)
        {
            GameObject droppedPW = primaryWeapons.GetComponent<WeaponController>().itemPrefab;
            Instantiate(droppedPW, new Vector3(playerTr.position.x, droppedPW.transform.position.y, playerTr.position.z), droppedPW.transform.rotation);

            primaryWeaponIcon.gameObject.SetActive(false);

            Destroy(primaryWeapons.gameObject);

            hasGun = false;

            if (secondaryWeapons == null && throwableWeapon == null)
            {
                playerAnim.SetLayerWeight(1, 0);
            }
            else if (secondaryWeapons != null && throwableWeapon == null)
            {
                playerAnim.SetLayerWeight(1, 1);
                hasRifle = true;
                weaponSlots.ToggleSlot(secondarySlot);
                primaryWeaponIcon.color = Color.white;
                secondaryWeaponIcon.color = Color.red;
            }
        }
        else if (hasRifle && secondaryWeapons != null)
        {
            GameObject droppedSW = secondaryWeapons.GetComponent<WeaponController>().itemPrefab;
            Instantiate(droppedSW, new Vector3(playerTr.position.x, droppedSW.transform.position.y, playerTr.position.z), droppedSW.transform.rotation);

            primaryWeaponIcon.gameObject.SetActive(false);

            Destroy(secondaryWeapons.gameObject);

            hasRifle = false;

            if (primaryWeapons == null && throwableWeapon == null)
            {
                playerAnim.SetLayerWeight(1, 0);
            }
            else if (primaryWeapons != null && throwableWeapon == null)
            {
                playerAnim.SetLayerWeight(1, 1);
                hasGun = true;
                weaponSlots.ToggleSlot(primarySlot);
                primaryWeaponIcon.color = Color.red;
                secondaryWeaponIcon.color = Color.white;
            }
        }
    }

    public void ItemLogic()
    {
        if (nearItem != null && Input.GetKeyDown(KeyCode.E))
        {
            GameObject instantiatedItem = null;

            int countWeapon = 0;

            foreach (GameObject itemPrefab in itemPrefabs)
            {
                if (itemPrefab.CompareTag("PW") && nearItem.CompareTag("PW"))
                {
                    instantiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);
                    primaryWeapons = instantiatedItem.gameObject;
                    countWeapon++;
                    weapons++;

                    Destroy(nearItem.gameObject);
                    instantiatedItem.transform.parent = primarySlot;

                    nearItem = null;

                    WeaponController pwIcon = instantiatedItem.GetComponentInChildren<WeaponController>();

                    primaryWeaponIcon.sprite = pwIcon.weaponIcon;
                    primaryWeaponIcon.gameObject.SetActive(true);

                    break;
                }
                else if (itemPrefab.CompareTag("SW") && nearItem.CompareTag("SW"))
                {
                    instantiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);
                    secondaryWeapons = instantiatedItem.gameObject;
                    countWeapon++;
                    weapons++;

                    Destroy(nearItem.gameObject);
                    instantiatedItem.transform.parent = secondarySlot;

                    nearItem = null;

                    WeaponController swIcon = instantiatedItem.GetComponentInChildren<WeaponController>();

                    secondaryWeaponIcon.sprite = swIcon.weaponIcon;
                    secondaryWeaponIcon.gameObject.SetActive(true);

                    break;
                }
                else if (itemPrefab.CompareTag("TW") && nearItem.CompareTag("TW"))
                {
                    instantiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);
                    throwableWeapon = instantiatedItem.gameObject;
                    countWeapon++;
                    weapons++;

                    Destroy(nearItem.gameObject);
                    instantiatedItem.transform.parent = throwableSlot;

                    nearItem = null;

                    GranadeController twIcon = instantiatedItem.GetComponentInChildren<GranadeController>();

                    throwableWeaponIcon.sprite = twIcon.weaponIcon;
                    throwableWeaponIcon.gameObject.SetActive(true);

                    break;
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Debug.Log("Moristee!!");
            playerRagdoll.Active(true);
            Active = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("Hay un item cerca!");
            nearItem = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("Ya no hay item cerca...");
            nearItem = null;
        }
    }

    public void AnimLogic()
    {
        playerAnim.SetFloat("X", newDirection.x);
        playerAnim.SetFloat("Y", newDirection.y);

        playerAnim.SetBool("holdGun", hasGun);
        playerAnim.SetBool("holdRifle", hasRifle);
        playerAnim.SetBool("holdGranade", hasGranade);

        if (hasGun || hasRifle)
        {
            playerAnim.SetLayerWeight(2, 0);
            playerAnim.SetLayerWeight(1, 1);
        }
        else if (hasGranade)
        {
            playerAnim.SetLayerWeight(1, 0);
            playerAnim.SetLayerWeight(2, 1);
        }
    }
}
