using DataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * [�۾� ����]  
 * <v1.0 - 2024_0113_�ֿ���>
 * 1- �κ��丮 �� ������ ���� ���� Ŭ������ �ۼ�
 * �׷��ȷ���ĳ������ ����ĳ������ UI �̺�Ʈ�� �ٸ� ĵ���� (�ٸ�����)���� �߻����� �ʱ� ������,
 * �÷��̾ ��ȣ�ۿ��� ���Ͽ� �ٸ� �κ��丮 â�� Open�� �ش� �κ��丮�� GraphicRaycaster �������� �˾ƾ�
 * ���ÿ� ����ĳ������ �ɾ��� �� �ִ�. 
 * 
 * <v2.0 - 2024-0114_�ֿ���>
 * 
 * 1- ����Ʈ clientInfo�� Awake������ isServer�� ��쿡�� �Ҵ��ϵ��� ����
 * 
 * 2- ConnectInventory�� void���� bool�� ���� ���� ��ȯ���� �ְԵǾ���.
 * ������ ��� �κ��丮�� �������� �� �˷��ֱ� ����.
 * 
 * 3- DisconnectInventory�޼��忡�� ���ڷ� InventoryInfo�� �޴� �κ� ����
 * ���� ���� ���� ������ Ŭ���̾�Ʈ�� �ڽ��� ������ �����Ͽ� ���ͱ��� ������ ����
 * 
 * 
 * 4- �ݱ� ��ư�� �����ϴ� ����(BtnInventoryClose)�� InventoryInteractiveŬ������ �ű�
 * ������ x��ư�� ������ �ڵ����� ��� �κ��丮���� ���� ���� Ȥ�� �ݱ⸦ �� �� �ֱ� ������
 * �����ϰ��� ������ �޼��带 ȣ������ �ʿ䰡 ���� ����
 * 
 * 5- �����κ��丮�� ��� serverInfo�� �ڱ��ڽ��� �����ϵ��� �����Ͽ�����, �б����� ������Ƽ�� �Ҵ�
 * �������� �������� Ŭ���̾�Ʈ �κ��丮���� �Ͼ��, ���� �κ��丮���� �Ͼ�� �ٷ� �����ּҸ� ������ �� �ְ� �ϱ� ����
 * 
 * 6- ClientInfo �б����� ������Ƽ�� �Ҵ��Ͽ� clientInfo ����Ʈ�� AsReadOnly�޼��带 ����Ͽ� 
 * �б����� �������̽��� ��ȯ�Ͽ�, ItemSelect���� ������ �� �ְ� �Ͽ�����, �׸� ������ ����.
 * 
 * <v2.1 - 2024-0114_�ֿ���>
 * 1- ��ӽ�ũ��Ʈ QuickSlot�� �����ϰ� ���üӼ��� ��ӹޱ����� private ������ �޼��带 protectedó��
 * 
 * <v2.2 -2024_0122_�ֿ���>
 * 1- �׷��� ����ĳ������ ItemSelect�� �ƴ϶� InventoryInfo�ʿ��� �����ϰ� ����Ʈ�� ��ȯ�ϴ� 
 * RaycastAllToConnectedInventory�޼��� �ۼ�
 * => ������ �������ʿ��� Ŭ���̾�Ʈ�� ���� �����ؼ� ����� �����ϴ� �ͺ��� ����� ��ȯ�ϴ� ���� ���뼺�� ���� ����
 * (ItemSelect ��ũ��Ʈ���� �������� ������ �� �Ӹ� �ƴ϶�, ������ ��� ������ ����
 * �÷��̾ �κ��丮 â�� �������·� ��Ŭ���� ���� ����ĳ������ �����ؾ� �ϴ� ��찡 ����.)  
 * 
 * <v2.3 - 2024_0123_�ֿ���>
 * 1- RaycastAllToConnectedInventory�޼��忡�� Ŭ���̾�Ʈ �ΰ�� ������ ����ĳ������ ��ȣ�� ���ֵ��� ����
 * 
 * <v2.4 - 2024_0125_�ֿ���>
 * 1- public���� inventoryCG�� HideInInspector ��Ʈ����Ʈ �߰�
 * 
 * 2- RegisterOwnerInfo�޼��� �ۼ��Ͽ� �κ��丮 �������� ���� �ĺ���ȣ�� ����� �� �ְ� �Ͽ���.
 * (DataManager���� ó�� �ε��� �� ���� �κ��丮�� �ѹ� ȣ���� �� �ְ���)
 * 
 * <v2.5 - 2024_0125_�ֿ���>
 * 1- RegisterOwnerInfo�� IdType�� Inventory���� User�� �����Ͽ���.
 * ������ ���� �κ��丮�� �������� ����� �� ������ ���������� ���� ������ �־�� ������, 
 * �κ��丮�� ��� ������ �̸��� �������� ����� �� ���� �ĺ���ȣ�� �ʿ��ϱ� ����(���������� �޸� �ϱ� ���ؼ�)
 * 
 * 2- RegisterOwnerInfo �޼������  RegisterUserInfo�� ����
 * (������ �÷��̾� �ʿ��� UserId�� �ο��޾ƾ� ������, ���ǻ� �κ��丮���� �����ϴ� ������ ����) 
 * 
 * 3- RegisterUserInfo �޼��带 ����
 * ������ �÷��̾��� Id�ο��� �� ���� �� �α���ȭ�鿡�� �Ҵ�Ǿ� �̷������ �ϴµ� ����� ������ �ȵǾ������Ƿ�
 * �̴ϼȶ������� ��ü�ϴ� ���̰�, ���� ������� �ʾƵ� �Ǳ� ����
 * (���߿� �κ� ȭ��(�α���ȭ��)�� ����� ������ ����)
 * 
 * <v2.6 - 2024_0126_�ֿ���>
 * 1- SwitchInventoryAppear�޼��� ���ο� SwitchAllItemAppearAs2D�޼��带 ���� ȣ���ϵ��� ����
 * �������� �κ��丮 â�� �ݾƵ� �������� �Ͼ�� ������ �־� �κ��丮 â�� ���� �� �������� ��ɵ� ���� ��Ȱ��ȭ �ϵ��� ����
 * 
 * 
 * <v2.7 - 2024_0128_�ֿ���>
 * 1- �ִϸ��̼��� ���̰� ��� �� ���Ŀ� �κ��丮 â�� �������� �Ͽ���
 * AllItemAppearAfterAnimation�� GetLongestAnimationLength�޼��带 ����
 * 
 * <v2.8 - 2024_0131_�ֿ���>
 * 1- Disconnect�޼��忡�� ���� �ڽ��� �κ��丮 ������ ���Ϸ� �����ϵ��� ���� 
 * (�������� �� �����Ǿ��� �� ���Ϸ� ������ �ȵǴ� ������ �����Ƿ�)
 * 
 * <v2.9 - 2024_0210_�ֿ���>
 * 1- DisconnectInventory�޼��� ���ο��� �ڽŰ� ��� �κ��丮�� �����ϴ� �ڵ带 0�� ����(���� ���̺� ����)�� �����ϵ��� ����
 * ������ ���� ���Կ� ����ڰ� �����ϴ� ���� �ƴ϶� �����͸� �ӽ������� �����ϱ� ���� �����̹Ƿ�
 * 
 * <v3.0 - 2024_0213_�ֿ���>
 * 1- DisconnectInventory�޼��� ���ο��� �ڽŰ� ��� �κ��丮�� �����ϴ� �ڵ带 �ּ�ó�� 
 * (����������Ʈ�� �ٸ� ���� ��ũ��Ʈ������ ���似�̺� ����� �������� �ʱ� ������ �ϰ����� �ֱ� ����)
 * 
 * <v3.1 - 2024_0222_�ֿ���>
 * 1- SwitchInventoryAppear�޼��忡�� inventoryCG�� Interactable�Ӽ��� OnOff�ϵ��� ����
 * => ĳ���Ͱ� �޸��ų� �Ҷ� Ű���带 ������ �Ǵµ� ����ؼ� �κ��丮�� �����۰� ��ȣ�ۿ��ϰ� �ǹǷ� �ش� �Ӽ��� �����ؾ��ϴ� ���� �߰�
 * 
 * 2- SwitchInventoryAppear �޼������ InventoryAppearSwitch�� ����
 * 
 * 3- AllItemAppearAfterAnimation�޼��� ���ο� �����κ��丮�� ��� ����� ��� �κ��丮�� �������� 2D��� ����Ī �޼��带 ȣ�����ֵ��� �Ͽ���
 * => ������ �����԰��� �����ִ� �����κ��丮�� ��� ������ �κ��丮 â�� ���� �� (���콺 ���ɸ����¿���) ��ȣ�ۿ��� ���� �Ͼ���� �ȵǱ� ����
 * 
 * 4- InitOpenState�޼��带 InventoryAppearSwitch�� �������� �ʰ� ���� �Ҵ��ϴ� ���·� ����
 * ������ InventoryAppearSwitch ���������� ����� ��� �κ��丮�� ����� �ǵ帮�� ������
 * ������ ���� ��� ��ũ��Ʈ���� ȣ�� �� �ߺ�ȣ���� ���輺�� ũ�� ����
 * 
 * 5- �޼���� InventoryAppearSwitch�� InventoryOperateSwitch�� ����
 * 
 * 6- ConnectInventory �� DisconnectInventory ������ InitOpenState�޼��� ����� InventoryOperateSwitch�޼��� ȣ��� ����
 * ������ InitOpenState�޼��尡 �κ��丮 ĵ�����׷츸 �ǵ帮�� ������ 
 * �κ��丮 â�� ���� ���¿����� �������� ĵ�����׷��� ������ �ǵ�ȱ� ������ InitOpenState�δ� ������ ������ �� ���� ������
 * �������� ���� �����ص� InitOpenState�޼��带 ȣ���ϰԵǸ� �κ��丮�� ���̰� �κ��丮�� ������ �ʰ� ��.
 * 
 */



public partial class InventoryInfo : MonoBehaviour
{
    // �÷��̾�� ��ȣ�ۿ��ϴ� �κ��丮�ʿ��� �����ϰ� �� �÷��̾�(����) �κ��丮
    protected InventoryInfo serverInfo = null;

    // �÷��̾�(����) �ʿ��� ��ȣ�ۿ� �� �����ϰ� �� Ŭ���̾�Ʈ �κ��丮
    protected List<InventoryInfo> clientInfo = null;

    /// <summary>
    /// �ش� �κ��丮�� �ٸ� �κ��丮�� ��ȣ�ۿ� (����) ���¶�� ����(�÷��̾�) �κ��丮�� ������ ��ȯ�մϴ�.<br/>
    /// </summary>
    public InventoryInfo ServerInfo { get { return serverInfo; } }


    /// <summary>
    /// �ش� �κ��丮�� ���� �κ��丮���, ���� ���� Ŭ���̾�Ʈ �κ��丮�� ������ ������ ������ 
    /// �� ����Ʈ�� ������ ��ȯ�մϴ�.<br/>
    /// ������ �ƴ϶�� null���� ��ȯ�˴ϴ�.
    /// </summary>
    public IReadOnlyList<InventoryInfo> ClientInfo { get { return clientInfo.AsReadOnly(); } }





    protected bool isServer;

    /// <summary>
    /// �ٸ� �κ��丮�� ���� �����ϱ� ���� ���������� �ϰ� �˴ϴ�.<br/>
    /// �÷��̾� �κ��丮�� ��� �ν����� �信�� ������ üũ�ؾ� �մϴ�.
    /// </summary>
    public bool IsServer { get { return isServer; } }


    protected bool isOpen;

    protected bool isConnect = false;

    /// <summary>
    /// �κ��丮 â�� ���� �ִ��� ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsOpen { get { return isOpen; } }

    /// <summary>
    /// ���� �κ��丮�� �ٸ� �κ��丮�� ����Ǿ��ִ��� ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsConnect { get { return isConnect; } }


    [HideInInspector]
    public CanvasGroup inventoryCG;        // �κ��丮�� ĵ���� �׷�

    /// <summary>
    /// ���� �κ��丮�� �����θ� ĵ������ �׷��ȷ���ĳ���͸� ��ȯ�մϴ�.
    /// </summary>
    public GraphicRaycaster gRaycaster { get { return inventoryTr.parent.GetComponent<GraphicRaycaster>(); } }



    
    /// <summary>
    /// �κ��丮 â�� �������� ���� �ݴ� �޼����Դϴ�.<br/>
    /// �ڱ� �ڽ� �κ��丮�� ���¸� �ݿ��ϸ�, �ִϸ��̼��� ���� ȣ������ �ʽ��ϴ�.<br/>
    /// connect, disconnect ���¿����� ȣ�⿡ ����մϴ�.
    /// </summary>
    public void InitOpenState( bool isOpen )
    {
        inventoryCG.blocksRaycasts=isOpen;    // �׷��� ��� ����ĳ��Ʈ�� �������ݴϴ�
        inventoryCG.interactable=isOpen;      // �׷��� ��ȣ�ۿ��� �����մϴ�.
        inventoryCG.alpha=isOpen ? 1f : 0f;   // �׷��� ������ �������ݴϴ�
        this.isOpen=isOpen;               // �κ��丮 Open ���¸� �ʱ�ȭ�մϴ�.
    }


    /// <summary>
    /// �κ��丮 â�� �ڵ����� ����ݴ� �޼����Դϴ�.<br/>
    /// �÷��̾� InputSystem������ IŰ�� ���� �� ȣ���ؾ� �մϴ�.<br/>
    /// </summary>
    public void InventoryOpenSwitch()
    {
        // �ٸ� �κ��丮�� ����� ���¶�� �۵����� �ʽ��ϴ�.
        if( isConnect )
            return;

        InventoryOperateSwitch( !isOpen );   // ȣ�� �� ���� �ݴ� ���·� �־��ݴϴ�
        isOpen=!isOpen;                     // ���� ��ȭ�� �ݴ�� ����մϴ�
    }


    /// <summary>
    /// �κ��丮�� â�� �������� ���� �ݽ��ϴ�.<br/>
    /// �κ��丮�� ���ü��� ����� ����Ī�ϱ� ���� 
    /// �׷��� ����ĳ����, ��ȣ�ۿ�, ������ �����ϸ�, �ִϸ��̼��� �۵���ŵ�ϴ�.
    /// </summary>
    protected void InventoryOperateSwitch( bool isOpen )
    {
        inventoryCG.blocksRaycasts=isOpen;    // �׷��� ��� ����ĳ��Ʈ�� �������ݴϴ�
        inventoryCG.interactable=isOpen;      // �׷��� ��ȣ�ۿ��� �����մϴ�.
        inventoryCG.alpha=isOpen ? 1f : 0f;   // �׷��� ������ �������ݴϴ�

        StartCoroutine( AllItemAppearAfterAnimation(isOpen) );    // �κ��丮 â ���� �� �ִϸ��̼� ���
    }

    /// <summary>
    /// isOpen���¿� ���� �ִϸ��̼� ����� ���� ��<br/> 
    /// �ڽ��� �κ��丮�� ���� �������� 2D ����� ����� ����Ī �ϸ�, <br/>
    /// ����Ǿ� �ִ� ���� �κ��丮�� ��ɸ� ����Ī�մϴ�.<br/>
    /// </summary>
    /// <returns></returns>
    protected IEnumerator AllItemAppearAfterAnimation(bool isOpen)
    {
        // �κ��丮 â�� ų �� �ִϸ��̼� �ִ���� ��ŭ ������ �� ���� �κ��丮�� ���� ��� �������� 2D ��� �۵��� ����Ī�մϴ�.
        if( isOpen )
        {
            if( animations!=null )  // �ִϸ��̼��� �����ϴ� ���
            {
                foreach( Animation animation in animations )
                    animation.Play();

                yield return animationWaitTime;
                
                AllItemOperateSwitchWithLinkedOpenInventory(isOpen); 
            }     
        }
        // �κ��丮 â�� �� ���� �ִϸ��̼��� �۵���Ű�� �ʰ� �ٷ� �����մϴ�.
        else
            AllItemOperateSwitchWithLinkedOpenInventory(isOpen);
        






        // ����Ǿ� �ִ� �κ��丮 �߿��� �����ִ� ������ �κ��丮�� ������ 2D ��� ����Ī�� ���ÿ� �����մϴ�.
        void AllItemOperateSwitchWithLinkedOpenInventory(bool isOpen)
        {
            // ���� �κ��丮�� ����� �κ��丮�� �ִ� ���� ����� ��� �κ��丮�� ��� ������ 2d��� ����Ī�� �����մϴ�.
            if( isServer&&clientInfo!=null )
            {
                foreach( InventoryInfo inventoryInfo in clientInfo )
                {
                    // �߽� �κ��丮�� ���� �������� ������ �ǵ帮�� ����� ����Ī�մϴ�.
                    if( inventoryInfo.isServer )
                        inventoryInfo.AllItemOperateSwitchAs2d( isOpen, true );

                    // �߽� �κ��丮�� �ƴϸ�, ����� �κ��丮�� �����ִ� ��� �������� ������ �ǵ帮�� �ʰ� ��ɸ� ����Ī�մϴ�.
                    else if( inventoryInfo.isOpen )
                        inventoryInfo.AllItemOperateSwitchAs2d( isOpen, false );
                }
            }
            // �߽� �κ��丮�� �ƴϰų�, ���������� ���� ���� �ڽ� �κ��丮�� ��� ������ 2d��� ����Ī�� �����մϴ�.
            else
                AllItemOperateSwitchAs2d( isOpen, true );
        }
    }



    /// <summary>
    /// �ִϸ��̼� Ŭ���� ���� ū ���̸� ���ϴ� �޼����Դϴ�.
    /// </summary>
    /// <returns>�ִϸ��̼� ����� �ʿ��� ���� ū ����</returns>
    protected float GetLongestAnimationLength( Animation[] animations )
    {
        float maxLength = 0f;
        
        foreach( Animation animation in animations )
        {
            if( animation.clip.length > maxLength )
                maxLength = animation.clip.length;
        }

        return maxLength;
    }



    




    /// <summary>
    /// ����, Ŭ���̾�Ʈ �κ��丮 �� ������ �õ��մϴ�.<br/><br/>
    /// *** ������ �κ��丮 ������ �߸��Ǿ��ų�, �̹� ���� ���� �����̰ų�, Ŭ���̾�Ʈ ���� ���谡 �ƴ϶�� ���ܰ� �߻�***
    /// </summary>
    /// <returns>������ �κ��丮�� ���� ���� ���¶�� flase�� ��ȯ, ���ῡ ���� �� true�� ��ȯ</returns>
    public bool ConnectInventory(InventoryInfo otherInfo)
    {
        if(otherInfo==null || otherInfo==this )
            throw new Exception("�κ��丮 ������ ��Ȯ���� �ʽ��ϴ�. �ٸ� �κ��丮�� �������� �ʿ��մϴ�.");

        if(this.isConnect)
            throw new Exception("���� �κ��丮�� ���� �����Դϴ�.");

        // ��밡 ���� ���¶�� ���и� ��ȯ�մϴ�.
        if(otherInfo.isConnect)
            return false;
            

        // �ڽ� ����<-> ��� Ŭ���̾�Ʈ ������ ���
        if( this.isServer && !otherInfo.isServer )
        {
            // �� �κ��丮 ������� Ȱ��ȭ �� Open
            this.isConnect=true;
            this.InventoryOperateSwitch(true);

            otherInfo.isConnect = true;
            otherInfo.InventoryOperateSwitch(true);

            // �ڽ��� Ŭ���̾�Ʈ ������ ��븦 �߰��մϴ�.
            this.clientInfo.Add(otherInfo);  

            // ����� ���� ������ �ڽ��� ����մϴ�.
            otherInfo.serverInfo = this;

            return true;
        }
        // �ڽ� Ŭ���̾�Ʈ<->��� ���� ������ ���
        else if( !this.isServer && otherInfo.isServer)
        {
            // �� �κ��丮 ������� Ȱ��ȭ �� Open
            this.isConnect = true;
            this.InventoryOperateSwitch(true);

            otherInfo.isConnect = true;
            otherInfo.InventoryOperateSwitch(true);

            // ����� Ŭ���̾�Ʈ ������ �ڽ��� �߰��մϴ�.
            otherInfo.clientInfo.Add(this);  

            // �ڽ��� ���������� ��븦 �����մϴ�.
            this.serverInfo = otherInfo;   
            
            return true;
        }



        throw new Exception("����-Ŭ���̾�Ʈ ���谡 �ƴմϴ�. �������� ������ �� ������, Ŭ���̾�Ʈ���� ���� �� �� �����ϴ�.");
    }





    /// <summary>
    /// �κ��丮 �� ������ �����ϰ� ������ �ʱ�ȭ �մϴ�.<br/>
    /// �ڽ��� �κ��丮�� ����� ����� �κ��丮 ������ ��� �ʱ�ȭ�ϰ� ������ �����մϴ�.<br/><br/>
    /// *** �޼��带 ȣ���� �κ��丮�� ������°� �ƴ϶�� ���ܰ� �߻� ***
    /// </summary>
    public void DisconnectInventory()
    {
        if(!this.isConnect)
            throw new Exception("������°� �ƴմϴ�.");

        // �ڽ��� �κ��丮 �����͸� �����մϴ�.
        //SaveInventoryData(0); 

        // �ڽ��� �������,
        if (this.isServer)
        {   
            // ���������� �߰� �� Ŭ���̾�Ʈ ������ �����Ͽ� Ŭ���̾�Ʈ�� �����մϴ�.
            InventoryInfo clientInfo = this.clientInfo[this.clientInfo.Count-1];

            // Ŭ���̾�Ʈ�� �κ��丮 �����͸� �����մϴ�.
            //clientInfo.SaveInventoryData(0);  

            // �� �κ��丮 ������� ��Ȱ��ȭ �� Close
            this.isConnect = false;
            this.InventoryOperateSwitch(false);
            
            clientInfo.isConnect=false;
            clientInfo.InventoryOperateSwitch(false);

            // �ڽ��� Ŭ���̾�Ʈ �������� ��븦 ����
            this.clientInfo.Remove(clientInfo);  

            // ����� ������������ �ڽ��� ����
            clientInfo.serverInfo = null;
        }
        // �ڽ��� Ŭ���̾�Ʈ���,
        else
        {
            // ������ �κ��丮 �����͸� �����մϴ�.
            //serverInfo.SaveInventoryData(0);

            // �� �κ��丮 ������� ��Ȱ��ȭ �� Close
            serverInfo.isConnect=false;
            serverInfo.InventoryOperateSwitch(false);
                        
            
            this.isConnect = false;
            this.InventoryOperateSwitch(false);        
            
            // ����� Ŭ���̾�Ʈ �������� �ڽ��� ����
            serverInfo.clientInfo.Remove(this); 
            
            // �ڽ��� ���� �������� ��븦 ����
            this.serverInfo = null;                 
        }
        
    }



    /// <summary>
    /// ���� ���� �� �ش� �κ��丮�� �ٸ� �κ��丮�� ��ũ�� ��ϸ��ϴ� �޼����Դϴ�.<br/>
    /// �÷��̾� �κ��丮, �������� ���ῡ ���˴ϴ�.
    /// </summary>
    protected void RegisterInventoryLink(InventoryInfo otherInfo)
    {
        if(otherInfo==null || otherInfo==this )
            throw new Exception("�κ��丮 ������ ��Ȯ���� �ʽ��ϴ�. �ٸ� �κ��丮�� �������� �ʿ��մϴ�.");

        // �ڽ��� �����̰� ��밡 Ŭ���̾�Ʈ���
        if( this.isServer && !otherInfo.isServer )
        {            
            this.clientInfo.Add(otherInfo); // �ڽ��� Ŭ���̾�Ʈ ������ ���ڷ� ���� �κ��丮�� ����մϴ�.
            otherInfo.serverInfo = this;    // ���ڷ� ���� �κ��丮�� ���� ������ �ڽ��� ����մϴ�.
            otherInfo.isConnect = true;     // Ŭ���̾�Ʈ�ʸ� ������·� ����ϴ�.
        }
        // �ڽ��� Ŭ���̾�Ʈ�̰� ��밡 �������
        else if( !this.isServer && otherInfo.isServer )
        {
            this.isConnect = true;          // Ŭ���̾�Ʈ�ʸ� ������·� ����ϴ�.
            this.serverInfo = otherInfo;    // �ڽ��� ���������� ���ڷ� ���� �κ��丮�� ����մϴ�.
            otherInfo.clientInfo.Add(this); // ���ڷ� ���� �κ��丮�� Ŭ���̾�Ʈ ������ �ڽ��� ����մϴ�.
        }
        else
            throw new Exception("����-Ŭ���̾�Ʈ ���谡 �ƴմϴ�. �������� ������ �� ������, Ŭ���̾�Ʈ���� ���� �� �� �����ϴ�.");
    }

    




    // �׷��� ����ĳ���� �� ���ڷ� ���� �� ������ �̺�Ʈ
    PointerEventData pEventData = new PointerEventData(EventSystem.current);

    // �׷��� ����ĳ���� ����� ���� ����Ʈ
    List<RaycastResult> raycastResults = new List<RaycastResult>();


    /// <summary>
    /// ����� ��� �κ��丮�� ���� ����ĳ������ �����ϰ� ����ĳ���� ����� ��ȯ�մϴ�.<br/>
    /// ���� �ɼ����� ���� ����� ������Ʈ �̸��� ����ϵ��� ������ �� �ֽ��ϴ�.
    /// </summary>
    public IReadOnlyList<RaycastResult> RaycastAllToConnectedInventory(bool isPrintDebugInfo=false)
    {
        // ������ �ƴ϶�� ������ ����ĳ������ �����մϴ�.
        if( !isServer )
        {
            if(!isConnect)
                throw new Exception("�κ��丮�� ������ ����Ǿ� ���� �ʽ��ϴ�.");

            return serverInfo.RaycastAllToConnectedInventory(isPrintDebugInfo);
        }


        // ����ĳ��Ʈ �������Ʈ�� �ʱ�ȭ�մϴ�.
        raycastResults.Clear();

        // �̺�Ʈ�� �Ͼ �������� ���콺�� �ٽ� Ŭ������ ���� �������� �����մϴ�.
        pEventData.position = Input.mousePosition;

        // ����� ��� �κ��丮���� �׷��� ����ĳ������ �����ϰ� ����� �޽��ϴ�.
        for( int i = 0; i<clientInfo.Count; i++ )
            clientInfo[i].gRaycaster.Raycast( pEventData, raycastResults );

        if( isPrintDebugInfo )
            PrintGRayCastDebugInfo( raycastResults );

        return raycastResults;
    }

    /// <summary>
    /// ����ĳ���� ����� ��� ������Ʈ �̸� ������ ����մϴ�.
    /// </summary>
    protected void PrintGRayCastDebugInfo(List<RaycastResult> raycastResults)
    {
        string objNames = "";

        for( int i = 0; i<raycastResults.Count; i++ )
            objNames+=raycastResults[i].gameObject.name+"\n";

        print( "[����� ������Ʈ �̸�]\n" + objNames 
            + "\n���� �κ��丮 ����: " + clientInfo.Count
            + "\n-------------------\n"  );
    }




}
