using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * [�۾� ����]
 * <v1.0 - 2024_0129_�ֿ���>
 * 1- ���� ��ũ��Ʈ �ۼ�
 * ItemSelect��ũ��Ʈ�� ����Ͽ� ���� �������� ������ �Ͼ �� �θ� �޼��尡 �״�� ȣ��ǵ��� �Ͽ��� 
 * 
 * <v1.1 - 2024_0214_�ֿ���>
 * 1- ItemSelect��ũ��Ʈ ����� ����ϰ� MonoBehaviour�� ����ϴ� ���·� �ѹ�
 * ������ �������� �Ͼ �� ���̰� �������� �Ͼ�� �ϴ� ���� �ƴ϶� 
 * ���̴� ������ ����ǰ� ���콺 �����Ǹ� �Ѱܼ� ������ 2D ������Ʈ�� ���������Ͼ���� �ؾ� �ϹǷ� 
 * 
 * 2- �������̽��� OnSelect�� ����Ͽ�, ���� ������ ������ �� ������ ������ �����ǰ� �ٷ� ���� �������� �������� �̷���� �� �ְ� �Ͽ���
 * 
 * <v1.2 - 2024_0216_�ֿ���>
 * 1- �������̽��� OnUpdateSelected�� ����ϴ� ������ �����Ͽ���
 * ������ OnSelect���� EventSystem.current.SetSelectedGameObject(null);�޼��带 ȣ���Ͽ� �������� ������ �����ϴ� ���
 * ������ ���¿��� �������� �õ��ߴٰ� ������ �߻��ϱ� ����
 * 
 * => 
 * ����Ʈ�� ���۵Ǹ� �̺�Ʈ �ý����� ���� �̺�Ʈ�� �߻��� ���Ͽ� ��ȣ Ȥ�� ���� �ɱ� ������ ����Ʈ ���� ���� OnUpdateSelected���� ȣ�� ����� ��. 
 * ����� Ư�� �ڵ� �κ� Ȥ�� �ߺ� �ڵ�� �����ÿ� ������ �ִ� ��� ������ �߻��� �� �ֱ� ������ �Ʒ��� ���� �����ÿ��θ� Ȯ���� ȣ���ϴ� ��쵵 ����
 * if (!EventSystem.current.alreadySelecting) eventSystem.SetSelectedGameObject (null);
 * 
 * 
 * 
 * 
 * 
 * 
 */




public class DummyItemSelect : MonoBehaviour, IUpdateSelectedHandler
{ 
    DummyInfo dummyInfo;                // ������ ���� ������ ���� ���� ���� ���� �� 
    Button dummyBtn;                    // ������ ������ ���� ���� ��ư

    private void Awake()
    {
        dummyBtn = GetComponent<Button>();
        dummyInfo = GetComponent<DummyInfo>();
    }
    
            
    public void OnUpdateSelected( BaseEventData eventData )
    {
        dummyBtn.OnDeselect( eventData );                   // ��ư�� Deselect���·� ����ϴ�.
        EventSystem.current.SetSelectedGameObject( null );  // �̺�Ʈ �ý����� ����Ʈ ���¸� null�� ����ϴ�.

        QuickSlot quickSlot = dummyInfo.EquipItemInfo.InventoryInfo as QuickSlot;
        
        // �������� �����Կ� ����������, ���� ���� ���¶��
        if( quickSlot!=null&&dummyInfo.EquipItemInfo.IsEquip )       
        {          
            // �����Կ� �������� �˸��� ������ �����մϴ�.
            quickSlot.OnQuickSlotSelect( dummyInfo.EquipItemInfo ); 

            // ���� ���¿��� ��� �ý����� �������� ���� 2D ������Ʈ�� ����Ʈ ���·� ����ϴ�.
            EventSystem.current.SetSelectedGameObject( dummyInfo.EquipItemInfo.Item2dTr.gameObject );
        }        
    }




}
