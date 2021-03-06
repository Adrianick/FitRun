﻿using UnityEngine;

public class CameraMotor : MonoBehaviour
{

    private Transform lookAt;
    private Vector3 startOffset;
    private Vector3 moveVector;
    private Vector3 animationOffset = new Vector3(0, 5, 5);

    private float transition = 0.0f;
    private float animationDuration = 2.75f;
    private float verticalOffset = 0;
    void Start()
    {
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        startOffset = transform.position - lookAt.position;
        verticalOffset = lookAt.position.y + startOffset.y;
    }

    void Update()
    {
        moveVector = lookAt.position + startOffset;
        moveVector.x = 0;
        //moveVector.y = Mathf.Clamp(moveVector.y, 3, 11);
        moveVector.y = verticalOffset;

        if (transition > 1.0f)
        {
            transform.position = moveVector;
        }
        else
        {
            transform.position = Vector3.Lerp(moveVector + animationOffset, moveVector, transition);
            transition += Time.deltaTime * 1.3f / animationDuration;
            //transform.LookAt(lookAt.position + Vector3.up);
        }

    }
}
