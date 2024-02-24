using UnityEngine;
using InventoryManagement;
using DataManagement;
using ItemData;
using System;
using System.Collections.Generic;
using CreateManagement;
using UnityEngine.UI;
using Unity.VisualScripting;

/*
 * [�۾� ����]  
 * <v1.0 - 2023_1106_�ֿ���>
 * 1- �ʱ� Ŭ���� ����
 * ���۰��� �������� �����ϵ��� ����, �̱������� ���ټ� ����
 * <v1.1 - 2023_1106_�ֿ���>
 * 1- ���۸���� List���� Ŭ������ ���� �������� ���� ����
 * 2- �ּ�����
 * 
 * <v1.2 - 2023_1106_�ֿ���>
 * 1- Start�� ������ OnEnable�� ����. 
 * �ٸ� ��ũ��Ʈ���� Start�������� instance�� �����Ͽ� ������ �޾ư��� �ֱ� ����.
 * 2- ����ȭ �ȵǴ� ������ �߻��Ͽ� �ٽ� OnEnable���� Start�� ����.
 * 
 * <v2.0 - 2023_1107_�ֿ���>
 * 1- ���� �ʱ�ȭ�� �̷������ ��ũ��Ʈ���� ������ �޾ư��Ƿ� Awake������ �ű�.
 * �븮�ڸ� �̿��Ͽ� �������� �ּ�ȭ�Ͽ� ������� �ʱ�ȭ�� �̷�������� ��. 
 * 
 * <v2.1 - 2023_1108_�ֿ���>
 * 1- �������� ���� �Ѿ�� �ı��Ǳ� ������ ��ũ��Ʈ�� �����Ϸ��� �Ͽ����� ��ũ��Ʈ ���� �ı��Ǵ� ���� �߻�
 * => ��ųʸ��� �̿��ؼ� �̸��� ������ �����ϰ� ���Ӱ� �����ϴ� ��ü ������� ����. 
 * 
 * 2- �� �ε� �� ������ �ϳ��� ��Ḹ ����ִ� ���� �߻�
 * => �������ʿ��� �ı��ɋ����� ��ųʸ��� �����߾��� ������ CraftManager�ʿ��� �������� �ѹ� �̸� �������ֵ��� ����
 * 
 * <v3.0 - 2023_1120_�ֿ���>
 * 1- CraftManager Ŭ���� �� ���ϸ� ���� PlayerInven, ������ġ �̵� Common���� -> Player����
 * 
 * 2- PlayerInven Ŭ���� ���� ����
 * a. �÷��̾��� �κ��丮 �����͸� �����ϴ� ����
 * b. ���ӽ��� �� �������� �� �÷��̾� �κ��丮�� �ε� �� ���̺����ָ�, �� ��ȯ�� �κ��丮�� �����ϰ� ����� �Ѵ�.
 * c. ���� �÷��̾� ������Ʈ�� ��ũ��Ʈ�� �پ��־�� �Ѵ�. (�÷��̾ ����ִ� �����͸����� �� ���̴�.)
 * d. ������Ʈ�� �ı��� ��(���� ���� ��)
 * 
 * 3- weapList, miscList�� ���� (inventory�ʿ� ����� �߰��Ͽ� ���ټ��� ���� ��ȹ�̹Ƿ�)
 * 4- ��Ÿ ������ ���� �� �ּ�ó�� (�̿Ϸ�)
 * 5- UpdateInventoryText �޼��� �ּ�ó�� - inventory�ʿ� �ش� ����� �߰� �� �����̹Ƿ�
 * 
 * <v3.1 - 2023_1122_�ֿ���>
 * 1- �κ��丮 �� ���û��� �ּ� ���� ����
 * 
 * <v4.0 - 2023_1216_�ֿ���>
 * 1- slotListTr�׸��� �߰� - CreateManager�� PlayerInven ��ũ��Ʈ�� �����Ͽ� ������ų �ʿ伺 ����
 * 2- �̱��� ���� - �ٸ� ��ũ��Ʈ�� start ���� ��� ������ �� �� �̱����� �׺��� �� ������ �ʱ�ȭ���� ������ �ȵǴµ�
 * OnSceneLoad�� �ʱ�ȭ�� �ϴٺ��� ������ ����
 * 
 * <v4.1 - 2023_1217_�ֿ���>
 * 1- ��ũ��Ʈ ����ȭ, LoadPlayerData, SavePlayerData �޼��� �߰��Ͽ� 
 * Awake,OnDestroy,OnApplicationQuit���� ȣ��ǵ��� ����
 * 
 * 2- ���ӸŴ��� �̱��濡�� isNewGame���¸� �о�ͼ� ���ο� �κ��丮�� �����ϴ� ���� �߰�
 * 
 * <v4.2 - 2023_1221_�ֿ���>
 * 1- OnApplicationQuit �޼��峻�ο� �����Ϳ� ���α׷� ���� ������ �־��� �� ���� 
 * 2- ���ӸŴ����� �ν��Ͻ��� isNewGame�� �Ǻ��ϴ� ������ PlayerPrefs�� Ű�� ������ ����
 * ������ PlayerInven ��ũ��Ʈ�� Awake�� ���� �ʱ�ȭ�� �ؾ��ϴµ� �̱��������� ���ٽ����� ���Ƽ� �ҷ����� ��Ʊ� ���� 
 * 3- �׽�Ʈ�� Ű���� ���� PlayerPrefs.DeleteAll �߰�
 * 
 * <v5.0 - 2023_1222_�ֿ���>
 * 1- Ŭ������ ���ϸ��� PlayerInven���� InventoryInfo�� ���� 
 * (ItemInfo�� �̸��� �ϰ����� ���߱� ����)
 * 
 * <v5.1 - 2023_1222_�ֿ���>
 * 1- Awkae������ PlayerPrefs�� Ű���� ���� ó�����۰� �̾��ϱ⸦ �����ؼ� �޼��带 ȣ���ϴ� ������ �����ϰ�
 * LoadData�ϳ��� ȣ���ص� ó�� �����Ͱ� ����������� �����Ͽ����ϴ�.
 * 
 * <v5.2 - 2023_1224_�ֿ���>
 * 1- InitPlayerData() �޼��� ����
 * 2- LoadPlayerData() �޼��带 Start������ �ű�
 * ������ �κ��丮�� ������ȭ �� �� ���������� CreateManager�� �̱����� �޼��带 �ҷ����� ������ ȣ������� ���� �ʿ䰡 ����
 * 
 * <V5.3 - 2023_1226_�ֿ���>
 * 1- �κ��丮 �ε�� LoadAllItem�޼��忡�� UpdateItemInfo�޼��� ȣ��� ����
 * <v5.4 - 2023_1226_�ֿ���>
 * 1- �κ��丮 ���̺� �ε�޼��带 �Ϲ�ȭ �޼��� ȣ��� ����
 * 
 * <v6.0 - 2023_1229_�ֿ���>
 * 1- craftDic, gold, silver�� �������� ���� ���� �� ���� �ε嵵 inventory�� �ҷ������� ����
 * 
 * <v6.1 - 2023_1230_�ֿ���>
 * 1- slotListTr���� �߰� - �κ��丮�� �ڽ��� ���Ը�� �ּҸ� �����ϵ��� �Ͽ���
 * 
 * 2- InventoryInteractive���� ���� �������� �����ϴ� �ڵ带 �Űܿ�
 * ������ ������ �ø��ų� ���̰ų� �ϴ� �޼��带 �����, ������ �ݿ��ϱ� ����
 * 
 * 
 * <v6.2 - 2024_0101_�ֿ���>
 * 1-Inventory�� AddItem, RemoveItem�޼��带 �߰� (���� ���Ǵ� �߰�����)
 * ���� �κ��丮�� ����ó���ϰ�, ����� ������ �ֱ�����
 * 
 * 2-Inventory�� FindNearstSlotIdx�޼��� �߰�
 * (ItemInfo���� �ڽ��� ���� �κ��丮�� ���� ����� ���� �ε����� ��ȯ�ޱ� ���� �ʿ�)
 * 
 * 3- isSlotEnough�޼��� �߰�
 * ItemInfo���� �κ��丮 ������ ������Ʈ �ϱ� ������ ���� �� ������ �ִ����� Ȯ���ϰ� �����ϴ� ������ �ʿ�
 * 
 * 
 * (���� ����)
 * 1- �κ��丮�� ������ �����Ǿ����� �ݿ��ϴ� �̺�Ʈ ���� �� ȣ��
 * �κ��丮 ������ �����ϴ� Interactive��ũ��Ʈ���� ����� �� �ֵ��� �ϱ� ���Ͽ�
 * 
 * 
 * 
 * 
 * <v7.0 - 2024_0103_�ֿ���>
 * 1- AddItem, RemoveItem, CreateItem�޼��� Info2Ŭ������ ����ó��
 * 
 * 2- DataManager �ν��Ͻ� ������Ŀ��� ������Ʈ ���� ������� ����
 * (��Ƽ���� ��ũ��Ʈ���� �ν��Ͻ� ������ ����)
 * 
 * 3- �����ε��޼��� isLatestReduce �������� isLatestModify�� ���� (InventoryŬ������ ����)
 * 
 * 4- UpdateAllItemInfo�޼��� ���Ӱ� ����
 * �ε� �� ��� �������� ������Ʈ ������ �ֽ�ȭ�ϱ� ���� ȣ��
 * 
 * <7.1 - 2024_0104_�ֿ���>
 * 1- IsItemEnough���� ItemPair ����ü�ϳ��� �޴� �����ε� �޼��� ����
 * (������Ʈ 1���� �̸��� ������ �����ϴ� �޼��尡 ���� �ֱ� �����̰�, 
 * AddItem�޼��忡�� ItemPiar�迭�� ItemPair�� �޴� ����ü�� �߰��ϴٺ��� �ڵ尡 ������� ����)
 * 
 * <v7.2 - 2024_0105_�ֿ���>
 * 1- ������ �ϳ��� ������ ������Ʈ�ϴ� UpdateItemInfo�޼��� ����
 * ������ �߰� �� ���� ���� ������ �Ͼ �� ItemInfo ������ ���� ������Ʈ �޼��带 ȣ���ϸ�Ǹ�,
 * ���԰� ����� itemInfo ������ ������Ʈ�� ȣ������ ���̱� ����
 * 
 * 2- ��ŸƮ���� �κ��丮�� �ε��� �� Deserialize�޼��� ȣ�� �� createManager�������� �����ϵ��� ����
 * �̴� �κ��丮 Ŭ������ ���� createManager�� ã�� �� ���� ����
 * 
 * 3- UpdateAllItemInfo�޼��忡��
 * �ʱ� ��ųʸ��� ���� ���� ��쿡�� Update�޼��带 ȣ���ϴ� ���� ����
 * 
 * 4- IsSlotEnough���� ItemType�� �޴� �����ε� �޼��带 IsSlotEnoughIgnoreOverlap�κ���,
 * itemName�� overlapCount�� �޴� �޼��带 IsSlotEnough�� ����
 * ItemInfo�� �����޴� IsSlotEnough�߰�
 * 
 * 5- SetItemSlotIdxBothToNearstSlot���ο��� IsSlotEnough�� ItemType��� ȣ�⿡�� ItemInfo��� ȣ��� ����
 * -> ��ȭ�������� ��� �����ε����� �ʿ����� ���� ��찡 �ִ�.
 * 
 * (�̽�)
 * ���� �������� �ְ� �ε��� ������ �־��ִ� ��������
 * �ε��� ������ ���� ���ؼ� �־��ְ�, �������� �־��ִ� ������ �����ϰ� �Ǿ���.
 * ���� ��ȭ�������� �ε��� ������ ���� ���� �󽽷� �������� ���ϰ� �Ǵµ�,
 * �̸� ���� ���Կ� ������ ������ ���θ� Ȯ���ϰ� ������ ��á�� �� -1�� ��ȯ���� �ʵ��� �ؾ� �Ѵ�.
 * => (����) ��ȭ �������� ó��if������ SlotEnough���� ����Ͽ� �ε��� ������ -1�� ������
 * AddItem�� �� �ı��ǹǷ� �������δ�.
 * 
 * <v7.3 - 2024_0108_�ֿ���>
 * 1- �ν����ͺ� �󿡼� �����ϴ� �ɼ��� baseDropTr, isBaseDropSetParent ���� �߰�
 * ����ڰ� �����ġ�� �巡�׾� ������� �����Ͽ� OnItemWorldDrop�޼��带 ���� ���� ȣ�� �� ���ϰ� ����ϵ��� �ϱ�����.
 * 
 * <v7.4 - 2024_0109_�ֿ���>
 * 1- IsSlotEnough�ּ� ����
 * 
 * <v8.0 - 2024_0110_�ֿ���>
 * 1- ���̺� ���� �̸��� Awake������ �ѹ� ���������� ���� �ε� ���������� �ٽ� �ʱ�ȭ�Ǿ��ִ� ������ �־
 * ���̺�, �ε� �޼��� ȣ�� �ٷ� ���� FileSettings�� ȣ�����ִ� ������ ����
 * 
 * 2- ���̺� ���� �̸��� �κ��丮�� �ֻ��� �θ������Ʈ��+Inventory�� ���� 
 * 
 * 3- �κ��丮 â�� Ȱ��ȭ ���θ� �˷��ִ� ������ ������Ƽ IsWindowOpen ���� �� 
 * UpdateOpenState�� interactive Ŭ�������� ���������� ȣ���ϵ��� ����
 * 
 * 4- LoadPlayerData�޼��带 Start���� �ƴ϶� Awake�� ȣ��� ����.
 * LoadData�޼��� ���ο��� ȣ��Ǵ� UpdateAllItemInfo�� Awake�� �ܺη� ��������.
 * 
 * 5- ���� �� UpdateAllItemInfo�� UpdateAllItemVisualInfo�� �����Ͽ���.
 * 
 * 
 * 5- ������ ������ interactive ��ũ��Ʈ�� �±����μ� �޼��� ȣ����踦 ������
 * infoŬ���� �κ��丮 ���� �ε� ����-> interactive Ŭ�������� ���� ����-> �������� ����� ���Կ� ǥ��
 * (��, �ε� ���� ������ �����ؾ� �ϸ�, ������ ���� �� �� �������� ����� ǥ���ؾ� �ϱ� ����)
 * 
 * 6- interactive ��ũ��Ʈ�� ȣ���� �ݵ�� Info��ũ��Ʈ���� �ϵ��� ����
 * (interactive ��ũ��Ʈ�� �̺�Ʈ ������� �����ϱ� ������ Awake�� �ʱ�ȭ�� �ʿ���� ���� + Info��ũ��Ʈ �������� �ذ�)
 * 
 * 7- Load�޼��带 Awake���� Start�� �����Ͽ���.
 * ������ �ε� �ϸ鼭 �ٸ� ��ũ��Ʈ�� �޼��带 ȣ���ϴµ� �ʱ�ȭ�� �̷������ �ʾƼ� ������ �߻��ϱ� ����
 * (DataManager�� Path, CreateManager�� itemPrefab3D, VisualManager ��..)
 * 
 * <v8.1 - 2024_0112_�ֿ���>
 * 1- ������ ������Ƽ�� isWindowOpen�� isOpen���� ����
 * 
 * <v8.2 - 2024_0113_�ֿ���>
 * 1- ���ü��� ������ isOpen ������ InventoryInfo_3.cs�� ����
 * 
 * 2- UpdateOpenState�޼��� ����
 * InteractiveŬ������ InventoryOpenSwitch�޼��带 ���� Inventory_3.cs�� �ű� �����
 * �� �̻� interactive���� isOpen������ ������Ʈ ���� �ʿ䰡 ���ԵǾ���.
 * 
 * <v9.0 -2024_0114_�ֿ���>
 * 1- �ڱ� Ʈ������ ĳ�� inventoryTr���� �߰� �� slotListTr public ����, emptyList�߰�
 * 
 * 2- Awake���� Inventory_3.cs ���� ���� �ʱ�ȭ�� ����
 * 
 * 3- ��ӽ�ũ��Ʈ QuickSlot�� �����ϰ� ���üӼ��� ��ӹޱ����� private ������ �޼��带 protectedó��
 * 
 * 4- OnDestroy���� Save�ϴ� �ڵ带 ����
 * ������ �ı��� �������� ������Ʈ�� �ϳ��� �����ͼ� �����Ϸ��� �ϸ� �̹� ������Ʈ�� �ı��Ǿ��� ������ ������ �߱� ����
 * (�� ��ȯ ��ư�� ������ ������ Save�� ȣ������� ��)
 * 
 * 5- �޼���� SetItemSlotIdxBothNearst�� SetItemSlotIndexBothLatest�� ���� �� InventoryŬ������ �ű�
 * ������ AddItem �� �� �ѵ������� �̷������ �ϸ�, ���� inventory���� �ۿ� ����� �� ���� ����. 
 * 
 * 6- IsSlotEnough�� ���� �����ε����� �޾Ƽ� Ȯ���ϴ� �����ε� �޼��� �߰�
 * 
 * 7- IsSlotEnoughIgnoreOverlap�޼��� ����
 * 
 * <v9.1 -2024_0115_�ֿ���>
 * 1- interactive ���� protected���� public���� ����
 * ������ �� infoŬ�������� ������ �����Ͽ� actvieTab���� ������ ����
 * 
 * 2- FindNearstRemainSlotIdx�޼��� ����
 * �ε��� ������ ���� inventory���� ó���ؾ� ��Ȯ�ϱ� ����
 * 
 * 3- IsSlotEnough �������� �ε����� ���ڷ� �޴� �����ε� �޼������ IsSlotEnoughCertain�� ����
 * 
 * <v9.2 - 2024_0115_�ֿ���>
 * 1- IsSlothEnoughCertain �޼��带 IsSlotEmpty�� ����, IsSlotEnough �ּ� ����
 * 
 * <v9.3 - 2024_0116_�ֿ���>
 * 1- UpdateAllItemVisualInfo�޼��� ���� for������ ItemType ��ü��ŭ �ݺ��ϸ� i�� ���� ����־� ������ ��ȯ�޴� �ڵ带
 * dicLen��ŭ �ݺ��Ͽ� dicType[i]�� ���� ������ ��ȯ�޵��� ����
 * (=> �������� ������ ������ ��� ������ �����ϰ� �־����� ���� ���� �� �ʿ��� ������ �����ϴ� ���·� �����Ͽ����Ƿ�)
 * 
 * <v9.4 - 2024_0116_�ֿ���>
 * 1- UpdateDicItemPosition�޼��忡�� GetItemDic���� null�� �˻繮 �߰�
 * 2- UpdateAllItemVisualInfo�޼��忡�� GetItemDic���� null�� �˻繮 �߰�
 * 
 * <v10.0 - 2024_0124_�ֿ���>
 * 1- InventoryŬ������ ��ųʸ� ���������� GameObject��ݿ��� ItemInfo�� �����ϸ鼭 ���� �޼��� ����
 * (UpdateAllItemVisualInfo, UpdateDicItemPosition)
 * 
 * 2- ownerTr������ OwnerId������Ƽ�� �����Ͽ� �κ��丮�� ������ �θ������Ʈ���� ID�� �ο����� �� �ְ� �Ͽ���.
 * (AddItem,RemoveItem��� �����ۿ� �����ָ� �����ϰ� �ϴ� �뵵�� ���)
 * 
 * 3- �޼���� ���� SavePlayerData LoadPlayerData -> SaveOwnerData LoadOwnerData 
 * 
 * 
 * 4- Awake������ saveFileName�� �ʱ�ȭ�� �� inventoryTr�� ���������� ���� �����Ͽ� name�� �޾Ƽ� �ʱ�ȭ�ϴ� �κ���
 * OwnerId�� ȣ���Ͽ� �ʱ�ȭ�ϴ� ������ ����
 * 
 * 5- InventoryInfo�� IsWorldPositioned�Ӽ��� �߰�
 * Initializer���� IsWorldPositioned���� �޾Ƽ� InventoryInfo�� IsWorldPositioned�Ӽ��� �����ǵ��� �Ͽ���.
 * (����� �������� �κ��丮���� ���θ� ����)
 * 
 * 6- UpdateViusalizationInfo�޼��带 IsWorldPositioned �Ӽ��� �ƴ� ��쿡�� ȣ���ϵ��� ����
 * ������ ���� �������� ��� 2D �̹����� ������ �ʿ䰡 ������, 3D������Ʈ�� �������ִ� �޼��尡 ȣ��Ǿ����� �ϱ� ����
 * 
 * <v10.1 - 2024_0124_�ֿ���>
 * 1- �޼���� ���� SaveOwnerData LoadOwnerData -> SaveInventoryData LoadInventoryData
 * 
 * 2- IsWorldPositioned �������� isItem3dStore�� �����Ͽ���.
 * ������ ���忡 �������� �Ӽ��� �ƴ϶� 3D������ �������� �����ϴ� ������ �ϴ� �κ��丮�̱� ����
 * 
 * 3- OwnerId�� Ÿ���� string���� int������ ���� �� �ּ� ����
 * ������ ������� id�� �������� �ο��ϱ� ����
 *
 * 4- UpdateAllItemTransformInfo�޼��带 �߰�.
 * 3d������ �������� �����ϴ� �κ��丮�� ��� �ε�Ǹ� �ش� ��ġ������ �������� �Ű��־�� �ϱ� ����.
 * 
 * 5- �������� �����Ǵ� �κ��丮���� �Ǵ��ϴ� isInstantiated������ �߰��ϰ� initializer�� ���� �ʱ�ȭ�� ����
 * 
 * 6- ���������̸��� ��Ÿ���� ������ saveFileName�� �ʱ�ȭ�� ���� �ֻ��� ������Ʈ��� "Inventory"�� �ٿ��� �����ϴ� ����
 * �ֻ��� ������Ʈ��� initializer�� inventoryId�� ���޹޾� �ʱ�ȭ�ϴ� ������� ����.
 * (������ ������Ʈ ���� ������ ���ֱ� ����)
 * 
 * <v10.2 - 2024_0125_�ֿ���>
 * 1- �б����� ������Ƽ OwnerName�� �߰�
 * ������ �������� OwnerName�� �����ϰ� �־�� IdData�� Ű������ Id�� ������ ���������� ����
 *
 * 2- public���� inventory�� interactive�� HideInInspector ��Ʈ����Ʈ �߰�
 * 
 * 3- UpdateAllItemTransformInfo�޼������ LoadAllItemTransformInfo�� ����
 * Save�޼��嵵 ���� ������̱� ����
 * 
 * 4- LoadAllItemTransformInfo �޼��带 WorldInventoryInfo�� �ű�
 * ������ 3D�������� �����ϴ� �κ��丮 �������� ȣ������� �� �޼����̱� ����
 *
 *<11.0 - 2024_0126_�ֿ���>
 * 
 * 1- �κ��丮 �˻� �� ������� �޼��� (SetItemOverlapCount, IsItemEnough, IsSlotEnough, IsSlotEmpty)�� InventoryInfo_2.cs�� �ű�
 * 
 * 2- �κ��丮 ������ ���� UserInfo Ŭ������ ���� �� �����Ͽ� ���� ���� Id�� �ʱ�ȭ�ϰ�, �̸� ���� ���̺� ���ϸ��� ������ �� �ְ� �Ͽ���.
 * 
 * 3- (IsInstantiated�Ǿ�����) �������� �κ��丮�� ��� UserTr�� Ư���� �� ������, UserInfo�� ���� ������
 * UpdateItemInventoryInfo�޼��尡 �ܺο��� ȣ��Ǿ ������ �ĺ���ȣ (ownerID)�� �������� �ְ� ����.
 * => ��, �������� �κ��丮�� ��� �����ڴ� ������ 3D������Ʈ�̰�, ������ �ĺ���ȣ�� ������ 2D ��ũ��Ʈ�� ItemInfo�� ����Ǿ��ִ� ���� �ĺ���ȣ�� �ȴ�. 
 *
 *4- isItem3dStore, isInstantiated�Ӽ��� ����
 *
 *<v11.1 - 2024_0126_�ֿ���>
 *1- SwitchAllItemAppearAs2D�޼��带 �ۼ��Ͽ� �κ��丮 â Off�� ���������� 2D ����� �ߴ��ϵ��� ����
 *
 *<v11.2 - 2024_0128_�ֿ���>
 *1- �ִϸ��̼��� ���̰� ��� �� ���Ŀ� �κ��丮 â�� �������� Animation������Ʈ �迭�� WaitForSeconds ���� �߰�
 *
 *<v11.3 - 2024_0129_�ֿ���>
 *1 - ��ü ���� ���� ���θ� ��ȯ�ϴ� IsShareAll ������Ƽ �߰�
 *ItemInfo���� �����Ͽ� ���� �ε��� ������ Ȱ��
 *
 *
 *<v11.4 - 2024_0130_�ֿ���>
 *1- GridLayoutGroup�� cellSize�� ��ȯ�ϴ� cellSize������ ������Ƽ�� �߰�
 *DummyInfo ��ũ��Ʈ���� �����ϱ� ����
 *
 *2- UpdateAllItemVisualInfo�޼��忡�� ���� ���� �������� 3Dǥ���ϴ� �ڵ带 �߰� (OnItemEquip�޼��� ȣ��)
 *
 *
 *<v11.5 - 2024_0131_�ֿ���>
 *1- UpdateAllItemVisualInfo�޼��忡��
 *���� ����������Ʈ �÷��̾� ���¸޼��� �̱������� ���� ���� ���� �ʰ� ���� ���¸� �����ϵ��� ����
 *
 *<v11.6 - 2024_0210_�ֿ���>
 *1- ������ inventory, interactive, initializer�� �б����� ������Ƽ�� �����ϰ� �빮�ڷ� ���� ��, protected ���� ������ �ҹ��ڷ� ���Ӱ� ������� 
 *
 *2- SaveInvetoryData, LoadInvetoryData�� virtualŰ���带 �־� ��� �� �������̵� �����ϰ� �ϰ�,
 *int slotNo�� ���ڷ� �޴� �޼���� �����Ͽ� ������ ���� ��ȣ�� �Է¹ް� �Ͽ���.
 *
 *3- InitOpenState�޼����� ȣ���� �κ��丮 �ε� ���ķ� ���� (Awake���� Start�� ����)
 *������ ���������� �Ҵ�� ������ ����Ͽ� null���۷����� ������ �����ε� �̴� �κ��丮 ���� ��� �������� 2D ���¸� ��Ȱ��ȭ�ϱ� ����
 *
 *4- loadSlotNo�� SaveLoadManager���� ������ PlayerPrefs�� SlotNo Ű���� �޾ƿͼ� �����ϵ��� ����
 *
 *5- OnApplicationQuit�� �����ϴ� ���̺� �ڵ带 �����ϰ� SaveLoadManager�� �̺�Ʈ �ڵ鷯�� �����Ͽ� ȣ���ϵ��� ����
 *
 *6- OnDestroy���� �̺�Ʈ ������ ���̺� �ڵ带 ���� �����ϵ��� ����
 *������ ���� ���� ȭ�鿡�� �ε��� �� ���� ���� �ε�Ǵ��� ���� ������Ʈ�� �ı��ǰ� ���ο� ������Ʈ�� �����Ǳ� ������
 *static Event�� ���� ������Ʈ�� �ڵ尡 ����� ���°� �Ǳ� ����
 *
 *<v11.6 - 2024_0216_�ֿ���>
 *1- UpdateAllItemVisualInfo�޼��忡�� ���� ���¸� ������ �����ϴ� �ڵ带 �����ϰ�, ���� ���� �������� EquipSwitch �޼��带 ȣ���ϵ��� ����
 *(���� ���� �޼��带 �����ϸ鼭 IsEquip�����̱⸸ �ϸ� �ش� �޼��带 ȣ�����ָ� �ε�� �� �ֵ��� �����Ͽ���.
 *������ �ʿ��� ���� ���¸� �����ϸ鼭 ���� ���� ���¸� ������ ���� ���� �� �ֵ��� �Ͽ���.) 
 *
 *
 * <v11.7- 2024_0218_�ֿ���>
 * 1- IsAbleToAddAnotherInventoryItem �ű� �޼��� �߰� - ItemInfo�� ���ڷ� �޾� �ٸ� �κ��丮������ ���� �̵����ɿ��θ� �Ǵ��ϴ� �޼���
 * �ش� �������� ���� �������� ���ο� ���ü��� ������� �ʴ� ���(�� �� ���� ���� Ȱ��ȭ�� ���������� �������� ��ġ���� �ʴ� ���)
 * �� �˻����ִ� �ڵ带 �߰��Ͽ���
 * 
 * <v11.8 - 2024_0220_�ֿ���>
 * 1- IsAbleToAddAnotherInventoryItem�޼����� ���ü� ���� �����߰�
 * ������ ��ü���� ��쿡, ��ü��->������������ �̵� �� 
 * �������� �ű� �κ��丮�� �����ǿ� ���� �ʴ� ������ �������̶��, ���ü��� ������� �����Ƿ� �����ؾ� ��
 * (������->��ü���� ���ü��� ����ǹǷ� �ݵ�� ���)
 * (�̴� ���� â�� �Ǵ� ���۴��κ��丮����->�����κ��丮���� �̵� �� �ش�Ǵ� ���)
 *  
 * <v11.9 - 2024_0222_�ֿ���>
 * 1- SwitchAllItemAppearAs2D�޼����� �Ű������� isEnable2D�� isOperateAs2D�� ����
 * 
 * 2- SwitchAllItemAppearAs2D�޼����� �������ڷ� ���� �������θ� �߰�
 * 
 * 3- �޼���� SwitchAllItemAppearAs2D�� AllItemOperateSwitchAs2d�� ����
 * 
 * 4- AllItemOperateSwitchAs2d�޼��� ���ο� ���������� �������� ��� ���������� OperateSwitchAs2d�޼��带 ȣ���ϵ��� �б��ڵ� �ۼ�
 *
 * <v12.0 - 2024_0223_�ֿ���>
 * 1- SlotListTr�б����� ������Ƽ�� �߰�
 * ������ �������� �� �Ͻ������� ���� ������ ����� ������ �ε����� ������ ���Կ� �����ϱ� ����
 * 
 * 2-�޼���� IsAbleToAddAnotherInventoryItem -> IsAbleToSlotDrop���� ����
 *
 * <v12.1 - 2024_0224_�ֿ���>
 * 1- GetSlotItemInfo �űԸ޼��� �߰�
 * �κ��丮 ������ ���Կ� ��� ������ ������ ��ȯ�ϴµ�, 
 * ��� ��ũ��Ʈ(������)�� ��� �������̵� �Ǿ� ���̰� �ƴ� ���� ������ ������ ��ȯ�� �� �ֵ��� virtual�� ����
 *
 * 2- IsAbleToSlotDrop�޼��带 virtual�� �����Ͽ� �ڽ� ��ũ��Ʈ(������)��� �߰��� ���� �˻縦 ������� ��ģ ������ ��ȯ���� �� �ֵ��� ����
 * => ������ ���� ��ӽ� ���ǰ˻縦 �����ϴµ� �κ��丮 ��� ��ũ��Ʈ�� �߰��� ������ 
 * �ٸ� �κ��丮���� ���ο� ���� ���� �˻縦 ����ؼ� �߰��ؾ� �ϱ� ������ �̸� �����ϰ� �ϳ��� �����ϱ� ����.
 *
 * 3- �κ��丮�� ���������� ����ȯ���� �ʰ� ������ Ȯ���ϰ� �������� ���� �� �ֵ���,
 * isQuickSlot���� ���� �� IsQuickSlot, ThisQuickSlot ������Ƽ�� �߰�
 *
 *
 */



/// <summary>
/// ���� ���� �� ���� ���� �ǽð� �÷��̾� ���� ���� �����ϰ� �ִ� ���� ���� Ŭ�����Դϴ�.<br/>
/// �ν��Ͻ��� �����Ͽ� ������ Ȯ���Ͻñ� �ٶ��ϴ�.
/// </summary>
public partial class InventoryInfo : MonoBehaviour
{

    protected Inventory inventory;                // ���ο� �����ϰ� �ִ� �κ��丮 ������

    protected Transform inventoryTr;              // �ڽ��� Ʈ������ ĳ��
    protected Transform slotListTr;               // ���� �κ��丮�� �����ϴ� ���� ����Ʈ�� Transform �����Դϴ�.
    protected Transform emptyListTr;              // ���� �κ��丮�� �����ϴ� �� ����Ʈ�� Transform �����Դϴ�.

    public InventoryInitializer initializer;      // ����ڰ� ������ ������� �κ��丮�� �ʱ�ȭ�� �����ϱ� ���� ����
    public InventoryInteractive interactive;      // �ڽ��� ���ͷ�Ƽ�� ��ũ��Ʈ�� �����Ͽ� Ȱ��ȭ �������� �޾ƿ��� ���� ���� ����
        
    
    protected DataManager dataManager;            // ����� �ε� ���� �޼��带 ȣ�� �� ��ũ��Ʈ ����
    protected CreateManager createManager;        // ������ ������ ��û�ϰ� ��ȯ���� ��ũ��Ʈ ����
    
    [Header("�� �κ��丮�� ������ �⺻ �����ġ")]
    public Transform baseDropTr;                // �������� �⺻������ ���� Ʈ�� ��ġ�� �ν����ͺ信�� ���� ����
    
    [Header("�⺻ �����ġ �θ� ���� �ɼ�")]
    public bool isBaseDropSetParent;            // �����ҿ� �θ� ������ ������ �����ϴ� �ɼ� (������ �뵵 �� �θ�� �Բ� �����̵��� �ϴ� �뵵)
        
        
    protected Transform ownerTr;                // �κ��丮 ������ ��ġ ����
    protected UserInfo ownerInfo;               // �κ��丮 ������ ����
    protected int ownerId = -1;                 // �κ��丮 ������ ���� �ĺ� ��ȣ
    
    protected Animation[] animations;           // �κ��丮 ���¿� ���õ� �ִϸ��̼�
    protected WaitForSeconds animationWaitTime; // IEnumrator���� ����� �ִϸ��̼��� �����µ� �ɸ��� �ð�
        
    protected Vector2 cellSize;                 // ���� �̹����� ũ�⸦ ���� ������
    protected bool isQuickSlot;                 // �κ��丮�� ���������� Ȯ���� �� �ִ� ���º��� 


    /// <summary>
    /// �� ��ũ��Ʈ�� ���������� �����ϰ� �ִ� �κ��丮 �����͸� �б����� ���������� ��ȯ�մϴ�.
    /// </summary>
    public Inventory Inventory { get { return inventory; } }
    /// <summary> 
    /// �κ��丮 �ʱ�ȭ ���� ��ũ��Ʈ�� �б����� ���� �� �Դϴ�
    /// </summary>
    public InventoryInitializer Initializer { get { return initializer; } }
    
    /// <summary>
    /// �κ��丮 ���ͷ�Ƽ�� ��ũ��Ʈ�� �б����� ������ �Դϴ�.
    /// </summary>
    public InventoryInteractive Interactive { get { return interactive; } }  


    

    /// <summary>
    /// ������ �̹��� ũ�⸦ ��ȯ�մϴ�.
    /// </summary>
    public Vector2 CellSize { get { return cellSize;}  }



    /// <summary>
    /// �κ��丮�� ������ ������ ��ȯ�մϴ�.<br/>
    /// �κ��丮�� ���� �ֻ��� �θ� ������Ʈ�̸�, �÷��̾� �κ��丮�� ��� �ش� �÷��̾� ������Ʈ�� ���մϴ�.
    /// </summary>
    public Transform OwnerTr { get { return ownerTr;}  }
        
    /// <summary>
    /// �κ��丮�� �����ڸ� �ĺ��� �� �ִ� ���� �ĺ� ��ȣ�Դϴ�.
    /// </summary>
    public int OwnerId { get { return ownerId; } }          


    /// <summary>
    /// �κ��丮 �������� �ֻ��� ������Ʈ���� ��ȯ�մϴ�.
    /// </summary>
    public string OwnerName { get { return ownerTr.name; } }


    /// <summary>
    /// InventoryInfo�� ��ũ��Ʈ���� ��ȯ�޽��ϴ�.<br/>
    /// ��� ��ũ��Ʈ�� ��� ��ũ��Ʈ���� Ʋ���� �� �ֽ��ϴ�.
    /// </summary>
    public string ScriptName { 
        get {  
                string fullName = GetType().Name;                
                return fullName.Substring(fullName.LastIndexOf('.')+1);
            }   
        }
    
    /// <summary>
    /// ���� ���� ���� ��ȯ�մϴ�.<br/>
    /// ���� �����̸� ���� - Player0_InventoryInfo_
    /// </summary>
    public string SaveFileName { get { return OwnerName + OwnerId + "_" + ScriptName + "_"; } }             


    /// <summary>
    /// ���� �κ��丮�� ��ü ���� ���� ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsShareAll { get { return initializer.isShareAll; } }


    /// <summary>
    /// �κ��丮 ���ο� ������ ������� �������� ���� �� �ִ� ����Ʈ�� Transform ���� ���� ��ȯ
    /// </summary>
    public Transform EmptyListTr { get { return emptyListTr; } }

    /// <summary>
    /// �κ��丮 ���� ������ �����ϴ� ����Ʈ�� Transform ���� ���� ��ȯ
    /// </summary>
    public Transform SlotListTr {  get { return slotListTr; } }




    /// <summary>
    /// ���� �κ��丮�� ���������� ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsQuickSlot { get { return isQuickSlot; } }

    /// <summary>
    /// ���� �κ��丮�� �������̶�� �ش� �������� ��ȯ�մϴ�.
    /// </summary>
    public QuickSlot ThisQuickSlot { get { return this as QuickSlot; } }



    protected virtual void Awake()
    {         
        inventoryTr = transform; 
        isQuickSlot = this is QuickSlot;
        slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);
        emptyListTr = inventoryTr.GetChild(0).GetChild(1);
                          
        cellSize = slotListTr.GetComponent<GridLayoutGroup>().cellSize;
        interactive = GetComponent<InventoryInteractive>(); 
        initializer = GetComponent<InventoryInitializer>();     // �ڽ� ������Ʈ�� ��ũ��Ʈ ����
              
        Transform gameController = GameObject.FindWithTag("GameController").transform;
        dataManager = gameController.GetComponent<DataManager>();           // ������Ʈ�ѷ� �±װ� �ִ� ������Ʈ�� ������Ʈ ����
        createManager = gameController.GetComponent<CreateManager>();       // ������ �Ŵ����� ������ ������Ʈ�� ������Ʈ ����
               
        SaveLoadManager.OnSaveData += SaveInventoryData;    // �κ��丮 ���̺� �޼��带 UI �̺�Ʈ �ڵ鷯�� �����մϴ�.


        /*** Inventory_3.cs ���� ���� ***/
        isServer = initializer.isServer;            // ���� �κ��丮 ���θ� �����մϴ�.
        inventoryCG = GetComponent<CanvasGroup>();  // �κ��丮�� ĵ�����׷��� �����մϴ�

        // �÷��̾�(����)�κ��丮�� üũ�Ǿ��ִٸ�, 
        if( isServer )
        {
            clientInfo = new List<InventoryInfo>();     // Ŭ���̾�Ʈ �κ��丮�� ���� �� �ִ� ����Ʈ�� �Ҵ��մϴ�.
            clientInfo.Add(this);                       // ���� �κ��丮 ������ �ڽ��� ����մϴ�.
            serverInfo = this;                          // �ڱ��ڽ��� ������ ����մϴ�.
        }


        /*** �κ��丮 ������ ������ �ʱ�ȭ �մϴ�. ***/
        ownerTr = inventoryTr.parent.parent.parent;     // ���� �ֻ��� �θ� �κ��丮 �����ڰ� �˴ϴ�.           
        ownerInfo = ownerTr.GetComponent<UserInfo>();   // ������ ������ �������� ��ũ��Ʈ�� �����մϴ�.
        
        // �κ��丮 �����ڰ� UserInfo�� �ִ� ���
        if(ownerInfo != null)
            ownerId = ownerInfo.UserId;                 // ����Id�� �κ��丮 ������ �ĺ���ȣ�� �˴ϴ�. 

    }



    // dataManager�� createManager�� �ʱ�ȭ�� �̷���� ���� �ε��ؾ���.
    protected virtual void Start()
    {                
        // ������ �����Ǳ� ���� �ִϸ��̼� Ŭ���� �ִ���̸� ���մϴ�.
        float animationTime = GetLongestAnimationLength( GetComponentsInChildren<Animation>() );

        /** ȣ�� ���� ����: �ε� -> ���ͷ�Ƽ�꽺ũ��Ʈ �ʱ�ȭ �� ���Ի�����û -> ������ǥ�� ***/
                 
        // �ε��� ���Թ�ȣ - SlotNo Ű���� �������� �ʴ´ٸ�(�� �����̶��) 0��, �����Ѵٸ�(���� �����̶��) �ش� Ű���� �޾Ƽ� �����մϴ�.
        int loadSlotNo = 0;    
        if(PlayerPrefs.HasKey("SlotNo"))
            loadSlotNo = PlayerPrefs.GetInt("SlotNo");        
        
        LoadInventoryData(loadSlotNo);              // ����� �÷��̾� �����͸� �ҷ��ɴϴ�. 
        interactive.Initialize(this);               // ���ͷ�Ƽ�� ��ũ��Ʈ �ʱ�ȭ�� �����մϴ�.
        UpdateAllItemVisualInfo();                  // ���Կ� ��� �������� �ð�ȭ�� �����մϴ�.

        // ���� ���� ���� ��� �ִϸ��̼� ������Ʈ�� �����մϴ�.
        animations = GetComponentsInChildren<Animation>();  
        animationWaitTime = new WaitForSeconds(animationTime);  //�ִϸ��̼� �ִ� ���̿� �ش��ϴ� �ν��Ͻ��� �ʱ�ȭ�մϴ�.
                
        InitOpenState(false);                       // �κ��丮�� ���»��¸� �������� ����ϴ�          
    }



    protected virtual void OnDestroy()
    {
        SaveLoadManager.OnSaveData -= SaveInventoryData;    // �� ��ȯ �� �̺�Ʈ �ڵ鷯�� ����� ���̺� �޼��带 �����մϴ�. 
    }








    /// <summary>
    /// �κ��丮 ���� �����͸� �ҷ��ɴϴ�
    /// </summary>
    protected virtual void LoadInventoryData(int slotNo)
    {      
        // �ε� �� ���ϸ��� �����մϴ�
        dataManager.FileSettings(SaveFileName, slotNo); 

        // ���Ͽ��� �ε��� �������� ������ �����մϴ�.
        InventorySaveData loadData = dataManager.LoadInventoryData(initializer);              
        
        // ������ȭ�Ͽ� ���� ���� �κ��丮�� ��ȯ�մϴ�.
        inventory=loadData.savedInventory.Deserialize(initializer, createManager); 
                  
    }


    /// <summary>
    /// �κ��丮 ���� �����͸� �����մϴ� 
    /// </summary>
    protected virtual void SaveInventoryData(int slotNo)
    {        
        // ���̺� �� ���ϸ��� �����մϴ�
        dataManager.FileSettings(SaveFileName, slotNo);   

        // �޼��� ȣ�� ������ �ٸ� ��ũ��Ʈ���� save���� ���� �����Ƿ� ���Ӱ� �������� �ʰ� ���� ������ �ֽ�ȭ�մϴ�
        InventorySaveData saveData = dataManager.LoadInventoryData(initializer);

        // ����ȭ�Ͽ� ���� ������ �κ��丮�� ��ȯ�մϴ�.
        saveData.savedInventory.Serialize(inventory);   
        
        // ������ �����մϴ�.
        dataManager.SaveData(saveData);
    }



    /// <summary>
    /// ������ȭ �κ��丮�� ������ �ĺ���ȣ�� ����մϴ�.<br/>
    /// ���� ���ڷ� ���� �ĺ� ��ȣ�� ��ϵǾ� �ִ� ItemInfo�� ���޵Ǿ�� �մϴ�.<br/><br/>
    /// ������ȭ �κ��丮�� �����ڴ� ���� �ֻ��� �θ��� ������ 3D ������Ʈ�̸�,<br/>
    /// ������ �ĺ� ��ȣ(Id)�� ���� ItemInfo�� ����Ǿ� �ִ� Item ���� �ĺ���ȣ�Դϴ�.
    /// </summary>
    public void UpdateItemInventoryInfo(ItemInfo inventoryItemInfo)
    {
        if(inventoryItemInfo.BuildingType != BuildingType.Inventory )
            throw new Exception("�Ǽ� �������� ���� ������ BuildingType.Inventory�� �ƴմϴ�.");
        if(inventoryItemInfo.ItemId<0)
            throw new Exception("�������� ���� �ĺ� ��ȣ�� �Ҵ�Ǿ� ���� �ʽ��ϴ�.");

        ownerId = inventoryItemInfo.ItemId;       // �����ۿ� ���� �Ǿ��ִ� ���� Id�� ������ Id�� �˴ϴ�.
    }















    /// <summary>
    /// �κ��丮�� �����ϰ� �ִ� ��� ������ �����ۿ� �κ��丮 ������ �����Ͽ� �̹����� ��ġ ���� ���� �ֽ�ȭ�մϴ�.<br/>
    /// 2D �������� �����ϴ� �κ��丮���� �ε� ���� ȣ��Ǿ����� �ϴ� �޼����Դϴ�.<br/>
    /// </summary>
    protected void UpdateAllItemVisualInfo()
    {   
        Dictionary<string, List<ItemInfo>> itemDic;                     // ������ ������ ������ �����մϴ�.
            
        for(int i=0; i<inventory.dicLen; i++)                           // �κ��丮 ������ ������ŭ �ݺ��մϴ�.
        {
            itemDic =inventory.GetItemDic( inventory.dicType[i] );      // ������ ������ ���� �κ��丮�� ������ �Ҵ�޽��ϴ�.
                          
            // ������ ������ ���ų� ����Ʈ�� �������� �ʴ´ٸ� ���� ������ �����մϴ�.
            if(itemDic==null || itemDic.Count==0)   
                continue;

            // �κ��丮 �������� ItemInfo�� �ϳ��� ������ �����ɴϴ�.
            foreach( List<ItemInfo> itemInfoList in itemDic.Values )    
            {
                foreach( ItemInfo itemInfo in itemInfoList )
                {
                    // ���� �κ��丮 �������� �����Ͽ� OnItemAdded�޼��带 ȣ���մϴ�
                    itemInfo.OnItemAdded( this );            

                    // ���� ���� 3D �������� ǥ���� ���� ���� ���¸� �����ϸ� ������ �������ݴϴ�.    
                    if( itemInfo.IsEquip )
                        itemInfo.EquipSwitch(true, true);
                    
                }
            }
        }
    }



    /// <summary>
    /// Ư�� ������ ��ųʸ��� �����ϴ� �������� ���� ������ ������Ʈ���ִ� �޼����Դϴ�.<br/>
    /// �������� ������ ���� ��ųʸ��� �����Ͽ� 
    /// �ش� ��ųʸ� ������ ��� �������� ������� UpdatePositionInSlotList�޼��带 ȣ���մϴ�.<br/><br/>
    /// ����� interactiveŬ�������� �� �޼��带 Ȱ���ϰ� �ֽ��ϴ�.<br/>
    /// </summary>
    /// <param name="itemType"></param>
    public void UpdateDicItemPosition(ItemType itemType)
    {            
        // �κ��丮�� ���� Ȱ��ȭ �������� ��ġ�ϴ� ��ųʸ��� �����մϴ�.
        Dictionary<string, List<ItemInfo>> itemDic = inventory.GetItemDic(itemType);

        // �ش� ������ ������ �������� �ʰų� ����Ʈ�� �������� �ʴ´ٸ� �ٷ� �����մϴ�. 
        if(itemDic==null || itemDic.Count==0)
            return;

        foreach( List<ItemInfo> itemInfoList in itemDic.Values )    // �ش� ��ųʸ��� ItemInfo����Ʈ�� �����ɴϴ�.
        {
            foreach( ItemInfo itemInfo in itemInfoList )            // ItemInfo����Ʈ���� ItemInfo�� �ϳ��� �����ɴϴ�.
                itemInfo.UpdatePositionInfo();                      // Ȱ��ȭ �� ������� �ش� ������ ��ġ������ ������Ʈ�մϴ�.
        }
    }



    /// <summary>
    /// ��� �������� 2D ��� �۵��� �ߴ��ϰų� Ȱ��ȭ �մϴ�.<br/>
    /// �κ��丮 ���� �� ���� �������� OperateSwitchAs2d�޼��带 ȣ���Ͽ�<br/>
    /// ����ĳ��Ʈ ��� �� ��ȣ�ۿ� �Ӽ��� ������ �����մϴ�.<br/><br/>
    /// ���ڷ� 2D ��� �۵� ���θ� �����ؾ� �ϸ�, ���� �ɼ����� ���� ���� ���θ� ������ �� �ֽ��ϴ�. (�⺻��: ���� ����)
    /// </summary>
    public void AllItemOperateSwitchAs2d(bool isOperateAs2D, bool isModifyAlpha=true)
    {   
        Dictionary<string, List<ItemInfo>> itemDic;                     // ������ ������ ������ �����մϴ�.

        
        for( int i = 0; i<inventory.dicLen; i++ )                           // �κ��丮 ������ ������ŭ �ݺ��մϴ�.
        {
            itemDic=inventory.GetItemDic( inventory.dicType[i] );      // ������ ������ ���� �κ��丮�� ������ �Ҵ�޽��ϴ�.

            // ������ ������ ���ų� ����Ʈ�� �������� �ʴ´ٸ� ���� ������ �����մϴ�.
            if( itemDic==null||itemDic.Count==0 )
                continue;

            // �κ��丮 �������� ItemInfo�� �ϳ��� ������ �����ɴϴ�.
            foreach( List<ItemInfo> itemInfoList in itemDic.Values )
            {
                foreach( ItemInfo itemInfo in itemInfoList )
                {
                    // ���������� �������� �������ΰ��� ���̱���� ����Ī�մϴ�.
                    if(itemInfo.EquipType == EquipType.Weapon && itemInfo.IsEquip)
                        itemInfo.DummyInfo.OperateSwitchAs2d(isOperateAs2D);

                    // �׿� �������� ���� ������������ ����� ����Ī�մϴ�.
                    else
                        itemInfo.OperateSwitchAs2d( isOperateAs2D, isModifyAlpha );
                }
            }
        }
        
    }
       




    
    /// <summary>
    /// (�ٸ� �κ��丮�� �����ϴ�) �������� ���� �κ��丮�� ���� ��� ������ �� ���θ� ��ȯ�մϴ�.<br/>
    /// ���ڷ� (�ٸ� �κ��丮�� �����ϴ�) ������ ������ ���� �κ��丮�� Ȱ�� ������ �����ؾ� �մϴ�.<br/><br/>
    /// 
    /// *** ���� �κ��丮���� ����� �ݵ�� �����մϴ�. (��ø, ������ �����ϹǷ�) ***<br/><br/>
    /// 
    /// *** �ٸ� �κ��丮���� ��� ���� ���� �Ǵ� ������ ������ �����ϴ�. ***<br/>
    /// 1.������ �߰��� �� �ִ���(�ش� ������ �����ϴ� ��)<br/><br/>
    /// 2.�ǿ� ���� ���ü��� �������� �� �ִ���(�������� �Ű��� �� �ű� ����� ������ �� �ִ���)<br/>
    /// a- ��ü��->��ü���� ���<br/>
    /// b- ������->��ü���� ���<br/>
    /// c- ��ü��->�������� ��쿡�� �߰��� �������� �����ǿ� �ش��ϴ� ������ ������ ��쿡��<br/>
    /// d- ������->�������� ��쿡�� �������� ���� ��ġ�� ����<br/><br/>
    /// 
    /// *** ��� ��ũ��Ʈ���� ���� ��� �� ������ �ʿ��ϴٸ� �������̵��ؼ� ������ �߰��� �� �ֽ��ϴ�. ***
    /// </summary>
    /// <returns>�ش� �������� ���� ����� �����ϴٸ� true��, �Ұ����ϴٸ� false�� ��ȯ</returns>
    public virtual bool IsAbleToSlotDrop( ItemInfo dropItemInfo, Transform dropSlotTr )
    {
        if(dropItemInfo==null)
            throw new Exception("������ ������ ���޵��� �ʾҽ��ϴ�.");

        /*** ������ �������� �κ��丮 ������ �������� �ʴ� ��� ***/
        if(dropItemInfo.InventoryInfo == null)
            throw new Exception("���� �� ��ӵǴ� ��Ȳ�� �ƴմϴ�. ������ �߰��� �̿��ؾ� �մϴ�.");

        /*** ������ �������� �κ��丮 ������ �ڱ��ڽ��� ��� ***/
        if( dropItemInfo.InventoryInfo == this)
            return true;                            // ���� �κ��丮 ����� ���� �˻����� �ʰ� �ٷ� ������ ��ȯ�մϴ�. 

        

        /*** ������ �������� �κ��丮 ������ �ٸ� �κ��丮�� ��� ***/


        // ���ڷ� ���� �������� ������ �ش��ϴ� �����ε����� �ִ����� �˻��մϴ�.(���� ���翩�� �˻�)
        if( inventory.GetDicIndex( dropItemInfo.Type )!=-1 )
        {
            /*** ���ü��� ������� �ʴ� ��� ����ó���մϴ�. ***/
            // �� �� ���� ���� Ȱ��ȭ �Ǿ� �ִ� ���
            if( !dropItemInfo.InventoryInteractive.IsActiveTabAll && !this.interactive.IsActiveTabAll )
            {
                // ���� ���� ������ ��ġ���� �ʴ°��
                if( dropItemInfo.InventoryInteractive.CurActiveTab != this.interactive.CurActiveTab )
                    return false;
            }

            // �� �� �ϳ��� ��ü ���� ��� - ���� �κ��丮�� �������ΰ�� (��ü��->�������ΰ��)
            else if( !this.interactive.IsActiveTabAll )
            {
                // �������� ������ �ű� �κ��丮�� Ȱ���ǿ� ���� ������ ����ó�� �մϴ�.
                if( Inventory.ConvertItemTypeToTabType(dropItemInfo.Type) != this.interactive.CurActiveTab )
                    return false;
            }
            


            /*** ���ü��� ����Ǵ� ��� ����ó�� �մϴ�. ***/
            // a.��ü��->��ü���� ���
            // b.������->��ü���� ���
            // c.��ü��->�������� ��� - �߰��� �������� �����ǿ� �´� �������� ���
            // d.������->�������� ��� - ���� ���� ������ ��ġ�ϴ� ���
            return true;
        }
        // ���� �ε����� ���ٸ� (������ ������ �ش��ϴ� ������ �������� �ʴ´ٸ�)
        else
            return false;        
    }


    /// <summary>
    /// Ư�� ���Կ� ��� ������ ������ ��ȯ�մϴ�.<br/>
    /// ��� ��ũ��Ʈ�� ��� �������̵� �� �� �ֽ��ϴ�.
    /// </summary>
    /// <returns>���Կ� ��� �������� �ִ� ��� �ش� ������ ��������, ���� ��� null�� ��ȯ</returns>
    public virtual ItemInfo GetSlotItemInfo(Transform slotTr)
    {
        if(slotTr==null)
            throw new Exception("������ Transform ���� ���� ���� ���� �ʾҽ��ϴ�.");

        if(slotTr.childCount!=0)
            return slotTr.GetChild(0).GetComponent<ItemInfo>();
        else
            return null;
    }



}
