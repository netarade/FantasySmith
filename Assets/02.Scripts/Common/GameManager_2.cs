using UnityEngine;

/* [�۾� ����]
 * <v1.0 - 2023_1218_�ֿ���>
 * 1- �ʱ� ���� �ۼ� 
 * ���ӻ��� ���� isNewGame �߰�, ��ư ����, �̾��ϱ� ��ɿ��� �ش� ���º����� �����ϵ��� ���� 
 * 
 * <v1.1 - 2023_1229_�ֿ���>
 * 1- ���ϸ��� GameManager_part2 -> GameManager_2
 * 
 */




public partial class GameManager : MonoBehaviour
{
    public static GameManager instance;     // ���ӸŴ��� �̱��� (���� �� ���� ����)
    public bool isNewGame = true;           // ������ ù �������� ���� (�⺻ true)

    // ���ӸŴ��� �̱��� �ʱ�ȭ (���� �� ���� ����)
    private void Awake()
    {
        if(instance == null)    
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(instance);
    }

    /// <summary>
    /// �� ���� ���� ��ư�� ���� ��
    /// </summary>
    public void btnNewGame()
    {
        isNewGame = true;
    }

    /// <summary>
    /// �̾��ϱ� ��ư�� ���� ��
    /// </summary>
    public void btnContinueGame()
    {
        isNewGame = false;
    }
}
