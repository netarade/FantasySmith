using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
 * �����ϰ����� ������ �޼��带 ȣ������ �ʿ䰡 ���� ����
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
 * 
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
    public InventoryInfo ServerInfo {  get { return serverInfo; } }

    
    /// <summary>
    /// �ش� �κ��丮�� ���� �κ��丮���, ���� ���� Ŭ���̾�Ʈ �κ��丮�� ������ ������ ������ 
    /// �� ����Ʈ�� ������ ��ȯ�մϴ�.<br/>
    /// ������ �ƴ϶�� null���� ��ȯ�˴ϴ�.
    /// </summary>
    public IReadOnlyList<InventoryInfo> ClientInfo { get { return clientInfo.AsReadOnly(); } }





    /// <summary>
    /// �ٸ� �κ��丮�� ���� �����ϱ� ���� ���������� �ϰ� �˴ϴ�.<br/>
    /// �÷��̾� �κ��丮�� ��� �ν����� �信�� ������ üũ�ؾ� �մϴ�.
    /// </summary>
    [Header("�÷��̾� �κ��丮�� ��� üũ")]
    [SerializeField] protected bool isServer = false;

    
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
    


    protected CanvasGroup inventoryCG;        // �κ��丮�� ĵ���� �׷�

    /// <summary>
    /// ���� �κ��丮�� �����θ� ĵ������ �׷��ȷ���ĳ���͸� ��ȯ�մϴ�.
    /// </summary>
    public GraphicRaycaster gRaycaster { get { return inventoryTr.parent.GetComponent<GraphicRaycaster>(); } }

    
    /// <summary>
    /// �κ��丮 â�� �ڵ����� �����ݴ� �޼����Դϴ�.<br/>
    /// �÷��̾� InputSystem������ IŰ�� ���� �� ȣ���ؾ� �մϴ�.<br/>
    /// </summary>
    public void InventoryOpenSwitch()
    {
        // �ٸ� �κ��丮�� ����� ���¶�� �۵����� �ʽ��ϴ�.
        if(isConnect)
            return;
        
        SwitchInventoryAppear( !isOpen );   // ȣ�� �� ���� �ݴ� ���·� �־��ݴϴ�
        isOpen = !isOpen;                   // ���� ��ȭ�� �ݴ�� ����մϴ�
    }


    /// <summary>
    /// �κ��丮 â�� �������� ���� �ݴ� �޼����Դϴ�.<br/>
    /// connect, disconnect ���¿����� ȣ�⿡ ����մϴ�.
    /// </summary>
    public void InitOpenState(bool isOpen)
    {        
        SwitchInventoryAppear(isOpen);  // ���� ���� �� �κ��丮 �ǳ��� ���д�.
        this.isOpen = isOpen;           //�ʱ⿡ �κ��丮�� ���� ����
    }


    /// <summary>
    /// �κ��丮�� ��� �̹����� �ؽ�Ʈ�� ���ݴϴ�.
    /// </summary>
    protected void SwitchInventoryAppear( bool isOpen )
    {
        inventoryCG.blocksRaycasts = isOpen;   // �׷��� ���� ����ĳ��Ʈ�� �������ݴϴ�
        inventoryCG.alpha = isOpen ? 1f : 0f;  // �׷��� �������� �������ݴϴ�
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
            this.InitOpenState(true);

            otherInfo.isConnect = true;
            otherInfo.InitOpenState(true);

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
            this.InitOpenState(true);

            otherInfo.isConnect = true;
            otherInfo.InitOpenState(true);

            // ����� Ŭ���̾�Ʈ ������ �ڽ��� �߰��մϴ�.
            otherInfo.clientInfo.Add(this);  

            // �ڽ��� ���������� ��븦 �����մϴ�.
            this.serverInfo = otherInfo;   
            
            return true;
        }


        throw new Exception("�������� ������ �� ������, Ŭ���̾�Ʈ���� ���� �� �� �����ϴ�.");
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
        
        // �ڽ��� �������,
        if(this.isServer)
        {   
            // ���������� �߰� �� Ŭ���̾�Ʈ ������ �����Ͽ� Ŭ���̾�Ʈ�� �����մϴ�.
            InventoryInfo clientInfo = this.clientInfo[this.clientInfo.Count-1];

            // �� �κ��丮 ������� ��Ȱ��ȭ �� Close
            this.isConnect = false;
            this.InitOpenState(false);
            
            clientInfo.isConnect=false;
            clientInfo.InitOpenState(false);

            // �ڽ��� Ŭ���̾�Ʈ �������� ��븦 ����
            this.clientInfo.Remove(clientInfo);  

            // ����� ������������ �ڽ��� ����
            clientInfo.serverInfo = null;
        }
        // �ڽ��� Ŭ���̾�Ʈ���,
        else
        {               
            // �� �κ��丮 ������� ��Ȱ��ȭ �� Close
            serverInfo.isConnect=false;
            serverInfo.InitOpenState(false);
                        
            this.isConnect = false;
            this.InitOpenState(false);        
            
            // ����� Ŭ���̾�Ʈ �������� �ڽ��� ����
            serverInfo.clientInfo.Remove(this); 
            
            // �ڽ��� ���� �������� ��븦 ����
            this.serverInfo = null;                 
        }
        
    }





}