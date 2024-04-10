using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Movement : MonoBehaviour
{
    Animator animator;

    public float moveSpeed = 0.2f;

    Vector3 stopPosition;

    float walkTime;
    public float walkCounter;

    float waitTime;
    public float waitCounter;

    int WalkDirection;

    public bool isWalking;

    Rigidbody animalRb;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        animalRb = GetComponent<Rigidbody>();

        //So that all the prefabs don't move/stop at the same time
        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);


        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {

            animator.SetBool("isRunning", true);

            walkCounter -= Time.deltaTime;

            switch (WalkDirection)
            {
                case 0:
                    transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    MoveForward();
                    break;
                case 1:
                    transform.localRotation = Quaternion.Euler(0f, 90, 0f);
                    MoveForward();
                    break;
                case 2:
                    transform.localRotation = Quaternion.Euler(0f, -90, 0f);
                    MoveForward();
                    break;
                case 3:
                    transform.localRotation = Quaternion.Euler(0f, 180, 0f);
                    MoveForward();
                    break;
            }

            if (walkCounter <= 0)
            {
                StopWalking();
            }


        }
        else
        {

            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                ChooseDirection();
            }
        }
    }
    void MoveForward()
    {
        //float moveZ = Input.GetAxis("Vertical");

        //Vector3 forward = transform.forward * moveSpeed * moveZ * Time.deltaTime;

        //Vector3 verticalVelocity = animalRb.velocity.y * Vector3.up;

        //Vector3 movement = forward + verticalVelocity;
        //Debug.Log("En Moviment" + movement);

        //animalRb.velocity = movement;



        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }


    void StopWalking()
    {
        stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isWalking = false;
        transform.position = stopPosition;
        animator.SetBool("isRunning", false);
        waitCounter = waitTime;
    }


    public void ChooseDirection()
    {
        WalkDirection = Random.Range(0, 4);

        isWalking = true;
        walkCounter = walkTime;
    }
}
