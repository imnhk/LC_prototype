using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;     // UI이벤트 사용을 위해

public class DragablePanel : MonoBehaviour,
    IPointerDownHandler,        // 마우스 클릭
    IDragHandler,               // 마우스 드래그 
    IPointerUpHandler           // 마우스 클릭 종료 인터페이스
{

    private Canvas parentCanvas = null;         
    private bool isDragging = false;            
    private Vector2 eventPos = Vector2.zero;    
    private Vector2 startOffset = Vector2.zero; //드래그 시작위치 오프셋


    void Awake()
    {
        this.parentCanvas = this.GetComponentInParent<Canvas>();
    }
	
	void Update ()
    {
        if (this.isDragging)
        {
            // 이벤트 위치를 Canvas Local 위치로 변경
            Vector2 localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                this.parentCanvas.transform as RectTransform,
                eventPos,
                this.parentCanvas.worldCamera,
                out localPos);

            // 최종 localPosition 변경
            Vector2 finalLocalPos = localPos;
            this.transform.localPosition = startOffset + finalLocalPos;
        }	
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        // 제일 위에 그려져야 한다
        this.transform.SetAsLastSibling();

        isDragging = true;             
        // 이벤트 위치 설정
        eventPos = eventData.position; 

        // eventPos를 local 위치로 변환
        Vector2 localOffset = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.parentCanvas.transform as RectTransform,
            eventPos,
            this.parentCanvas.worldCamera,
            out localOffset);

        this.startOffset.x = this.transform.localPosition.x - localOffset.x;
        this.startOffset.y = this.transform.localPosition.y - localOffset.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 중 event 위치 갱신
        eventPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
