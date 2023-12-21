using ItemData;
using UnityEngine;

namespace CraftData
{    
    public partial class Inventory : MonoBehaviour
    {
        public void AddItem(string itemName, int count=1)
        {
            CreateManager.instance.CreateItemToNearstSlot(this, itemName, count);
        }

    }
}
