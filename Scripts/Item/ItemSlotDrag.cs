using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSlotDrag : IconSlot, IComparer<IconSlot>
{
    private static ItemSlotDrag instance;
    public static ItemSlotDrag Instance { get { return instance; }}

    public List<IconSlot> targetSlots = new List<IconSlot>();
    public IconSlot targetSlot = null;
    public IconSlot catchSlot = null;

    protected new void Awake()
    {
        instance = this;
        this.gameObject.SetActive(false);

        base.Awake();
    }

    void Start() {}

    public void Update()
    {
        if (this.targetSlots.Count > 0)
        {
            // 가장 위에 있는 아이템을 찾기 위해 정렬
            this.targetSlots.Sort(this);
            this.targetSlot = this.targetSlots[0];
        }
        else
        {
            this.targetSlot = null;
        }
    }

    // 아이템을 집는다
    public void Catch(IconSlot itemSlot, Vector2 screenPos)
    {
        this.gameObject.SetActive(true);

        this.catchSlot = itemSlot;
        this.SetItem(itemSlot.GetItem());

        // 집은 놈은 해제
        itemSlot.SetItem(null);

        // 가장 앞으로 이동
        this.transform.SetAsLastSibling();
        this.transform.position = new Vector3(screenPos.x, screenPos.y, 0.0f);
    }

    public void DragMove(Vector2 screenPos)
    {
        this.transform.position = new Vector3(
           screenPos.x,
           screenPos.y,
           0.0f);
    }

    // 아이템을 놓는다
    public void Drop()
    {        
        // slot이 아닌 곳에 놓았을 때
        if (this.targetSlot == null)
        {
            this.catchSlot.SetItem(this.GetItem());
        }
        else
        {
            Item targetItem = this.targetSlot.GetItem();

            // slot이 비어 있음
            if (targetItem == null)
            {
                this.targetSlot.SetItem(this.GetItem());
            }

            // slot이 비어있지 않음. swap
            else
            {
                this.catchSlot.SetItem(targetItem);
                this.targetSlot.SetItem(this.GetItem());
            }

        }

        this.targetSlots.Clear();
        this.SetItem(null);
        this.catchSlot = null;

        this.gameObject.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        IconSlot itemSlot = col.GetComponent<IconSlot>();
        if (itemSlot != null)
        {
            this.targetSlots.Add(itemSlot);
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        IconSlot itemSlot = col.GetComponent<IconSlot>();
        if (itemSlot != null)
        {
            this.targetSlots.Remove(itemSlot);
        }
    }

    // slot 거리 비교
    public int Compare(IconSlot x, IconSlot y)
    {

        float distX = Vector3.Distance(x.transform.position, this.transform.position);
        float distY = Vector3.Distance(y.transform.position, this.transform.position);

        if (distX < distY)
        {
            return -1;
        }            
        else if (distX > distY)
        {
            return 1;
        }            

        return 0;
    }
}
