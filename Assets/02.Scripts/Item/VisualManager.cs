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
 * <v2.0 - 2024_0111_�ֿ���>
 * 1- ������ Ŭ���� �߰� �� �������� ���� ����
 * IVCType�� ivcNum����
 * GetIVCIndex�޼��� ���� ����Ʈ Ÿ�� �߰�
 * 
 * <v2.1 - 2024_0118_�ֿ���>
 * 1- ItemType�������� Item.Weapon�� Item.Equip���� �����ϸ鼭
 * GetIVCIndex�޼��忡�� ItemEquip�� ���̽����� ItemWeapon�� �ѹ� �� �˻��� �� �ֵ��� �Ͽ���.
 * (��� - Item.Equip�� �ٽ� Item.Weapon���� ���� ������ ����� ���� ���� ������ �� ���� ����)
 * 
 * 2- GetIVCIndex�޼��峻�� MiscType�� switch���� �����ϰ� if������ ����
 * (switch������ default���� ����ó���� ������ �κ� ���� - 
 * ���� IVC�� Basic������Ʈ �ۿ� �����Ƿ�, Craft���� ����Ÿ���� �����Ǹ� ���� �߻�)
 * 
 * 
 */
namespace CreateManagement
{
    public enum SpriteType { innerSprite, statusSprite }             // � Sprite���� ������� ���� ����

    // �̹��� ���� �迭�� �ε��� ����
    public enum IVCType { Weapon, Quest, MiscBasic, MiscBuilding }


    public class VisualManager : MonoBehaviour
    {
        /*** ������ �ܺ� ���� ���� ***/
        public ItemVisualCollection[] ivcArr;    // �ν����� �� �󿡼� ����� ������ �̹��� ���� �迭
        
        

        void Awake()
        {
            // �ν����ͺ� �󿡼� ������ �޾Ƴ��� ��������Ʈ �̹��� ������ �����մϴ�.
            Transform imageCollectionsTr = transform.GetChild( 0 );
            
            // �̹��� ���� �迭�� ����
            int ivcNum = imageCollectionsTr.childCount;

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
                    ivcIdx=(int)IVCType.Weapon;
                    break;

                case ItemType.Quest:
                    ivcIdx=(int)IVCType.Quest;
                    break;

                case ItemType.Misc:
                    ItemMisc miscItem = (ItemMisc)item;

                    if(miscItem.MiscType==MiscType.Building)
                        ivcIdx=(int)IVCType.MiscBuilding;       // ���� ���
                    else
                        ivcIdx=(int)IVCType.MiscBasic;          // ������ �⺻ ���

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