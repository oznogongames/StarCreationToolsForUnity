using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CoronaRotator : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.parent.LookAt(Camera.main.transform);
    }
}