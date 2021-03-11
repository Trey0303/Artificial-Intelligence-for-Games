﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Agent agent;
    public float speed = 5.0f;

    public int coinsPickedUp;

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        input = Vector3.ClampMagnitude(input, 1);
        agent.velocity = input * speed;
        agent.UpdateMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickup")
        {
            other.gameObject.SetActive(false);
            coinsPickedUp++;

        }
    }
}