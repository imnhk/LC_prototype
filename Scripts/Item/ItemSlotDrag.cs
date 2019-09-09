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
            // 나랑 충돌되어 Drop될 Slot List를 나랑 제일 가까운 놈순으로 정렬
            this.targetSlots.Sort(this);

            // 위에서 Sort했기 때문에 0 번에있는 놈이 나랑 제일 가까운 놈이다....
            this.targetSlot = this.targetSlots[0];
        }
        else
        {
            this.targetSlot = null;
        }
    }

    // 아이템을 집다.
    public void Catch(IconSlot itemSlot, Vector2 screenPos)
    {
        this.gameObject.SetActive(true);

        // 집은 놈 기억
        this.catchSlot = itemSlot;

        // 집은 아이템 정보로 셋팅
        this.SetItem(itemSlot.GetItem());

        // 집은 놈은 해제
        itemSlot.SetItem(null);

        // 나를 제일 마지막에 그리게
        this.transform.SetAsLastSibling();

        // 이벤트 위치로..
        this.transform.position = new Vector3(screenPos.x, screenPos.y, 0.0f);
    }

    // 집은 아이템 Drag
    public void DragMove(Vector2 screenPos)
    {
        this.transform.position = new Vector3(
           screenPos.x,
           screenPos.y,
           0.0f);
    }

    // 집은 아이템 놓는다.
    public void Drop()
    {        
        if (this.targetSlot == null) // 집으로 돌아간다.
        {
            this.catchSlot.SetItem(this.GetItem());
        }
        else // 들어가야할 위치가 있다..
        {
            Item targetItem = this.targetSlot.GetItem();

            // 들어가야 할곳이 비어있다면...
            if (targetItem == null)
            {
                this.targetSlot.SetItem(this.GetItem());
            }

            // 들어가야할 곳이 비어있지 않다면...
            else
            {
                //Swap
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

    public int Compare(IconSlot x, IconSlot y)
    {
        //x 가 가깝다면 -1
        //y 가 가깝다면 1
        //같다면 0
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
