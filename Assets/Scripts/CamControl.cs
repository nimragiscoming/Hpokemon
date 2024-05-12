using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CamControl : MonoBehaviour
{
    public Vector3 lookPos;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(lookPos);
        transform.Translate(Vector3.right * Time.deltaTime);
    }
}
