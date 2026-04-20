using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필요

public class Menu : MonoBehaviour
{
    // 메인 메뉴로 이동하는 함수
    public void GoToMainMenu()
    {
        // "MenuScene" 자리에 실제 본인의 메인 메뉴 씬 이름을 적으세요.
        SceneManager.LoadScene("Maie");
    }
}
