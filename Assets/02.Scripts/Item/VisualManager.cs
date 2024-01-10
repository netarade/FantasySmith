using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldItemData;

/*
 * 
 * [�۾� ����]
 * 
 * <v1.0 - 2024_0108_�ֿ���> 
 * 1- ItemInfo ���� �ڵ� ������ ��κ� �Űܿ�. 
 * 
 * 
 * 2- �����ϰ� �ִ� 2D �̹����� ������ �ε��� �ѹ��� �����ϸ� 
 * �ش� �ε����� ���� �ν����� �� ���� 2D Sprite�̹��� �������� �����ִ� �޼��� �ۼ�
 * 
 * 3- CreateManager���� 3D ������Ʈ�� �����ϱ� ���� �ν����ͺ� �� �����س��� 3D ������ �������� �����ϴ� �޼���
 * 
 * 4- �ܺο��� Ư���� ���� ������ ����� �ʿ�����Ƿ�, CreateManagement ���ӽ����̽��� ���ϵ��� �Ͽ���.
 * 
 * 5- ������ ����
 * iicArr ������ - ivcArr
 * iicNum - ivcNum
 * iicIdx - ivcIdx
 * 
 * <v1.1 - 2024_0109_�ֿ���>
 * 1- �ּ��Ϻ� ����
 * 
 * <v1.2 - 2024_0110_�ֿ���>
 * 1- GetIVCIndex�޼��忡�� ������ ���� �ε������� �����Ǿ� ���� �ʴ��� ����
 * 
 * 
 * 
 */
namespace CreateManagement
{
    public enum SpriteType { innerSprite, statusSprite }             // � Sprite���� ������� ���� ����

    // �̹��� ���� �迭�� �ε��� ����
    public enum IVCType { MiscBase, MiscAdd, MiscOther, Sword, Bow, Axe }


    public class VisualManager : MonoBehaviour
    {
        /*** ������ �ܺ� ���� ���� ***/
        public ItemVisualCollection[] ivcArr;    // �ν����� �� �󿡼� ����� ������ �̹��� ���� �迭
         
        
        // �̹��� ���� �迭�� ����
        private readonly int ivcNum = 6;                                 


        void Awake()
        {
            // �ν����ͺ� �󿡼� ������ �޾Ƴ��� ��������Ʈ �̹��� ������ �����մϴ�.
            Transform imageCollectionsTr = transform.GetChild( 0 );

            // �迭�� �ش� ������ŭ �������ݴϴ�.
            ivcArr=new ItemVisualCollection[ivcNum];

            // �� iicArr�� imageCollectionsTr�� ���� �ڽĿ�����Ʈ�μ� ItemImageCollection ��ũ��Ʈ�� ������Ʈ�� ������ �ֽ��ϴ�
            for( int i = 0; i<ivcNum; i++ )
                ivcArr[i]=imageCollectionsTr.GetChild( i ).GetComponent<ItemVisualCollection>();
        }


        /// <summary>
        /// ���̷�Ű�信 ����ȭ ��ũ��Ʈ ������Ʈ�� �����Ǿ��ִ� IVC Collection ������Ʈ�� �� ��° �ε����� ���� �� ������
        /// ItemInfo�� ������� ���ϴ� �޼����Դϴ�.
        /// </summary>
        /// <returns>IVC Collection ������Ʈ�� �ε��� ���� ��</returns>
        private int GetIVCIndex(ItemInfo itemInfo)
        {
            ItemType itemType = itemInfo.Item.Type;
            Item item = itemInfo.Item;

            // �ν����� �信�� ������ IIC �ε��� ������ ����
            int ivcIdx = -1;            

            // �ν����� �信�� ������ �ε����� ���� ���� �������� �⺻Ÿ�� �� ����Ÿ�Կ� ���� ���մϴ�. 
            switch( itemType )        
            {
                case ItemType.Weapon:
                    ItemWeapon weapItem = (ItemWeapon)item;
                    WeaponType weaponType = weapItem.WeaponType;  // �������� ����Ÿ���� �����մϴ�.

                    switch( weaponType )
                    {
                        case WeaponType.Sword:             // ����Ÿ���� ���̶��,
                            ivcIdx=(int)IVCType.Sword;
                            break;
                        case WeaponType.Bow:               // ����Ÿ���� Ȱ�̶��,
                            ivcIdx=(int)IVCType.Bow;
                            break;
                        case WeaponType.Axe:               // ����Ÿ���� �������,
                            ivcIdx=(int)IVCType.Axe;
                            break;
                    }
                    break;

                case ItemType.Misc:
                    ItemMisc miscItem = (ItemMisc)item;
                    MiscType miscType = miscItem.MiscType;

                    switch( miscType )
                    {
                        case MiscType.Basic:           // ����Ÿ���� �⺻ �����,
                            ivcIdx=(int)IVCType.MiscBase;
                            break;
                        case MiscType.Additive:        // ����Ÿ���� �߰� �����,
                            ivcIdx=(int)IVCType.MiscAdd;
                            break;
                        default:                       // ����Ÿ���� ��Ÿ �����,
                            ivcIdx=(int)IVCType.MiscOther;
                            break;
                    }
                    break;
            }

            // ������ �ε����� �������� �ʾҴٸ�, ���ܸ� ������ �����Ǿ��ٸ� ��ȯ�մϴ�.
            if(ivcIdx==-1)
                throw new Exception("���� �ε������� ����� �������� �ʾҽ��ϴ�. Ȯ���Ͽ��ּ���.");
            else
                return ivcIdx;        
        }


        /// <summary>
        /// �������� Sprite�̹����� ���� ���� ������� ���մϴ�.<br/>
        /// � IVCType�� ������ ��������, ���� �ε���, ��������Ʈ ������ �����ؾ� �մϴ�<br/>
        /// </summary>
        /// <returns>�ش� �������� SpriteType�� ���� Sprite ���� ��</returns>
        public Sprite GetSpriteDirectByIVCIndex( IVCType ivcType, int refIdx, SpriteType spriteType )
        {
            if(spriteType == SpriteType.statusSprite)
                return ivcArr[(int)ivcType].vcArr[refIdx].statusSprite;
            else
                return ivcArr[(int)ivcType].vcArr[refIdx].innerSprite;
        }


        /// <summary>
        /// �������� Sprite�̹����� ���մϴ�.<br/>
        /// ������ ������ � ������ ��������Ʈ �̹����� ���� ������ �����ؾ� �մϴ�.<br/>
        /// </summary>
        /// <returns>�ش� �������� SpriteType�� ���� Sprite ���� ��</returns>
        public Sprite GetItemSprite( ItemInfo itemInfo, SpriteType spriteType )
        {
            // ������ ������ �������� (����ȭ ��ũ��Ʈ�� ��ϵǾ��ִ�) ���� �� ������Ʈ�� ���� �ε��� �ѹ��� ���մϴ�.
            int ivcIdx = GetIVCIndex(itemInfo);

            // ���������� ���ڷ� ���޵� ��������Ʈ ������ ���� �������� �����մϴ�.
            
            if(spriteType == SpriteType.innerSprite)
                return ivcArr[ivcIdx].vcArr[itemInfo.Item.VisualRefIndex.innerImgIdx].innerSprite;
            else
                return ivcArr[ivcIdx].vcArr[itemInfo.Item.VisualRefIndex.statusImgIdx].statusSprite;
        }



        /// <summary>
        /// ItemVisualCollection ����ȭ ������Ʈ�� �����Ͽ�<br/>
        /// ������ ���� �� 2D ������Ʈ�� ���� �� 3D ������Ʈ �������� �ҷ��ɴϴ�.<br/>
        /// ���ڷ� ���޹��� ItemInfo�� VisualRefIndex �ε��� ������ ���������� �����մϴ�.<br/>
        /// </summary>
        /// <returns>������ ���� �� �����ϰ��� �� 3D ������ ������</returns>
        public GameObject GetItemPrefab3D( ItemInfo itemInfo )
        {
            int ivcIdx = GetIVCIndex( itemInfo );

            return ivcArr[ivcIdx].vcArr[itemInfo.Item.VisualRefIndex.outerPrefabIdx].outerPrefab;
        }




    }
}