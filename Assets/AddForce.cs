using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{

    public Rigidbody r;
    public Vector3 force;
    // Use this for initialization
    void Start()
    {
        r = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DoAddForce()
    {
        Debug.Log("Adding force");
        r.AddForce(force, ForceMode.Impulse);
    }
}
