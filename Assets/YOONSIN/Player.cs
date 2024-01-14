using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    float xAxis;
    float yAxis;
    float speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        speed = 10f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

   void InputKey()
    {
        yAxis = Input.GetAxisRaw("Horizontal");
        xAxis = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        Vector3 MoveVec = new Vector3(xAxis, 0, yAxis);

        rigid.velocity =  (MoveVec * speed);

    }



    // Update is called once per frame
    void Update()
    {
        InputKey();
        Move();
    }

    private void FixedUpdate()
    {
        FreezeRotation();

    }

    void FreezeRotation()
    {
        //rigid.angularVelocity = Vector3.zero;
    }
}
