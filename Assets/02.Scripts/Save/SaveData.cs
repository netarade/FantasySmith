
/*
 * [���� ����]
 * ���� ���̺�� �ε忡 �ʿ��� ������ Ŭ���� ���� 
 * 
 * [�۾� ����]  
 * <v1.0 - 2023_1106_�ֿ���>
 * 1- �ʱ� GameData ����
 * 
 * <v1.1 - 2023_1106_�ֿ���
 * 1- Transform ���� ����, Ŭ���� Craftdic ���� �߰�
 * 2- �ּ� ����
 * 
 * <v1.2 - 2023_1108_�ֿ���>
 * 1- Inventory Ŭ������ ���ӿ�����Ʈ ����Ʈ�� �����ϰ� �ֱ� ������ ����ȵǴ� ������ ������ �˰�
 * ���������� ItemInfo ����Ʈ�� ����� ������ �õ��Ͽ���. 
 * �� �ٸ� �������� �ִµ� ItemInfo�� Image������Ʈ�� �����ϰ� �ֱ� ������ Ŭ���� ������ ����ȭ�ϱ� ����ٰ� ������.
 * 
 * <v2.0 - 2023_1119_�ֿ���>
 * 1- Inventory Ŭ������ ����ȭ �� ������ȭ �����Ϸ�
 * 2- SerializableInventory Ŭ������ Inventory ���Ϸ� ��ġ�� �ű�.
 * �� ��ȯ�ÿ��� ����ϱ� ����
 * 3- �÷��̾� ��ġ, ȸ�� ������ ��� ���� STransform Ŭ���� ����
 * 
 * <v2.1 - 2023_1218_�ֿ���>
 * 1- ��ȭ ��ȭ ���� int������ ����
 * 
 * <v3.0 - 2023_1222_�ֿ���>
 * 1- DataManagerŬ������ Load�޼��忡�� GameData �⺻�����ڸ� ȣ���ϴµ� ���ǰ� �Ǿ����� �ʾ� ���Ӱ� ���� (STransform �⺻ ������ ����)
 * 
 * 2- saveInventory������ private���� public���� �����Ͽ���.
 * public�ƴ� ������ Json���� ����ȭ�� �Ұ����� ������ �Ǳ� ����
 * 
 * 3- �κ��丮 Ŭ������ ������Ƽ�� ����ȭ �κ��丮 Ŭ������ ��ȯ�Ͽ� �����ϴ� ���� �޼��带 ���� ��ȯ�Ͽ� �����ϵ��� ������.
 * (Inventory������Ƽ ����, SaveInventory, LoadInventory�޼��带 �߰�)
 * =>������ ������Ƽ�� set������ ����ȭ������ ������ �����Ѵٰ� �ϴ��� ������Ƽ ��ü�� ���ڸ��� ������ �Ǳ� ������ 
 * GameData�� �ش� ������Ƽ�� ���ԵǾ� ������ ����ȭ ó���� �Ұ����� ���� �߰��Ͽ���. 
 * (Inventory Ŭ������ List<GameObject>�� �����ϱ� ������ ������Ƽ�� GameData�� ���� ���� ����ȭó�� �Ұ����� ������ �ȵǾ���.)
 * 
 * 
 * <v3.1 - 2023_1222_�ֿ���>
 * 1- private������ ����ȭ�ϱ� ���� [JsonProperty] ��Ʈ����Ʈ�� �߰��Ͽ���
 * 2- STransform Ŭ������ Seiralize�޼��带 �߰��Ͽ� �ϰ����� �־�����, �ּ� ����
 * 
 * <v4.0 - 2023_1227_�ֿ���>
 * 1- GameDataŬ������ ������ �޼������ PlayerBasicData�� PlayerInvenData�� ���� ��, GameData�� �������̽� ó���Ͽ���.
 * ������ ���̺�� �ε带 ������ �۾��ڳ� ���Ǿ����� ��ũ��Ʈ ���� ��ġ�� Ʋ���� ����
 * 
 * 2- SerializableInventory�� LoadInventory �� SaveInventory�޼��带 �����ϰ�
 * Ŭ�������� SInventory�� ���� �� �ش� Ŭ���� ���ο� Serialize �޼���� Deserialize�޼��带 ȣ���ϰ� �Ͽ���.
 * ������ STransform�� �޼��带 ����ϰ� ����� ��뿡 �ϰ����� �ֱ� ����.
 * 
 * <v4.1 - 2023_1229_�ֿ���>
 * 1- Ŭ���� �� ���ϸ� ���� GameData->SaveData
 * 2- ���Ϻи� - SaveData_Player, SaveData_Inventory, SaveData_Transform���� �и� (���� Ŭ������ ������ ����)
 * 
 */



namespace DataManagement
{    
    /// <summary>
    /// ������ ���̺� �� �ε带 ���� �������̽��Դϴ�. <br/>
    /// ���̺��� ������ Ŭ������ ���� �� �� �������̽��� ����ؼ� Save�� Load�� �ؾ� �մϴ�. <br/>
    /// </summary>
    public interface SaveData { }
           


}