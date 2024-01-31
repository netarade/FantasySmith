using CreateManagement;
using ItemData;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * [작업 사항]
 * 
 * <v1.0>
 * 1- 플레이어 테스트용 스크립트 작성
 * 
 * <v2.0>
 * 1- E키를 사용하여 인벤토리를 여는 코드 테스트
 * 
 * <v3.0 - 2024_0117_최원준>
 * 1- 숫자키패드를 사용하여 퀵슬롯을 여는 코드 테스트
 * 2- 스크립트명 PlayerInteractive로 변경
 * 
 * <v3.1 - 2024_0118_최원준>
 * 1- 퀵슬롯과 플레이어인벤토리 계층참조 (퀵슬롯을 먼저찾은 후 퀵슬롯을 토대로 찾는 형태로) 수정
 *
 */




public class PlayerInteractive : MonoBehaviour
{


    InventoryInfo playerInventory;
    QuickSlot quickSlot;

    Animator animator;
    bool bMouseCoolTime = true;
    
    float playerHp = 100f;
    float playerHunger = 10f;
    float playerThirsty = 10f;

    PlayerStatus playerStatus;
        
    void Start()
    {
        quickSlot = GetComponentInChildren<QuickSlot>();
        playerInventory = quickSlot.transform.parent.GetChild(0).GetComponent<InventoryInfo>();
        animator = GetComponent<Animator>();
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();  
    }

    public void PlayerStatusChange(ItemStatus itemStatus)
    {
        playerHp += itemStatus.hp;
        playerHunger += itemStatus.hunger;
        playerThirsty += itemStatus.thirsty;
    }


    private void Update()
    {        
        // 플레이어가 인벤토리를 여는 상황
        Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(clickRay.origin, clickRay.direction*100f, Color.cyan);


        // 플레이어 포션먹는 상황 (우클릭)
        if(Input.GetMouseButtonDown(1) && playerInventory.IsOpen )
        {
            bMouseCoolTime = false;

            // 연결되어있는 모든 인벤토리에 레이캐스팅을 시전하고 결과를 받습니다.
            IReadOnlyList<RaycastResult> resultList = playerInventory.RaycastAllToConnectedInventory();
            
            foreach(RaycastResult result in resultList )
            {               
                ItemInfo itemInfo = result.gameObject.GetComponent<ItemInfo>(); 
                DummyInfo dummyInfo = result.gameObject.GetComponent<DummyInfo>();

                if( itemInfo != null )
                {          
                    // 장착 가능한 아이템의 경우
                    if(itemInfo.EquipType!=EquipType.None )
                    {
                        // 미착용상태일 경우 장착을 실행하고 레이캐스팅을 끝냅니다.
                        if( !itemInfo.IsEquip )
                        {
                            itemInfo.OnItemEquip();
                            
                            // 팀원 전용 상태 변화 코드 호출
                            if( itemInfo.EquipType==EquipType.Helmet )
                            {
                                if( playerStatus!=null )
                                    playerStatus.isHood=true;
                            }

                            break;  
                        }
                        // 착용상태일 경우 
                        else
                            throw new System.Exception("착용상태인 아이템이 UI상으로 검출되었습니다.");
                    }
                    // 음식 아이템의 경우, 플레이어의 상태 변화메서드를 전달하여, 아이템 섭취메서드를 호출하고 레이캐스팅을 끝냅니다.
                    else if( itemInfo.MiscType==MiscType.Food )
                    {
                        itemInfo.PrintDebugInfo();
                        itemInfo.OnItemEat( PlayerStatusChange );
                        break;     
                    }   
                }



                else if(dummyInfo!=null)
                {
                    // 더미 아이템이 참조하는 아이템이 장착상태인 경우 - 장착해제를 실행하고 레이캐스팅을 끝냅니다.
                    if( dummyInfo.EquipItemInfo.IsEquip )     
                    {
                        dummyInfo.EquipItemInfo.OnItemUnequip();
                                                
                        // 팀원 전용 상태 변화 코드 호출
                        if( dummyInfo.EquipItemInfo.EquipType==EquipType.Helmet )
                        {
                            if( playerStatus!=null )
                                playerStatus.isHood=false;
                        }

                        break;      
                    }
                    // 장착 상태가 아닌 경우(다음 레이캐스팅을 찾습니다.)
                    else
                    {
                        Debug.Log( "장착상태가 아닙니다." );
                    }

                }
                else
                    Debug.Log("어떠한 정보도 없습니다.");



            }

            Debug.Log($"현재 캐릭터 수치 HP:{playerHp} 배고픔:{playerHunger} 목마름:{playerThirsty}");


            StartCoroutine( MouseDelayTime(1f) );
            //animator.SetTrigger("Attack");
        }
        // 아이템 먹는 상황
        else if( Input.GetMouseButtonDown(1) && !playerInventory.IsOpen)
        {
            RaycastHit hitInfo;

            if( Physics.Raycast(clickRay, out hitInfo, Mathf.Infinity) )
            {
                if( hitInfo.collider.CompareTag("Item") )
                {
                    ItemInfo itemInfo = hitInfo.collider.GetComponentInChildren<ItemInfo>();
                    if(itemInfo!=null ) 
                        itemInfo.OnItemWorldGain(playerInventory);
                }
            }
        }




        // 플레이어가 인벤토리 창을 여는 상황
        if(Input.GetKeyDown(KeyCode.I))
        {
            playerInventory.InventoryOpenSwitch();
        }
        


        if(Input.GetKeyDown(KeyCode.E))
        {            
            RaycastHit hitInfo;

            if( Physics.Raycast(clickRay, out hitInfo, Mathf.Infinity) )
            {
                if( hitInfo.collider.CompareTag("Inventory") )
                {
                    InventoryInfo chestInventory = hitInfo.collider.GetComponentInChildren<InventoryInfo>();
                    playerInventory.ConnectInventory(chestInventory);
                }
            }
        }


        // 퀵슬롯 버튼을 누르는 상황
        if(Input.GetKeyDown(KeyCode.Alpha1))
            quickSlot.ItemEquipSwitch(1);

        if(Input.GetKeyDown(KeyCode.Alpha2))
            quickSlot.ItemEquipSwitch(2);

        if(Input.GetKeyDown(KeyCode.Alpha3))
            quickSlot.ItemEquipSwitch(3);
        
        if(Input.GetKeyDown(KeyCode.Alpha4))
            quickSlot.ItemEquipSwitch(4);

        if(Input.GetKeyDown(KeyCode.Alpha5))
            quickSlot.ItemEquipSwitch(5);

        if(Input.GetKeyDown(KeyCode.Keypad0))
            quickSlot.ItemEquipSwitch(5);

        // 마우스 우클릭을 누르는 상황
        // if(Input.GetMouseButtonDown(1))
        //    playerInventory.

        

    }


    /// <summary>
    /// itemStatus가 보유한 수치만큼 플레이어의 스테이터스 수치를 증감시키는 메서드입니다.<br/>
    /// 아이템 쪽에서 호출되는 용도로 사용
    /// </summary>
    public void VaryPlayerStatus(ItemStatus status)
    {
        playerHp += status.hp;
        playerHunger += status.hunger;
        playerThirsty += status.thirsty;
    }

    public void PrintDebugInfo()
    {
        string debugStr = string.Format(
            $"체력 : {playerHp}" +
            $"허기 : {playerHunger}" +
            $"갈증 : {playerThirsty}"
            );

        Debug.Log( debugStr );
    }




    /// <summary>
    /// 공격 마우스 버튼의 쿨타임 간격
    /// </summary>
    IEnumerator MouseDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        bMouseCoolTime = true;
    }

    // 애니메이션 이벤트 발생 시 호출
    public void Hit()
    {
        
    }

    /// <summary>
    /// 플레이어가 아이템을 습득하는 상황
    /// </summary>
    private void OnTriggerEnter( Collider other )
    {

        if( other.CompareTag( "Item" ) )
        {
            ItemInfo itemInfo = other.GetComponentInChildren<ItemInfo>();

            if( itemInfo!=null )
            {
                itemInfo.OnItemWorldGain( playerInventory );
            }
        }
    }

}
