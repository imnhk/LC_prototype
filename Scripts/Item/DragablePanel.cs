using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;     //이걸 추가해야 UI 이벤트를 사용할수 있다..

public class DragablePanel : MonoBehaviour,
    IPointerDownHandler,        //마우스 다운 인터페이스
    IDragHandler,               //마우스 드래그 인터페이스
    IPointerUpHandler           //마우스 업 인터페이스
{

    private Canvas parentCanvas = null;         //부모 Canvas
    private bool isDragging = false;            //드래그 중이니?
    private Vector2 eventPos = Vector2.zero;    //드래그 이벤트 위치
    private Vector2 startOffset = Vector2.zero; //드래그 시작위치 오프셋


    void Awake()
    {
        this.parentCanvas = this.GetComponentInParent<Canvas>();
    }

	void Start () {}
	
	void Update ()
    {
        if (this.isDragging)
        {
            //이벤트 위치를 Canvas Local 위치로..
            Vector2 localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                this.parentCanvas.transform as RectTransform,
                eventPos,
                this.parentCanvas.worldCamera,
                out localPos);

            //최종 로컬위치 셋팅
            Vector2 finalLocalPos = localPos;
            this.transform.localPosition = startOffset + finalLocalPos;
        }	
	}

    //눌렀을때 
    public void OnPointerDown(PointerEventData eventData)
    {
        //내가 제일 앞에 그려져야한다.
        this.transform.SetAsLastSibling();      //부모에서 제일 밑으로 간다 ( 집으면 제일위로 )
        //this.transform.SetAsFirstSibling();   //부모에서 제일 위로 간다
        //this.transform.SetSiblingIndex(0); 
        //this.transform.childCount             //자식의 갯수

        isDragging = true;                      //드래그 시작 
        eventPos = eventData.position;          //화면상의 이벤트 위치를 받는다.

        //지금 클릭한 로컬위치
        Vector2 localOffset = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.parentCanvas.transform as RectTransform,
            eventPos,
            this.parentCanvas.worldCamera,
            out localOffset);


        //localOffset 은 PrentCanvas 기준의 local 위치
        this.startOffset.x = this.transform.localPosition.x - localOffset.x;
        this.startOffset.y = this.transform.localPosition.y - localOffset.y;
    }

    //마우스 드래그 될때
    public void OnDrag(PointerEventData eventData)
    {
        eventPos = eventData.position;     //이벤트 위치 갱신
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
