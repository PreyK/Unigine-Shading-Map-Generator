using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public Vector3 rotationVector;
    public float speed;
    private void Update()
    {
        transform.Rotate(rotationVector * Time.deltaTime*speed);
    }
}
