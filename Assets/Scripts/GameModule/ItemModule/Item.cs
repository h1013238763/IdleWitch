using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Item
{
    public string item_id { get; private set; }
    public string item_name { get; private set; }
    public Sprite item_icon { get; private set; }
    public string item_desc { get; private set; }
    public int item_price { get; private set; }
    public int item_num { get; set; }
    public int invent_index { get; set; }

    public Item()
    {
        item_id = "";
        item_name = "";
        item_icon = null;
        item_desc = "";
        item_price = 0;
        item_num = 0;
        invent_index = 0;
    }
}