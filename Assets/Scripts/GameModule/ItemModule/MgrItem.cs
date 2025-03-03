using System.Collections.Generic;

public class MgrItem : MgrBase<MgrItem>
{
    private Dictionary<string, Item> item_dict = new Dictionary<string, Item>();
    private List<List<string>> invent_list = new List<List<string>>();
    public int invent_size { get; private set; }

    public void AddItem(string item_id, int item_num)
    {
        if(!item_dict.ContainsKey(item_id)) return;
        Item item = item_dict[item_id];

        
    }

    public void RemoveItem(string item_id, int item_num)
    {

    }

    public Item GetItemInfo(string item_id)
    {
        if(!item_dict.ContainsKey(item_id)) return null;
        return item_dict[item_id];
    }
}