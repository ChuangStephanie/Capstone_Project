using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMover : MonoBehaviour
{
    private Vector3 moveVector = Vector3.zero;

    private void Awake()
    {
        moveVector = new Vector3(1, 0, 0);
    }
    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;

        if (transform.position.x > 1)
        {
            transform.position = new Vector3(1, 1, -1);
            moveVector = new Vector3(0, 0, 1);
        }
        else if (transform.position.x < -1)
        {
            transform.position = new Vector3(-1, 1, 1);
            moveVector = new Vector3(0, 0, -1);
        }

        if (transform.position.z > 1)
        {
            transform.position = new Vector3(1, 1, 1);
            moveVector = new Vector3(-1, 0, 0);
        }
        else if (transform.position.z < -1)
        {
            transform.position = new Vector3(-1, 1, -1);
            moveVector = new Vector3(1, 0, 0);
        }

    }
}
