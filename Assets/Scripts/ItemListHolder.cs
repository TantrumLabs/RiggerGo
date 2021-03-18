using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListHolder : MonoBehaviour
{
    public List<ItemInformation> itemInformation = new List<ItemInformation>();
    
    public void SendToPopulate(PoulateItemList p_poulate)
    {
        p_poulate.SetAndPopulate(itemInformation);
    }
}