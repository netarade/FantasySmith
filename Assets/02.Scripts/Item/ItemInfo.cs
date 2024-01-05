using UnityEngine;
using UnityEngine.UI;
using ItemData;
using InventoryManagement;
using System;

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
 *
 *
 *
 */


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
    private Item item;             // �������� ���� ������ ��� ����

    private Image itemImage;       // �������� �κ��丮���� 2D�󿡼� ������ �̹��� ������Ʈ ����  
    public Sprite innerSprite;     // �������� �κ��丮���� ������ �̹��� ��������Ʈ
    public Sprite statusSprite;    // �������� ����â���� ������ �̹��� ��������Ʈ (����â ��ũ��Ʈ���� ������ �ϰ� �˴ϴ�.)
    private Text countTxt;         // ��ȭ �������� ������ �ݿ��� �ؽ�Ʈ
            
    private RectTransform itemRectTr;       // �ڱ��ڽ� 2D Ʈ������ ����(�ʱ� ���� - ���� �θ�)
    private Transform itemTr;               // �ڱ��ڽ� 3D Ʈ������ ����(�ʱ� ���� - ���� ������ �ڽ�)
    private CanvasGroup itemCG;             // �������� ĵ���� �׷� ������Ʈ (�������� ����� ������ �� 2D�̺�Ʈ�� �������� �뵵) 
    

    /*** ������ �ܺ� ���� ���� ***/
    public ItemImageCollection[] iicArr;                             // �ν����� �� �󿡼� ����� ������ �̹��� ���� �迭
    public enum eIIC { MiscBase,MiscAdd,MiscOther,Sword,Bow,Axe }    // �̹��� ���� �迭�� �ε��� ����
    private readonly int iicNum = 6;                                 // �̹��� ���� �迭�� ����



    /*** ������ ���� ���� ***/

    /*** Locate2DToWorld �Ǵ� Locate3DToWorld �޼��� ȣ�� �� ����***/
    private bool isWorldPositioned;         // �������� ���忡 �����ִ��� ����

    /**** InventoryInfoChange �޼��� ȣ�� �� ���� ****/
    private Transform inventoryTr;              // ���� �������� ����ִ� �κ��丮�� ���������� �����մϴ�.
    private Transform slotListTr;               // ���� �������� ����ִ� ���Ը���Ʈ Ʈ������ ����
    private Transform emptyListTr;              // �������� �ӽ÷� �̵� ��ų ����� ����Ʈ

    private InventoryInfo inventoryInfo;        // ���� �������� ���� �� �κ��丮���� ��ũ��Ʈ
    private InventoryInteractive interactive;   // ���� �������� ���� �� ���ͷ�Ƽ�� ��ũ��Ʈ

    private Transform playerTr;                 // ���� �������� �����ϰ� �ִ� �÷��̾� ĳ���� ���� ����
    private Transform playerDropTr;             // �÷��̾ �������� ��ӽ�ų �� �������� ������ ��ġ


    /**** InventoryInfoChange �޼��� ȣ��� ���� ****/
    /**** inveractive���� �����Ͼ� �� ������ ����*****/
    private bool isActiveTabAll;                // ���� �������� ����ִ� �κ��丮�� Ȱ��ȭ ���� ������ ��ü����, �������� ����

    
    /**** InventoryInfoChange �޼��� ȣ��� ���� ****/
    /**** OnItemSlotDrop �̺�Ʈ ȣ�� �� ���� ****/
    private Transform prevDropSlotTr;    // ����̺�Ʈ�� �߻��� �� ������ ����̺�Ʈ ȣ���ڸ� ����ϱ� ���� ���� ���� 





    
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
    /// ���� �������� ����ִ� ���Ը���Ʈ�� Transform�� ��ȯ�մϴ�.<br/>
    /// </summary>
    public Transform SlotListTr { get {return slotListTr;} }







    /*** ���� �������� �Ӽ��� �������ִ� ������Ƽ ***/

    /// <summary>
    /// ������ ������Ʈ�� �ش��ϴ� �ǿ��� ��� ° ���Կ� ����ִ� �� �ε����� ��ȯ�ϰų� �����մϴ�.
    /// </summary>
    public int SlotIndex { get{return item.SlotIndex;} set{ item.SlotIndex=value;} }

    
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
        itemRectTr = transform.GetComponent<RectTransform>();   // �ڱ��ڽ� 2d Ʈ������ ����(�ʱ� ���� - ���� �θ�)
        itemTr = itemRectTr.GetChild(itemRectTr.childCount-1);  // �ڱ��ڽ� 3d Ʈ������ ����(�ʱ� ���� - ���� ������ �ڽ�)

        itemImage = GetComponent<Image>();
        countTxt = GetComponentInChildren<Text>();
                
        // �ν����ͺ� �󿡼� �޾Ƴ��� ��������Ʈ �̹��� ������ �����մϴ�.
        Transform imageCollectionsTr = GameObject.FindAnyObjectByType<CreateManager>().transform.GetChild(0);

        // �迭�� �ش� ������ŭ �������ݴϴ�.
        iicArr = new ItemImageCollection[iicNum];

        // �� iicArr�� imageCollectionsTr�� ���� �ڽĿ�����Ʈ�μ� ItemImageCollection ��ũ��Ʈ�� ������Ʈ�� ������ �ֽ��ϴ�
        for( int i = 0; i<iicNum; i++)
            iicArr[i] = imageCollectionsTr.GetChild(i).GetComponent<ItemImageCollection>();
        
        itemCG = GetComponent<CanvasGroup>();
    }
    

    /// <summary>
    /// �������� ���Ӱ� �����Ǵ°�쿡 ȣ���ؾ� �� �޼����Դϴ�.<br/>
    /// �ε��Ͽ� ���� ������ ������ �������� ������Ʈ�� �����ϰų�, �ű� �������� ���� ������ų �� ����մϴ�.<br/><br/>
    /// ���� Inventory�� Load�޼���, Inventory�� CreateItem�޼���, CreateManager�� CreateItemToWorld�޼��忡�� ��뿹��<br/>
    /// **** ���ڷ� ���޹��� inventoryInfo�� slotIndex�� ������� ������Ʈ�� �����ֱ� ������<br/>
    /// �ű� �������� ���� �ÿ��� �ݵ�� �ε��� ������ �Է� �� ȣ���ؾ� �մϴ�. ****<br/>
    /// </summary>
    public void OnItemCreated(InventoryInfo inventoryInfo)
    {
        /*** ���� �� ���� �� ����ó�� ***/
        if(inventoryInfo==null)
            throw new Exception("�� �޼���� �κ��丮 ������ �ݵ�� �ʿ��մϴ�. Ȯ���Ͽ� �ּ���");

        /*** ������ ������Ʈ�� ���� ������ �о� �鿩 ������Ʈ�� �ݿ� ***/
        UpdateImage();                      // �̹��� �ֽ�ȭ
        UpdateCountTxt();                   // ��ø ���� ���� �ֽ�ȭ

        /*** �κ��丮 ������ �о�鿩 ������Ʈ�� �ݿ� ***/
        UpdateInventoryInfo(inventoryInfo); // �κ��丮 ������ �ֽ�ȭ
        UpdatePositionInSlotList();         // ���� ��ġ �ֽ�ȭ
    }



    /// <summary>
    /// �������� �̹��� ������ �޾ƿͼ� ������Ʈ�� �ݿ��մϴ�.<br/>
    /// Item Ŭ������ ���ǵ� �� �ܺο��� ������ �̹��� �ε����� �����ϰ� �ֽ��ϴ�.<br/>
    /// �ش� �ε����� �����Ͽ� �ν����ͺ信 ��ϵ� �̹����� �����մϴ�.
    /// </summary>
    public void UpdateImage()
    {
        if(iicArr.Length == 0 )     // ������ ���� ������ iicArr�� �����ϴ� ���� �����Ͽ� �ݴϴ�.
            return;

        int imgIdx = -1;            // ������ �̹��� �ε��� ����
                   
        switch( Item.Type )         // �������� ����Ÿ���� �����մϴ�.
        {

            case ItemType.Weapon:
                ItemWeapon weapItem = (ItemWeapon)Item;
                WeaponType weaponType = weapItem.WeaponType;  // �������� ����Ÿ���� �����մϴ�.

                switch (weaponType)
                {
                    case WeaponType.Sword :             // ����Ÿ���� ���̶��,
                        imgIdx = (int)eIIC.Sword;
                        break;
                    case WeaponType.Bow :               // ����Ÿ���� Ȱ�̶��,
                        imgIdx = (int)eIIC.Bow;
                        break;
                }
                break;
                
            case ItemType.Misc:
                ItemMisc miscItem = (ItemMisc)Item; 
                MiscType miscType = miscItem.MiscType;

                switch (miscType)
                {
                    case MiscType.Basic :           // ����Ÿ���� �⺻ �����,
                        imgIdx = (int)eIIC.MiscBase;
                        break;
                    case MiscType.Additive :        // ����Ÿ���� �߰� �����,
                        imgIdx = (int)eIIC.MiscAdd;
                        break;
                    default :                       // ����Ÿ���� ��Ÿ �����,
                        imgIdx = (int)eIIC.MiscOther;
                        break;
                }
                break;
        }

        // ������ ������Ʈ �̹����� �ν����ͺ信 ����ȭ�Ǿ� �ִ� ItemImageCollection Ŭ������ ���� ����ü �迭 ImageColection[]��
        // ����������� ���� ������ ������ �ִ� ImageReferenceIndex ����ü�� �ε����� �����ͼ� �����մϴ�.                
             
        innerSprite = iicArr[imgIdx].icArrImg[Item.ImageRefIndex.innerImgIdx].innerSprite;
        statusSprite = iicArr[imgIdx].icArrImg[Item.ImageRefIndex.statusImgIdx].statusSprite;

        // ������ ��������Ʈ �̹����� ������� �������� ������ 2D�̹����� �����մϴ�.
        itemImage.sprite = innerSprite;
    }


    /// <summary>
    /// ��ȭ �������� ��øȽ���� �������� �����մϴ�. ��ȭ �������� ������ ����� �� ���� ȣ���� �ֽʽÿ�.
    /// </summary>
    public void UpdateCountTxt()
    {
        if( Item.Type==ItemType.Misc )                // ��ȭ �������� ��ø ������ ǥ���մϴ�.
        {
            countTxt.enabled=true;
            countTxt.text = ((ItemMisc)Item).OverlapCount.ToString();
        }
        else
            countTxt.enabled = false;                // ��ȭ�������� �ƴ϶�� ��ø �ؽ�Ʈ�� ��Ȱ��ȭ�մϴ�.
    }
        
    /// <summary>
    /// ���� �������� ���� ����Ʈ�� �����ִٸ� ��ġ�� ���� �ε����� �°� �ֽ�ȭ�����ݴϴ�.<br/>
    /// ���� �ε����� �߸��Ǿ� �ִٸ� �ٸ� ��ġ�� �̵� �� �� �ֽ��ϴ�.<br/><br/>
    /// ** �������� ���� �� �ְų� ���� ������ �� ���� ��� ���ܸ� �����ϴ�. **<br/>
    /// </summary>
    public void UpdatePositionInSlotList()
    {
        // �������� ����� �����ְų� ������ ������ �ʾҴٸ�, ���ܸ� �����ϴ�.
        if( isWorldPositioned && slotListTr == null )   
            throw new Exception("������ ������ ������Ʈ �� �� �ִ� ��Ȳ�� �ƴմϴ�. Ȯ���Ͽ� �ּ���.");

        // ���� ����Ʈ�� ������ �����Ǿ����� �ʴٸ� ���������� �������� �ʽ��ϴ�.
        if( slotListTr.childCount==0 )
        {
            Debug.Log( "���� ������ �������� ���� �����Դϴ�." );
            return;
        }
                 
        // ���� Ȱ��ȭ ���� ���� ������� � �ε����� �������� �����մϴ�.
        int activeIndex = isActiveTabAll? item.SlotIndexAll : item.SlotIndex;
        itemRectTr.SetParent( slotListTr.GetChild(activeIndex) );     // �������� �θ� �ش� �������� �����մϴ�.
        itemRectTr.localPosition = Vector3.zero;                      // ������ġ�� ���Կ� ����ϴ�.
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
            interactive = null;

            slotListTr = null;
            emptyListTr = null;

            playerTr = null;
            playerDropTr = null;

            prevDropSlotTr = null;
        }
        else // �ٸ� �κ��丮�� ���޵� ���
        {
            // �κ��丮 ���� ������ ������Ʈ �մϴ�.
            inventoryInfo = newInventoryInfo;
            inventoryTr = inventoryInfo.transform;
            interactive = inventoryTr.GetComponent<InventoryInteractive>();

            if(inventoryInfo == null || interactive == null )
                throw new Exception("�κ��丮 ���� ������ �߸��Ǿ����ϴ�. Ȯ���Ͽ� �ּ���.");
                        
            slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);
            emptyListTr = inventoryTr.GetChild(0).GetChild(1);

            playerTr = inventoryTr.parent.parent;
            playerDropTr = playerTr;                // �÷��̾� ������� �ֽ�ȭ(���߿� ���濹��)
                                                    
            prevDropSlotTr = itemRectTr.parent;     // ���� ����̺�Ʈ ȣ���ڸ� ���� ����ִ� �������� �ֽ�ȭ
            UpdateActiveTabInfo();                  // ��Ƽ�� �� ���� �ֽ�ȭ   
        }      
    }

    /// <summary>
    /// ������ �ܺ� ��ũ��Ʈ�� �κ��丮�� interactive��ũ��Ʈ���� Ȱ��ȭ ���� ������ �õ��Ǿ��� ��<br/>
    /// Ȱ��ȭ �� ������ �ֽ�ȭ �ϱ� ���� ȣ���ϴ� �޼����Դϴ�.<br/><br/>
    /// �ٸ� �κ��丮���� ������ ������ ������, Ȥ�� ���� �κ��丮 ������ �������� ������ ���� �� ȣ���� �����մϴ�.<br/><br/>
    /// ** 3D ���忡 �ִ� ���¿��� ȣ���ϸ� ���ܸ� �߻���ŵ�ϴ�.<br/>
    /// </summary>
    public void UpdateActiveTabInfo()
    {
        if(IsWorldPositioned)
            throw new Exception("�������� ���忡 ���� ���� ȣ�� �� �� �����ϴ�. Ȯ���Ͽ� �ּ���.");

        isActiveTabAll = interactive.IsActiveTabAll; 
    }

     








    /// <summary>
    /// �������� 2D ����� �ߴ��ϰų� �ٽ� Ȱ��ȭ��Ű�� �޼����Դϴ�.<br/>
    /// isWorldPositioned�� ������� �ֽ�ȭ�մϴ�.<br/>
    /// DimensionShift���� ���� �޼���� ���ǰ� �ֽ��ϴ�.
    /// </summary>
    private void SwitchAppearAs2D(bool isWorldPositioned)
    {        
        // ���� �� �������� �ִٸ�, UI �̺�Ʈ�� �� �̻� ���� ������, 2D�̹����� ����ó���մϴ�.
        itemCG.blocksRaycasts = !isWorldPositioned;
        itemCG.alpha = isWorldPositioned ? 0f:1f;
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
    /// �������� ���� ���·� ��ȯ�Ǿ��� �� �ڵ����� dorp��ġ�� �������ֱ� ���� �޼����Դϴ�.<br/>
    /// </summary>
    private void SetDropPosition(Transform dropPosTr, bool isSetParent=true)
    {
        if( !IsWorldPositioned )
            return;
        
        // 3D ������Ʈ�� �θ� ����
        if(isSetParent)
            itemRectTr.SetParent(dropPosTr);
                
        // 3D ������Ʈ�� ��ġ�� ȸ���� ����
        itemTr.position = dropPosTr.position;
        itemTr.rotation = dropPosTr.rotation;
    }


    /// <summary>
    /// ������ ������ 2D UI���� 3D ���� �Ǵ� 3D���忡�� 2D UI�� ��ȯ���ִ� �޼����Դϴ�. 
    /// ���� ���¸� Ȱ��ȭ �ϰ� ������Ʈ ���������� �����ϰ�, ��� ������ ���ִ� �޼����Դϴ�.<br/>
    /// </summary>
    public void DimensionShift(bool isMoveToWorld)
    {   
        // ������� ���� Ȱ��ȭ
        isWorldPositioned = isMoveToWorld;  
                
        // �������� ����� �����մϴ�
        SwitchAppearAs2D(isMoveToWorld);
                    
        // ���������� �����մϴ�.
        ChangeHierarchy(isMoveToWorld);
    }
     




    /// <summary>
    /// ������ �������� �����ϴ� ��쿡 ������ �ʿ��� Ư�� �κ��丮�� �������� �߰��ϱ� ���� �ʿ��� �޼����Դϴ�.<br/>
    /// �κ��丮���� AddItem�޼��带 ȣ���ϴ� �Ͱ� �����ϹǷ� �� �� �ϳ��� ����Ͻʽÿ�.<br/>
    /// <br/><br/>
    /// *** �κ��丮 ������ �������� ������ ���ܰ� �߻��մϴ�. ***
    /// </summary>
    /// <param name="inventoryInfo"></param>
    /// <returns>�ش� �κ��丮�� ���Կ� �� �ڸ��� ���ٸ� false�� ��ȯ, ���� �� true�� ��ȯ</returns>
    public bool OnItemGain(InventoryInfo inventoryInfo)
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