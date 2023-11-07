using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems; // ���콺 Ŭ��, �巡�� ���, �̺�Ʈ ���� ����
using UnityEngine.UI;


/* [�۾� ����]
 * <v1.0 - 2023_1108_�ֿ���>
 * 1- �ʱ� ���� �ۼ�
 * 
 */


/// <summary>
/// InventoryDrag��ũ��Ʈ�� Inventory ������Ʈ�� ������Ʈ�� ������ �־�� �մϴ�.<br/>
/// �κ��丮 ������Ʈ�� �巡�׸� �����ϰ� ���ݴϴ�.
/// </summary>
public class InventoryDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform tr;                               // ������ �ݺ� ȣ�� �ؾ� �ϹǷ� �ڽ� transform ĳ��ó��
    private CanvasGroup cg;
    
    void Start()
    {
        tr= transform;
        cg = GetComponent<CanvasGroup>();
    }
    
    public void OnBeginDrag( PointerEventData eventData )
    {
        cg.blocksRaycasts = false;       // �ٸ� UI �̺�Ʈ�� ���� �ʽ��ϴ�.
    }
    public void OnDrag( PointerEventData eventData )
    {
        tr.position = Input.mousePosition;                // �������� ��ġ�� ���콺�� ��ġ�� ��ġ��Ų��.
    }


    public void OnEndDrag( PointerEventData eventData )
    {
        cg.blocksRaycasts = true;          // �ٸ� UI �̺�Ʈ�� �޽��ϴ�.
    }
}
