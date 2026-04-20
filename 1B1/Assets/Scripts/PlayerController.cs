using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator pAni;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private float moveInput;
    private bool isGiant = false;
    private bool isInvincible = false;

    private float originalSpeed;
    private float originalJumpForce; // 원래 점프력을 기억할 변수

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalSpeed = moveSpeed;
        originalJumpForce = jumpForce; // 시작 시 점프력 저장
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .2f, groundLayer);

        float scaleVal = isGiant ? 2f : 1f;
        if (moveInput > 0) transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
        else if (moveInput < 0) transform.localScale = new Vector3(-scaleVal, scaleVal, scaleVal);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>().x;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("Jump");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Enemy"))
        {
            if (!isInvincible)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (collision.CompareTag("Spped"))
        {
            StartCoroutine(SpeedUpRoutine());
            Destroy(collision.gameObject);
        }

        // 'jump' 태그 아이템 처리
        if (collision.CompareTag("jump"))
        {
            StartCoroutine(JumpUpRoutine());
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("qjtjt"))
        {
            StartCoroutine(PowerUpRoutine());
            Destroy(collision.gameObject);
        }
    }

    // 점프력 강화 코루틴 (5초 유지)
    IEnumerator JumpUpRoutine()
    {
        jumpForce = originalJumpForce + 3f; // 점프력 3 증가
        yield return new WaitForSeconds(5f); // 5초 대기
        jumpForce = originalJumpForce; // 원상복구
    }

    IEnumerator SpeedUpRoutine()
    {
        moveSpeed = originalSpeed + 3f;
        yield return new WaitForSeconds(5f);
        moveSpeed = originalSpeed;
    }

    IEnumerator PowerUpRoutine()
    {
        isGiant = true;
        isInvincible = true;

        float timer = 5f;
        while (timer > 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.1f);
            timer -= 0.2f;
        }

        ResetPowerUp();
    }

    private void ResetPowerUp()
    {
        isGiant = false;
        isInvincible = false;
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }
}
