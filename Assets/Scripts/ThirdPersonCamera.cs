﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    //https://upload.wikimedia.org/wikipedia/commons/thumb/0/04/Flight_dynamics_with_text_ortho.svg/1200px-Flight_dynamics_with_text_ortho.svg.png
    float pitch = 0f, yaw = 0f;
    public Transform player;
    public float distToTarget = 4f;
    public float minPitch = -45f, maxPitch = 45f;
    public Vector3 cameraOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //acumulam deplasamentul mouseului la unghiurile pe care le face camera cu axele lumii
        pitch -= Input.GetAxis("Mouse Y");
        yaw   += Input.GetAxis("Mouse X");

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);//limitare unghi de view

        //scriem rotatia cu unghiurile corespunzatoare
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        //de la pozitia playerului, ne dam in spate distToTarget unitati
        transform.position = player.position - transform.forward * distToTarget +
                        transform.TransformDirection(cameraOffset); //pozitionare camera grid de interes
    }
}
