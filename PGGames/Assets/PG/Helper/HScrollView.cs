using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//[AddComponentMenu("Help/ScrollView")]
[AddComponentMenu("UI/H Scroll View", 38)]
[DisallowMultipleComponent]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(RectMask2D))]
[SelectionBase]
public class HScrollView : UIBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler, ICanvasElement, ILayoutGroup//,ILayoutElement
{
    protected HScrollView()
    {
    }
    public RectTransform Prefab;
    public uint Count = 0;
    [SerializeField]
    private Padding padding;
    [SerializeField]
    private float spacing = 0;
    [SerializeField]
    private RectTransform content;
    public RectTransform Content
    {
        get { return content; }
    }
    public float Speed = 1;
    public MovementType moveType = MovementType.Unrestricted;
    [Range(0.001f, 1)]
    public float ElasticSpeed = 0.135f;
    public bool IsEnable = false;
    [SerializeField]
    private RectTransform viewport;
    public RectTransform Viewport
    {
        get { return viewport; }
    }
    public Scrollbar VerticalScrollbar;
    public ScrollRectEvent onValueChang;
    /// <summary>
    /// 当起始位置小于零时表示为初始化
    /// </summary>
    public ScrollViewEvent onValueFormToEnd;
    [Obsolete("Use onValueFormToEnd")]
    public ScrollHViewEvent onValueInit;


    protected float Height;         //滑动区域的高度
    protected float m_Everyheight;  //每个物体的高度
    protected float MaxDis;         //上至下的最大距离
    protected int minIndex =0;
    protected int maxIndex =0;
    protected int MaxCount =0;      //最大存在的滑动物体的数量
    protected uint m_Count;         //
    protected Vector2 m_topheight;
    protected Vector2 m_bottonheight;


    #region 滑动功能块

    //滑动时鼠标的位置
    protected Vector2 Dragposition;
    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        Dragposition = eventData.position;
        DragTragetPos = content.localPosition;
        CancelInvoke("DragMove");
        CancelInvoke("SpringbackMove");
    }
    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (content == null)
            return;
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (!IsActive())
            return;
        Vector2 pos = Dragposition- eventData.position;
        Dragposition = eventData.position;
        Vector2 current = content.localPosition;
        if (Vector2.Distance(current, DragTragetPos) < 0.1f)
            current = new Vector2(current.x, current.y - pos.y);
        else
            current = new Vector2(current.x, DragTragetPos.y - pos.y);
        DragTragetPos = current;
        Vector2 temppos = Vector2.Lerp(content.localPosition, current, 0.5f);
        content.localPosition = new Vector3(current.x, temppos.y, 0);
        onValueChang.Invoke(content.localPosition);
    }
    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        Dragposition = eventData.position;
        Vector2 current = content.localPosition;
        if (moveType == MovementType.Unrestricted)
        {
            InvokeRepeating("DragMove", 0, 0.0001f);
        }
        else
        {
            if (Vector2.Distance(current, DragTragetPos) <= 0.1f)
            {
                //计算回弹
                if (moveType == MovementType.Elastic)
                    CalculateSpringback();
            }
            else
            {
                Vector2 pos = DragTragetPos;
                Vector2 toppos = CalculateTop();
                Vector2 bottnpos = CalculateBottn();
                if (pos.y < toppos.y)
                {
                    SpringbackTragetPos = toppos;
                    InvokeRepeating("SpringbackMove", 0, 0.001f);
                }
                else if (pos.y > bottnpos.y)
                {
                    SpringbackTragetPos = bottnpos;
                    InvokeRepeating("SpringbackMove", 0, 0.001f);
                }
                else
                {
                    InvokeRepeating("DragMove", 0, 0.0001f);
                }
            }
        }
    }
    //鼠标点机对话框
    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        UpdateScrollBarSize();
    }
    /// <summary>
    /// 滚动鼠标
    /// </summary>
    /// <param name="eventData"></param>
    public void OnScroll(PointerEventData eventData)
    {
        if (content == null)
            return;
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (!IsActive())
            return;
        ScrollMove(new Vector2(Speed * eventData.scrollDelta.x, Speed * eventData.scrollDelta.y));
        //计算回弹//计算回弹
        if (moveType == MovementType.Elastic)
            CalculateSpringback();
        onValueChang.Invoke(content.localPosition);
    }
    /// <summary>
    /// 滑动条变动
    /// </summary>
    /// <param name="value"></param>
    protected void OnClickValueBar(float value)
    {
        try
        {
            CancelInvoke("DragMove");

            if ((m_Count < MaxCount || Count < MaxCount) || MaxCount == 0)
                return;
            float Scrollbarvalue = VerticalScrollbar.value;
            content.localPosition = new Vector3(content.localPosition.x, m_topheight.y + MaxDis * Scrollbarvalue, 0);
            //InvokeRepeating("ValueMove", 0, 0.0001f);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public void Rebuild(CanvasUpdate executing)
    {
        Debug.LogError("Rebuild");
    }

    public void LayoutComplete()
    {
        Debug.LogError("LayoutComplete");
    }

    public void GraphicUpdateComplete()
    {
        Debug.LogError("GraphicUpdateComplete");
    }

    public void SetLayoutHorizontal()
    {
    }
    public void SetLayoutVertical()
    {
        if (VerticalScrollbar != null)
        {
            VerticalScrollbar.size = ((RectTransform)this.transform).rect.height / Height;
        }
    }
    public enum MovementType
    {
        Unrestricted, // Unrestricted movement -- can scroll forever
        Elastic, // Restricted but flexible -- can go past the edges, but springs back in place
        Clamped, // Restricted movement where it's not possible to go past the edges
    }
    protected Vector2 SpringbackTragetPos;
    protected Vector2 DragTragetPos;
    protected void CalculateSpringback()
    {
        Vector2 pos = content.localPosition;
        Vector2 toppos = CalculateTop();
        Vector2 bottnpos = CalculateBottn();
        if (pos.y < toppos.y)
        {
            SpringbackTragetPos = toppos;
            InvokeRepeating("SpringbackMove", 0, 0.001f);
        }
        else if (pos.y > bottnpos.y)
        {
            SpringbackTragetPos = bottnpos;
            InvokeRepeating("SpringbackMove", 0, 0.001f);
        }
        onValueChang.Invoke(content.localPosition);
    }
    protected void DragMove()
    {
        Vector2 current = content.localPosition;
        if (Vector2.Distance(DragTragetPos, current) <= 0.1f)
        {
            CancelInvoke("DragMove");
        }
        else
        {
            Vector2 pos = Vector2.Lerp(current, DragTragetPos, 0.01f);
            content.localPosition = new Vector3(pos.x, pos.y, 0);
        }
        onValueChang.Invoke(content.localPosition);
    }
    protected void ScrollMove(Vector2 postion)
    {
        //纵向移动
        Vector3 current = content.localPosition;
        content.localPosition = new Vector3(current.x, current.y - postion.y, 0);
        onValueChang.Invoke(content.localPosition);
    }
    protected void SpringbackMove()
    {
        Vector2 current = content.localPosition;
        if (Vector2.Distance(SpringbackTragetPos, current) <= 0.1f)
        {
            CancelInvoke("SpringbackMove");
        }
        else
        {
            Vector2 pos = Vector2.Lerp(current, SpringbackTragetPos, ElasticSpeed);
            content.localPosition = new Vector3(pos.x, pos.y, 0);
        }
        onValueChang.Invoke(content.localPosition);
    }
    protected void ValueMove()
    {
        float tempvalue = (Vector2.Distance(content.localPosition, m_topheight)) / MaxDis;
        if (Mathf.Abs(maxIndex/Count-tempvalue)<(1/Count))
        {
            CancelInvoke("ValueMove");
        }
        else
        {
            onValueChang.Invoke(content.localPosition);
        }
    }
    //计算上顶点
    protected Vector2 CalculateTop()
    {
        Vector2 pos = Vector2.zero;
        float tempHeight = ((RectTransform)this.transform).rect.height;
        if (Height > tempHeight)
            return new Vector2(0, -(Height - tempHeight) / 2.0f);
        else
            return new Vector2(0, (tempHeight - Height) / 2.0f);
    }
    //计算下顶点
    protected Vector2 CalculateBottn()
    {
        Vector2 pos = Vector2.zero;
        float tempHeight = ((RectTransform)this.transform).rect.height;
        if (Height > tempHeight)
            return new Vector2(0, (Height - tempHeight) / 2.0f);
        else
            return new Vector2(0, (tempHeight - Height) / 2.0f);
    }
    //计算滑动区域的高度
    protected void CalculateHdight()
    {
        if (Count <= 0)
            Height = 0;
        else
            Height = Prefab.rect.height * Count + spacing * (Count - 1) + padding.top + padding.botton;

        content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);

        //计算上顶点
        m_topheight = CalculateTop();
        //计算下顶点
        m_bottonheight = CalculateBottn();
        //上下顶点间的最大距离
        MaxDis = Vector2.Distance(m_topheight, m_bottonheight);
    }
    protected override void OnDisable()
    {
        //终止移动
        CancelInvoke("DragMove");
        CancelInvoke("SpringbackMove");
    }
    #endregion

    protected override void Awake()
    {
        if (VerticalScrollbar != null)
        {
            VerticalScrollbar.onValueChanged.AddListener(OnClickValueBar);
        }
    }
    protected override void Start()
    {
        if (IsEnable)
            return;
        Init();
        //m_Count = Count;
        //if (Prefab == null)
        //    return;
        //m_Everyheight = Prefab.rect.height;
        //Height = Prefab.rect.height * Count + spacing * (Count - 1) + padding.top + padding.botton;
        //MaxCount = CalculateSum(m_Everyheight + spacing);
        //content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);
        //Vector2 top = CalculateTop();
        //content.localPosition = new Vector3(top.x, top.y, 0);
        //m_topheight = CalculateTop();
        //m_bottonheight = CalculateBottn();
        //MaxDis = Vector2.Distance(m_topheight, m_bottonheight);
        //if (MaxCount > Count)
        //    InitGameObject((int)Count);
        //else
        //    InitGameObject(MaxCount);
    }
    protected override void OnEnable()
    {
        if (!IsEnable)
            return;
        Init();
        //m_Count = Count;
        //if (Prefab == null)
        //    return;
        //m_Everyheight = Prefab.rect.height;
        //Height = Prefab.rect.height * Count + spacing * (Count - 1) + padding.top + padding.botton;
        //MaxCount = CalculateSum(m_Everyheight + spacing);
        //content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);
        //Vector2 top = CalculateTop();
        //content.localPosition = new Vector3(top.x, top.y, 0);
        //m_topheight = CalculateTop();
        //m_bottonheight = CalculateBottn();
        //MaxDis = Vector2.Distance(m_topheight, m_bottonheight);
        //if (MaxCount > Count)
        //    InitGameObject((int)Count);
        //else
        //    InitGameObject(MaxCount);
        base.OnEnable();
    }


    List<GameObject> ListClient = new List<GameObject>();
    List<GameObject> ListWaste = new List<GameObject>();

    protected void InitGameObject(int sum)
    {
        for (int i = 0; i < ListClient.Count; i++)
        {
            ListClient[i].SetActive(false);
            ListWaste.Add(ListClient[i]);
        }
        ListClient.Clear();
        float temp_y = m_topheight.y;
        Vector3 pos = Vector3.zero;
        if (sum <= MaxCount)
            pos = new Vector3(0, Height / 2 - padding.top - m_Everyheight/2, 0);
        minIndex = 0;
        maxIndex = 0;
        for (int i = 0; i < sum; i++)
        {
            GameObject tempGame = GetGameObject();
            if (tempGame == null)
            {
                tempGame = GameObject.Instantiate<GameObject>(Prefab.gameObject);
                tempGame.transform.SetParent(this.content);
                tempGame.transform.localScale = Vector3.one;
                ((RectTransform)tempGame.transform).anchorMin = new Vector2(0.5f, 1);
                ((RectTransform)tempGame.transform).anchorMax = new Vector2(0.5f, 1);
            }
            tempGame.SetActive(true);
            tempGame.name = Prefab.name + maxIndex;
            tempGame.transform.localPosition = pos - new Vector3(0, (m_Everyheight + spacing) * i, 0);
            tempGame.transform.SetAsLastSibling();
            ListClient.Add(tempGame);
            //onValueInit.Invoke(i, tempGame);
            onValueFormToEnd.Invoke(-1, i, tempGame);
            maxIndex++;
        }
    }
    protected GameObject GetGameObject()
    {
        if (ListWaste.Count == 0)
            return null;
        GameObject tempobject = ListWaste[0];
        ListWaste.RemoveAt(0);
        return tempobject;

    }
    protected void OnClick(Vector2 pos)
    {
        UpDataScrollBarValue();
        if (ListClient.Count <= 0)
            return;
        float tempDis = content.localPosition.y - m_topheight.y;
        tempDis -= padding.top;
        int sum = (int)(tempDis / (m_Everyheight + spacing));
        if (sum > minIndex && maxIndex < Count)
        {
            GameObject tempGame = ListClient[0];
            ListClient.RemoveAt(0);
            tempGame.transform.name = Prefab.name + maxIndex;
            tempGame.transform.localPosition = new Vector3(0, ListClient[ListClient.Count - 1].transform.localPosition.y - (m_Everyheight + spacing), 0);
            onValueFormToEnd.Invoke(minIndex, maxIndex, tempGame);
            minIndex++;
            maxIndex++;
            ListClient.Add(tempGame);
            tempGame.transform.SetAsLastSibling();
        }
        else if (sum < minIndex && minIndex > 0)
        {
            GameObject tempGame = ListClient[ListClient.Count - 1];
            ListClient.RemoveAt(ListClient.Count - 1);
            tempGame.transform.name = Prefab.name + (minIndex - 1);
            tempGame.transform.localPosition = new Vector3(0, ListClient[0].transform.localPosition.y + (m_Everyheight + spacing), 0);
            onValueFormToEnd.Invoke(maxIndex - 1, minIndex - 1, tempGame);
            minIndex--;
            maxIndex--;
            ListClient.Insert(0, tempGame);
            tempGame.transform.SetAsFirstSibling();
        }
    }
    protected void UpDataScrollBarValue()
    {
        if (VerticalScrollbar == null)
            return;
        if (content.localPosition.y >= m_topheight.y)
            VerticalScrollbar.value = (Vector2.Distance(content.localPosition, m_topheight)) / MaxDis;
        else
            VerticalScrollbar.value = 0;
    }
    protected void UpdateScrollBarSize()
    {
        if (VerticalScrollbar != null)
        {
            VerticalScrollbar.size = ((RectTransform)this.transform).rect.height / Height;
        }
    }
    protected int CalculateSum(float height)
    {
        RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();
        return (int)(rectTransform.rect.height / height + 2);
    }
    protected void UpDataGameObject()
    {
        if (MaxCount == 0)
        {
            m_Everyheight = Prefab.rect.height;
            MaxCount = CalculateSum(m_Everyheight + spacing);
            //return;
        }
        if (m_Count < MaxCount || Count < MaxCount)
        {
            if (Count == m_Count)
                return;
            minIndex = 0;
            maxIndex = 0;
            if (Count > MaxCount)
                InitGameObject(MaxCount);
            else
                InitGameObject((int)Count);
        }
    }
    private void Update()
    {
        if (Count == 0)
            return;
        if (m_Count != Count)
        {
            float temp_Distance = Vector2.Distance(content.localPosition, m_topheight);
            //Update
            CalculateHdight();
            //Height = Prefab.rect.height * Count + spacing * (Count - 1) + padding.top + padding.botton;
            //content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);
            //m_topheight = CalculateTop();
            //m_bottonheight = CalculateBottn();
            //MaxDis = Vector2.Distance(m_topheight, m_bottonheight);
            if (temp_Distance < MaxDis)
                content.localPosition = new Vector3(0, m_topheight.y + temp_Distance, 0);
            else
                content.localPosition = new Vector3(0, m_topheight.y + MaxDis, 0);
            UpDataGameObject();
            m_Count = Count;
            UpdateScrollBarSize();
        }
        OnClick(Vector2.zero);
    }
    /// <summary>
    /// 初始化信息
    /// </summary>
    public void Init()
    {
        m_Count = Count;
        if (Prefab == null)
            return;
        m_Everyheight = Prefab.rect.height;
        CalculateHdight();
        //Height = Prefab.rect.height * Count + spacing * (Count - 1) + padding.top + padding.botton;
        //content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);
        MaxCount = CalculateSum(m_Everyheight + spacing);
        Vector2 top = CalculateTop();
        content.localPosition = new Vector3(top.x, top.y, 0);
        //m_topheight = CalculateTop();
        //m_bottonheight = CalculateBottn();
        //MaxDis = Vector2.Distance(m_topheight, m_bottonheight);
        if (MaxCount > Count)
            InitGameObject((int)Count);
        else
            InitGameObject(MaxCount);
    }
    [System.Serializable]
    public class Padding
    {
        public float top;
        public float botton;
    }
    [Serializable]
    public class ScrollRectEvent : UnityEvent<Vector2> { }
    [Serializable]
    public class ScrollViewEvent : UnityEvent<int, int,GameObject> { }
    [Serializable]
    public class ScrollHViewEvent : UnityEvent<int,GameObject> { }
    
}
