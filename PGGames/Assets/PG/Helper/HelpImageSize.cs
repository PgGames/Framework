using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("Help/Image Size",152)]
[RequireComponent(typeof(Image))]
public class HelpImageSize : MonoBehaviour {
    public Text m_Content;
    [Header("图片与字体的边框距离")]
    public Pingging m_Ping;

    private void Awake()
    {
        //m_Content.onCullStateChanged.AddListener();
        //m_Content.on
    }

    // Update is called once per frame
    void Update () {
        Set_Image_Size();
    }
    private void Set_Image_Size()
    {
        if (m_Content == null)
        {
            Debug.LogError("Content is Noll");
            this.enabled = false;
            return;
        }


        float Size_Wight = m_Content.preferredWidth;
        float Size_Hight = m_Content.preferredHeight;
        if (m_Content.horizontalOverflow == HorizontalWrapMode.Wrap)
            if (Size_Wight > m_Content.rectTransform.rect.width)
                Size_Wight = m_Content.rectTransform.rect.width;
        if (m_Content.verticalOverflow == VerticalWrapMode.Truncate)
            if (Size_Hight > m_Content.rectTransform.rect.height)
                Size_Hight = m_Content.rectTransform.rect.height;

        Set_Image_Pos(Size_Wight, Size_Hight);

        RectTransform rect = this.GetComponent<RectTransform>();

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Size_Wight + m_Ping.Left + m_Ping.Right);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Size_Hight + m_Ping.Up + m_Ping.Down);
    }
    /// <summary>
    /// 设置图片的位置
    /// </summary>
    /// <param name="SizeWight"></param>
    /// <param name="SizeHight"></param>
    private void Set_Image_Pos(float SizeWight,float SizeHight)
    {
        Vector3 pos = m_Content.transform.localPosition;
        Vector2 size = m_Content.rectTransform.rect.size;
        Vector3 temp_pos = pos;
        switch (m_Content.alignment)
        {
            case TextAnchor.UpperLeft:
                temp_pos = Set_Image_Pos_UpperLeft(SizeWight, SizeHight,pos,size);
                break;
            case TextAnchor.UpperCenter:
                temp_pos = Set_Image_Pos_UpperCenter(SizeWight, SizeHight, pos, size);
                break;
            case TextAnchor.UpperRight:
                temp_pos = Set_Image_Pos_UpperRight(SizeWight, SizeHight, pos, size);
                break;
            case TextAnchor.MiddleLeft:
                temp_pos = Set_Image_Pos_MiddleLeft(SizeWight, SizeHight, pos, size);
                break;
            case TextAnchor.MiddleCenter:
                temp_pos = Set_Image_Pos_MiddleCenter(SizeWight, SizeHight, pos, size);
                break;
            case TextAnchor.MiddleRight:
                temp_pos = Set_Image_Pos_MiddleRight(SizeWight, SizeHight, pos, size);
                break;
            case TextAnchor.LowerLeft:
                temp_pos = Set_Image_Pos_LowerLeft(SizeWight, SizeHight, pos, size);
                break;
            case TextAnchor.LowerCenter:
                temp_pos = Set_Image_Pos_LowerCenter(SizeWight, SizeHight, pos, size);
                break;
            case TextAnchor.LowerRight:
                temp_pos = Set_Image_Pos_LowerRight(SizeWight, SizeHight, pos, size);
                break;
            default:
                break;
        }
        //计算边框
        temp_pos.x -= (m_Ping.Left - m_Ping.Right) / 2.0f;
        temp_pos.y += (m_Ping.Up - m_Ping.Down) / 2.0f;
        this.transform.localPosition = temp_pos;
    }
    private Vector3 Set_Image_Pos_UpperLeft(float SizeWight, float SizeHight,Vector3 pos,Vector2 size)
    {
        //初步位置
        float x = pos.x - size.x / 2.0f;
        float y = pos.y + size.y / 2.0f;
        //计算字体偏移
        x += SizeWight / 2.0f;
        y -= SizeHight / 2.0f;
        return new Vector3(x, y, pos.z);
    }
    private Vector3 Set_Image_Pos_UpperCenter(float SizeWight, float SizeHight, Vector3 pos, Vector2 size)
    {
        //初步位置
        float x = pos.x;
        float y = pos.y + size.y / 2.0f;
        //计算字体偏移
        //x += SizeWight / 2.0f;
        y -= SizeHight / 2.0f;

        return new Vector3(x, y, pos.z);
    }
    private Vector3 Set_Image_Pos_UpperRight(float SizeWight, float SizeHight, Vector3 pos, Vector2 size)
    {
        //初步位置
        float x = pos.x + size.x / 2.0f;
        float y = pos.y + size.y / 2.0f;
        //计算字体偏移
        x -= SizeWight / 2.0f;
        y -= SizeHight / 2.0f;
        return new Vector3(x, y, pos.z);
    }
    private Vector3 Set_Image_Pos_MiddleLeft(float SizeWight, float SizeHight, Vector3 pos, Vector2 size)
    {
        //初步位置
        float x = pos.x - size.x / 2.0f;
        float y = pos.y;
        //计算字体偏移
        x += SizeWight / 2.0f;
        return new Vector3(x, y, pos.z);
    }
    private Vector3 Set_Image_Pos_MiddleCenter(float SizeWight, float SizeHight,Vector3 pos, Vector2 size)
    {
        //初步位置
        float x = pos.x;
        float y = pos.y;
        //计算字体偏移
        return new Vector3(x, y, pos.z);
    }
    private Vector3 Set_Image_Pos_MiddleRight(float SizeWight, float SizeHight, Vector3 pos, Vector2 size)
    {
        //初步位置
        float x = pos.x + size.x / 2.0f;
        float y = pos.y;
        //计算字体偏移
        x -= SizeWight / 2.0f;
        return new Vector3(x, y, pos.z);
    }
    private Vector3 Set_Image_Pos_LowerLeft(float SizeWight, float SizeHight, Vector3 pos, Vector2 size)
    {
        //初步位置
        float x = pos.x - size.x / 2.0f;
        float y = pos.y - size.y / 2.0f;
        //计算字体偏移
        x += SizeWight / 2.0f;
        y += SizeHight / 2.0f;
        return new Vector3(x, y, pos.z);
    }
    private Vector3 Set_Image_Pos_LowerCenter(float SizeWight, float SizeHight, Vector3 pos, Vector2 size)
    {
        //初步位置
        float x = pos.x;
        float y = pos.y - size.y / 2.0f;
        //计算字体偏移
        //x -= SizeWight / 2.0f;
        y += SizeHight / 2.0f;
        return new Vector3(x, y, pos.z);
    }
    private Vector3 Set_Image_Pos_LowerRight(float SizeWight, float SizeHight, Vector3 pos, Vector2 size)
    {
        //初步位置
        float x = pos.x + size.x / 2.0f;
        float y = pos.y - size.y / 2.0f;
        //计算字体偏移
        x -= SizeWight / 2.0f;
        y += SizeHight / 2.0f;
        return new Vector3(x, y, pos.z);
    }

    [System.Serializable]
    public struct Pingging
    {
        public float Up;
        public float Down;
        public float Left;
        public float Right;
    }
}
