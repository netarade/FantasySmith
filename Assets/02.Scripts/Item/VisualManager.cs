using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldItemData;

/*
 * 
 * [작업 사항]
 * 
 * <v1.0 - 2024_0108_최원준> 
 * 1- ItemInfo 에서 코드 로직을 대부분 옮겨옴. 
 * 
 * 
 * 2- 저장하고 있는 2D 이미지를 참조할 인덱스 넘버만 전달하면 
 * 해당 인덱스를 통해 인스펙터 뷰 상의 2D Sprite이미지 참조값을 구해주는 메서드 작성
 * 
 * 3- CreateManager에서 3D 오브젝트를 생성하기 위해 인스펙터뷰 상에 저장해놓은 3D 프리팹 참조값을 전달하는 메서드
 * 
 * 4- 외부에서 특별한 일이 없으면 사용할 필요없으므로, CreateManagement 네임스페이스에 속하도록 하였음.
 * 
 * 5- 변수명 변경
 * iicArr 변수명 - ivcArr
 * iicNum - ivcNum
 * iicIdx - ivcIdx
 * 
 * <v1.1 - 2024_0109_최원준>
 * 1- 주석일부 보완
 * 
 * <v1.2 - 2024_0110_최원준>
 * 1- GetIVCIndex메서드에서 도끼에 대한 인덱스값이 설정되어 있지 않던점 수정
 * 
 * <v2.0 - 2024_0111_최원준>
 * 1- 아이템 클래스 추가 및 변경으로 인한 수정
 * IVCType과 ivcNum수정
 * GetIVCIndex메서드 내부 퀘스트 타입 추가
 * 
 * <v2.1 - 2024_0118_최원준>
 * 1- ItemType열거형의 Item.Weapon을 Item.Equip으로 변경하면서
 * GetIVCIndex메서드에서 ItemEquip의 케이스문에 ItemWeapon을 한번 더 검사할 수 있도록 하였음.
 * (취소 - Item.Equip을 다시 Item.Weapon으로 돌림 이유는 무기와 방어구를 같이 저장할 수 없기 때문)
 * 
 * 2- GetIVCIndex메서드내부 MiscType의 switch문을 삭제하고 if문으로 변경
 * (switch문에서 default에서 예외처리를 던지던 부분 삭제 - 
 * 현재 IVC가 Basic오브젝트 밖에 없으므로, Craft등의 서브타입이 생성되면 에러 발생)
 * 
 * 
 */
namespace CreateManagement
{
    public enum SpriteType { innerSprite, statusSprite }             // 어떤 Sprite값을 얻기위한 종류 정의

    // 이미지 집합 배열의 인덱스 구분
    public enum IVCType { Weapon, Quest, MiscBasic, MiscBuilding }


    public class VisualManager : MonoBehaviour
    {
        /*** 아이템 외부 참조 정보 ***/
        public ItemVisualCollection[] ivcArr;    // 인스펙터 뷰 상에서 등록할 아이템 이미지 집합 배열
        
        

        void Awake()
        {
            // 인스펙터뷰 상에서 하위에 달아놓은 스프라이트 이미지 집합을 참조합니다.
            Transform imageCollectionsTr = transform.GetChild( 0 );
            
            // 이미지 집합 배열의 갯수
            int ivcNum = imageCollectionsTr.childCount;

            // 배열을 해당 갯수만큼 생성해줍니다.
            ivcArr=new ItemVisualCollection[ivcNum];

            // 각 iicArr은 imageCollectionsTr의 하위 자식오브젝트로서 ItemImageCollection 스크립트를 컴포넌트로 가지고 있습니다
            for( int i = 0; i<ivcNum; i++ )
                ivcArr[i]=imageCollectionsTr.GetChild( i ).GetComponent<ItemVisualCollection>();
        }


        /// <summary>
        /// 하이러키뷰에 직렬화 스크립트 컴포넌트로 구성되어있는 IVC Collection 오브젝트의 몇 번째 인덱스를 참조 할 것인지
        /// ItemInfo를 기반으로 구하는 메서드입니다.
        /// </summary>
        /// <returns>IVC Collection 오브젝트의 인덱스 참조 값</returns>
        private int GetIVCIndex(ItemInfo itemInfo)
        {
            ItemType itemType = itemInfo.Item.Type;
            Item item = itemInfo.Item;

            // 인스펙터 뷰에서 참조할 IIC 인덱스 참조값 선언
            int ivcIdx = -1;            

            // 인스펙터 뷰에서 참조할 인덱스를 현재 들어온 아이템의 기본타입 및 서브타입에 따라서 구합니다. 
            switch( itemType )        
            {
                case ItemType.Weapon:
                    ivcIdx=(int)IVCType.Weapon;
                    break;

                case ItemType.Quest:
                    ivcIdx=(int)IVCType.Quest;
                    break;

                case ItemType.Misc:
                    ItemMisc miscItem = (ItemMisc)item;

                    if(miscItem.MiscType==MiscType.Building)
                        ivcIdx=(int)IVCType.MiscBuilding;       // 빌딩 재료
                    else
                        ivcIdx=(int)IVCType.MiscBasic;          // 나머지 기본 재료

                    break;

                    
            }

            // 참조할 인덱스가 수정되지 않았다면, 예외를 던지며 수정되었다면 반환합니다.
            if(ivcIdx==-1)
                throw new Exception("참조 인덱스값이 제대로 설정되지 않았습니다. 확인하여주세요.");
            else
                return ivcIdx;        
        }



        /// <summary>
        /// 아이템의 Sprite이미지를 직접 참조 방식으로 구합니다.<br/>
        /// 어떤 IVCType을 참조할 것인지와, 접근 인덱스, 스프라이트 종류를 전달해야 합니다<br/>
        /// </summary>
        /// <returns>해당 아이템의 SpriteType에 따른 Sprite 참조 값</returns>
        public Sprite GetSpriteDirectByIVCIndex( IVCType ivcType, int refIdx, SpriteType spriteType )
        {
            if(spriteType == SpriteType.statusSprite)
                return ivcArr[(int)ivcType].vcArr[refIdx].statusSprite;
            else
                return ivcArr[(int)ivcType].vcArr[refIdx].innerSprite;
        }


        /// <summary>
        /// 아이템의 Sprite이미지를 구합니다.<br/>
        /// 아이템 정보와 어떤 종류의 스프라이트 이미지를 구할 것인지 전달해야 합니다.<br/>
        /// </summary>
        /// <returns>해당 아이템의 SpriteType에 따른 Sprite 참조 값</returns>
        public Sprite GetItemSprite( ItemInfo itemInfo, SpriteType spriteType )
        {
            // 아이템 정보를 바탕으로 (직렬화 스크립트로 등록되어있는) 참조 할 오브젝트의 계층 인덱스 넘버를 구합니다.
            int ivcIdx = GetIVCIndex(itemInfo);

            // 마지막으로 인자로 전달된 스프라이트 종류에 따라서 참조값을 전달합니다.
            
            if(spriteType == SpriteType.innerSprite)
                return ivcArr[ivcIdx].vcArr[itemInfo.Item.VisualRefIndex.innerImgIdx].innerSprite;
            else
                return ivcArr[ivcIdx].vcArr[itemInfo.Item.VisualRefIndex.statusImgIdx].statusSprite;
        }



        /// <summary>
        /// ItemVisualCollection 직렬화 오브젝트를 참조하여<br/>
        /// 아이템 생성 시 2D 오브젝트에 부착 할 3D 오브젝트 참조값을 불러옵니다.<br/>
        /// 인자로 전달받은 ItemInfo의 VisualRefIndex 인덱스 정보를 내부적으로 참조합니다.<br/>
        /// </summary>
        /// <returns>아이템 생성 시 복제하고자 할 3D 프리팹 참조값</returns>
        public GameObject GetItemPrefab3D( ItemInfo itemInfo )
        {
            int ivcIdx = GetIVCIndex( itemInfo );

            return ivcArr[ivcIdx].vcArr[itemInfo.Item.VisualRefIndex.outerPrefabIdx].outerPrefab;
        }




    }
}