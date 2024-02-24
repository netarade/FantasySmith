using DataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * [작업 사항]  
 * <v1.0 - 2024_0113_최원준>
 * 1- 인벤토리 간 전송을 위한 분할 클래스를 작성
 * 그래픽레이캐스터의 레이캐스팅의 UI 이벤트는 다른 캔버스 (다른계층)간에 발생하지 않기 때문에,
 * 플레이어가 상호작용을 통하여 다른 인벤토리 창을 Open시 해당 인벤토리의 GraphicRaycaster 참조값을 알아야
 * 동시에 레이캐스팅을 걸어줄 수 있다. 
 * 
 * <v2.0 - 2024-0114_최원준>
 * 
 * 1- 리스트 clientInfo는 Awake문에서 isServer인 경우에만 할당하도록 변경
 * 
 * 2- ConnectInventory를 void에서 bool로 성공 실패 반환값을 주게되었음.
 * 이유는 상대 인벤토리가 연결중일 때 알려주기 위함.
 * 
 * 3- DisconnectInventory메서드에서 인자로 InventoryInfo를 받던 부분 삭제
 * 연결 중일 때는 서버든 클라이언트든 자신의 정보를 참조하여 상대것까지 해제가 가능
 * 
 * 
 * 4- 닫기 버튼시 수행하는 동작(BtnInventoryClose)를 InventoryInteractive클래스로 옮김
 * 이유는 x버튼만 누르면 자동으로 상대 인벤토리까지 연결 해제 혹은 닫기를 할 수 있기 때문에
 * 연결하고나서 별도로 메서드를 호출해줄 필요가 없기 때문
 * 
 * 5- 서버인벤토리의 경우 serverInfo에 자기자신을 설정하도록 설정하였으며, 읽기전용 프로퍼티를 할당
 * 아이템의 셀렉팅이 클라이언트 인벤토리에서 일어나든, 서버 인벤토리에서 일어나든 바로 서버주소를 참조할 수 있게 하기 위함
 * 
 * 6- ClientInfo 읽기전용 프로퍼티를 할당하여 clientInfo 리스트를 AsReadOnly메서드를 사용하여 
 * 읽기전용 인터페이스로 반환하여, ItemSelect에서 참조할 수 있게 하였으며, 항목 수정을 막음.
 * 
 * <v2.1 - 2024-0114_최원준>
 * 1- 상속스크립트 QuickSlot을 정의하고 관련속성을 상속받기위해 private 변수와 메서드를 protected처리
 * 
 * <v2.2 -2024_0122_최원준>
 * 1- 그래픽 레이캐스팅을 ItemSelect가 아니라 InventoryInfo쪽에서 시전하고 리스트를 반환하는 
 * RaycastAllToConnectedInventory메서드 작성
 * => 이유는 아이템쪽에서 클라이언트를 따로 참조해서 기능을 구현하는 것보다 결과만 반환하는 것이 재사용성이 높기 때문
 * (ItemSelect 스크립트에서 아이템을 선택할 때 뿐만 아니라, 포션의 사용 등으로 인해
 * 플레이어가 인벤토리 창이 열린상태로 우클릭할 때도 레이캐스팅이 시전해야 하는 경우가 생김.)  
 * 
 * <v2.3 - 2024_0123_최원준>
 * 1- RaycastAllToConnectedInventory메서드에서 클라이언트 인경우 서버의 레이캐스팅을 재호출 해주도록 수정
 * 
 * <v2.4 - 2024_0125_최원준>
 * 1- public변수 inventoryCG의 HideInInspector 어트리뷰트 추가
 * 
 * 2- RegisterOwnerInfo메서드 작성하여 인벤토리 소유자의 고유 식별번호를 등록할 수 있게 하였음.
 * (DataManager에서 처음 로드할 때 서버 인벤토리만 한번 호출할 수 있게함)
 * 
 * <v2.5 - 2024_0125_최원준>
 * 1- RegisterOwnerInfo의 IdType을 Inventory에서 User로 변경하였음.
 * 이유는 월드 인벤토리에 아이템이 저장될 때 누구의 아이템인지 유저 정보도 있어야 하지만, 
 * 인벤토리의 경우 동일한 이름의 아이템이 만들어 질 때의 식별번호가 필요하기 때문(저장파일을 달리 하기 위해서)
 * 
 * 2- RegisterOwnerInfo 메서드명을  RegisterUserInfo로 변경
 * (원래는 플레이어 쪽에서 UserId를 부여받아야 하지만, 편의상 인벤토리에서 설정하는 것으로 간주) 
 * 
 * 3- RegisterUserInfo 메서드를 제거
 * 이유는 플레이어의 Id부여는 씬 시작 전 로그인화면에서 할당되어 이루어져야 하는데 현재는 구현이 안되어있으므로
 * 이니셜라이저로 대체하는 것이고, 따로 등록하지 않아도 되기 때문
 * (나중에 로비 화면(로그인화면)이 생기면 진행할 예정)
 * 
 * <v2.6 - 2024_0126_최원준>
 * 1- SwitchInventoryAppear메서드 내부에 SwitchAllItemAppearAs2D메서드를 같이 호출하도록 설정
 * 아이템이 인벤토리 창을 닫아도 셀렉팅이 일어나는 현상이 있어 인벤토리 창을 닫을 때 아이템의 기능도 같이 비활성화 하도록 설정
 * 
 * 
 * <v2.7 - 2024_0128_최원준>
 * 1- 애니메이션의 길이가 재생 된 이후에 인벤토리 창이 열리도록 하였음
 * AllItemAppearAfterAnimation와 GetLongestAnimationLength메서드를 정의
 * 
 * <v2.8 - 2024_0131_최원준>
 * 1- Disconnect메서드에서 상대와 자신의 인벤토리 정보를 파일로 저장하도록 구현 
 * (보관함이 갓 생성되었을 때 파일로 저장이 안되는 문제가 있으므로)
 * 
 * <v2.9 - 2024_0210_최원준>
 * 1- DisconnectInventory메서드 내부에서 자신과 상대 인벤토리를 저장하는 코드를 0번 슬롯(오토 세이브 슬롯)만 저장하도록 변경
 * 이유는 지정 슬롯에 사용자가 저장하는 것이 아니라 데이터를 임시적으로 저장하기 위한 방편이므로
 * 
 * <v3.0 - 2024_0213_최원준>
 * 1- DisconnectInventory메서드 내부에서 자신과 상대 인벤토리를 저장하는 코드를 주석처리 
 * (통합프로젝트의 다른 팀원 스크립트에서는 오토세이브 기능을 구현하지 않기 때문에 일관성을 주기 위해)
 * 
 * <v3.1 - 2024_0222_최원준>
 * 1- SwitchInventoryAppear메서드에서 inventoryCG의 Interactable속성을 OnOff하도록 설정
 * => 캐릭터가 달리거나 할때 키보드를 누르게 되는데 계속해서 인벤토리의 아이템과 상호작용하게 되므로 해당 속성을 해제해야하는 것을 발견
 * 
 * 2- SwitchInventoryAppear 메서드명을 InventoryAppearSwitch로 변경
 * 
 * 3- AllItemAppearAfterAnimation메서드 내부에 다중인벤토리의 경우 연결된 모든 인벤토리의 아이템의 2D기능 스위칭 메서드를 호출해주도록 하였음
 * => 이유는 퀵슬롯같이 열려있는 연결인벤토리의 경우 유저가 인벤토리 창을 닫을 때 (마우스 락걸린상태에서) 상호작용이 따로 일어나서는 안되기 때문
 * 
 * 4- InitOpenState메서드를 InventoryAppearSwitch를 재사용하지 않고 직접 할당하는 형태로 변경
 * 이유는 InventoryAppearSwitch 내부적으로 연결된 모든 인벤토리의 기능을 건드리기 때문에
 * 퀵슬롯 등의 상속 스크립트에서 호출 시 중복호출의 위험성이 크기 때문
 * 
 * 5- 메서드명 InventoryAppearSwitch를 InventoryOperateSwitch로 변경
 * 
 * 6- ConnectInventory 및 DisconnectInventory 내부의 InitOpenState메서드 사용을 InventoryOperateSwitch메서드 호출로 변경
 * 이유는 InitOpenState메서드가 인벤토리 캔버스그룹만 건드리기 때문에 
 * 인벤토리 창을 닫은 상태에서는 아이템의 캔버스그룹의 투명도를 건드렸기 때문에 InitOpenState로는 복구를 시켜줄 수 없기 때문에
 * 아이템이 실제 존재해도 InitOpenState메서드를 호출하게되면 인벤토리만 보이고 인벤토리는 보이지 않게 됨.
 * 
 */



public partial class InventoryInfo : MonoBehaviour
{
    // 플레이어와 상호작용하는 인벤토리쪽에서 참조하게 될 플레이어(서버) 인벤토리
    protected InventoryInfo serverInfo = null;

    // 플레이어(서버) 쪽에서 상호작용 시 참조하게 될 클라이언트 인벤토리
    protected List<InventoryInfo> clientInfo = null;

    /// <summary>
    /// 해당 인벤토리가 다른 인벤토리와 상호작용 (연결) 상태라면 서버(플레이어) 인벤토리의 참조를 반환합니다.<br/>
    /// </summary>
    public InventoryInfo ServerInfo { get { return serverInfo; } }


    /// <summary>
    /// 해당 인벤토리가 서버 인벤토리라면, 연결 중인 클라이언트 인벤토리의 정보를 가지고 있으며 
    /// 이 리스트의 참조를 반환합니다.<br/>
    /// 서버가 아니라면 null값이 반환됩니다.
    /// </summary>
    public IReadOnlyList<InventoryInfo> ClientInfo { get { return clientInfo.AsReadOnly(); } }





    protected bool isServer;

    /// <summary>
    /// 다른 인벤토리를 다중 참조하기 위한 서버역할을 하게 됩니다.<br/>
    /// 플레이어 인벤토리의 경우 인스펙터 뷰에서 서버를 체크해야 합니다.
    /// </summary>
    public bool IsServer { get { return isServer; } }


    protected bool isOpen;

    protected bool isConnect = false;

    /// <summary>
    /// 인벤토리 창이 열려 있는지 여부를 반환합니다.
    /// </summary>
    public bool IsOpen { get { return isOpen; } }

    /// <summary>
    /// 현재 인벤토리가 다른 인벤토리와 연결되어있는지 여부를 반환합니다.
    /// </summary>
    public bool IsConnect { get { return isConnect; } }


    [HideInInspector]
    public CanvasGroup inventoryCG;        // 인벤토리의 캔버스 그룹

    /// <summary>
    /// 현재 인벤토리의 상위부모 캔버스의 그래픽레이캐스터를 반환합니다.
    /// </summary>
    public GraphicRaycaster gRaycaster { get { return inventoryTr.parent.GetComponent<GraphicRaycaster>(); } }



    
    /// <summary>
    /// 인벤토리 창을 수동으로 열고 닫는 메서드입니다.<br/>
    /// 자기 자신 인벤토리의 상태만 반영하며, 애니메이션을 따로 호출하지 않습니다.<br/>
    /// connect, disconnect 상태에서의 호출에 사용합니다.
    /// </summary>
    public void InitOpenState( bool isOpen )
    {
        inventoryCG.blocksRaycasts=isOpen;    // 그룹의 블록 레이캐스트를 조절해줍니다
        inventoryCG.interactable=isOpen;      // 그룹의 상호작용을 조절합니다.
        inventoryCG.alpha=isOpen ? 1f : 0f;   // 그룹의 투명도를 조절해줍니다
        this.isOpen=isOpen;               // 인벤토리 Open 상태를 초기화합니다.
    }


    /// <summary>
    /// 인벤토리 창을 자동으로 열고닫는 메서드입니다.<br/>
    /// 플레이어 InputSystem에서의 I키를 누를 때 호출해야 합니다.<br/>
    /// </summary>
    public void InventoryOpenSwitch()
    {
        // 다른 인벤토리와 연결된 상태라면 작동하지 않습니다.
        if( isConnect )
            return;

        InventoryOperateSwitch( !isOpen );   // 호출 시 마다 반대 상태로 넣어줍니다
        isOpen=!isOpen;                     // 상태 변화를 반대로 기록합니다
    }


    /// <summary>
    /// 인벤토리의 창을 수동으로 열고 닫습니다.<br/>
    /// 인벤토리의 가시성과 기능을 스위칭하기 위해 
    /// 그래픽 레이캐스팅, 상호작용, 투명도를 조절하며, 애니메이션을 작동시킵니다.
    /// </summary>
    protected void InventoryOperateSwitch( bool isOpen )
    {
        inventoryCG.blocksRaycasts=isOpen;    // 그룹의 블록 레이캐스트를 조절해줍니다
        inventoryCG.interactable=isOpen;      // 그룹의 상호작용을 조절합니다.
        inventoryCG.alpha=isOpen ? 1f : 0f;   // 그룹의 투명도를 조절해줍니다

        StartCoroutine( AllItemAppearAfterAnimation(isOpen) );    // 인벤토리 창 오픈 시 애니메이션 재생
    }

    /// <summary>
    /// isOpen상태에 따라 애니메이션 재생이 끝난 후<br/> 
    /// 자신의 인벤토리의 내부 아이템의 2D 모습과 기능을 스위칭 하며, <br/>
    /// 연결되어 있는 열린 인벤토리의 기능만 스위칭합니다.<br/>
    /// </summary>
    /// <returns></returns>
    protected IEnumerator AllItemAppearAfterAnimation(bool isOpen)
    {
        // 인벤토리 창을 킬 때 애니메이션 최대길이 만큼 쉬어준 후 다중 인벤토리에 속한 모든 아이템의 2D 기능 작동을 스위칭합니다.
        if( isOpen )
        {
            if( animations!=null )  // 애니메이션이 존재하는 경우
            {
                foreach( Animation animation in animations )
                    animation.Play();

                yield return animationWaitTime;
                
                AllItemOperateSwitchWithLinkedOpenInventory(isOpen); 
            }     
        }
        // 인벤토리 창을 끌 때는 애니메이션을 작동시키지 않고 바로 종료합니다.
        else
            AllItemOperateSwitchWithLinkedOpenInventory(isOpen);
        






        // 연결되어 있는 인벤토리 중에서 열려있는 상태의 인벤토리의 아이템 2D 기능 스위칭을 동시에 실행합니다.
        void AllItemOperateSwitchWithLinkedOpenInventory(bool isOpen)
        {
            // 다중 인벤토리에 연결된 인벤토리가 있는 경우는 연결된 모든 인벤토리의 모든 아이템 2d기능 스위칭을 실행합니다.
            if( isServer&&clientInfo!=null )
            {
                foreach( InventoryInfo inventoryInfo in clientInfo )
                {
                    // 중심 인벤토리의 경우는 아이템의 투명도를 건드리며 기능을 스위칭합니다.
                    if( inventoryInfo.isServer )
                        inventoryInfo.AllItemOperateSwitchAs2d( isOpen, true );

                    // 중심 인벤토리가 아니며, 연결된 인벤토리가 열려있는 경우 아이템의 투명도를 건드리지 않고 기능만 스위칭합니다.
                    else if( inventoryInfo.isOpen )
                        inventoryInfo.AllItemOperateSwitchAs2d( isOpen, false );
                }
            }
            // 중심 인벤토리가 아니거나, 연결정보가 없는 경우는 자신 인벤토리의 모든 아이템 2d기능 스위칭만 실행합니다.
            else
                AllItemOperateSwitchAs2d( isOpen, true );
        }
    }



    /// <summary>
    /// 애니메이션 클립의 가장 큰 길이를 구하는 메서드입니다.
    /// </summary>
    /// <returns>애니메이션 재생에 필요한 가장 큰 길이</returns>
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
    /// 서버, 클라이언트 인벤토리 간 연결을 시도합니다.<br/><br/>
    /// *** 전달한 인벤토리 참조가 잘못되었거나, 이미 연결 중인 상태이거나, 클라이언트 서버 관계가 아니라면 예외가 발생***
    /// </summary>
    /// <returns>전달한 인벤토리가 연결 중인 상태라면 flase를 반환, 연결에 성공 시 true를 반환</returns>
    public bool ConnectInventory(InventoryInfo otherInfo)
    {
        if(otherInfo==null || otherInfo==this )
            throw new Exception("인벤토리 참조가 정확하지 않습니다. 다른 인벤토리의 참조값이 필요합니다.");

        if(this.isConnect)
            throw new Exception("현재 인벤토리가 연결 상태입니다.");

        // 상대가 연결 상태라면 실패를 반환합니다.
        if(otherInfo.isConnect)
            return false;
            

        // 자신 서버<-> 상대 클라이언트 연결인 경우
        if( this.isServer && !otherInfo.isServer )
        {
            // 두 인벤토리 연결상태 활성화 및 Open
            this.isConnect=true;
            this.InventoryOperateSwitch(true);

            otherInfo.isConnect = true;
            otherInfo.InventoryOperateSwitch(true);

            // 자신의 클라이언트 정보에 상대를 추가합니다.
            this.clientInfo.Add(otherInfo);  

            // 상대의 서버 정보에 자신을 등록합니다.
            otherInfo.serverInfo = this;

            return true;
        }
        // 자신 클라이언트<->상대 서버 연결인 경우
        else if( !this.isServer && otherInfo.isServer)
        {
            // 두 인벤토리 연결상태 활성화 및 Open
            this.isConnect = true;
            this.InventoryOperateSwitch(true);

            otherInfo.isConnect = true;
            otherInfo.InventoryOperateSwitch(true);

            // 상대의 클라이언트 정보에 자신을 추가합니다.
            otherInfo.clientInfo.Add(this);  

            // 자신의 서버정보에 상대를 설정합니다.
            this.serverInfo = otherInfo;   
            
            return true;
        }



        throw new Exception("서버-클라이언트 관계가 아닙니다. 서버끼리 연결할 수 없으며, 클라이언트끼리 연결 할 수 없습니다.");
    }





    /// <summary>
    /// 인벤토리 간 연결을 제거하고 정보를 초기화 합니다.<br/>
    /// 자신의 인벤토리와 연결된 상대의 인벤토리 정보를 모두 초기화하고 연결을 해제합니다.<br/><br/>
    /// *** 메서드를 호출한 인벤토리가 연결상태가 아니라면 예외가 발생 ***
    /// </summary>
    public void DisconnectInventory()
    {
        if(!this.isConnect)
            throw new Exception("연결상태가 아닙니다.");

        // 자신의 인벤토리 데이터를 저장합니다.
        //SaveInventoryData(0); 

        // 자신이 서버라면,
        if (this.isServer)
        {   
            // 마지막으로 추가 된 클라이언트 정보를 참조하여 클라이언트로 설정합니다.
            InventoryInfo clientInfo = this.clientInfo[this.clientInfo.Count-1];

            // 클라이언트의 인벤토리 데이터를 저장합니다.
            //clientInfo.SaveInventoryData(0);  

            // 두 인벤토리 연결상태 비활성화 및 Close
            this.isConnect = false;
            this.InventoryOperateSwitch(false);
            
            clientInfo.isConnect=false;
            clientInfo.InventoryOperateSwitch(false);

            // 자신의 클라이언트 정보에서 상대를 삭제
            this.clientInfo.Remove(clientInfo);  

            // 상대의 서버정보에서 자신을 삭제
            clientInfo.serverInfo = null;
        }
        // 자신이 클라이언트라면,
        else
        {
            // 서버의 인벤토리 데이터를 저장합니다.
            //serverInfo.SaveInventoryData(0);

            // 두 인벤토리 연결상태 비활성화 및 Close
            serverInfo.isConnect=false;
            serverInfo.InventoryOperateSwitch(false);
                        
            
            this.isConnect = false;
            this.InventoryOperateSwitch(false);        
            
            // 상대의 클라이언트 정보에서 자신을 삭제
            serverInfo.clientInfo.Remove(this); 
            
            // 자신의 서버 정보에서 상대를 삭제
            this.serverInfo = null;                 
        }
        
    }



    /// <summary>
    /// 게임 시작 시 해당 인벤토리를 다른 인벤토리의 링크로 등록만하는 메서드입니다.<br/>
    /// 플레이어 인벤토리, 퀵슬롯의 연결에 사용됩니다.
    /// </summary>
    protected void RegisterInventoryLink(InventoryInfo otherInfo)
    {
        if(otherInfo==null || otherInfo==this )
            throw new Exception("인벤토리 참조가 정확하지 않습니다. 다른 인벤토리의 참조값이 필요합니다.");

        // 자신이 서버이고 상대가 클라이언트라면
        if( this.isServer && !otherInfo.isServer )
        {            
            this.clientInfo.Add(otherInfo); // 자신의 클라이언트 정보에 인자로 들어온 인벤토리를 등록합니다.
            otherInfo.serverInfo = this;    // 인자로 들어온 인벤토리의 서버 정보에 자신을 등록합니다.
            otherInfo.isConnect = true;     // 클라이언트쪽만 연결상태로 만듭니다.
        }
        // 자신이 클라이언트이고 상대가 서버라면
        else if( !this.isServer && otherInfo.isServer )
        {
            this.isConnect = true;          // 클라이언트쪽만 연결상태로 만듭니다.
            this.serverInfo = otherInfo;    // 자신의 서버정보에 인자로 들어온 인벤토리를 등록합니다.
            otherInfo.clientInfo.Add(this); // 인자로 들어온 인벤토리의 클라이언트 정보에 자신을 등록합니다.
        }
        else
            throw new Exception("서버-클라이언트 관계가 아닙니다. 서버끼리 연결할 수 없으며, 클라이언트끼리 연결 할 수 없습니다.");
    }

    




    // 그래픽 레이캐스팅 시 인자로 전달 할 포인터 이벤트
    PointerEventData pEventData = new PointerEventData(EventSystem.current);

    // 그래픽 레이캐스팅 결과를 받을 리스트
    List<RaycastResult> raycastResults = new List<RaycastResult>();


    /// <summary>
    /// 연결된 모든 인벤토리를 향해 레이캐스팅을 시전하고 레이캐스팅 결과를 반환합니다.<br/>
    /// 선택 옵션으로 맞은 결과의 오브젝트 이름을 출력하도록 설정할 수 있습니다.
    /// </summary>
    public IReadOnlyList<RaycastResult> RaycastAllToConnectedInventory(bool isPrintDebugInfo=false)
    {
        // 서버가 아니라면 서버의 레이캐스팅을 시전합니다.
        if( !isServer )
        {
            if(!isConnect)
                throw new Exception("인벤토리가 서버와 연결되어 있지 않습니다.");

            return serverInfo.RaycastAllToConnectedInventory(isPrintDebugInfo);
        }


        // 레이캐스트 결과리스트를 초기화합니다.
        raycastResults.Clear();

        // 이벤트가 일어날 포지션을 마우스를 다시 클릭했을 때의 지점으로 설정합니다.
        pEventData.position = Input.mousePosition;

        // 연결된 모든 인벤토리에게 그래픽 레이캐스팅을 시전하고 결과를 받습니다.
        for( int i = 0; i<clientInfo.Count; i++ )
            clientInfo[i].gRaycaster.Raycast( pEventData, raycastResults );

        if( isPrintDebugInfo )
            PrintGRayCastDebugInfo( raycastResults );

        return raycastResults;
    }

    /// <summary>
    /// 레이캐스팅 결과의 모든 오브젝트 이름 정보를 출력합니다.
    /// </summary>
    protected void PrintGRayCastDebugInfo(List<RaycastResult> raycastResults)
    {
        string objNames = "";

        for( int i = 0; i<raycastResults.Count; i++ )
            objNames+=raycastResults[i].gameObject.name+"\n";

        print( "[검출된 오브젝트 이름]\n" + objNames 
            + "\n연결 인벤토리 갯수: " + clientInfo.Count
            + "\n-------------------\n"  );
    }




}
