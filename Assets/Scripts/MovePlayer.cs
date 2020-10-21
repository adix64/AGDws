using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float speedMultiplier = 3f;
    Animator animator;
    // apelata o singura data, la inceputul aplicatiei
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // apelat de N ori pe secunda, preferabil N >= 60 FPS
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // -1 pentru tasta A, 1 pentru tasta D, 0 altfel
        float v = Input.GetAxis("Vertical"); // -1 pentru tasta S, 1 pentru tasta W, 0 altfel

        Vector3 dir = new Vector3(h, 0, v).normalized; //vector normalizat = vector cu magnitudine(lungime) 1
     //   transform.position += dir * Time.deltaTime * speedMultiplier; // adunam deplasamentul la personaj

        ApplyRootRotation(dir);
        UpdateAnimatorParams(dir);
    }

    void UpdateAnimatorParams(Vector3 dir)
    {
        Vector3 characterSpaceDir = transform.InverseTransformDirection(dir);
        animator.SetFloat("forward", characterSpaceDir.z, 0.2f, Time.deltaTime);
        animator.SetFloat("right", characterSpaceDir.x, 0.2f, Time.deltaTime);
    }

    void ApplyRootRotation(Vector3 dir)
    {
        if ((transform.forward - dir).magnitude > 0.001f &&//vectorii nu sunt confundati
            (transform.forward + dir).magnitude > 0.001f //vectorii nu sunt opusi
            )
        {
            float theta = Mathf.Acos(Vector3.Dot(transform.forward, dir)); // aflam unghiul dintre cei 2 vectori
            theta *= Mathf.Rad2Deg;
            Vector3 crossProd = Vector3.Cross(transform.forward, dir); // si axa perpendiculara pe ei
            // efectuam rotatia cu un sfert din unghiul respectiv, a.i. sa nu roteasca instant, teleportat
            transform.rotation = Quaternion.AngleAxis(theta * 0.25f, crossProd) * transform.rotation; 
        }
        if ((transform.forward + dir).magnitude < 0.001f)
        {//daca vectorii erau opusi, atunci il rotim putin(cu 2 grade) pe "inainte" astfel incat sa nu mai fie opusi
            transform.rotation = Quaternion.AngleAxis(2f, Vector3.up) * transform.rotation;
        }
    }
}
