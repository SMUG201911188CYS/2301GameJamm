using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public Animator animator;
    protected Rigidbody rigid;
    protected NavMeshAgent nav;

    protected float recognizationDistance = 5f;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(target.position);
    }


    private void FixedUpdate()
    {
        //FreezeVelocity();

    }

    void FreezeVelocity()
    {
        //물리력이 navagent 이동을 방해하지 않도록 하기
        //navagent 이동은 rigidbody 로직을 덮어 씌우는 모양?
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
}
