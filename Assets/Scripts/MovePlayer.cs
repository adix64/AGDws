using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float speedMultiplier = 3f;
    public float groundedThresh = 0.15f;
    public float jumpPower = 6f;
    Animator animator;
    public Transform cameraTransform;
    Rigidbody rigidbody;
    Vector3 moveDir;
    CapsuleCollider capsule;
    AnimatorStateInfo currentStateInfo;
    // apelata o singura data, la inceputul aplicatiei
    void Start()
    {
        capsule= GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // apelat de N ori pe secunda, preferabil N >= 60 FPS
    void Update()
    {
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        GetUserInput();
        ApplyRootMotion();
        ApplyRootRotation();
        UpdateAnimatorParams();
        HandleJump();
        HandleAttack();
    }
    private void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1"))
            animator.SetTrigger("attack");
    }
    private void HandleJump()
    {
        bool midair = true;//presupunem in aer
        //si facem 9 raycasturi din centrul bazei capsulei si de pe marginile sale:
        for (float offsetX = -1f; offsetX <= 1f; offsetX += 1f)
        {
            for (float offsetZ = -1f; offsetZ <= 1f; offsetZ += 1f)
            {
                Vector3 offset = new Vector3(offsetX, 0, offsetZ).normalized * capsule.radius;
                //raza aruncata de la baza picioarelor(putin deasupra, adica din capsula) in jos
                Ray ray = new Ray(transform.position + offset + Vector3.up * groundedThresh, Vector3.down);
                if (Physics.Raycast(ray, groundedThresh * 2f))// la o distanta maxima tolerata ca fiind pe sol
                {// daca a lovit vreo raza, e pe sol
                    midair = false;
                    break;
                }
            }
        }

        if(!midair)
        {//pe sol
            animator.SetBool("midair", false);
            if (Input.GetButtonDown("Jump"))
            {// imprima saritura playerului
                Vector3 jumpForce = (Vector3.up + moveDir) * jumpPower;
                rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
            }
        }
        else
        {//in aer
            animator.SetBool("midair", true);
        }
    }
    private void ApplyRootMotion()
    {
        //transform.position += dir * Time.deltaTime * speedMultiplier; // adunam deplasamentul la personaj
        if (animator.GetBool("midair"))
        {// daca e in aer, atunci lasa motorul de fizica sa controlze in totalitate playerul
            animator.applyRootMotion = false;
            return;
        }
        else
        {
            animator.applyRootMotion = true;
        }
        float velY = rigidbody.velocity.y;
        rigidbody.velocity = animator.deltaPosition / Time.deltaTime; // suprascriem viteza corpului cu root motion
        rigidbody.velocity = new Vector3(rigidbody.velocity.x,
                                         velY,//pastram viteza calculata de motorul fizic pe axa verticala
                                         rigidbody.velocity.z);
    }
    private void GetUserInput()
    {
        float h = Input.GetAxis("Horizontal"); // -1 pentru tasta A, 1 pentru tasta D, 0 altfel
        float v = Input.GetAxis("Vertical"); // -1 pentru tasta S, 1 pentru tasta W, 0 altfel

        moveDir = (h * cameraTransform.right + v * cameraTransform.forward); //directia deplasrii este relativa la orientarea camerei
        moveDir.y = 0f; // se deplaseaza doar in plan orizontal(xOz)
        moveDir = moveDir.normalized;//vector normalizat = vector cu magnitudine(lungime) 1

    }

    void UpdateAnimatorParams()
    {
        Vector3 characterSpaceDir = transform.InverseTransformDirection(moveDir);
        animator.SetFloat("forward", characterSpaceDir.z, 0.2f, Time.deltaTime);
        animator.SetFloat("right", characterSpaceDir.x, 0.2f, Time.deltaTime);
    }

    void ApplyRootRotation()
    {
        if (currentStateInfo.IsTag("attack") || animator.GetBool("midair"))
            return; //nu roti daca e in atac sau in aer
        Vector3 dir = moveDir;
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
