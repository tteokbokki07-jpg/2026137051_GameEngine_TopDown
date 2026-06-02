using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;
    public float frameTime = 0.15f;
    public float PickupItem = 0f;

    public bool canMove = true;
    private bool isRespawning = false;
    public Transform Respawnpoint;

    public float respawnRotationSpeed = 360f; // Respawn 중 Z축 회전 속도(도/초)

    public bool isDash;
    public float dashSpeed = 1.75f;
    public float normalSpeed = 0.75f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 input;
    private Vector2 velocity;
    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float timer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentSprites = spriteDown;
        sr.sprite = currentSprites[0];
    }

    public void OnMove(InputValue value)
    {
        // canMove가 true일 때만 입력을 처리하고, 아닐 경우 입력/속도를 0으로 유지
        if (!canMove)
        {
            input = Vector2.zero;
            velocity = Vector2.zero;
            return;
        }

        input = value.Get<Vector2>();
        velocity = input.normalized * moveSpeed;
        if (input.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0)
                    ChangeSprites(spriteRight);
                else
                    ChangeSprites(spriteLeft);
            }
            else
            {
                if (input.y > 0)
                    ChangeSprites(spriteUp);
                else
                    ChangeSprites(spriteDown);
            }
        }
    }

    private void Update()
    {
        if (input.sqrMagnitude <= 0.01f)
        {
            frameIndex = 0;
            sr.sprite = currentSprites[frameIndex];
            return;
        }
        timer += Time.deltaTime;
        if (timer >= frameTime)
        {
            timer = 0f;
            frameIndex++;
            if (frameIndex >= currentSprites.Length)
                frameIndex = 0;
            sr.sprite = currentSprites[frameIndex];
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDash = true;
            moveSpeed = dashSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isDash = false;
            moveSpeed = normalSpeed;
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
    private void ChangeSprites(Sprite[] newSprites)
    {
        if (currentSprites == newSprites)
            return;
        currentSprites = newSprites;
        frameIndex = 0;
        timer = 0f;
        sr.sprite = currentSprites[frameIndex];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Finish");
            SceneManager.LoadScene("Crafting");
        }

        if (collision.gameObject.CompareTag("Item"))
        {
            Debug.Log("item");
            PickupItem += 1f;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Respawn"))
        {
            Debug.Log("Respawn trigger entered");
            if (!isRespawning)
                StartCoroutine(RespawnCoroutine());
        }
    }

    private System.Collections.IEnumerator RespawnCoroutine()
    {
        if (Respawnpoint == null)
        {
            Debug.LogWarning("Respawnpoint이 할당되지 않았습니다.");
            yield break;
        }

        isRespawning = true;
        canMove = false;

        // 현재 회전을 저장(나중에 복원)
        Quaternion originalRotation = transform.rotation;

        // 0.5초 동안 매 프레임 Z축으로 회전
        float elapsed = 0f;
        float waitTime = 0.5f;
        while (elapsed < waitTime)
        {
            float dt = Time.deltaTime;
            elapsed += dt;
            transform.Rotate(0f, 0f, respawnRotationSpeed * dt);
            yield return null;
        }

        // 위치 강제 이동 및 물리 상태 초기화
        Vector2 respawnPos = Respawnpoint.position;
        rb.position = respawnPos;
        rb.linearVelocity = Vector2.zero;
        transform.position = respawnPos;

        // 입력/이동 벡터 초기화
        input = Vector2.zero;
        velocity = Vector2.zero;

        // 회전 원상복구
        transform.rotation = originalRotation;

        isRespawning = false;
        canMove = true;
        isDash = false;
        moveSpeed = normalSpeed;
    }
}