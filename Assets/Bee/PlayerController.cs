using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    private float rotationSpeed = 1500f;

    [SerializeField]
    private float maxSpeed = 5.0f;

    //���� ���ӵ�(����)
    [SerializeField]
    private float verticalAcceleration = 5.0f;
    //���� ���ӵ�(�¿��̵�)
    [SerializeField]
    private float horizontalAcceleration = 3.0f;

    private float decelerationRate = 4f;

    private Animator animator;

    Quaternion originalMeshRotation;

    //�޽��� Ʈ������
    private Transform meshTransform;

    //�޽� ȸ�� ����
    private float meshRotationSpeed;

    private bool rotating = false;

    [SerializeField]
    private float verticalRotation = 0f;
    [SerializeField]
    private float horizontalRotation;


    private Rigidbody rb;

    public float inputV;
    public float inputH;

    bool isDamaged = false;

    [SerializeField]
    public bool isRained = false;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        //�޽��� Ʈ������ ������ ��������
       // meshTransform = transform.GetChild(1);

        //ī�޶��� Ʈ������ ������ ��������
        originalMeshRotation = meshTransform.rotation;

       

    }

    private void FixedUpdate()
    {
        if (!isRained && !isDamaged)
        {
            ControlledMove();
        }
        else if(isRained)
        {
            Falling();
        }
    }

    private void Update()
    {
        ControlledRotate();
        Motioning();
        //  ControllCamera();

    }

    private void ControlledMove()
    {
        #region �Է¹ޱ�
        inputV = Input.GetAxisRaw("Vertical");
        inputH = Input.GetAxisRaw("Horizontal");
        #endregion
        #region ����
        if (Mathf.RoundToInt(inputV) == 0 && Mathf.RoundToInt(inputH) == 0)
        {
            // �ƹ� �Է��� ���� �� ���������� �ӵ��� ���ҽ�ŵ�ϴ�.
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, decelerationRate * Time.deltaTime);
        }
        #endregion
        #region ����
        Vector3 movement = new Vector3(inputH * horizontalAcceleration, 0f, inputV * verticalAcceleration) * Time.deltaTime;
        movement = transform.TransformDirection(movement);

        //�ִ� �ӵ��� ��ġ�� �������� 
        if (rb.velocity.magnitude < maxSpeed)
        {
            // �̵� ���⿡ ���� ���� ���� �÷��̾ �̵���ŵ�ϴ�.
            rb.AddForce(movement, ForceMode.Impulse);

        }
        #endregion

    }


    private void ControlledRotate()
    {
        // ���콺 �Է��� �˻��Ͽ� �÷��̾� ȸ���� ó���մϴ�.
        // �÷��̾� ȸ���� �����մϴ�.
        horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        verticalRotation -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        // ���� ȸ���� �����մϴ�.
        transform.Rotate(0f, horizontalRotation, 0f);

        // ���� ȸ���� �����մϴ�.
        transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y, 0f);
    }

    private void Motioning()
    {
        animator.SetFloat("Speed", Mathf.Lerp(0.6f, 3f, rb.velocity.magnitude));

        if (Mathf.RoundToInt(inputV) == 0 && Mathf.RoundToInt(inputH) == 0)
        {

        }

        //���� ������
        if (Mathf.RoundToInt(inputH) > 0 && !rotating)
        {

        }
        else if (Mathf.RoundToInt(inputH) < 0 && !rotating)
        {

        }
    }


    //���������� �ӵ� ����
    public void PersistentSetSpeed(float amount)
    {
        verticalAcceleration += amount;
        horizontalAcceleration += amount;
        maxSpeed += amount;
    }

    //�Ͻ������� �ӵ� ����
    public void TemporalSetSpeed(float amount, float duration)
    {
        StartCoroutine(SpeedUpCoroutine(amount, duration));
    }

    private IEnumerator SpeedUpCoroutine(float speed, float duration)
    {
        // ������ �� ����
        float originalSpeed = maxSpeed;
        float originalVAccel = verticalAcceleration;
        float originalHAccel = horizontalAcceleration;

        maxSpeed += speed;
        verticalAcceleration += speed;
        horizontalAcceleration += speed;

        // 3�ʵ��� ��ٸ�
        yield return new WaitForSecondsRealtime(duration);

        //��ٸ� ����
        maxSpeed = originalSpeed;
        verticalAcceleration = originalVAccel;
        horizontalAcceleration = originalHAccel;

    }


    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.CompareTag("Enemy"))
        {

            if (isDamaged)
            {
                return;
            }

            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.Normalize(transform.position - collision.transform.position) * 2.0f, ForceMode.VelocityChange);

            //������ ���� �����ð�(��Ÿ��) ������ ������ �� ����
            StartCoroutine(DamageCoolTime());

            //������ ����
            float damage = 0.0f;
            switch (collision.gameObject.GetComponent<EnemyTracking>().Type)
            {
                case EnemyType.hornet:
                    damage = 0.25f;
                    break;
                case EnemyType.frog:
                    damage = 0.5f;
                    break;
                case EnemyType.bird:
                    damage = 1.5f;
                    break;
                default:
                    break;
            }
            GameManager.Instance.LostHealth(damage);

        }

    }

    IEnumerator DamageCoolTime()
    {
        isDamaged = true;
        yield return new WaitForSeconds(1.0f);
        isDamaged = false;
    }


    //�ܺο��� �� �Դٰ� ȣ���ϴ� �Լ�
    public void Rained()
    {
        StartCoroutine(RainCoroutine());
    }

    public void Falling()
    {
        animator.SetFloat("Speed", 0);
    }

    
    private IEnumerator RainCoroutine()
    {
        rb.useGravity = true;
        isRained = true;
        yield return new WaitForSeconds(1.0f);
        rb.useGravity = false;
        isRained = false;
    }

    
}
