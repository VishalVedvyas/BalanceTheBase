using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagMover_23 : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float h_xRange = 4.5f;
    public float l_xRange = 0.84f;


    /*
     public float moveSpeed = 5f;
    public float h_xRange = 1.2f;
    public float l_xRange = -0.84f;
     */
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Transform postiion "+ transform.position);
        //if (transform.position.y < l_xRange)
        //{
        //    transform.position = new Vector3(transform.position.x, l_xRange, transform.position.z);
        //}
        //if (transform.position.y > h_xRange)
        //{
        //    transform.position = new Vector3(transform.position.x, h_xRange, transform.position.z);
        //}

        float horizontalInput = Input.GetAxis("Vertical");
        Debug.Log("Horrizontal input " + horizontalInput);
        transform.Translate(new Vector3((horizontalInput * moveSpeed) * Time.deltaTime, 0, 0));


    }
}
