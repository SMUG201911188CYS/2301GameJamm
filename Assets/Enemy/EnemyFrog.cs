using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyFrog : Enemy
{ 
    // Start is called before the first frame update
    void Start() 
    {
        StartCoroutine(JumpMove());
    }

    // Update is called once per frame
    void Update()
    {
        if ((target.position - this.transform.position).magnitude > 0.5)
        {
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Tongue");

        }
    }

    IEnumerator JumpMove()
    {
        while(true)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position), 3f * Time.deltaTime);
            
            if (distance < recognizationDistance)
            {
                rigid.AddForce(Vector3.Normalize(target.position - transform.position) * 3f, ForceMode.Impulse);
                nav.SetDestination(target.position);
                animator.SetTrigger("Jump");
                print("内风凭 俺备府 痢橇123");

            }
            else
            {
                rigid.velocity = Vector3.zero;
                print("内风凭 俺备府 痢橇44444");
            }
            print("内风凭 俺备府 痢橇");

            yield return new WaitForSeconds(2.0f);
        }
        

        
    }
}
