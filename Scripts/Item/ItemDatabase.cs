using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Weapon, Armor }

public class Item
{
    ItemInfo itemInfo;

    public ItemType type;
    public Sprite icon;
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
        // 테스트용 임시 추가        
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

        // 같은 아이템 둘을 선택했을 시 레벨업 가능
        if (item1.key == item2.key && item1.level == item2.level)
        {
            Debug.Log("Upgrade!");
            // 레벨에 따라 아이콘 변경(임시)
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

    public void Add(Item item)
    {
        inventory.Add(item);
        UpdateSlots();
    }

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

    public void ShowInventory()
    {
        Debug.Log("---- Inventory ----");
        foreach(Item item in inventory)
        {
            Debug.Log(item.ToString());
        }
    }

}

