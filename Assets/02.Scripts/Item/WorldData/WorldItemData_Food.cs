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
                    "물", new ItemFood( ItemType.Misc, "0100", "물", new VisualReferenceIndex(0),
                    MiscType.Food, new ItemStatus(0,0,30), "마실 수 있는 깨끗한 물이다.") 
                },
                {
                    "산딸기", new ItemFood( ItemType.Misc, "0101", "산딸기", new VisualReferenceIndex(1),
                    MiscType.Food, new ItemStatus(5,10,10), "식물을 채집해서 얻을 수 있는 기본적인 영양 공급원입니다.")
                },
                {
                    "버섯", new ItemFood( ItemType.Misc, "0102", "버섯", new VisualReferenceIndex(2),
                    MiscType.Food, new ItemStatus(5,10,10), "다행히 독이 없는 우산 모양의 곰팡이 입니다. 먹을 수 있습니다.")
                },
                {
                    "생고기", new ItemFood( ItemType.Misc, "0103", "생고기", new VisualReferenceIndex(3),
                    MiscType.Food, new ItemStatus(10,15,5), "동물을 사냥하여 얻을 수 있는 요리 재료입니다. 건강에 좋진 않으나 익히지 않고도 먹을 수 있습니다.")
                },
                {
                    "고기구이", new ItemFood( ItemType.Misc, "0104", "고기구이", new VisualReferenceIndex(4),
                    MiscType.Food, new ItemStatus(30,30,0), "알맞게 익힌 맛있는 고기 입니다.")
                },
                {
                    "버섯 구이", new ItemFood( ItemType.Misc, "0105", "버섯 구이", new VisualReferenceIndex(5),
                    MiscType.Food, new ItemStatus(30,30,10), "버섯과 산딸기를 같이 구운            요리입니다.")
                },
                {
                    "고기 버섯 볶음", new ItemFood( ItemType.Misc, "0106", "고기 버섯 볶음", new VisualReferenceIndex(6),
                    MiscType.Food, new ItemStatus(80,80,30), "버섯에 스테이크의 육즙이 스며든 훌륭한 요리입니다.")
                },
                {
                    "진흙 고기구이", new ItemFood( ItemType.Misc, "0107", "진흙 고기구이", new VisualReferenceIndex(7),
                    MiscType.Food, new ItemStatus(100,100,40,0,2), "중국 음식에 영감을 얻어 만들어낸 요리입니다. 건강에 매우 좋은 음식입니다.")
                },
            };




        }
    }

}