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
        //�������� navagent �̵��� �������� �ʵ��� �ϱ�
        //navagent �̵��� rigidbody ������ ���� ����� ���?
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
}
