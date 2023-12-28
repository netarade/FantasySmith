using UnityEngine;

/* [작업 사항]
 * <v1.0 - 2023_1218_최원준>
 * 1- 초기 로직 작성 
 * 게임상태 변수 isNewGame 추가, 버튼 시작, 이어하기 기능에서 해당 상태변수를 수정하도록 구현 
 * 
 * <v1.1 - 2023_1229_최원준>
 * 1- 파일명변경 GameManager_part2 -> GameManager_2
 * 
 */




public partial class GameManager : MonoBehaviour
{
    public static GameManager instance;     // 게임매니저 싱글톤 (통합 후 삭제 예정)
    public bool isNewGame = true;           // 게임이 첫 시작인지 여부 (기본 true)

    // 게임매니저 싱글톤 초기화 (통합 후 삭제 예정)
    private void Awake()
    {
        if(instance == null)    
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(instance);
    }

    /// <summary>
    /// 새 게임 시작 버튼을 누를 때
    /// </summary>
    public void btnNewGame()
    {
        isNewGame = true;
    }

    /// <summary>
    /// 이어하기 버튼을 누를 때
    /// </summary>
    public void btnContinueGame()
    {
        isNewGame = false;
    }
}
