using UnityEngine;
using UnityEngine.UI;
using ItemData;
using System;
using CreateManagement;
using InventoryManagement;
using DataManagement;

/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1102_�ֿ���>
 * 1- �����ۼ� �� �ּ�ó��
 * 
 * <v2.0 - 2023_1103_�ֿ���>
 * 1- �ּ� ����
 * 2- �̹��� ������Ʈ ��� ������ Start�޼��忡�� OnEnable�� ����
 * �ν��Ͻ��� �����Ǿ� �̹��� ������Ʈ�� ��� �����ϸ� OnItemAdded�� ȣ�� ������ ���ü��� ������ ��������Ʈ �̹����� ������� �ʴ´�.
 * 
 * <v3.0 - 2023-1105_�ֿ���>
 * 1- ��������� ������ item�� ������Ƽȭ ���Ѽ� set�� ȣ��Ǿ��� �� OnItemChanged()�� ȣ��ǵ��� ���� 
 * OnItemAdded�� privateó�� �� ���� ����ó�� ���� ����
 *
 *<v4.0 - 2023_1108_�ֿ���>
 *1- �������� �ı��� �� ������ �����ϵ��� �����Ͽ�����, ���������� ����� ������� �ʴ� �����߻�
 *=> �������ʿ��� �ı��ɋ����� ��ųʸ��� �����ؼ� CraftManager�ʿ��� �������� �ѹ� �̸� �������ֵ��� ����
 *
 *2- OnItemChanged �޼��� �ּ��߰�
 *
 *3- UpdateCountTxt �޼��� �߰�
 * ������ ������ ���� �ɶ� �������� �ؽ�Ʈ�� �������ֵ��� �Ͽ���.
 * item�ʿ��� �޼��带 ������ �������� �ؼ� ���� ���ټ� Ȯ��.
 *
 *<v5.0 - 2023_1112_�ֿ���>
 *1- OnItemAdded�޼��� �߰�����. (CreateManager�ʿ��� �ߺ��ڵ� ����ϰ� �ִ� �� ���� �� ����, �ּ� �߰�) 
 *
 *<v6.0 - 2023_1114_�ֿ���>
 *1- OnItemAdded�޼��带 OnItemChanged�� �̸�����
 *2- ItemInfo Ŭ���� ���� �ּ� �߰�
 *3- private �޼��� public �޼���� ����
 *4- ��� ���� item�� public�Ǿ��ִ� ���� private ó��. �ݵ�� ������Ƽ�� ���� �ʱ�ȭ�� ����
 *
 *<v7.0 - 2023_1116_�ֿ���>
 *1- ItemInfo Ŭ������ ItemImageCollection ��������� �����Ͽ� �ܺ��̹����� �����ϵ��� �����Ͽ����ϴ�. 
 *(CreateManager�� �ִ� ���� ������ �Űܿ�.)
 *
 *2- UpdateImage�޼��带 �����Ͽ����ϴ�.
 *������ ������ Ŭ������ ImageCollection ����ü ������ ����� �����ϰ� �ִ� ������ ImageReferenceIndex ����ü ������ ����� �����ϵ��� �ٲپ��� ������
 *item�� ImageReferenceIndex ��������� ���� �ε������� �޾ƿͼ� ImageCollection ������ �����Ͽ� ������Ʈ�� �̹����� �־��ֵ��� ����.
 *
 *<v7.1 - 2023_1119_�ֿ���>
 *1- OnDestroy()�޼��� �ּ�ó��. 
 *InventoryŬ������ ����ȭ �����ϰ� ������ �����̹Ƿ�
 *
 *2- RemoveItemObject �̱��� �޼��� ���� - inventoryŬ�������� ����
 *ItemInfo Ŭ������ ������Ʈ�� ������ �ֽ�ȭ �����ִ� ������ �ϰ� �ؾ��ϱ� �����̸�, 
 *ItemInfo���� item�� ���������� �����ϴ� �޼��带 �߰��ϱ� �����ϸ�, InventoryŬ���������� ����� �ߺ� ������ ���ɼ��� Ŀ��.
 *
 *<v8.0 - 2023_1216_�ֿ���>
 *1- �������� ����â �̹��� ���� statusImage �߰� �� UpdateImage�޼��� ���� ����
 *
 *2- ������ �ı��� ���� ���� �ּ�ó�� �Ǿ��ִ� �κ� ����
 *
 *3- slotList ������ slotListTr�� ����
 *
 *4- Transform imageCollectionsTr �ӽú��� ������ 
 * GameObject.Find( "ImageCollections" ) �ߺ� ȣ�� ���� ����
 *
 *<v8.1 - 2023_1217_�ֿ���>
 *1- ItemImageCollection �������� �ϳ��� �����ϴ� ���� �迭�� ���� ����
 *
 *<v8.2 - 2023_1221_�ֿ���>
 *1- GameObject.Find()�޼���� ������Ʈ�� �˻��ϴ� ���� ���������� ����
 *
 *<v9.0 - 2023_1222_�ֿ���>
 *1- �±����� ö�ڿ��� ���� (CANVAS_CHRACTER -> CANVAS_CHARACTER)
 *
 *2- ItemImageCollection[]�� �迭�� �������ϰ� ������ ���ؼ� �ߴ� �迭�� bounds���� ����
 *
 *3- �������� ���������� UpdateImage�� UpdatePosition�� ȣ���ϸ� ������ ������ �ʱ� ������ bounds������ �ߴµ�
 * OnItemChanged�޼��带 �������� �������� ȣ���� �ƴ϶�, �������� ���� ������ ȣ���ϵ��� �����Ͽ���.
 *
 *4 - SlotListTr�� ����Ʈ�� �����ִ� ���� ����
 *
 *<v9.1 - 2023_1224_�ֿ���>
 *1- Item������Ƽ�� �ּ��Ϻ� ����
 *2- ������Ʈ ���� ���� Start���� OnEanble�� �̵� �� ����
 *3- UpdataImage�޼��� ������ ������ ���� �ߺ����� ���� �� ����ȭ
 *
 *<v9.2 - 2023_1226_�ֿ���>
 *1- �Ϻ� ����� ��¸޼��� ����
 *2- UpdatePosition�� slotListTr�� childCount �˻籸�� �߰�
 *3- Item ������Ƽ �ٽ� �ּ����� 
 *
 *<v9.3 - 2023_1228_�ֿ���>
 *1- ������ ������ ���� ���� �������� ���� (3D������Ʈ ������ 2D ������Ʈ�� �δ� ����)
 *���� Transform, RectTransform itemTr�� itemRectTr ������ �����Ͽ� �ڱ�Ʈ������ ĳ��ó��
 *
 *2- UpdatePosition 2D������Ʈ�� �θ� �����ϴ� ���� �ֻ��� 3D������Ʈ�� �����ϵ��� ����
 *
 *3- UpdatePosition �������� UpdateInventoryPosition���� ����
 *
 *<v9.4 - 2023_1228_�ֿ���
 *1- ������ ������ �������� �纯������ ���� (3D������Ʈ, 2D������Ʈ ��ȯ���)
 *�ڵ带 2D�������� �ӽ� ���� (itemTr->itemRectTr)
 *
 *
 *<v10.0 - 2023-1229_�ֿ���>
 *1- UpdateSlotPosition���� �������ڸ� �߰��Ͽ� ���Ը���Ʈ ������ �ִ� ��쿡�� �ش� ���Ը���Ʈ�� �ε����� �����Ǿ�����Ʈ�� �ϸ�,
 *���Ը���Ʈ ������ ���� ���� ��쿡�� �������� ���� ����ִ� ������ �������� ���Ը���Ʈ�� �����ؼ� �ε����� ���� ������ ������Ʈ�� �ϵ��� ����
 *
 *<v10.1 - 2023_1230_�ֿ���>
 *1- UpdateSlotPoisition�޼������ UpdatePositionInSlotList�� ���� �ϰ�
 *���Ը���Ʈ�� ���ڷ� �޵��� ����. �������ڷ� ���ڰ� ���޵��� �ʾҴٸ� ���������� �����Ͽ� �ڵ�����ϵ��� ����
 *
 *2- OnItemChanged�޼��带 �ܺο��� ȣ���� �� slotList�� ���ڷ� �޾Ƽ� ȣ�Ⱑ���ϵ��� ����,
 *UpdatePositionInSlotList�� slotList���ڸ� �����Ͽ� ȣ��. ���������� �������ڷ� ȣ�Ⱑ��
 *
 *3- �������� ��ȯ�ڵ带 �����ϰ� ����
 *�̸� ItemTr ItemRectTr�� ��Ƴ��� �ʿ����
 *��ȯ�� �̷���� ���� �޼��带 ȣ���ϸ� 2D�� �����̿��� �Ҷ��� ���� �������ڽ� 
 *
 *<v10.2 - 2023_1231_�ֿ���>
 *1- Locate 2D, 3D �޼���
 *2- SetOverlapCount�޼���
 *3- FindNearstRemainSlotIdx �޼��� inventoryInfo �Ű����� �����ε�
 *
 *<v10.3 - 2024_0101_�ֿ���>
 *1- �������� ���� Ȱ��ȭ ������ ���� Ȯ���ϰ� �����Ǿ�����Ʈ�� �ؾ� �ϹǷ�, ���ͷ�Ƽ�� ��ũ��Ʈ���� Ȱ��ȭ�� ���� ������ �޾Ƶ��� �Ͽ���
 *2- prevDropEventCallerTr���� ���� �� OnItemDrop�޼��� �߰�
 *
 *3- FindNearstRemainSlotIdx �޼��� ��� ����
 *InventoryInfo�� �־�� �� ����̸�, �������� �κ��丮 ������ �����ϰ� �ֱ� ������ �κ��丮�� �����Ͽ� ȣ���ϱ⸸ �ϸ� �Ǳ� ����
 *
 *4- OnEnable���� ���Ը���Ʈ ���� �� canvsTr�� find �޼���� ã�Ƽ� ó���ϴ� �� �����ϰ�,
 *UpdateInventoryInfo�޼��� ȣ��� ����
 *
 * 
 *
 *
 *
 * [���� �����ؾ��� ��] 
 *  1- UpdateInventoryPosition�� ���� �ڽ� �κ��丮 �������� �����ϰ� ������,
 *  ���߿� UpdatePosition�� �� �� �������� ������ ������ �ε��� �Ӹ� �ƴ϶� ��� �����Կ� ����ִ����� ������ �־�� �Ѵ�.
 * 
 * 2- ���Ե���̺�Ʈ�� �߻��� �� �κ��丮 ������ �ٸ��ٸ� ���� �κ��丮���� �� ������ ����� �����ؾ��Ѵ�.
 *  ���������� �Ͼ �� �κ��丮���� �� �������� ��Ͽ� �߰��ϰų� �����ؾ� �Ѵ�.
 *  Drag���� �κ��丮 ������ ������ �� �κ��丮 ��Ͽ��� �� �������� �����ؾ� �Ѵ�.
 *
 *
 *
 *
 * [�̽�_0101] 
 * 1- ������ ����(�κ��丮 ����) ������Ʈ ����
 * a- Slot To Slot���� SlotDrop�� �Ͼ �� Slot�� ���� �޾ƾ� �Ѵ�.
 * b- ItemDrag�ؼ� �κ��丮 �ܺη� Drop�Ҷ� ItemDrag�� ���� �޾ƾ� �Ѵ�.
 * c- ItemInfo���� 2D to World(������������), 3D to Slot(�κ��丮��������) �Ҷ� ��ü������ Ȯ���ؾ� �Ѵ�.
 * => �ܺο��� ������Ʈ �޼��带 ȣ���� �� �ְ� ������Ѵ�.
 *
 * 2- Ÿ �κ��丮�� ������Ʈ�� �̷��� ���� 
 * slotIndexAll�� slotIndex ��θ� �޾߾� �Ѵ�.
 * ������ �ѹ� ���� ���¿��� �ٸ� �Ǻ����� �õ��ϸ� ��ġ ������ �ȸ±� ����
 *
 *
 *
 *<v10.4 - 2024_0102_�ֿ���>
 *1- OnItemDrop�޼��� �����Ϸ�
 *� �������� ��� �̺�Ʈ �߻��� �ܺ� ��ũ��Ʈ���� ȣ���ϵ��� ����
 *2- UpdateActiveTabInfo �޼��� �����ϰ� OnItemChanged ���ο� �߰�.
 *
 *
 * <v11.0 - 2024_0102_2_�ֿ���>
 * 1- OnItemWorldDrop�޼����� �Ű������� dropEventCallerTr���� worldPlaceTr�� ����
 * 
 * 2- Transfer2DToWorld�޼��� ����
 * 
 * a. prevDropEventCallerTr������ �ֽ�ȭ ���ϰ� null�� �����ִ� �� ����
 * b. inventoryInfo.RemoveItem(this);�� null�� ���� �� ���� �� ����.
 * c. ����ó�� ��Ȳ isWorldPositioned������� ����
 * 
 * 3- OnItemWorldDrop, Transfer2DToWorld�޼��� Vector3, Quaternion���� ���� ��� �����ε�, ��ȯ�� void�� ����
 * 
 * 4- UpdateInventoryInfo�޼��忡�� prevDropEventCaller ������ �ֽ�ȭ�ϴ� �ڵ� �߰�
 * 
 * 5- OnItemChanged�޼��带 OnItemCreated�� ���� �� �ּ� ����, ���ڷ� InventoryInfo ��ũ��Ʈ�� �޵��� ����.
 * �������� �κ��丮�� �����Ǵ� ��Ȳ���� ȣ���ϴ� ���� ���� ��Ȯ�� �ṉ̀��� ����
 * 
 * 6- OnEnable�޼��� �ּ�����, canvasTr�� ���������� ó���ϴ� �ڵ� ����
 * 
 * 7- UpdateActiveTabInfo �޼��� �ּ� ���� ��
 * ���ڸ� ���������� �޾� � ��Ȳ���� ȣ���ؾ� �ϴ� �� ��Ȯ�ϰ� ����
 * 
 * 8- OnItemSlotDrop �ּ� ����
 * 
 * 9- ChangeHierarchy�޼��� �������� null�� �߰�
 * 3D->Slot���� ȣ�� �� �ε����� �Է��ϰ� �ε����� ���� Position Update�� ���� ���־�� �ϱ� ������ �θ� ������ �ʿ����� ����.
 * 
 * 10- Locate3DToSlot, SlotList, InventoryInfo���� �����ε� �޼��� ���� �� 
 * Locate3DToInventory�޼��� �ϳ��� ����.
 * 
 * 11- ����ڰ� ȣ�� �� OnItemGain�޼��� �߰�
 *
 *
 * <v11.1 - 2024_0102_�ֿ���>
 * 1- ItemInfo Ŭ���� partialŬ������ ����
 * 2- ������ �������� �⺻ �޼��带 ItemInfo_2.cs�� �ű�
 * 3- emptyListTr ���� �߰�
 * �������� �����ϱ� �� �ӽ÷� �� �������� �ű� ���Ը���Ʈ
 * 
 * 
 *
 * [�̽�_0102]
 * 1- UpdateImage���� iicArr�� �ε����� �����Ͽ� �������� ã�ư��� �κ��� �ִµ�, 
 * ��� �������� �� �޼��带 ������ ���� �ʿ䰡 ���⶧���� Ư�� �ν��Ͻ��� ��û�ؼ� ������ �޾ƿ��� �ȴٰ� �����Ǵµ�,
 * CreateManager�� ��û�ϴ� ���� ����������, �̹��� �켱������ �̱��濡�� ������ �޾ƿ��� ������ �ʱ⶧���� �ٸ������ ���� ��.
 * => ������ ���� �� �̸� ������Ʈ�� �������� �ش� �޼��带 ȣ���ϵ��� �ϴ� ������� �����ϸ� ��� ����.
 * 
 * 2- OnEnable���� OnItemCreate�޼��带�ְ� UpdateInventoryInfo�� ���� ȣ������ ���� ��.
 * => �̹��� �켱������ OnItemCreate�� �������� ȣ��� ����, UpdateInventoryInfo�޼��带 ���ο� ����.
 * 
 * [���߿� ������ ��_0102]
 * 1- SetOverlap�޼��� ����
 * 2- OnEnable�� �ִ� iic���� ���� �޼��� ������ ���� Ŭ�����ʿ��� ȣ�� �� ����
 *
 * <v11.2 - 2024_0104_�ֿ���>
 * 1- RemoveItem(this) �� RemoveItem(item.Name)���� ����
 * InventoryInfoŬ������ ItemInfo �����ε��� �����Ͽ��� ����
 *
 *2- UpdateActiveTabInfo�� ���� caller�� interactiveCaller�� �̸�����
 *
 *3- UpdateInventoryInfo���� ���������� UpdateActiveTabInfo �޼������ ȣ��� ����.
 *=> �κ��丮 ������ ������Ʈ�ϴ� �޼��带 �ϳ��� �����ϱ� ����.
 *
 *4- UpdateActiveTabInfo �޼��带 �ٸ� �޼��忡�� ȣ���ϴ� �� ����
 *=> interactive Ŭ�������� �Ǻ������� ���� �Ǻ����� �� �̿ܿ��� ȣ������ �ʾƾ� ��.
 *
 *5- UpdateInventoryInfo�޼����� �Ű������� Transform���� InventoryInfoŬ������ ����
 *�ش� �޼��带 ȣ���ϴ� ��� �ڵ� ����
 *=> ������ inventoryInfoŬ�������� ȣ���� itemInfo�� �޾Ƽ� �ڱ����� �ּҸ� �־ ȣ���ϴ� ��찡 ����,
 *��� ȣ���� ������ �����ϱ� ���ؼ�
 *
 *<12.0-2024_0105_�ֿ���>
 *1- UpdatePosition���� ���Ը���Ʈ�� �����ڽİ˻縦 �ϴ� �κ� ����
 *2- IsSlotEnough�� ItemType�� ������� ȣ���ϴ� �κ��� ItemInfo�� ���ڷ� �־� ȣ���ϴ� �޼���� ����
 *(������ ��ȭ�������� ��� ��ø�ؼ� �� ��� ������ �ʿ����� ���� ��쵵 �ֱ� ����.)
 *
 *3- OnItemWorldDrop �޼��带 ����
 *�������� �κ��丮���� ����Ǿ�� ���� �� �ֱ� ������ �κ��丮���� �����ϱ� �������� ������� ����.
 *Remove�޼��带 ���� ������ ��.
 *
 *4- Transfer2DtoWorld, TransferWorldTo2D �޼��带 ��� DimensionShift�޼���� �����Ͽ����ϴ�.
 *��ġ���� ���ڸ� �־� �̵���Ű�� ���� �ϴ��κ��丮���� �����ϸ�, ����ڿ��� �ñ�� ������ �����մϴ�.
 *(�⺻ �����ġ�� playerDropTr�� �����Ǿ��ֽ��ϴ�.)
 *
 *<v12.1 - 2024_0105_�ֿ���>
 *1- ������ OnItemCreated�� OnItemCreatedInInventory�� ����, �űԸ޼��� OnItemCreateInWorld �߰�
 *�������� �κ��丮 ������ �������� �κ��丮 ���ο� �����Ǵ� ����, �κ��丮 �ܺο� �����Ǵ� ��츦 ��������.
 *
 *2- UpdateInventoryInfo�޼��� ������ prevDropSlotTr�� itemRrectTr.parent���� slotListTr.GetChild(item.SlotIndex)�� ����
 * (������ ���忡 ���»��¿��� ����ְ� �־��� ������ null�� ��������)
 *
 *<v12.2 - 2024_0106_�ֿ���>
 *1- ����â ��ũ��Ʈ�� �����ۿ��� �κ��丮�� �ű�鼭 statusInteractive������ �߰��Ͽ�, �κ��丮�� �ٲ𶧸��� �ֽ� ����â �������� �ݿ��ϵ��� ����
 *�������� �������� ����â�� ó���ϴ� ������ ��� ��� �־�����, ����â�� ������ �����Ը����, ItemInfo_2.cs���� �����ۿ� �����͸� �θ� �ش� ����â �޼��带 ȣ���Ű���� ����
 *
 *<v12.3 - 2024_0107_�ֿ���>
 *1- UpdatePositionInInventory�޼��忡�� �������� ũ�⸦ ������ ũ�⿡ �°� �������� �����ϵ��� ���� 
 *( ���� Slot�� �پ��ִ� VerticalLayoutGroup�� �������� ���Կ� ��� �� �ڵ����� ũ�⸦ ����������, �κ��丮�� ��� ������Ʈ�� Ȱ��ȭ,��Ȱ��ȭ �ɶ�����
 *�ڽ� �������� ũ��� ��ġ�� ������� �ǵ����� ����� ������ �ֱ� ����)
 *
 *<v12.4 - 2024_0108_�ֿ���>
 *1- SetDropPosition�޼������ OnItemWorldDrop���� ����
 *RemoveItem�Ŀ� itemInfo�� �������� �����ϱⰡ ���ŷӱ� ������ ���� ���޷� ���۽�ų �ʿ䰡 ������ �̸� ��ü�ϱ� ����.
 *(RemoveItem�� �ؾ�԰� ȣ���ϴ��� ����ó������ �ʰ� RemoveItem���ֵ��� ����)
 *
 *2- OnItemGain�޼������ OnItemWorldGain���� �̸� ����
 *
 *3- playerDropTr�� �÷��̾��� �Ǹ����� �ڽ� ��ġ�� ����
 *
 *4- ���� ������ �������� �Ծ��� �� 2d ȸ������ ���ư��ִ� ���� UpdatePosition���� �ٽ� 0���� ���߾��ֵ��� �߰�
 *
 *<v12.5 - 2024_0108_�ֿ���>
 *1- playerDropTr �������� baseDropTr�� �����ϰ�,
 *�⺻ ��������� �κ��丮�� ����� ������ �ش� �κ��丮�� ���� �޾Ƽ� �ʱ�ȭ�ϵ��� UpdateInventoryInfo���� ���� ����
 *
 *2- ItemInfo�� iicMisc�� ���� �����ϴ� ����� ���� ��
 *visualManager ������ �߰��Ͽ� �ش� �޼��带 ���� �������� ������ ������� ���� 
 *
 *innerSprite, statusSprite�� visualManager�� GetItemSprite�޼��带 ���� ȣ��� ����
 *
 *
 *<v12.6 - 2024_0109_�ֿ���>
 *1- 2D ������Ʈ�� 3D������Ʈ�� �и��ؼ� �����ϸ鼭 ���� ItemInfo���� OnEnable���� 3D������Ʈ�� ItemTr�� �ٷ� ���� �� ���� �Ǹ鼭
 *�ؽ�Ʈ ������ ������ �ʴ� ������ ����
 *=> ���� OnEnable�� �ƴ϶� Update�������� OnItemCreated�޼��忡�� �ٽ� �⵵�� ����
 *
 *2- OnItemCreatedInInventory�޼��带 OnItemAdded�� ����, OnItemCreatedInWorld�޼��带 OnItemCreated�� ���� �� ��������
 *������ �޼����� �������� OnItemCreated�� �������� �׻� ���� �󿡼� ������ ���� ������ ������ �����ϱ� ���ؼ��̸�,
 *OnItemAdded�� ������ ���忡�� ���� ���� �κ��丮�� �־��� ���� ���������� ���� �޼����̱� ����
 *
 *3- UpdateInventoryInfo���� baseDropTr�� null�� ����ְ� �ִ��� ����
 *OnItemWorldDrop�� �κ��丮�� RemoveItem�� ���� ���ְ� �Ǵµ�, �����ġ�� null�� ��ƹ����� ������ ��� �Ұ����� ���� �߻��Ͽ��� ����
 *
 *<v12.7 - 2024_0110_�ֿ���>
 *1- UpdateActiveTabInfo�� �������� ���� ȣ���ڸ� �Ű������� �־ ȣ���ϴ� ������� �����Ͽ� �ܺο����� ȣ�Ⱑ���ϰ� ��.
 *UpdateInventoryInfo���� ��Ƽ������ ������ �о�� ���� InventoryInteractive Ŭ������ ������Ƽ�� ���������Ͽ� �������� ������� ����
 *
 *<v12.8 - 2024_0111_�ֿ���>
 *1- ����Ʈ �������� �߰��ϸ鼭 UpdatePositionInSlotList���� ��ü���� ��쿡 ������ �۵����� ���ϵ��� ����
 *
 *<v12.9 - 2024_0112_�ֿ���>
 *1- ����Ʈ���� ������� ���¿��� �������� ������ ����Ʈ ���Կ� ������Ʈ �Ǵ� ������ �־ UpdatePositionInSlotList��
 *����Ʈ�������� �ƴϰ�, ������ �����̶��, ������ ������Ʈ�� ���������ʵ��� ���ǰ˻繮�� �߰�
 *
 *<v13.0 - 2024_0116_�ֿ���>
 *1- ���� ���ͷ�Ƽ�� ��ũ��Ʈ�� �������� �����ϴ� curActiveTab������ �߰�
 *�Ǻ����� �Ͼ��, ������ Add�� �Ͼ�� �������� ������Ʈ �Ǿ�� �ϸ�,
 *UpdatePositionInSlot �޼��尡 ȣ�� �� �� ���� �� ���¿� ���� ��ġ�� ������Ʈ �Ǿ�� �Ѵ�.
 *(��, ���� �ǿ� �ش��ϴ� �������� �ƴ� ��� �󸮽�Ʈ�� �̵��ϴ� ���� �־�� �Ѵ�.)
 *
 *2- curActiveTab������ ���Ӱ� �����Ͽ� �������� ���� Ȱ��ȭ �������� ������ �ֵ��� �Ͽ���. (������ ������Ʈ �� �ʿ��ϹǷ�)
 *
 *3- UpdateActiveTabInfo�޼��忡�� Ȱ��ȭ�� ���� ������Ʈ �� �Ű����ڷ� curActiveTab�߰�
 *
 *4- UpdateActiveTabInfo �Ű����� ���� �����ε� �޼��� �߰��Ͽ�
 *������ �ʿ��� OnItemAdded���� Ȱ��ȭ�������� ���ͷ�Ƽ�� ��ũ��Ʈ�κ��� �������� �޾� ������Ʈ �� �� �ְ� �Ͽ���.
 *
 *5- UpdatePositionInSlotList�޼���� ���� -> UpdatePositionInfo
 *
 *6- �ű� �޼��� MoveToEmptyList�� �����, UpdatePositionInfo�޼����� ���� �Ϻ� �ڵ带 MoveToSlot���� ���� ��
 *curActiveTab�� ���� ���� �б��Ͽ� ������ ������Ʈ�� �����ϵ��� ����
 *
 * <v13.1 - 2024_0116_�ֿ���>
 * 1- �������� ���� �����ִ� ���� ��ȯ�ϴ� CurActiveTab �б����� ������Ƽ �߰�
 * (�ٸ� �κ��丮�� ���� ����� ���� �������� � �ǿ� �����־����� Ȯ�� �� ����, 
 * ������ �κ��丮�� ���� ��ġ���� ������ ����ó������ ������ �������� ������� ó�� ���̰� �Ǳ� ����)
 * 
 * <v13.2 - 2024_0117_�ֿ���>
 * 1- UpdateCountTxt�޼��带 UpdateTextInfo�� �����Ͽ�����, 
 * �Ϻ� �ڵ带 InitCountTxt�޼���� �и��Ͽ� OnItemCreated�޼��忡�� �ѹ��� ȣ���ϵ��� ����
 * 
 * ���� ���� �Ӹ� �ƴ϶� ����â�� spec �ؽ�Ʈ���� ��������� �ִٸ� �ݿ��ؾ� �ϹǷ�,
 * ItemMisc�Ӹ� �ƴ϶� ��� �������� OnItemAdded�� �� ȣ���ϵ��� ����
 * (���� ������ ItemInfo_4.cs�� ���� �� Ȱ��)
 * 
 * 2- SlotIndex ������Ƽ�� SlotIndexEach�� ����
 * 
 * <v13.3 - 2024_0123_�ֿ���>
 * 1- �������� �ݶ��̴��� ���� ���� itemCol���� �߰��Ͽ� OnItemCreated���� ������ �� itemTr�� ���� �ʱ�ȭ�ϵ��� ����
 * 
 * <v13.4 - 2024_0124_�ֿ���>
 * 1- playerTr�� ownerTr�� ����
 * 
 * 2- ������ ������ ������ ��Ÿ���� OwnerId�� �߰�
 * 
 * <v13.5 - 2024_0124_�ֿ���>
 * 1- OwnerId�� Ÿ���� string���� int������ ���� �� �ּ� ����
 * ������ ������� id�� �������� �ο��ϱ� ����
 * 
 * 2- OwnerTr �� ItemTr �б����� ������Ƽ �߰� 
 *
 * <v13.6 - 2024_0125_�ֿ���>
 * 1- �б����� ������Ƽ OwnerName�� worldOnwnerName�� �߰��Ͽ���.
 * ������ �������� ����ɶ� �����ڸ� ���� �����ؾ� Id�� �ν��� �� Ű������ ������ �����ϱ� ����
 * 
 * 2- worldInventoryInfo�� �߰��Ͽ�, visualManager��ũ��Ʈ�� ��ġ�� ���� ������Ʈ�� �����ϵ��� �Ͽ���.
 * �̴� ItemInfo_3.cs�� RigesterToWorldInventory�޼��忡�� ����ϱ� ����
 * 
 * 3- SwitchAppearAs2D�޼��带 OnItemCreated���� ȣ�����ֵ��� ��.
 * �������� �����ϰ� �ٷ� ����ϴ� ��� DimensionShift�� ������� �ʾұ� ������ 2D����� ���ᰡ ���� ���� ���·� ������ �������� �Ͼ.
 * 
 * <v13.7 - 2024_0125_�ֿ���>
 * 1- �������� 3D������ �� ĵ�����׷��� blockRaycasts�� �ڵ�� false�� ���� ������� �ʴ� ������ �־
 * (�̴� 3D�����϶��� ĵ�����׷��� ���� ����ĳ������ �ްų� �긮�� ����� �ֱ� �����ΰ����� �������µ�)
 * => �̹��� ������Ʈ�� ����ĳ��Ʈ ����� �����ϰ�, Interactable�Ӽ��� ��Ȱ��ȭ ��Ű�� ������ ����
 * 
 * <v13.8 - 2024_0125_�ֿ���>
 * 1- OwnerId�̿��� ������ ��ü�� Id ������Ƽ �߰��Ͽ���.
 * ������ ���� �κ��丮�� �������� ����� �� ������ ���������� ���� ������ �־�� ������, 
 * �κ��丮�� ��� ������ �̸��� �������� ����� �� ���� �ĺ���ȣ�� �ʿ��ϱ� ����(���������� �޸� �ϱ� ���ؼ�)
 * 
 * <v14.0 - 2024_0126_�ֿ���>
 * 1- Id�������� ItemId�� ����
 * 2- UpdateInventoryInfo���� ����� ������ ownerId�� 0���� -1�� �ʱ�ȭ�ϴ� ������ ����
 * 
 * 
 * <v14.1 - 2024_0126_�ֿ���>
 * 1- ItemTr ������Ƽ ���� Item3dTr�� ���� 
 * (����� �� ȥ������ �ʱ� ����)
 * 
 * <v14.2 - 2024_0126_�ֿ���>
 * 
 * 1- TrApplyRange ������ �ɼ��� �߰��Ͽ� ���� ������ ����� �� Transform ���� ���� �� 
 * � �κи� �����ų �� ���ð����ϰ� �Ͽ���.
 * 
 * 2- OnItemWorldDrop�޼��带 �⺻������ ���޹��� Transform ������ ��ġ���� �����Ű���� �Ͽ�����,
 * TrApplyRange�� ���ڷ� ���޹޾� ȸ���� �����ϰ��� ���������� �����ų �� �ְ� ����. �ּ��� ����
 * 
 * 
 * 3- �������� �κ��丮â�� �ݾƵ� Ű����� ������ �Ͼ�� ������ �־�
 * SwitchAppearAs2D�޼��带 public ���� �����Ͽ� �κ��丮 â�� ���� �� 
 * �κ��丮�� ȣ���ϵ��� �Ͽ� ������ �������� ����.
 * 
 * <v14.3 - 2024_0128_�ֿ���>
 * 1- �б����� ������Ƽ StatusWindowInteractive�� �߰��Ͽ� ���������� �� �ش� ����â�� �� �� �ְ� �Ͽ���.
 * ������ �Ҹ� ������ ��Ŭ�� �� ���ŵǴ� ���� PointerExit�̺�Ʈ�� �߻����� �ʱ� ������ ����â�� �����ֱ� ������ ������ ����������� �ʿ伺�� ����  
 * 
 * <v14.4 - 2024_0128_�ֿ���>
 * 1- UpdatePositionInfo�޼��� ���� MoveToSlot�޼��忡�� itemRectTr�� rotation���� �ʱ�ȭ�ϰ� �ִ� �κ��� localRotation�� �ʱ�ȭ�ϵ��� ����
 * (�����۽��� �� 2D �̹����� ȸ���ع����� ������ ������)
 * 
 * <v14.5 - 2024_0129_�ֿ���>
 * 1- �������� Rigidbody itemRb ���� �� ������Ƽ �Ӽ��� �߰� �� OnItemCreated���� �ʱ�ȭ
 * ������ ���� �� ���� �� �߷��� OnOff�� �ʿ伺�� �����Ƿ�
 * 
 */

/// <summary>
/// ������ ���� ���� �޼��忡�� Transform ���ڸ� ������ �� � �κ��� ���� ��ų �� ������ �� �ֽ��ϴ�.<br/>
/// ��ġ���� ���� ��ų��, ��ġ���� ȸ������ ���ÿ� �����ų ��, ��ġ, ȸ��, ũ�� ��� ���� �����ų �� ������ �� �ֽ��ϴ�.
/// </summary>
public enum TrApplyRange { Pos, Pos_Rot, Pos_Rot_Scale }

/// <summary>
/// ���� ���� ������ ������Ʈ�� �� Ŭ������ ������Ʈ�� �������մϴ�.<br/><br/>
/// 
/// ItemInfo ��ũ��Ʈ�� ������Ʈ�� ���� ������ ������Ʈ�� ��ü���� ����� ������ �����ϴ�.<br/>
/// (ItemInfo�� ���� ������ �ν��Ͻ��� item�� �Ҵ�� �� �ڵ����� �̷�����ϴ�.)<br/><br/>
/// 
/// 1.������Ʈ�� �̹����� ���� �������� ������ �����Ͽ� ä��ϴ�.<br/>
/// 2.��ȭ�������� ��� ��øȽ���� ������������ ���Ͽ� ǥ���Ͽ� �ݴϴ�. ����ȭ �������� ��� �ؽ�Ʈ�� ���ϴ�.<br/>
/// 3.�κ��丮 ���� ���� �������� ������ ������ �����Ͽ� �ش� ���Կ� ��ġ��ŵ�ϴ�.<br/><br/>
/// 
/// ����) �������� ���� ������ �ٲ� �� ���� �ֽ� ������ ������Ʈ�� �ݿ��ؾ� �մϴ�.<br/>
/// 1,2,3�� ��� �� �޼��带 ���� ȣ�� �� �� ������ ��� ���� �ѹ��� ȣ���ϴ�  OnItemChanged�޼��尡 �ֽ��ϴ�.<br/>
/// </summary>
public partial class ItemInfo : MonoBehaviour
{
    /**** ������ ���� ���� ****/
    Item item;              // �������� ���� ������ ��� ����
    
    VisualManager visualManager;    // �������� Sprite�̹����� ���޹ޱ� ���� ������ ����� ���� Ŭ���� ����
    Image itemImage;                // �������� �κ��丮���� 2D�󿡼� ������ �̹��� ������Ʈ ����  
    public Sprite innerSprite;      // �������� �κ��丮���� ������ �̹��� ��������Ʈ
    public Sprite statusSprite;     // �������� ����â���� ������ �̹��� ��������Ʈ (����â ��ũ��Ʈ���� ������ �ϰ� �˴ϴ�.)
    Text countTxt;                  // ��ȭ �������� ������ �ݿ��� �ؽ�Ʈ
            
    RectTransform itemRectTr;       // �ڱ��ڽ� 2D Ʈ������ ����(�ʱ� ���� - ���� �θ�)
    Transform itemTr;               // �ڱ��ڽ� 3D Ʈ������ ����(�ʱ� ���� - ���� ������ �ڽ�)
    CanvasGroup itemCG;             // �������� ĵ���� �׷� ������Ʈ (�������� ����� ������ �� 2D�̺�Ʈ�� �������� �뵵) 
    Collider itemCol;               // �������� 3D������Ʈ�� ������ �ִ� �⺻ �ݶ��̴�
    Rigidbody itemRb;               // �������� 3D������Ʈ�� ������ �ִ� ������ٵ�

    InventoryInfo worldInventoryInfo;   // �������� 3D���·� ������ �� �ִ� ���� �κ��丮 ����

    /*** ������ ���� ���� ***/

    /*** Locate2DToWorld �Ǵ� Locate3DToWorld �޼��� ȣ�� �� ����***/
    bool isWorldPositioned;         // �������� ���忡 �����ִ��� ����

    /**** InventoryInfoChange �޼��� ȣ�� �� ���� ****/
    Transform inventoryTr;              // ���� �������� ����ִ� �κ��丮�� ���������� �����մϴ�.
    Transform slotListTr;               // ���� �������� ����ִ� ���Ը���Ʈ Ʈ������ ����
    Transform emptyListTr;              // �������� �ӽ÷� �̵� ��ų ����� ����Ʈ

    InventoryInfo inventoryInfo;                // ���� �������� ���� �� �κ��丮���� ��ũ��Ʈ
    InventoryInteractive inventoryInteractive;  // ���� �������� ���� �� �κ��丮 ���ͷ�Ƽ�� ��ũ��Ʈ
    StatusWindowInteractive statusInteractive;  // ���� �������� ���� �� ����â ���ͷ�Ƽ�� ��ũ��Ʈ

    Transform ownerTr;                  // ���� �������� �����ϰ� �ִ� �κ��丮 ������ ���� ����

    Transform baseDropTr;               // �������� ������ �⺻ ��� ��ġ
    bool isBaseDropSetParent;           // �⺻ �����ġ�� �θ��� �ɼ��� �ɷ��ִ��� ����


    /**** InventoryInfoChange �޼��� ȣ��� ���� ****/
    /**** inveractive���� �����Ͼ� �� ������ ����*****/
    bool isActiveTabAll;                // ���� �������� ����ִ� �κ��丮�� Ȱ��ȭ ���� ������ ��ü����, �������� ����
    TabType curActiveTab;               // ���� �������� ����ִ� �κ��丮�� Ȱ��ȭ �� ����
    

    
    /**** InventoryInfoChange �޼��� ȣ��� ���� ****/
    /**** OnItemSlotDrop �̺�Ʈ ȣ�� �� ���� ****/
    Transform prevDropSlotTr;    // ����̺�Ʈ�� �߻��� �� ������ ����̺�Ʈ ȣ���ڸ� ����ϱ� ���� ���� ���� 



    
    /*** ���ο����� ���������� �б����� ������Ƽ***/

    /// <summary>
    /// �������� ���忡 �����ִ��� (3D ������Ʈ ����) ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsWorldPositioned { get {return isWorldPositioned;} }
    
    /// <summary>
    /// ���� �������� ��� �κ��丮�� �����Դϴ�.
    /// </summary>
    public InventoryInfo InventoryInfo { get {return inventoryInfo;} }


    /// <summary>
    /// ���� �������� ��� �κ��丮 ���ͷ�Ƽ�� ��ũ��Ʈ �������� ��ȯ�մϴ�.
    /// </summary>
    public InventoryInteractive InventoryInteractive { get { return inventoryInteractive;} }


    /// <summary>
    /// ���� �������� ��� �κ��丮�� ����â ���ͷ�Ƽ�� ��ũ��Ʈ �������� ��ȯ�մϴ�.
    /// </summary>
    public StatusWindowInteractive StatusWindowInteractive { get { return statusInteractive;} }


    /// <summary>
    /// ���� �������� ����ִ� ���Ը���Ʈ�� Transform�� ��ȯ�մϴ�.
    /// </summary>
    public Transform SlotListTr { get {return slotListTr;} }

    /// <summary>
    /// ���� �������� �����ִ� ������ ���� ��ȯ�մϴ�.
    /// </summary>
    public TabType CurActiveTab { get { return curActiveTab;} }

    /// <summary>
    /// ���� ������ 3D ������Ʈ�� �ֻ����� �����Ǿ� �ִ� �ݶ��̴��Դϴ�.
    /// </summary>
    public Collider ItemCol { get { return itemCol;} }

    /// <summary>
    /// ���� ������ 3D ������Ʈ�� �ֻ����� �����Ǿ��ִ� ������ٵ��Դϴ�.
    /// </summary>
    public Rigidbody ItemRb { get { return itemRb;} }



    /// <summary>
    /// �������� 3D�� ��ȯ�Ǿ� ���忡 ������ ���°� �� ��, �ش� �������� �����ڸ��� ���մϴ�.
    /// </summary>
    public string worldOwnerName { get; } = "World";
        
    /// <summary>
    /// �������� ������ ���� ��ȯ�մϴ�.<br/>
    /// �������� �κ��丮�� �����ȴٸ� �ش� �κ��丮�� ���� �ֻ��� ������Ʈ ���� �����ڸ��� �˴ϴ�.
    /// </summary>
    public string OwnerName { get { return item.OwnerName; } }

    /// <summary>
    /// �������� �����ڸ� �ĺ��� �� �ִ� ���� ���ڸ� ��ȯ�մϴ�.<br/>
    /// �̴� �ش� �κ��丮�� �����ϰ� �ִ� ���� �ֻ��� �θ� ������Ʈ�� ���� �ĺ� ��ȣ�Դϴ�.
    /// </summary>
    public int OwnerId { get { return item.OwnerId; } }
    

    /// <summary>
    /// ������ �������� Transform �������� ��ȯ�մϴ�.<br/>
    /// �̴� �ش� �κ��丮�� �����ϰ� �ִ� ���� �ֻ��� �θ� ������Ʈ�� ���մϴ�.
    /// </summary>
    public Transform OwnerTr { get { return ownerTr; } }


    /// <summary>
    /// �������� 3D ������Ʈ�� ������ �ִ� Transform ������Ʈ �������� ��ȯ�մϴ�.
    /// </summary>
    public Transform Item3dTr { get { return itemTr; } }

    /// <summary>
    /// ������ ������ �ĺ���ȣ�Դϴ�.<br/>
    /// ������ �̸��� �����ۿ� �ĺ���ȣ�� �ʿ��� ��� �ο��� �� �ֽ��ϴ�. (ex. �κ��丮 ������)<br/>
    /// �ĺ���ȣ�� �ο����� ���� �������� Id �⺻���� -1�Դϴ�.
    /// </summary>
    public int ItemId {get {return item.Id;} }



    /*** ���� �������� �Ӽ��� �������ִ� ������Ƽ ***/

    /// <summary>
    /// ������ ������Ʈ�� �ش��ϴ� �ǿ��� ��� ° ���Կ� ����ִ� �� �ε����� ��ȯ�ϰų� �����մϴ�.
    /// </summary>
    public int SlotIndexEach { get{return item.SlotIndexEach;} set{ item.SlotIndexEach=value;} }

    
    /// <summary>
    /// ������ ������Ʈ�� ��ü �ǿ��� ��� ° ���Կ� ����ִ� �� �ε����� ��ȯ�ϰų� �����մϴ�.
    /// </summary>
    public int SlotIndexAll { get{return item.SlotIndexAll;} set{item.SlotIndexAll=value;} }
      
    /// <summary>
    /// �������� ����ִ� ���� ������ ���� �����ϰų� ��ȯ�޽��ϴ�.<br/>
    /// Ŭ�� �� Item �ν��Ͻ��� �����ϰ�, ���� �Ǿ��ִ� �ν��Ͻ��� �ҷ��� �� �ֽ��ϴ�.<br/>
    /// </summary>
    public Item Item { set{ item=value; } get { return item; } }
    

    



    /// <summary>
    /// �̹��� ������Ʈ�� ��� �켱������ ���̱� ���� OnEnable ���<br/>
    /// �ڱ��ڽ��� ������Ʈ ������ ������ ����� ���� �ܺ� ������Ʈ�� ������ ��µ� ����մϴ�.<br/>
    /// </summary>
    private void OnEnable()
    {
        // �ڱ��ڽ� 2d Ʈ������ ����(���� ���� �� - �ڽ� ������Ʈ)
        itemRectTr = transform.GetComponent<RectTransform>();   

        itemImage = GetComponent<Image>();
        countTxt = GetComponentInChildren<Text>();
        
        itemCG = GetComponent<CanvasGroup>();
        visualManager = GameObject.FindWithTag("GameController").GetComponent<VisualManager>();
        worldInventoryInfo = visualManager.GetComponentInChildren<InventoryInfo>();
    }




    /// <summary>
    /// �������� �κ��丮 ���ο� �߰��Ǵ� ��쿡 ȣ�� �� �޼����Դϴ�.<br/>
    /// �������� ���� ������ �������� �κ��丮�� �߰��ǰ� ǥ���Ǳ� ���� �۾��� �����մϴ�.<br/><br/>
    /// ���� InventoryInfo�� AddItem�޼���� Load�޼��忡�� ���ǰ��ֽ��ϴ�.<br/>
    /// </summary>
    public void OnItemAdded(InventoryInfo inventoryInfo)
    {
        /*** ���� �� ���� �� ����ó�� ***/
        if(inventoryInfo==null)
            throw new Exception("�� �޼���� �κ��丮 ������ �ݵ�� �ʿ��մϴ�. Ȯ���Ͽ� �ּ���");
        
        // ���� �󿡼� �߰��Ǿ��ٸ� ������ ������ �����մϴ�
        if(isWorldPositioned)
            DimensionShift(false);
        
        // �ؽ�Ʈ ������ �ֽ�ȭ �մϴ�.     
        UpdateTextInfo();              

        // �κ��丮 ������ �ֽ�ȭ�մϴ�
        UpdateInventoryInfo(inventoryInfo); 

        // ���� ��ġ ������ �ֽ�ȭ�մϴ�
        UpdatePositionInfo();
    }
        


    /// <summary>
    /// �������� ���忡 ���Ӱ� �����Ǵ°�쿡 ȣ���ؾ� �� �޼����Դϴ�.<br/>
    /// ������ ������ �������� ������Ʈ�� �����մϴ�.<br/><br/>
    /// ���� CreateManager�� CreateItem�޼��忡�� ���ǰ� �ֽ��ϴ�.<br/>
    /// </summary>
    public void OnItemCreated()
    {   
        // �������� ���� ���¸� Ȱ��ȭ�մϴ�.
        isWorldPositioned = true;   

        // 2D������Ʈ�� �и��Ǿ ������ 3D ������Ʈ�� ���� ������ �����մϴ�
        // (2D������Ʈ ������ Canvas�� �� �� �� ���� ������ ���ɼ� ����!)
        itemTr = transform.parent;

        // 3D������Ʈ�� �ݶ��̴� ������ �����մϴ�.
        itemCol = itemTr.GetComponent<Collider>();      

        // 3D ������Ʈ�� ������ٵ� ������ �����մϴ�.
        itemRb = itemTr.GetComponent<Rigidbody>();
        
        // �������� ��ø���� �ؽ�Ʈ�� �ʱ�ȭ�մϴ�.
        InitCountTxt();

        // ������ ������Ʈ�� ���� ������ �о� �鿩 2D ������Ʈ�� �ݿ��մϴ�
        UpdateImage();          

        // �ؽ�Ʈ ������ �ֽ�ȭ �մϴ�.
        UpdateTextInfo();                   
                
        // �κ��丮 ������ �ʱ�ȭ�մϴ�
        UpdateInventoryInfo(null);
        
        // 2D ����� �ߴ��� ���·� �����մϴ�.
        SwitchAppearAs2D(true);
    }


    





    /// <summary>
    /// �������� �̹��� ������ �޾ƿͼ� ������Ʈ�� �ݿ��մϴ�.<br/>
    /// Item Ŭ������ ���ǵ� �� �ܺο��� ������ �̹��� �ε����� �����ϰ� �ֽ��ϴ�.<br/>
    /// �ش� �ε����� �����Ͽ� �ν����ͺ信 ��ϵ� �̹����� �����մϴ�.
    /// </summary>
    public void UpdateImage()
    {        
        // ������ ��������Ʈ �̹����� visual ���� �޼��带 ���� �ڽ��� ������ �����Ͽ� �������� �޾� �����մϴ�.
        innerSprite = visualManager.GetItemSprite(this, SpriteType.innerSprite);
        statusSprite = visualManager.GetItemSprite(this, SpriteType.statusSprite);
        
        // ������ ��������Ʈ �̹����� ������� �������� ������ 2D�̹����� �����մϴ�.
        itemImage.sprite = innerSprite;
    }


    /// <summary>
    /// �������� �ؽ�Ʈ ������ ������Ʈ �մϴ�.<br/>
    /// �������� ���� ��ġ�� �������� �� �ڵ����� ȣ�����ֱ� ���� �޼����Դϴ�.
    /// </summary>
    public void UpdateTextInfo()
    {        
        // ����â�� ǥ�õ� ���������� �ֽ�ȭ �մϴ�.
        strSpec = GetItemSpec();

        // ��ȭ �������̶�� ��ø ���������� �ֽ�ȭ�մϴ�.
        ItemMisc itemMisc = item as ItemMisc;
        if(itemMisc!=null)
            countTxt.text = itemMisc.OverlapCount.ToString();
    }


    /// <summary>
    /// �������� ��ø ����ǥ�ø� �ʱ�ȭ�մϴ�.<br/>
    /// ��ȭ ������ ������ ���� ǥ�ø� Ȱ��ȭ�ϰ�, ����ȭ �������� ��� ����ǥ�ø� ��Ȱ��ȭ�մϴ�.
    /// </summary>
    private void InitCountTxt()
    {
        if(item.Type is ItemType.Misc)
            countTxt.enabled = true;
        else
            countTxt.enabled = false;
    }








        
    /// <summary>
    /// ���� �������� ���� ����Ʈ�� �����ִٸ� ��ġ�� ���� �ε����� �°� �ֽ�ȭ�����ݴϴ�.<br/>
    /// ���� �ε����� �߸��Ǿ� �ִٸ� �ٸ� ��ġ�� �̵� �� �� �ֽ��ϴ�.<br/><br/>
    /// ** �������� ���� �� �ְų� ���� ������ �� ���� ��� ���ܸ� �����ϴ�. **<br/>
    /// </summary>
    public void UpdatePositionInfo()
    {
        // ���� ����Ʈ�� ������ �����Ǿ����� �ʴٸ� ���������� �������� �ʽ��ϴ�.
        if( slotListTr.childCount==0 )
        {
            Debug.Log( "���� ������ �������� ���� �����Դϴ�." );
            return;
        }

        // �������� Ÿ���� �޾ƿɴϴ�.
        ItemType itemType = item.Type;


        /**** �������� �ǿ� ǥ�� ���� ���� ���� ****/
        // 1. ��ü���� ��� ����Ʈ �������� �����ϰ� ǥ���մϴ�.
        // 2. ��ü���� �ƴ϶�� ����Ȱ��ȭ �ǰ� �������� ���� ���� ��ġ�ؾ� ǥ���մϴ�.

        // ���� Ȱ��ȭ���� ��ü ���ΰ��
        if(curActiveTab == TabType.All)
        {
            // ����Ʈ �������̶��, EmptyList�� �̵��մϴ�.
            if( itemType==ItemType.Quest )
            {
                MoveToEmptyList(); // �� �������� �� ����Ʈ�� �̵���ŵ�ϴ�.
                return;
            }
        }
        // �������� ��ü���� �ƴ� ���, �������� ��Ÿ���� ����Ȱ��ȭ ���� �ƴ϶��
        else if( Inventory.ConvertItemTypeToTabType( item.Type )!=curActiveTab )
        {
            MoveToEmptyList();      // �� �������� �� ����Ʈ�� �̵���ŵ�ϴ�.
            return;
        }


        MoveToSlot();   // �������� �������� �̵���ŵ�ϴ�.
    }


    
    /// <summary>
    /// �������� �Ͻ������� �󸮽�Ʈ�� �̵���Ű�� �޼����Դϴ�.<br/>
    /// ������ ���� �� ���������� ���˴ϴ�.
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void MoveToEmptyList()
    {
        if(isWorldPositioned)
            throw new Exception("���� ������ ���� �������� �̵��� �� �����ϴ�.");
        if(emptyListTr==null)
            throw new Exception("���� ������ �������� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");

        itemRectTr.SetParent(emptyListTr, false);
    }

    /// <summary>
    /// �������� �ε��� ������ �����Ͽ� �ش�Ǵ� �������� �̵������ִ� �޼����Դϴ�.<br/>
    /// ���� Ȱ��ȭ ���� �� ������ ���� �̵����Ѿ� �ϹǷ� �����ؼ� ����ؾ� �մϴ�.<br/>
    /// </summary>
    private void MoveToSlot()
    {
        if(isWorldPositioned)
            throw new Exception("���� ������ ���� �������� �̵��� �� �����ϴ�.");
        if(slotListTr==null)
            throw new Exception("���� ������ �������� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");

        

        // ���� Ȱ��ȭ ���� ���� ������� � �ε����� �������� �����մϴ�.
        int activeIndex = isActiveTabAll? item.SlotIndexAll : item.SlotIndexEach;
        
        // �������� ũ�⸦ ���Ը���Ʈ�� cellũ��� �����ϰ� ����ϴ�.(������ ũ��� �����ϰ� ����ϴ�.)
        itemRectTr.sizeDelta = slotListTr.GetComponent<GridLayoutGroup>().cellSize;

        // �������� �θ� �ش� �������� �����մϴ�.
        itemRectTr.SetParent( slotListTr.GetChild(activeIndex) );  

        // ��ġ�� ȸ������ �����մϴ�.
        itemRectTr.localPosition = Vector3.zero;
        itemRectTr.localRotation = Quaternion.identity;
    }









    /// <summary>
    /// �������� �κ��丮 ������ ����� �� ���ڷ� ���� InventoryInfo�� �������� �ٽ� �������ִ� �޼����Դϴ�.<br/>
    /// ���ο� �κ��丮�� ���õ� ���� ����� �������ڸ� �������� �ʱ�ȭ �մϴ�.<br/><br/>
    /// �������� ���� �κ��丮���� �����̰� �ִ� ���� ȣ��� �ʿ䰡 �����ϴ�.<br/>
    /// �������� ���ο� �κ��丮�� �����ų�, ���� �κ��丮���� ���� �� ȣ������� �մϴ�.<br/>
    /// </summary>
    public void UpdateInventoryInfo(InventoryInfo newInventoryInfo)
    {        
        // null���� ���޵� ���� ����� �����ٰ� �Ǵ��մϴ�.
        if(newInventoryInfo == null)  
        {
            inventoryTr = null;
            inventoryInfo = null;
            inventoryInteractive = null;
            statusInteractive = null;

            slotListTr = null;
            emptyListTr = null;
            prevDropSlotTr = null;

            ownerTr = null;                    // ������ ������ �ʱ�ȭ
            item.OwnerId = -1;                 // ������ ������ �ĺ� ��ȣ�� �ʱ�ȭ�մϴ�.
            item.OwnerName = worldOwnerName;   // ������ ������ ���� ����� �����մϴ�.
        }
        else // �ٸ� �κ��丮�� ���޵� ���
        {
            // �κ��丮 ���� ������ ������Ʈ �մϴ�.
            inventoryInfo = newInventoryInfo;
            inventoryTr = inventoryInfo.transform;
            inventoryInteractive = inventoryTr.GetComponent<InventoryInteractive>();
            statusInteractive = inventoryTr.GetComponent<StatusWindowInteractive>();

            if(inventoryInfo == null || inventoryInteractive == null )
                throw new Exception("�κ��丮 ���� ������ �߸��Ǿ����ϴ�. Ȯ���Ͽ� �ּ���.");
                        
            slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);
            emptyListTr = inventoryTr.GetChild(0).GetChild(1);
                                         
            baseDropTr = inventoryInfo.baseDropTr;                      // �⺻ �����ġ�� �θ����� �κ��丮�κ��� ����
            isBaseDropSetParent = inventoryInfo.isBaseDropSetParent;      
            prevDropSlotTr = slotListTr.GetChild(item.SlotIndexEach);   // ���� ����̺�Ʈ ȣ���ڸ� ���� ����ִ� �������� �ֽ�ȭ                                  

            UpdateActiveTabInfo();                                      // ��Ƽ�� �� ������ �ֽ�ȭ�մϴ�.   

            ownerTr = inventoryInfo.OwnerTr;                // ������ �����ڸ� �κ��丮 �����ڷ� �����մϴ�.
            item.OwnerId = inventoryInfo.OwnerId;           // ������ ������ �ĺ���ȣ�� ���� �����ۿ� �����մϴ�.
            item.OwnerName = inventoryInfo.OwnerName;       // ������ �����ڸ��� �κ��丮 �����ڸ����� �����մϴ�.
        }      
    }

    /// <summary>
    /// �������� ���� ���� �κ��丮�� Ȱ��ȭ �� ������ ������Ʈ�մϴ�.<br/>
    /// InventoryInteractive��ũ��Ʈ���� Ȱ��ȭ ���� ������ �õ��Ǿ��� �� ���ͷ�Ƽ�� ��ũ��Ʈ �ʿ��� ���� ����ִ� ��� ��������
    /// Ȱ��ȭ �� ������ �ֽ�ȭ �ϱ� ���� ȣ���ϴ� �޼����Դϴ�.<br/><br/>
    /// �ٸ� �κ��丮���� ������ ������ ������, Ȥ�� ���� �κ��丮 ������ �������� ������ ���� �� ȣ���� �����մϴ�.<br/><br/>
    /// ** ���� �������� ���� �κ��丮�� Interactive ��ũ��Ʈ�� �ƴ϶�� ���ܰ� �߻��մϴ�. **<br/>
    /// </summary>
    public void UpdateActiveTabInfo(InventoryInteractive caller, TabType curActiveTab, bool isActiveTabAll)
    {
        if(caller != inventoryInteractive)
            throw new Exception("���� �Ұ����� ȣ�����Դϴ�. Ȯ���Ͽ��ּ���.");
                
        this.curActiveTab = curActiveTab;
        this.isActiveTabAll = isActiveTabAll; 
    }

    /// <summary>
    /// �������� ���� ���� �κ��丮�� Ȱ��ȭ �� ������ ������Ʈ�մϴ�.<br/>
    /// �������� ���Ӱ� �߰��Ǿ��� �� �������ʿ��� �������� Interactive��ũ��Ʈ���Լ� ���� �Ҵ�޽��ϴ�.<br/>
    /// </summary>
    private void UpdateActiveTabInfo()
    {
        curActiveTab = inventoryInteractive.CurActiveTab;
        isActiveTabAll = inventoryInteractive.IsActiveTabAll;
    }

     






    /// <summary>
    /// �������� 2D ����� �ߴ��ϰų� �ٽ� Ȱ��ȭ��Ű�� �޼����Դϴ�.<br/>
    /// isWorldPositioned�� ������� �ֽ�ȭ�մϴ�.<br/>
    /// DimensionShift���� ���� �޼���� ���ǰ� �ֽ��ϴ�.
    /// </summary>
    public void SwitchAppearAs2D(bool isWorldPositioned)
    {        
        itemCG.interactable = !isWorldPositioned;
        itemCG.alpha = isWorldPositioned ? 0f:1f;
        itemImage.raycastTarget = !isWorldPositioned;
    }
           
    /// <summary>
    /// 2D�� 3D ������Ʈ�� �������踦 �����ϴ� �޼����Դϴ�.<br/>
    /// isWorldPositioned�� ������� �ڵ����� �ֽ�ȭ�մϴ�.<br/>
    /// DimensionShift���� ���� �޼���� ���ǰ� �ֽ��ϴ�.
    /// </summary>
    private void ChangeHierarchy(bool isWorldPositioned)
    {
        if(isWorldPositioned)
        {  
            itemTr.gameObject.SetActive(true);      // 3D������Ʈ�� Ȱ��ȭ   
            itemTr.SetParent( null );               // 3D������Ʈ�� �θ� ���Կ��� null���� ����            
            itemRectTr.SetParent(itemTr);           // 2D������Ʈ�� �θ� 3D������Ʈ�� ����
        }
        else
        {            
            itemRectTr.SetParent( emptyListTr );    // 2D ������Ʈ�� �θ� ��������� ����            
            itemTr.SetParent( itemRectTr );         // 3D ������Ʈ�� �θ� 2D ������Ʈ�� ����
            itemTr.gameObject.SetActive( false );   // 3D ������Ʈ ��Ȱ��ȭ
        }
    }
      


    /// <summary>
    /// ������ ������ 2D UI���� 3D ���� �Ǵ� 3D���忡�� 2D UI�� ��ȯ���ִ� �޼����Դϴ�. 
    /// ���� ���¸� Ȱ��ȭ �ϰ� ������Ʈ ���������� �����ϰ�, ��� ������ ���ִ� �޼����Դϴ�.<br/>
    /// </summary>
    public void DimensionShift(bool isMoveToWorld)
    {   
        // ������� ���� Ȱ��ȭ
        isWorldPositioned = isMoveToWorld;  
                
        // ���������� �����մϴ�.
        ChangeHierarchy(isMoveToWorld);

        // �������� ����� �����մϴ�
        SwitchAppearAs2D(isMoveToWorld);
    }
     




    /// <summary>
    /// �������� �κ��丮���� �������� ���� ���·� ��ȯ�Ǿ��� �� ��� �� ��ġ�� �������ֱ� ���� �޼����Դϴ�.<br/>
    /// **ù ��° �������� - Transform�� ������ ������ ������Ʈ�� �Է��մϴ�.<br/><br/>
    /// 
    /// ���ڸ� �������ϸ� �����ص� �����ġ(�÷��̾�)�� ���۵˴ϴ�.(�⺻��: ItemInfoŬ������ playerDropTr)<br/><br/>
    /// 
    /// **�� ��° �������� - TransferType�� ���޹��� Transform�� ������ �����ų�� �����ϴ� �ɼ��Դϴ�.<br/>
    /// (�⺻��: ��ġ ���� ����, �ɼ��� ���� ȸ�� ��, ũ�� ���� ����� ų �� �ֽ��ϴ�.)<br/><br/>
    /// 
    /// **�� ��° ���� ���� - isSetParent�� true�� ����� Transform ������ �ڽ����μ� ���ϰ� �˴ϴ�. (�⺻��: false)<br/><br/>
    /// </summary>
    public void OnItemWorldDrop(Transform dropPosTr=null, TrApplyRange transferType = TrApplyRange.Pos, bool isSetParent=false)
    {
        if( !IsWorldPositioned )
            inventoryInfo.RemoveItem(this);
        
        // ��� �������� ���� ���޵��� �ʾҴٸ�,
        if(dropPosTr==null)
            dropPosTr = baseDropTr;
        
        // 3D ������Ʈ�� �θ� ����
        if(isSetParent)
            itemTr.SetParent(dropPosTr);   
        
        // 3D ������Ʈ�� ��ġ�� ȸ���� ���� (TransferType�� ���� Transform�� �����ų ������ �����ϰ� �˴ϴ�.)         
        itemTr.position = dropPosTr.position;
                
        if( transferType >= TrApplyRange.Pos_Rot )
            itemTr.rotation = dropPosTr.rotation;

        if( transferType >= TrApplyRange.Pos_Rot_Scale)
            itemTr.localScale = dropPosTr.localScale;
    }



    /// <summary>
    /// ������ �������� �����ϴ� ��쿡 ������ �ʿ��� Ư�� �κ��丮�� �������� �߰��ϱ� ���� �ʿ��� �޼����Դϴ�.<br/>
    /// �κ��丮���� AddItem�޼��带 ȣ���ϴ� �Ͱ� �����ϹǷ� �� �� �ϳ��� ����Ͻʽÿ�.<br/>
    /// <br/><br/>
    /// *** �κ��丮 ������ �������� ������ ���ܰ� �߻��մϴ�. ***
    /// </summary>
    /// <param name="inventoryInfo"></param>
    /// <returns>�ش� �κ��丮�� ���Կ� �� �ڸ��� ���ٸ� false�� ��ȯ, ���� �� true�� ��ȯ</returns>
    public bool OnItemWorldGain(InventoryInfo inventoryInfo)
    {        
        // ���� ������ �� ����ó��
        if(inventoryInfo == null)
            throw new Exception("�κ��丮 ������ ���޵��� �ʾҽ��ϴ�. Ȯ���Ͽ� �ּ���.");
        // ���Կ� ���ڸ��� ���ٸ� ����ó��
        else if( !inventoryInfo.IsSlotEnough(this) )           
            return false;

        // �������� ���忡�� 2D���·� ��ȯ�մϴ�.
        DimensionShift(false);

        // �������� �κ��丮�� �߰��մϴ�.
        inventoryInfo.AddItem(this);

        // ������ ��ȯ�մϴ�.
        return true;
    }










    


    










}