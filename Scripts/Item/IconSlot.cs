using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
// Regex.Match
using System.Text.RegularExpressions;

public class IconSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Image itemIcon;
    private Item item;
    
    protected void Awake()
    {
        this.itemIcon = this.transform.Find("ItemIcon").GetComponent<Image>();
    }

    public void SetItem(Item item)
    {
        this.item = item;

        if (this.item != null)
        {
            this.itemIcon.enabled = true;
            this.itemIcon.sprite = item.icon;
        }        
        else
        {
            this.itemIcon.enabled = false;
        }
    }

    public Item GetItem()
    {
        return this.item;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.item == null) return;

        ItemSlotDrag.Instance.Catch(this, eventData.position);

    }

    public void OnDrag(PointerEventData eventData)
    {
        ItemSlotDrag.Instance.DragMove(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ItemSlotDrag.Instance.Drop();
    }
   
}
