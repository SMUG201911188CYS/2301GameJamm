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

    //수직 가속도(직진)
    [SerializeField]
    private float verticalAcceleration = 5.0f;
    //수평 가속도(좌우이동)
    [SerializeField]
    private float horizontalAcceleration = 3.0f;

    private float decelerationRate = 4f;

    private Animator animator;

    Quaternion originalMeshRotation;

    //메쉬의 트랜스폼
    private Transform meshTransform;

    //메쉬 회전 각도
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

        //메쉬의 트랜스폼 정보를 가져오기
       // meshTransform = transform.GetChild(1);

        //카메라의 트랜스폼 정보를 가져오기
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
        #region 입력받기
        inputV = Input.GetAxisRaw("Vertical");
        inputH = Input.GetAxisRaw("Horizontal");
        #endregion
        #region 감속
        if (Mathf.RoundToInt(inputV) == 0 && Mathf.RoundToInt(inputH) == 0)
        {
            // 아무 입력이 없을 때 점진적으로 속도를 감소시킵니다.
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, decelerationRate * Time.deltaTime);
        }
        #endregion
        #region 가속
        Vector3 movement = new Vector3(inputH * horizontalAcceleration, 0f, inputV * verticalAcceleration) * Time.deltaTime;
        movement = transform.TransformDirection(movement);

        //최대 속도에 미치지 못했을때 
        if (rb.velocity.magnitude < maxSpeed)
        {
            // 이동 방향에 대해 힘을 가해 플레이어를 이동시킵니다.
            rb.AddForce(movement, ForceMode.Impulse);

        }
        #endregion

    }


    private void ControlledRotate()
    {
        // 마우스 입력을 검사하여 플레이어 회전을 처리합니다.
        // 플레이어 회전을 적용합니다.
        horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        verticalRotation -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        // 수평 회전을 적용합니다.
        transform.Rotate(0f, horizontalRotation, 0f);

        // 수직 회전을 적용합니다.
        transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y, 0f);
    }

    private void Motioning()
    {
        animator.SetFloat("Speed", Mathf.Lerp(0.6f, 3f, rb.velocity.magnitude));

        if (Mathf.RoundToInt(inputV) == 0 && Mathf.RoundToInt(inputH) == 0)
        {

        }

        //각도 돌리기
        if (Mathf.RoundToInt(inputH) > 0 && !rotating)
        {

        }
        else if (Mathf.RoundToInt(inputH) < 0 && !rotating)
        {

        }
    }


    //영구적으로 속도 층가
    public void PersistentSetSpeed(float amount)
    {
        verticalAcceleration += amount;
        horizontalAcceleration += amount;
        maxSpeed += amount;
    }

    //일시적으로 속도 증가
    public void TemporalSetSpeed(float amount, float duration)
    {
        StartCoroutine(SpeedUpCoroutine(amount, duration));
    }

    private IEnumerator SpeedUpCoroutine(float speed, float duration)
    {
        // 원래의 값 저장
        float originalSpeed = maxSpeed;
        float originalVAccel = verticalAcceleration;
        float originalHAccel = horizontalAcceleration;

        maxSpeed += speed;
        verticalAcceleration += speed;
        horizontalAcceleration += speed;

        // 3초동안 기다림
        yield return new WaitForSecondsRealtime(duration);

        //기다린 이후
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

            //데미지 입은 일정시간(쿨타임) 동안은 움직일 수 없게
            StartCoroutine(DamageCoolTime());

            //데미지 입음
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


    //외부에서 비가 왔다고 호출하는 함수
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
