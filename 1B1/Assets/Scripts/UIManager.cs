using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void GameStartButtonAction()
    {
        // 본인 첫 씬 이름 쓰기
        SceneManager.LoadScene("Level_1");
    }
    public void QuitGame()
    {
        // 실제 빌드된 게임을 종료함
        Application.Quit();

        // 유니티 에디터 상에서 작동하는지 확인하기 위해 로그 출력
        Debug.Log("게임이 종료되었습니다.");
    }
}
