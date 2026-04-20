using UnityEngine;

public class Quit : MonoBehaviour
{
    // ... 기존 코드들 (GameStartButtonAction 등) ...

    // 게임 종료 함수
    public void QuitGame()
    {
        // 실제 빌드된 게임을 종료함
        Application.Quit();

        // 유니티 에디터 상에서 작동하는지 확인하기 위해 로그 출력
        Debug.Log("게임이 종료되었습니다.");
    }
}
