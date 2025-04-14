using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private int hp = 3;
    
    private bool isSpeedUp = false;
    private float boostSpeed = 12f;

    private Animation anim;
    private bool isSliding = false;
    
    private bool isInvincible = false;
    private float invincibilityTime = 3f;
    
    public GameObject shieldUI;
    public GameObject SpeedUI;
    
    private Renderer bodyRenderer;
    private Color originalColor;

    private Vector3 inputDir;
    
    public GameObject[] hpImages;
    private Rigidbody playerRigidbody; // 이동에 사용할 리지드바디 컴포넌트
    public float speed = 8f; // 이동 속력
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hitSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 컴포넌트<타입> 정보를 얻어옴
        playerRigidbody = GetComponent<Rigidbody>();
        if (bodyRenderer == null)
        {
            // 이름으로 정확히 찾아도 됨
            bodyRenderer = transform.Find("Character/DeliveryMan_mesh")
                ?.GetComponent<SkinnedMeshRenderer>();
        }

        originalColor = bodyRenderer.material.color;
        if (shieldUI != null)
            shieldUI.SetActive(false);
        if (SpeedUI != null)
            SpeedUI.SetActive(false);
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    // 업데이트 함수는 사용자 입력을 지속적으로 감시한다.
    /*
    void Update()
    {
        // Input Manager에 있는 Horizontal에서 방향을 얻어옴
        // xInput과 zInput에는 (+1, -1)의 방향성만을 변수에 가지게 된다.
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        
        // 방향 * 스피드
        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;
        
        // 속력 벡터를 이용해서 캐릭터를 일정하게 움직이게 한다.
        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        // 이동 방향이 있을 때만 회전
        if (newVelocity != Vector3.zero)
        {
            // 이동 방향을 바라보게 함 (자연스럽게 회전)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newVelocity), 0.2f);
        }
        playerRigidbody.linearVelocity = newVelocity;
        /*
         if (Input.GetKey(KeyCode.UpArrow) == true)
        {
            // 위쪽 방향키 입력이 감지된 경우 z 방향 힘주기
            // AddForce 사용시 가속도가 붙음 ( 힘이 지속적으로 가해짐 )
            playerRigidbody.AddForce(0f, 0f, speed);
        }       
        if (Input.GetKey(KeyCode.DownArrow) == true)
        {
            // 아래쪽 방향키 입력이 감지된 경우 -z 방향 힘주기
            playerRigidbody.AddForce(0f, 0f, -speed);
        }        
        if (Input.GetKey(KeyCode.RightArrow) == true)
        {
            // 오른쪽 방향키 입력이 감지된 경우 x 방향 힘주기
            playerRigidbody.AddForce(speed, 0f, 0f);
        }       
        if (Input.GetKey(KeyCode.LeftArrow) == true)
        {
            // 왼쪽 방향키 입력이 감지된 경우 -x 방향 힘주기
            playerRigidbody.AddForce(-speed, 0f, 0f);
        }
        #1#
        if (Input.GetKey(KeyCode.R))
        {
            gameObject.SetActive(true);
        }
    }
    */
    void Update()
    {
        // 입력만 처리
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        inputDir = new Vector3(x, 0f, z).normalized;
        if (Input.GetKeyDown(KeyCode.Space) && !isSliding)
        {
            StartCoroutine(SlideRoutine());
        }
        if (Input.GetKey(KeyCode.R))
        {
            gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("StageSelect");
        }
    }
    private IEnumerator SlideRoutine()
    {
        isSliding = true;
        isInvincible = true;

        // 슬라이드 애니메이션 실행
        anim.Play("Runtoslide");

        // 일정 시간 대기
        yield return new WaitForSeconds(0.75f);

        // 다시 Run 애니메이션으로 전환
        anim.Play("Run");

        isSliding = false;
        isInvincible = false;
    }

    void FixedUpdate()
    {
        if (inputDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputDir), 0.2f);
        }

        Vector3 moveDelta = inputDir * speed * Time.fixedDeltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDelta);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
        if (isInvincible) return;
        hp--;

        if (hp > 0 && hp <= hpImages.Length)
        {
            hpImages[hp].SetActive(false);
        }

        if (hp <= 0)
        {
            foreach (var img in hpImages)
            {
                img.SetActive(false);
            }

            Die();
        }
    }


    public void Die() // 플레이어가 죽으면 호출되는 함수
    {
        StageClearTimer timer = FindObjectOfType<StageClearTimer>();
        if (timer != null)
            timer.isPlayerDead = true;

        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);

        // 현재 스크립트에 객체 = gameObject 를 나타낸다 java 에서 this와 같은 개념
        // gmaeobject : 해당 스크립트가 적용되어있는 오브젝트
        gameObject.SetActive(false);
        
        // 씬에 존재하는 GameManager 타입의 오브젝트를 찾아서 가져오기
        GameManager gameManager = FindObjectOfType<GameManager>();
        // 가져온 GameManager 오브젝트의 EndGame() 메서드 실행
        gameManager.EndGame();
    }

    public void Heal(int amount)
    {
        if (hp >= 3) return;

        hp += amount;
        if (hp > 3) hp = 3;

        int count = 0;
        for (int i = 0; i < hpImages.Length; i++)
        {
            if (!hpImages[i].activeSelf)
            {
                hpImages[i].SetActive(true);
                count++;
                if (count == amount) break;
            }
        }
    }
    
    public void ActivateInvincibility()
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }
    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        // 색상 변경 (예: 노란색)
        bodyRenderer.material.color = Color.yellow;

        // Shield UI 깜빡임 시작
        if (shieldUI != null)
            StartCoroutine(BlinkShieldUI());

        yield return new WaitForSeconds(invincibilityTime);

        // 원래 상태 복원
        isInvincible = false;
        bodyRenderer.material.color = originalColor;
        if (shieldUI != null)
            shieldUI.SetActive(false);
    }

    private IEnumerator BlinkShieldUI()
    {
        float blinkTime = 0.2f;
        float elapsed = 0f;
        while (elapsed < invincibilityTime)
        {
            shieldUI.SetActive(!shieldUI.activeSelf);
            yield return new WaitForSeconds(blinkTime);
            elapsed += blinkTime;
        }
        shieldUI.SetActive(false); // 무조건 꺼주기
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
    
    public void ActivateSpeedUp()
    {
        if (!isSpeedUp)
        {
            StartCoroutine(SpeedUpRoutine());
        }
    }
    
    private IEnumerator SpeedUpRoutine()
    {
        isSpeedUp = true;
        speed = boostSpeed;

        if (SpeedUI != null)
            StartCoroutine(BlinkSpeedUI());

        yield return new WaitForSeconds(3f);

        speed = 8f;
        isSpeedUp = false;

        if (SpeedUI != null)
            SpeedUI.SetActive(false);
    }
    private IEnumerator BlinkSpeedUI()
    {
        float blinkTime = 0.2f;
        float elapsed = 0f;
        while (elapsed < 3f)
        {
            SpeedUI.SetActive(!SpeedUI.activeSelf);
            yield return new WaitForSeconds(blinkTime);
            elapsed += blinkTime;
        }
        SpeedUI.SetActive(false);
    }
    
}
