using UnityEngine;
using InventoryManagement;
using DataManagement;
using ItemData;
using System;
using System.Collections.Generic;
using CreateManagement;

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
 *
 */



/// <summary>
/// ���� ���� �� ���� ���� �ǽð� �÷��̾� ���� ���� �����ϰ� �ִ� ���� ���� Ŭ�����Դϴ�.<br/>
/// �ν��Ͻ��� �����Ͽ� ������ Ȯ���Ͻñ� �ٶ��ϴ�.
/// </summary>
public partial class InventoryInfo : MonoBehaviour
{
    /// <summary>
    /// �÷��̾ �����ϰ� �ִ� �������� �����ϴ� �κ��丮�� ���õ� ������ ������ �ִ� Ŭ�����Դϴ�.<br/>
    /// �κ��丮�� �������� �����ϰ� �����ϰų� ���� �������� �˻� ��� ���� ������ �ֽ��ϴ�.<br/>
    /// ��ųʸ� ���ο� ���� ������Ʈ�� �����ϰ� �����Ƿ� �� ��ȯ�̳� ���̺� �ε� �ÿ� �ݵ�� Item ������ List���� Convert�� �ʿ��մϴ�.
    /// </summary>
    public Inventory inventory;


    protected Transform inventoryTr;              // �ڽ��� Ʈ������ ĳ��
    protected Transform slotListTr;               // ���� �κ��丮�� �����ϴ� ���� ����Ʈ�� Transform �����Դϴ�.
    protected Transform emptyListTr;              // ���� �κ��丮�� �����ϴ� �� ����Ʈ�� Transform �����Դϴ�.

    protected InventoryInitializer initializer;   // ����ڰ� ������ ������� �κ��丮�� �ʱ�ȭ�� �����ϱ� ���� ����
    public InventoryInteractive interactive;      // �ڽ��� ���ͷ�Ƽ�� ��ũ��Ʈ�� �����Ͽ� Ȱ��ȭ �������� �޾ƿ��� ���� ���� ����
    protected DataManager dataManager;            // ����� �ε� ���� �޼��带 ȣ�� �� ��ũ��Ʈ ����
    protected CreateManager createManager;        // ������ ������ ��û�ϰ� ��ȯ���� ��ũ��Ʈ ����
    
    [Header("�� �κ��丮�� ������ �⺻ �����ġ")]
    public Transform baseDropTr;                // �������� �⺻������ ���� Ʈ�� ��ġ�� �ν����ͺ信�� ���� ����
    
    [Header("�⺻ �����ġ �θ� ���� �ɼ�")]
    public bool isBaseDropSetParent;            // �����ҿ� �θ� ������ ������ �����ϴ� �ɼ� (������ �뵵 �� �θ�� �Բ� �����̵��� �ϴ� �뵵)
    
    protected string saveFileName;              // �������� �̸� ����


    protected bool isItem3dStore;               // 3d ������ �������� �����ϴ� �κ��丮���� ����
    protected bool isInstantiated;              // ���� �߿� �������� �����Ǵ� �κ��丮���� ����
    
    protected Transform ownerTr;                // �κ��丮 ������ ����
    protected int ownerId;                      // �κ��丮 ������ ���� �ĺ� ��ȣ
        

    /// <summary>
    /// �κ��丮�� ������ ������ ��ȯ�մϴ�.<br/>
    /// �κ��丮�� ���� �ֻ��� �θ� ������Ʈ�̸�, �÷��̾� �κ��丮�� ��� �ش� �÷��̾� ������Ʈ�� ���մϴ�.
    /// </summary>
    public Transform OwnerTr { get { return ownerTr;}  }
        
    /// <summary>
    /// �κ��丮�� �����ڸ� �ĺ��� �� �ִ� ���� �ĺ� ��ȣ�Դϴ�.
    /// </summary>
    public int OwnerId { get { return ownerId; } }          


    protected virtual void Awake()
    {         
        inventoryTr = transform; 
        slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);
        emptyListTr = inventoryTr.GetChild(0).GetChild(1);
                          
        interactive = GetComponent<InventoryInteractive>(); 
        initializer = GetComponent<InventoryInitializer>();     // �ڽ� ������Ʈ�� ��ũ��Ʈ ����
                                                                                                               
        isItem3dStore = initializer.isItem3dStore;    // ���� ������ ���� �κ��丮������ �����մϴ�.         
        isInstantiated = initializer.isInstantiated;  // ���� �����Ǵ� �κ��丮������ �����մϴ�.  
        ownerTr = inventoryTr.parent.parent;          // ���� �ֻ��� �θ� �����ڰ� �˴ϴ�.
        
        Transform gameController = GameObject.FindWithTag("GameController").transform;
        dataManager = gameController.GetComponent<DataManager>();           // ������Ʈ�ѷ� �±װ� �ִ� ������Ʈ�� ������Ʈ ����
        createManager = gameController.GetComponent<CreateManager>();       // ������ �Ŵ����� ������ ������Ʈ�� ������Ʈ ����
               
        // ���� �����̸��� �̴ϼȶ������� id�� ��ġ ��ŵ�ϴ�
        saveFileName = ownerTr.name + initializer.inventoryId;  


        /*** Inventory_3.cs ���� ���� ***/
        inventoryCG = GetComponent<CanvasGroup>();  // �κ��丮�� ĵ�����׷��� �����մϴ�
        InitOpenState(false);                       // �κ��丮�� ���»��¸� �������� ����ϴ�

        // �÷��̾�(����)�κ��丮�� üũ�Ǿ��ִٸ�, 
        if( isServer )
        {
            clientInfo = new List<InventoryInfo>(); // Ŭ���̾�Ʈ �κ��丮�� ���� �� �ִ� ����Ʈ�� �Ҵ��մϴ�.
            clientInfo.Add(this);                   // ���� �κ��丮 ������ �ڽ��� ����մϴ�.
            serverInfo = this;                      // �ڱ��ڽ��� ������ ����մϴ�.
        }
    }

    // dataManager�� createManager�� �ʱ�ȭ�� �̷���� ���� �ε��ؾ���.
    protected virtual void Start()
    {        
        /** ȣ�� ���� ����: �ε�->���ͷ�Ƽ�꽺ũ��Ʈ �ʱ�ȭ �� ���Ի�����û->������ǥ�� ***/
        this.LoadInventoryData();              // ����� �÷��̾� �����͸� �ҷ��ɴϴ�. 
        interactive.Initialize(this);       // ���ͷ�Ƽ�� ��ũ��Ʈ �ʱ�ȭ�� �����մϴ�.

        // 3D�������� �����ϴ� �κ��丮��� ����� ������ �������� Transform ������ �ݿ��մϴ�.
        if(isItem3dStore)
            UpdateAllItemTransformInfo();   
        // 2D�������� �����ϴ� �κ��丮��� ���Կ� ��� �������� �ð�ȭ�� �����մϴ�.
        else
            UpdateAllItemVisualInfo(); 
    }




    /// <summary>
    /// ���� ����� �����մϴ�.
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        SaveInvetoryData(); // �÷��̾� ������ ����
    }


    /// <summary>
    /// �κ��丮 ���� �����͸� �ҷ��ɴϴ�
    /// </summary>
    protected void LoadInventoryData()
    {
        // �ε� �� ���ϸ��� �����մϴ�
        dataManager.FileSettings(saveFileName); 

        // ���Ͽ��� �ε��� �������� ������ �����մϴ�.
        InventorySaveData loadData = dataManager.LoadInventoryData(initializer);              
        
        // ������ȭ�Ͽ� ���� ���� �κ��丮�� ��ȯ�մϴ�.
        inventory=loadData.savedInventory.Deserialize(initializer, createManager);   
    }


    /// <summary>
    /// �κ��丮 ���� �����͸� �����մϴ� 
    /// </summary>
    protected void SaveInvetoryData()
    {        
        // ���̺� �� ���ϸ��� �����մϴ�
        dataManager.FileSettings(saveFileName);   

        // �޼��� ȣ�� ������ �ٸ� ��ũ��Ʈ���� save���� ���� �����Ƿ� ���Ӱ� �������� �ʰ� ���� ������ �ֽ�ȭ�մϴ�
        InventorySaveData saveData = dataManager.LoadInventoryData(initializer);

        // ����ȭ�Ͽ� ���� ������ �κ��丮�� ��ȯ�մϴ�.
        saveData.savedInventory.Serialize(inventory);   
        
        // ������ �����մϴ�.
        dataManager.SaveData(saveData);
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
                    itemInfo.OnItemAdded( this );    // ���� �κ��丮 �������� �����Ͽ� OnItemAdded�޼��带 ȣ���մϴ�
            }
        }
    }



    /// <summary>
    /// �κ��丮�� �����ϰ� �ִ� ��� ������ ������ 3D ������Ʈ�� Transform ������ �ֽ�ȭ�մϴ�.<br/>
    /// 3D �������� �����ϴ� �κ��丮���� �ε� ���� ȣ��Ǿ����� �ϴ� �޼����Դϴ�.<br/>
    /// </summary>
    protected void UpdateAllItemTransformInfo()
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
                    // �������� STransform ������ �ҷ��ͼ� Transform�� ����ȭ�����ݴϴ�.
                    itemInfo.SerializedTr.Deserialize( itemInfo.ItemTr );
                }
            }
        }
    }








    /// <summary>
    /// �κ��丮�� �����ϴ� �ش� �̸��� *����* ��ȭ ������ ������ ������Ű�ų� ���ҽ�ŵ�ϴ�.<br/>
    /// ���ڷ� �ش� ������ �̸��� ������ �����ؾ� �մϴ�.<br/><br/>
    /// ������ ������ ���ҽ�Ű���� ���� ���ڷ� ������ �����Ͽ��� �ϸ�,<br/>
    /// ���� ������ ���ҷ� ���� 0�̵Ǹ� �������� �κ��丮 ��Ͽ��� ���ŵǸ�, �ı��˴ϴ�.<br/><br/>
    /// ������ ������ ������Ű���� ���� ���ڷ� ����� �����Ͽ��� �ϸ�,<br/>
    /// ������ �ִ� ���� �������� ���� �� �̻� ������ ������Ű�� ���ϴ� ���� ������ �ʰ� ������ ��ȯ�մϴ�.<br/><br/>
    /// �ֽ� ��, ������ ������ ���ҿ��θ� ������ ���ֽ��ϴ�. (�⺻��: �ֽż�)<br/><br/>
    /// ** ������ �̸��� �ش� �κ��丮�� �������� �ʰų�, ��ȭ�������� �ƴ� ��� ���ܸ� �߻���ŵ�ϴ�. **
    /// </summary>
    /// <returns></returns>
    public int SetItemOverlapCount( string itemName, int inCount, bool isLatestModify = true )
    {
        return inventory.SetOverlapCount(itemName, inCount, isLatestModify, null);
    }


    
    /// <summary>
    /// �������� ������ ������� �������� �ش� ���� ��ŭ �κ��丮�� �����ϴ��� ���θ� ��ȯ�մϴ�.<br/>
    /// ������ �̸��� �������� �̷���� ����ü �迭�� ���ڷ� �޽��ϴ�.<br/><br/>
    /// �Ϲ� �������� ������Ʈ�� ������ �ǹ��ϸ�, ��ȭ �������� ��ø������ �ǹ��մϴ�.<br/>   
    /// �ش� ������ŭ ���� �� �ı��ɼ��� ������ �� �ֽ��ϴ�. (�⺻��: ���� 1, ���� ���� �� �ı� ����, �ֽż� ���� �� �ı�)<br/><br/>
    /// *** ���� ���ڰ� 0���϶�� ���ܸ� �߻���ŵ�ϴ�. ***
    /// </summary>
    /// <returns>�������� �����ϸ� ������ ����� ��� true��, �������� �ʰų� ������ ������� �ʴٸ� false�� ��ȯ</returns>
    public bool IsItemEnough( ItemPair[] pairs, bool isReduceAndDestroy = false, bool isLatestModify = true )
    {
        return inventory.IsEnough(pairs, isReduceAndDestroy, isLatestModify);
    }



    
    /// <summary>
    /// �������� ������ ������� �������� �ش� ���� ��ŭ �κ��丮�� �����ϴ��� ���θ� ��ȯ�մϴ�.<br/>
    /// ������ �̸��� ������ ���ڷ� �޽��ϴ�.<br/><br/>
    /// �Ϲ� �������� ������Ʈ�� ������ �ǹ��ϸ�, ��ȭ �������� ��ø������ �ǹ��մϴ�.<br/>   
    /// �ش� ������ŭ ���� �� �ı��ɼ��� ������ �� �ֽ��ϴ�. (�⺻��: ���� 1, ���� ���� �� �ı� ����, �ֽż� ���� �� �ı�)<br/><br/>
    /// *** ���� ���ڰ� 0���϶�� ���ܸ� �߻���ŵ�ϴ�. ***
    /// </summary>
    /// <returns>�������� �����ϸ� ������ ����� ��� true��, �������� �ʰų� ������ ������� �ʴٸ� false�� ��ȯ</returns>
    public bool IsItemEnough( string itemName, int count=1, bool isReduceAndDestroy = false, bool isLatestModify=true )
    {
        return inventory.IsEnough(itemName, count, isReduceAndDestroy, isLatestModify);      
    }






    












    /// <summary>
    /// �� �κ��丮�� ���Կ� �������� ���ϴ� ������ŭ �������� �� �ƹ� ���Կ� �� �� �ڸ��� �ִ��� ���θ� ��ȯ�մϴ�.<br/>
    /// ���ڷ� ������ �̸��� �������ڸ� �����ؾ� �մϴ�. (���������� �⺻���� 1�Դϴ�.)<br/><br/>
    /// 
    /// ����ȭ�������� ��� �������ڸ�ŭ ������Ʈ�� �����ϴ�. (��, �������ڰ� ������Ʈ�� ������ ���մϴ�.)<br/>
    /// ��ȭ �������� ��� �������ڸ� �־ �ִ� ��ø������ �����ϱ� �������� ������Ʈ�� 1���� �����մϴ�. (��, �������ڴ� ��ø������ ���մϴ�.)<br/><br/>
    /// </summary>
    /// <returns>�������� ���������ϴٸ� true��, �Ұ����ϴٸ� false�� ��ȯ�մϴ�.</returns>
    public bool IsSlotEnough(string itemName, int overlapCount=1)
    {
        ItemType itemType = createManager.GetWorldItemType(itemName);

        if( itemType==ItemType.Misc )
            return inventory.IsAbleToAddMisc( itemName, overlapCount ); 
        else
            return inventory.GetCurRemainSlotCount(itemType) >= overlapCount; // ���� ���� ĭ ���� overlapCount�̻�
    }

    /// <summary>
    /// �ش� �������� �ƹ� ���Կ� �� �� �ڸ��� �ִ��� ���θ� ��ȯ�մϴ�.<br/>
    /// �⺻ �������� ��� ���Կ��ο� ���� ����, ���и� ��ȯ�ϸ�,<br/>
    /// ��ȭ �������� ��� ��ø�Ǿ ������ �ʿ���� ��� ���Կ� ���ڸ��� ��� ������ ��ȯ�� �� �ֽ��ϴ�.<br/>
    /// ���� ������ ������ �����ؾ� �մϴ�.
    /// </summary>
    /// <returns>�������� ���������ϴٸ� true��, �Ұ����ϴٸ� false�� ��ȯ�մϴ�.</returns>
    public bool IsSlotEnough(ItemInfo itemInfo)
    {
        ItemType itemType = itemInfo.Item.Type;

        if( itemType == ItemType.Misc )
        {
            ItemMisc itemMisc = (ItemMisc)itemInfo.Item;
            return inventory.IsAbleToAddMisc(itemMisc.Name, itemMisc.OverlapCount);
        }
        else
            return inventory.IsRemainSlotIndirect(itemType);  // �ƹ��ڸ��� ���� ���� ĭ���� �ִ���
    }




    /// <summary>
    /// �ش� �������� ���� ���� Ư�� ���Կ� �� �� ���� �� ���θ� ��ȯ�մϴ�.<br/>
    /// ������ ������ ������ ���� �ε���, ��ü �� ���θ� ���ڷ� �޽��ϴ�.
    /// </summary>
    /// <returns>���Կ� ���ڸ��� ���� ��� false��, ���ڸ��� �ִ� ��� true�� ��ȯ</returns>
    public bool IsSlotEmpty(ItemInfo itemInfo, int slotIndex, bool isActiveTabAll)
    {
        return inventory.IsRemainSlotDirect(itemInfo.Item.Type, slotIndex, isActiveTabAll);         
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



}
