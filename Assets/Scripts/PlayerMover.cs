using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _rb.velocity = 5f * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
