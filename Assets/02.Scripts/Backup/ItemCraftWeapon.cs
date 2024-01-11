
/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- ���۹���(ItemCraftWeapon) Ŭ���� ���� �и�.
 * 2- �� �� �⺻ ���� �� ������Ƽ, �����ڿ� �޼��� ����
 * 3- �ּ�ó��
 * 
 * <v1.1 - 2023_1106_�ֿ���>
 * 1- CraftMaterial ����ü ������ �߰�
 * 2- �� �Ӽ��� ������ �� �� �ִ� ������Ƽ �߰�
 * 3- ������Ƽ �� ���� 
 * 
 * <v1.2 - 2023_1116_�ֿ���>
 * 1- ������ Ŭ���� ���� �������� ���� �����ڳ����� �̹��� ���� ������ �̹��� �ε��� ������ ����
 * 
 * <v2.0 - 2023_1222_�ֿ���>
 * 1- private������ ����ȭ�ϱ� ���� [JsonProperty] ��Ʈ����Ʈ�� �߰��Ͽ���
 * 
 * <v2.1 - 2023_1224_�ֿ���>
 * 1- Clone�޼��� ���� (ItemŬ�������� ���� ����� ����ϹǷ�)
 * 
 * <v3.0 - 2023_1226_�ֿ���>
 * 1- ������Ƽ ��� JsonIgnoreó��
 * (������Ƽ�� ������� ���� �� �ε�� ������Ƽ�� set�� ���� ������ ������ �ݿ����� �ʱ� ����)
 * 2- cmArr ������ sArr��Ī���� ����
 * 
 * <v3.1 - 2023_1229_�ֿ���>
 * 1- ���ϸ� ���� ItemData_CraftWeapon->ItemCraftWeapon
 * 
 * <v3.2 - 2024_0108_�ֿ���>
 * 1- ItemImageCollection�� ItemVisualCollection��Ī ��������
 * �������� �Ű������� imgRefIndex�� visualRefIndex ����
 * 
 * <v4.0 - 2024_0111_�ֿ���>
 * 1- ũ������ �帣�� �°� Ŭ���� ���� �������� ���� �ּ�ó��
 * 
 */

//namespace ItemData
//{
//    /// <summary>
//    /// ���� ���ۿ� �ʿ��� ��� - ��� �̸��� ����
//    /// </summary>
//    [Serializable]
//    public struct CraftMaterial
//    {
//        public string name;
//        public int count;

//        public CraftMaterial(string name, int count)
//        {
//            this.name = name;
//            this.count = count;
//        }
//    }
        
//    /// <summary>
//    /// �������� ���� - ��, ��, ��, ��, ��, ��, ��
//    /// </summary>
//    public enum Recipie { Ga,Na,Da,Ra,Ma,Eu,Ee }


    
//    /// <summary>
//    /// ���� ���� ������
//    /// </summary>
//    [Serializable]
//    public sealed class ItemCraftWeapon : ItemWeapon
//    {
//        /*** ���� ���� �Ӽ� ***/
//        [JsonProperty] private float fCraftChance;                 // ���� Ȯ�� (�⺻ ��޿� ���� ���� 90%, 60%, 25%)
//        [JsonProperty] private Recipie enumRecipie;                        // 2�ܰ� ������
//        [JsonProperty] private CraftMaterial[] sArrBaseMaterial;          // ���� �� �ʿ��� �⺻ ���
//        [JsonProperty] private CraftMaterial[] sArrAdditiveMaterial;      // ���� �� �ʿ��� �߰� ���
//        [JsonProperty] private int iFirePower;                   // 2�ܰ� ���ۿ� �ʿ��� ȭ��

//        /// <summary>
//        /// ���� ������ �⺻ ���� Ȯ���Դϴ�.
//        /// </summary>
//        [JsonIgnore] public float BaseCraftChance { get{ return fCraftChance; } }

//        /// <summary>
//        /// ���� ������ ������ �Դϴ�.
//        /// </summary>
//        [JsonIgnore] public Recipie EnumRecipie { get{ return enumRecipie; } }
        
//        /// <summary>
//        /// ���� ������ �⺻ ����Դϴ�. 2�ܰ� ���ۿ� ���Ǹ�, ����ü �迭�� �����ϰ� �ֽ��ϴ�.
//        /// </summary>
//        [JsonIgnore] public CraftMaterial[] BaseMaterials { get{ return sArrBaseMaterial; } }

//        /// <summary>
//        /// ���� ������ �߰� ����Դϴ�. 3�ܰ� ���ۿ� ���Ǹ�, ����ü �迭�� �����ϰ� �ֽ��ϴ�.
//        /// </summary>
//        [JsonIgnore] public CraftMaterial[] AdditiveMaterals { get{ return sArrAdditiveMaterial; } }

//        /// <summary>
//        /// ���ۿ� �ʿ��� ȭ���Դϴ�. 2�ܰ迡 ���ۿ� ���˴ϴ�.
//        /// </summary>
//        [JsonIgnore] public int FirePower { get{ return iFirePower; } }


//        public ItemCraftWeapon(ItemType mainType, WeaponType subType, string No, string name, float price, VisualReferenceIndex visualRefIndex // ������ �⺻ ���� 
//            , ItemGrade basicGrade, int power, int durability, float speed, int weight, AttributeType attribute                         // ���� ���� ����
//            , CraftMaterial[] baseMaterial, CraftMaterial[] additiveMaterial, Recipie recipie                                           // ���� ���� ����   
//        ) : base( mainType, subType, No, name, price, visualRefIndex, basicGrade, power, durability, speed, weight, attribute )
//        {          

//            /*** ���� ���� ���� ***/
//            switch(basicGrade)
//            {
//                case ItemGrade.Low :
//                    fCraftChance = 0.9f;
//                    break;
//                case ItemGrade.Medium :
//                    fCraftChance = 0.6f;
//                    break;
//                case ItemGrade.High :
//                    fCraftChance = 0.25f;
//                    break;
//            }

//            sArrBaseMaterial = baseMaterial;
//            sArrAdditiveMaterial = additiveMaterial;
//            enumRecipie = recipie;
            
//            iFirePower = 0;
//            for(int i=0; i<baseMaterial.Length; i++)
//                iFirePower += baseMaterial[i].count;        // 2�ܰ� ȭ�� - �⺻ ����� ���� ���� ���� 
//        }

        
//    }

//}
