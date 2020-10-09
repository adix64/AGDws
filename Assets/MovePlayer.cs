using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float speedMultiplier = 3f;
 
    // apelata o singura data, la inceputul aplicatiei
    void Start()
    {
        //cod de initializare
    }

    // apelat de N ori pe secunda, preferabil N >= 60 FPS
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // -1 pentru tasta A, 1 pentru tasta D, 0 altfel
        float v = Input.GetAxis("Vertical"); // -1 pentru tasta S, 1 pentru tasta W, 0 altfel

        Vector3 dir = new Vector3(h, 0, v).normalized; //vector normalizat = vector cu magnitudine(lungime) 1
        transform.position += dir * Time.deltaTime * speedMultiplier; // adunam deplasamentul la personaj
    }
}
