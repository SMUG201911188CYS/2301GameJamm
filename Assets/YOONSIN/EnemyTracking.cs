using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracking : MonoBehaviour
{

    public Transform Target;
    public EnemyType Type;

    Rigidbody rigid;
    float speed = 1.0f;
    float MaxDistance = 5.0f;
    bool isCollision = false;
    

    // Start is called before the first frame update
    void Start()
    {
       rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCollision)
        {
            float distance = Vector3.Distance(Target.position, transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Target.position), speed * Time.deltaTime);

            if (distance < MaxDistance)
            {
                rigid.velocity = Vector3.Normalize(Target.position - transform.position) * speed;
            }
            else
            {
                rigid.velocity = Vector3.zero;
            }
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("?");
            isCollision = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("?");
            isCollision = false;
        }
    }
}
