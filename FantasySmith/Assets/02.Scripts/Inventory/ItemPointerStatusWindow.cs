using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ItemData;
using UnityEngine.UI;

/* [�۾� ����]
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- �ʱ� ���� �ۼ�
 * Ŀ���� ���ٴ�� ���� ������ �������ͽ� â�� �� �� ������
 * ���� ���� �������ͽ� â�� �������� �Ͽ���.
 * �������ͽ� â�� ������ ������ �ݿ�
 */



/// <summary>
/// �� ��ũ��Ʈ�� �ݵ�� ������ ������Ʈ(������)�� ��ġ�ؾ� �մϴ�. �������� �̺�Ʈ�� ������ �޾ƾ� �ϱ� �����Դϴ�.
/// </summary>
public class ItemPointerStatusWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{    
    private GameObject statusWindow;    // ����â ������Ʈ
    private Image imageItem;            // ����â�� ������ �̹���
    private Text txtEnhancement;        // ����â�� ������ ��ȭ �ؽ�Ʈ
    private Text txtName;               // ����â�� ������ �̸�
    private Text txtSpec;               // ����â�� ������ ����
    private Text txtDesc;               // ����â�� ������ ����

    private Item item;                  // ���� �������� �����ϱ� ���� ����


    
    /// <summary>
    /// ��� ������ �ν��Ͻ��� �������ͽ� â�� ������ ���� �����ؾ� �ϹǷ� �ݵ�� Awake�� �ξ�� �մϴ�.
    /// </summary>
    void Awake()
    {        
        statusWindow = GameObject.Find("Panel-ItemStatus");
        imageItem = statusWindow.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        txtEnhancement = statusWindow.transform.GetChild(1).GetComponent<Text>();
        txtName = statusWindow.transform.GetChild(2).GetComponent<Text>();
        txtSpec = statusWindow.transform.GetChild(3).GetComponent<Text>();
        txtDesc = statusWindow.transform.GetChild(4).GetComponent<Text>();
    }


    void Start()
    {
        item = GetComponent<ItemInfo>().item;
    }

    
    /// <summary>
    /// �����ۿ� Ŀ���� ���� ��� ���� �������� ������ �� �� �ֽ��ϴ�.
    /// </summary>
    public void OnPointerEnter( PointerEventData eventData )
    {
        if(item == null)
            return;

        statusWindow.SetActive(true);       // ����â Ȱ��ȭ
        statusWindow.transform.localPosition 
            = transform.position + Vector3.left*350f + Vector3.down*600f; //����â�� ��ġ
    
        imageItem.sprite = item.Image.statusSprite;
        txtName.text = item.Name;
        txtDesc.text = item.Name;

        if(item.Type == ItemType.Weapon)
        {
            ItemWeapon weap = (ItemWeapon)item;
            txtEnhancement.enabled = true;
            //txtEnhancement.text 

        }
        else if( item.Type == ItemType.Misc )
        { 
            txtEnhancement.enabled = false;
        }
        
    }


    /// <summary>
    /// �����ۿ��� Ŀ���� ���� ���� ������ �������ͽ� â�� ������ϴ�.
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerExit( PointerEventData eventData )
    {
        statusWindow.SetActive(false);      // ����â ��Ȱ��ȭ
    }


}
