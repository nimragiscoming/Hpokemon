using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    public Animator animator;

    public Camera frontCam;

    // Start is called before the first frame update
    void Start()
    {
        frontCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
