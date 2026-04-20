using UnityEngine;

public class EnemyTraceController : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float raycastDistance = 0.5f; // 레이캐스트 거리를 살짝 늘려 장애물 감지를 원활하게 함

    private Transform player;

    private void Start()
    {
        // 태그가 "Player"인 오브젝트를 찾아 추적 대상으로 설정
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        // 플레이어와의 방향 계산
        Vector2 direction = player.position - transform.position;
        Vector2 directionNormalized = direction.normalized;

        // 장애물 감지 (Obstacle 태그 체크)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionNormalized, raycastDistance, LayerMask.GetMask("Default")); // 필요 시 레이어 설정
        Debug.DrawRay(transform.position, directionNormalized * raycastDistance, Color.red);

        // 장애물이 감지되었고, 그 오브젝트의 태그가 "Obstacle"인 경우
        if (hit.collider != null && hit.collider.CompareTag("Obstacle"))
        {
            // 옆으로 비켜가기 (회전된 방향으로 이동)
            Vector3 alternativeDirection = Quaternion.Euler(0f, 0f, -90f) * directionNormalized;
            transform.Translate(alternativeDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            // 장애물이 없으면 플레이어 방향으로 직진
            transform.Translate(directionNormalized * moveSpeed * Time.deltaTime);
        }
    }
}
