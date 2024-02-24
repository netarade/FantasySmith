using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems; // ���콺 Ŭ��, �巡�� ���, �̺�Ʈ ���� ����
using UnityEngine.UI;


/* [�۾� ����]
 * <v1.0 - 2023_1108_�ֿ���>
 * 1- �ʱ� ���� �ۼ�
 * 
 * <v1.1 - 2024_0106_�ֿ���>
 * 1- isStop ���� �߰��Ͽ� Ư����Ȳ���� �������� �ʵ��� �Ͽ���.
 * 2- UpdateDragStopConditon�޼��带 ���� �ܺο��� �޼��� ȣ���� ���� isStop������ ���氡���ϵ��� ����
 * 
 * <v1.2 - 2024_0107_�ֿ���>
 * 1- isStop������ UpdateDragStopCondition�޼��� ����,
 * �������� �巡�װ� �Ͼ�� InventoryInteractive��ũ��Ʈ�� IsItemSelecting ���¸� Ȱ��ȭ��Ű���� �Ͽ���,
 * �� ���º����� Ȱ��ȭ ���θ� �о �� ��ũ��Ʈ�� �۵����� �ʰԲ� ����
 * 
 * 2- ������ tr�� inventoryTr�� ����
 * 
 * <v1.3 - 2024_0223_�ֿ���>
 * 1- InventoryInteractive�� IsItemSlecting ���� ������ ���� �κ��丮 �巡�׸� ���� ������ ����
 * ������ ������ �������� ���̻� ���º����� �������� �ʰ� ������ �� ĵ�����׷��� �Ӽ��� ���������μ� ��ȣ�ۿ��� ���� ����
 * 
 */


/// <summary>
/// InventoryDrag��ũ��Ʈ�� Inventory ������Ʈ�� ������Ʈ�� ������ �־�� �մϴ�.<br/>
/// �κ��丮 ������Ʈ�� �巡�׸� �����ϰ� ���ݴϴ�.
/// </summary>
public class InventoryDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    RectTransform inventoryTr;  // ������ �ݺ� ȣ�� �ؾ� �ϹǷ� �ڽ� transform ĳ��ó��
    CanvasGroup inventoryCG;    // �ڽ��� ĵ���� �׷� ������Ʈ ����
    Vector3 moveVecToCenter;    // �κ��丮�� ���콺 ��ġ�� �������� ������ ���콺 ��ġ�� ���� �� �ٽ� �������� �������� ����
                            

    void Start()
    {
        inventoryTr= GetComponent<RectTransform>();
        inventoryCG = GetComponent<CanvasGroup>();
    }
    
    public void OnBeginDrag( PointerEventData eventData )
    {                 
        // ���� �κ��丮 ���� ��ġ - ���콺 �̺�Ʈ�� �߻��� ��ġ => ���콺 �̺�Ʈ ��ġ���� �κ��丮 �������� �̵��� �� �ִ� �̵�����
        moveVecToCenter = inventoryTr.position - Input.mousePosition;

        // �ٸ� UI �̺�Ʈ�� ���� �ʽ��ϴ�.
        inventoryCG.blocksRaycasts = false;       
    }
    public void OnDrag( PointerEventData eventData )
    {   
        // ���콺 ��ġ�� �κ��丮 ��ġ�� ��ġ��Ű�� ������ ���콺��ġ�� �������Ƿ�, ���콺��ġ���� �������� ���� �����Ǵ�� �����̵��� ���ݴϴ�.
        inventoryTr.position = Input.mousePosition + moveVecToCenter;                
    }


    public void OnEndDrag( PointerEventData eventData )
    {   
        // �ٸ� UI �̺�Ʈ�� �޽��ϴ�.
        inventoryCG.blocksRaycasts = true;          
    }
}
