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
    private float originalJumpForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalSpeed = moveSpeed;
        originalJumpForce = jumpForce;
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
        // 데드 타일(함정) 충돌 로직 수정
        if (collision.CompareTag("Respawn"))
        {
            // 무적 상태가 아닐 때만 씬 재시작
            if (!isInvincible)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Enemy"))
        {
            // 적 충돌: 무적 상태가 아닐 때만 죽음
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

    IEnumerator JumpUpRoutine()
    {
        jumpForce = originalJumpForce + 3f;
        yield return new WaitForSeconds(5f);
        jumpForce = originalJumpForce;
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
            // 깜빡이는 시각 효과
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
