using UnityEngine;
using UnityEngine.EventSystems;
using ItemData;
using UnityEngine.UI;
using System;
using WorldItemData;
using CreateManagement;

/* [�۾� ����]
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- �ʱ� ���� �ۼ�
 * Ŀ���� ���ٴ�� ���� ������ �������ͽ� â�� �� �� ������
 * ���� ���� �������ͽ� â�� �������� �Ͽ���.
 * �������ͽ� â�� ������ ������ �ݿ�
 * 
 * <v1.1 - 2023_1106_�ֿ���>
 * 1- txtDesc�� txtSpec ������ ����
 * 2- ����â�� ����� ��ȭ�������� ������ �ݿ��ϵ��� ����
 * 
 * <v1.2 - 2023_1106_�ֿ���>
 * 1- ��ȭ�������� ����â ���� �̸��� ������ ǥ�õǵ��� ����
 * 2- ����â ������ �����Ҵ����δ� ����� ������ �ʾ� ���� ���� �� �����ϴ� ������ �̸� ��Ƴ����� InventoryManagement�� static ���� ������ ����.
 * 
 * <v2.0 - 2023_1109_�ֿ���>
 * 1- ���� �ǳ� �߰� �� ���� Ȱ��ȭ ����, ���� �ݿ�
 * 
 * <v2.1 - 2023_1112_�ֿ���>
 * 1- ����â ��ġ�� ������ �ٷ� ������ ���� �� �ְ� ����
 * 2- ����â�� �ٸ� �̹����� ��ġ�� ��� ���� �տ� ǥ�õ� �� �ֵ��� ����
 * 
 * <v3.0 - 2023_1216_�ֿ���
 * 1- statusWindow�� ������ PlayerInven���� �����ϴ� ���� ���� ĳ���� ĵ������ �±׷� ã�� �����ϵ��� ����
 * 2- ����â�� ���� �� �������� �ణ ����
 * 
 * <v3.1 -2023_1217_�ֿ���>
 * 1- ���μ� ���� ����ü���� �ε����� �޾Ƽ� IIC ����ȭ ��ũ��Ʈ���� �̹����� �����Ͽ� ���μ� �̹����� �ݿ��ϵ��� ���� 
 * 2- iicMiscOther�� ������ CreateManager�� �̱����� ���� ������ ����
 * 
 * <v4.0 - 2023_1223_�ֿ���>
 * 1- ����â ��ġ ����
 * �κ��丮 �������� ��� ���� ���� ���� �ϴ����� ������ �����ְ�, �ϴܿ� ���� ���� ���� ������� �÷��� �����ְ� ����
 * 
 * <v4.1 - 2023_1226_�ֿ���>
 * 1- ���Ը���Ʈ ������ �����ۿ�����Ʈ �������� �����ִ� �� ����
 * �������� ���Ը���Ʈ �ܺο� �����Ǿ����� ��� ���Ը���Ʈ�� �������� ���ϰ� �Ǳ� ����
 * 
 * 2- ������ SlotListTr�� SlotListRectTr�� ����
 * 
 * 3- GameObject������ statusWindow�� Transform������ statusWindowTr�� ����
 * 
 * <v4.2 - 2023_1227_�ֿ���>
 * 1- statusWindowTr�� ������ ���ο� �ӽú��� Transform canvasTr������� ����
 * 
 * <v4.3 - 2023_1229_�ֿ���>
 * 1- ����â �������� ���� (�κ��丮 ���� ������ �ڽ��ε���)���� ���� ���� ����
 * 
 * <v4.4 - 2024_0105_�ֿ���>
 * 1- iicMiscOther�� ������ CreateManager�� �̱����������� FindWithTag������ �ӽ� ���� (���� �������ɼ� ����)
 * 
 * <v5.0 - 2024_0106_�ֿ���>
 * 1- Ŭ������ ���ϸ� ItemPointerStatusWindow���� StatusWindowInteractive�� ����
 * �����ۿ� �����Ͽ��� ��ũ��Ʈ�� �κ��丮�� �����ϱ�� ����
 * ������ �������� �Ź� �κ��丮�� �Űܴٴϱ� ������ ����â�� ������ �Ź� �ٸ��� �޾Ƽ� ����â�� ����� �ϸ�,
 * ��� �������� ����â �ڵ带 ������ �ִ� �ͺ��� ����â�� �޼��带 �ΰ� ������ �̺�Ʈ �߻� �� �ش� �޼��带 ȣ���ϱ⸸ �ϸ� �Ǳ� ����
 * 
 * 
 * 2- �ʿ���� ���� ���� �� ������ ����
 * ������ ���� itemStatusImage->statusImage
 * ������ ���� statusWIndorTr (statusRectTr�� ��ġ�� ����)
 * ������� itemInfo, item���� (����â�� ��� �� ItemInfo�� �޸��ؼ� �ޱ� ����)
 * ������� slotListRectTr ���� (����â�� �����ġ�� �Ǵ� �����̵Ǵ� �����̳� inventoryRectTr�� ��ü)
 * 
 * <5.1 - 2024_0107_�ֿ���>
 * 1- InventoryInteractive�� IsItemSelecting ���¿� ���� ����â�� ����� �ʵ��� ����
 * 
 * <v5.2 - 2024_0108_�ֿ���>
 * 1- ���μ��� Sprite�̹��� �������� ���� ã�Ƽ� �����ϴ� ��Ŀ���
 * VisualManager�� ���� �ε��� �ѹ��� �����Ͽ� �������� ��� ������� ����
 * 
 * <v5.3 - 2024_01111_�ֿ���>
 * 1- �����̹� �帣�� �°� ������ Ŭ���� ���� �������� ���� ���μ����� �ڵ带 ���� �� ������ Desc�� ��ü
 * 
 */



/// <summary>
/// �� ��ũ��Ʈ�� �ݵ�� ������ ������Ʈ(������)�� ��ġ�ؾ� �մϴ�. �������� �̺�Ʈ�� ������ �޾ƾ� �ϱ� �����Դϴ�.
/// </summary>
public class StatusWindowInteractive : MonoBehaviour
{    
    RectTransform statusRectTr; // ����â�� ��Ʈ Ʈ������
    Image statusImage;          // ����â�� ������ �̹���
    Text txtName;               // ����â�� ������ �̸�
    Text txtDesc;               // ����â�� ������ ����
    Text txtSpec;               // ����â�� ������ ���� ��ġ

    RectTransform inventoryRectTr;                  // �ڱ��ڽ� ��Ʈ Ʈ������
    InventoryInteractive inventoryInteractive;      // ������ ������ ���¸� Ȯ���� ��ũ��Ʈ ����


    void Start()
    {        
        // ���� ������Ʈ�� ��� ������ 0��° �ڽ��� �κ��丮 ������Ʈ�� ������ �ڽ��� ����â ������Ʈ ������ ������� �մϴ�.
        inventoryRectTr = GetComponent<RectTransform>();
        inventoryInteractive = GetComponent<InventoryInteractive>();
        statusRectTr = inventoryRectTr.GetChild(inventoryRectTr.childCount-1).GetComponent<RectTransform>();

        // ����â �� ���� ������Ʈ ���� ����
        statusImage = statusRectTr.GetChild(0).GetChild(0).GetComponent<Image>();
        txtName = statusRectTr.GetChild(1).GetComponent<Text>();
        txtDesc = statusRectTr.GetChild(2).GetComponent<Text>();
        txtSpec = statusRectTr.GetChild(3).GetComponent<Text>();
                      
        // ���� ���� �� ����â�� ���Ӵϴ�.
        statusRectTr.gameObject.SetActive(false);
    }
    

    /// <summary>
    /// �����ۿ� Ŀ���� ���� ��� ���� �������� ������ �� �� �ֽ��ϴ�.
    /// </summary>
    public void OnItemPointerEnter( ItemInfo itemInfo )
    {
        if(itemInfo == null)
            throw new Exception("�ش� �������� ������ ���޵��� �ʾҽ��ϴ�. Ȯ���Ͽ� �ּ���.");
        
        // �������� ������ ���¶�� �������� �ʽ��ϴ�.
        if( inventoryInteractive.IsItemSelecting )
            return;


        RectTransform itemRectTr = itemInfo.gameObject.GetComponent<RectTransform>();
        Item item = itemInfo.Item;


        /*** ������ ���� ������� ���� ���� ***/

        statusRectTr.gameObject.SetActive( true );        // ����â Ȱ��ȭ


        float delta = inventoryRectTr.position.y-itemRectTr.position.y;         // ����â�� ��� ��ġ �Ǵܱ���(�κ��丮�� �������� y��ġ ����)
        Vector3 rightPadding = Vector3.right*( statusRectTr.sizeDelta.x/2+30f );  // ����â ������ ����
        Vector3 upPadding = Vector3.up*( statusRectTr.sizeDelta.y/2 );              // ����â ��� ����


        if( delta<statusRectTr.sizeDelta.y/3 )          // �κ��丮 ��ܿ��� 1/3�̸� �������ִٸ�,
            statusRectTr.position=itemRectTr.position+rightPadding-upPadding;
        else if( delta<statusRectTr.sizeDelta.y*2/3 )   // �κ��丮 ��ܿ��� 2/3�̸� ������ �ִٸ�,
            statusRectTr.position=itemRectTr.position+rightPadding;
        else                                             // �κ��丮 ��ܿ��� 2/3�̻� ������ �ִٸ�,
            statusRectTr.position=itemRectTr.position+rightPadding+upPadding;

        statusImage.sprite=itemInfo.statusSprite;         // �̹����� ����� statusSprite �̹����� �����ش�.
        txtName.text = item.Name;                         // �̸� �ؽ�Ʈ�� ������ �̸��� �����ش�.
        txtDesc.text = item.Desc;                         // ���� �ؽ�Ʈ�� ������ ������ �����ش�.
        txtSpec.text = itemInfo.Spec;
    }






    



    /// <summary>
    /// �����ۿ��� Ŀ���� ���� ���� ������ �������ͽ� â�� ������ϴ�.
    /// </summary>
    public void OnItemPointerExit()
    {
        statusRectTr.gameObject.SetActive( false );      // ����â ��Ȱ��ȭ
    }


}
