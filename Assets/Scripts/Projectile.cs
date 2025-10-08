using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class BulletScript : MonoBehaviour
{

    void Update()
    {

        GameObject sp;

        Rigidbody body;

        Camera cam;



        cam = Camera.main;



        if (Input.GetMouseButtonDown(0))
        {

            sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            sp.transform.position = cam.transform.position;



            body = sp.AddComponent(typeof(Rigidbody)) as Rigidbody;



            body.AddRelativeForce(cam.transform.forward.normalized * 1000);

        }

    }

}