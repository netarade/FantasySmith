using ItemData;
using System.Collections.Generic;

namespace WorldItemData
{
    public partial class WorldItem
    {
        private Dictionary<string, Item> InitDic_Food()
        {
            //키 값, (아이템 타입, 넘버링, 스트링 네임, 아이템 비주얼 그래픽 배열, 잡화 타입, 보유할 스테이터스값, "아이템 설명") 
            return new Dictionary<string, Item>()
            {
                {
                    "Food1", new ItemFood( ItemType.Misc, "0000", "Food1", new VisualReferenceIndex(0),
                    MiscType.Food, new ItemStatus(3,3,3), "Soil mixed with water.") 
                },
                {
                    "Food2", new ItemFood( ItemType.Misc, "0001", "Food2", new VisualReferenceIndex(1),
                    MiscType.Food, new ItemStatus(-5,0,-1,2), "Materials can be bundled")
                },
            };




        }
    }

}