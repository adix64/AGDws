using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsCtrl : MonoBehaviour
{
    GameObject L, R, F, B;
    // Start is called before the first frame update
    void Start()
    {
        F = transform.GetChild(0).gameObject;
        B = transform.GetChild(1).gameObject;
        L = transform.GetChild(2).gameObject;
        R = transform.GetChild(3).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // -1 pentru tasta A, 1 pentru tasta D, 0 altfel
        float v = Input.GetAxis("Vertical"); // -1 pentru tasta S, 1 pentru tasta W, 0 altfel
        F.SetActive(v > 0f);
        B.SetActive(v < 0f);
        L.SetActive(h < 0f);
        R.SetActive(h > 0f);
    }
}
