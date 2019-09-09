using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Weapon, Armor }

public class Item
{
    ItemInfo itemInfo;

    public ItemType type;
    public Sprite icon;         // 아이템 아이콘
    public int level;
    public int key;

    public Item() {}
    public Item(ItemType type, int level, int key, Sprite icon)
    {
        this.type = type;
        this.level = level;
        this.key = key;
        this.icon = icon;
    }

    public override string ToString()
    {
        return $"id: {key}, level: {level}";
    }
}

public class ItemDatabase : PumpFramework.Common.Singleton<ItemDatabase>
{
    /* Container*/
    private List<Item> inventory = new List<Item>();
    public Sprite[] icons;
    private IconSlot[] iconSlots;


    private IconSlot[] upgradeSlots;

    void Awake()
    {        
        this.icons = Resources.LoadAll<Sprite>("Textures/ICons");

        iconSlots = GameObject.Find("SlotGrid").GetComponentsInChildren<IconSlot>();
        upgradeSlots = GameObject.Find("UpgradePanel").GetComponentsInChildren<IconSlot>();
        
        this.UpdateSlots();
    }

    private void Start()
    {
        // 임시 아이템 데이터 베이스 추가        
        inventory.Add(new Item(ItemType.Weapon, 0, 1, this.icons[0]));
        inventory.Add(new Item(ItemType.Weapon, 0, 2, this.icons[1]));
        UpdateSlots();

    }

    public void UpgradeItems()
    {
        Item item1 = upgradeSlots[0].GetItem();
        Item item2 = upgradeSlots[1].GetItem();
        if(item1 == null || item2 == null)
        {
            Debug.LogError("Fill both slots!");
            return;
        }

        if (item1.key == item2.key && item1.level == item2.level)
        {
            Debug.Log("Upgrade!");
            // 레벨에 따라 아이콘이 바뀌도록 함(임시)
            item1.level += 1;
            item1.icon = icons[35 + item1.level];

            inventory.Add(item1);
            inventory.Remove(item1);
            inventory.Remove(item2);
            UpdateSlots();
        }
        else
        {
            Debug.LogError("Can't upgrade with different items.");
        }
    }

    public void UpdateSlots()
    {
        for (int i=0; i < iconSlots.Length; i++)
        {
            if(i < inventory.Count)
                iconSlots[i].SetItem(inventory[i]);
            else
                iconSlots[i].SetItem(null);
        }
        upgradeSlots[0].SetItem(null);
        upgradeSlots[1].SetItem(null);
    }

    public Item GetItem(int index)
    {
        return this.inventory[index];
    }

    public Item this[int index]
    {
        get
        {
            return this.inventory[index];
        }
    }

    public Item RandomItem
    {
        get
        {
            return this.inventory[Random.Range(0, this.inventory.Count)];
        }
    }

    public int ItemCount
    {
        get
        {
            return this.inventory.Count;
        }
    }

    /* Method */
    // Insert
    public void Add(Item item)
    {
        inventory.Add(item);
        UpdateSlots();
    }

    // Delete
    public void Delete(Item item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            UpdateSlots();
        }
        else
            Debug.LogError($"items don't have item: {item}");
    }

    // Update
    
    // Select
    // Sort

    public void ShowInventory()
    {
        Debug.Log("---- Inventory ----");
        foreach(Item item in inventory)
        {
            Debug.Log(item.ToString());
        }
    }

}

