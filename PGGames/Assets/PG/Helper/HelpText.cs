/*-----------------------------------
 *------------[功能脚本]-------------*  
 *----创建时间：2017/9/11
 *----脚本功能：对UGUI组件中的Text组件进行优化
 *              脚本仅支持文字内容的中途变化
 *        [Password]---类型对输入的文字进行密码掩饰
 *        [Text]-------类型显示原有文字不做任何掩饰
 *        [Name]-------类型根据文本框的长度进行操作超出用“…”进行掩饰
 *        [Money]------类型根据数值大小显示万、亿
 *----脚本兼容：
 *        该脚本支持组件---[Outline]
 *        该脚本支持组件---[Gradient]
 *        该脚本支持组件---[Shadow]
 *        该脚本支持组件---[TextSpacing]
 *        该脚本支持组件---[Text]
 *----脚本弊端：
 *        仅支持[Text]脚本中的文字动态变动
 *-----------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PG.UI
{

    [AddComponentMenu("Help/Text")]
    [RequireComponent(typeof(Text), typeof(Text))]
    public class HelpText : MonoBehaviour
    {
        [TextArea(3, 10)]
        public string text;
        private string Content;

        [Header("文本类型")]
        public Type m_Type = Type.Text;
        [Header("Money Type")]
        [SerializeField]
        public bool Isfloat = true;
        
        public int fontSize { set; get; }
        public float lineSpacing { set; get; }

    

        public enum Type
        {
            Password,
            Text,
            Name,
            Money
        }
        Text m_text;
        RectTransform m_Rect;
        Type losatType = Type.Text;
        
        private void Awake()
        {
            m_text = gameObject.GetComponent<Text>();
            m_Rect = gameObject.GetComponent<RectTransform>();

            
            losatType = m_Type;
            fontSize = m_text.fontSize;
            lineSpacing = m_text.lineSpacing;
        }
        private void Start()
        {
        }
        //private void SpritsHelp(ref GameObject varGame)
        //{
        //    var compoten = this.gameObject.GetComponents<Component>();

        //    for (int i = 0; i < compoten.Length; i++)
        //    {
        //        var Com = compoten[i];
        //        if (Com.GetType() == typeof(Gradient))
        //        {
        //            var thisCom = Com as Gradient;
        //            Gradient gradient = varGame.AddComponent<Gradient>();
        //            gradient.enabled = thisCom.enabled;
        //            gradient.topColor = thisCom.topColor;
        //            gradient.bottomColor = thisCom.bottomColor;
        //            thisCom.enabled = false;
        //        }
        //        else if (Com.GetType() == typeof(Outline))
        //        {
        //            var thisCom = Com as Outline;
        //            var outline = varGame.AddComponent<Outline>();
        //            outline.enabled = thisCom.enabled;
        //            outline.effectColor = thisCom.effectColor;
        //            outline.effectDistance = thisCom.effectDistance;
        //            outline.useGraphicAlpha = thisCom.useGraphicAlpha;
        //            thisCom.enabled = false;
        //        }
        //        else if (Com.GetType() == typeof(Shadow))
        //        {
        //            var thisCom = Com as Shadow;
        //            var outline = varGame.AddComponent<Shadow>();
        //            outline.enabled = thisCom.enabled;
        //            outline.effectColor = thisCom.effectColor;
        //            outline.effectDistance = thisCom.effectDistance;
        //            outline.useGraphicAlpha = thisCom.useGraphicAlpha;
        //            thisCom.enabled = false;
        //        }
        //        else if (Com.GetType() == typeof(TextSpacing))
        //        {
        //            var thisCom = Com as TextSpacing;
        //            var outline = varGame.AddComponent<TextSpacing>();
        //            outline.enabled = thisCom.enabled;
        //            outline._Text_Spacing = thisCom._Text_Spacing;
        //            thisCom.enabled = false;
        //        }
        //    }
        //}
        //private void AddTran(ref GameObject var_Game)
        //{
        //    RectTransform tmp_Rect = var_Game.GetComponent<RectTransform>();
        //    if (tmp_Rect == null)
        //    {
        //        tmp_Rect = var_Game.AddComponent<RectTransform>();
        //    }
        //    tmp_Rect.anchorMax = Vector2.one;
        //    tmp_Rect.anchorMin = Vector2.zero;
        //    tmp_Rect.localPosition = Vector3.zero;
        //    tmp_Rect.localEulerAngles = Vector3.zero;
        //    tmp_Rect.localScale = Vector3.one;
        //    tmp_Rect.offsetMax = Vector2.zero;
        //    tmp_Rect.offsetMin = Vector2.zero;
        //}
        //private void SetText(ref Text var_Text)
        //{
        //    //Character 属性
        //    var_Text.font = m_text.font;
        //    var_Text.fontSize = m_text.fontSize;
        //    var_Text.fontStyle = m_text.fontStyle;
        //    var_Text.lineSpacing = m_text.lineSpacing;
        //    var_Text.supportRichText = m_text.supportRichText;
        //    //Paragraph 属性
        //    var_Text.alignment = m_text.alignment;
        //    var_Text.alignByGeometry = m_text.alignByGeometry;
        //    var_Text.horizontalOverflow = m_text.horizontalOverflow;
        //    var_Text.verticalOverflow = m_text.verticalOverflow;
        //    var_Text.resizeTextForBestFit = m_text.resizeTextForBestFit;
        //    var_Text.resizeTextMinSize = m_text.resizeTextMinSize;
        //    var_Text.resizeTextMaxSize = m_text.resizeTextMaxSize;
        //    //其他
        //    var_Text.raycastTarget = false;
        //}

        void Update()
        {
            if (losatType != m_Type || text != Content || m_text.fontSize != fontSize || lineSpacing != m_text.lineSpacing)
            {
                m_text.fontSize = fontSize;
                m_text.lineSpacing = lineSpacing;
                if (m_Type == Type.Password)
                    PasswordText();
                else if (m_Type == Type.Name)
                    NameText();
                else if (m_Type == Type.Money)
                    MoneyText();
                else
                    TextText();
                losatType = m_Type;
                Content = text;
            }
        }
        /// <summary>
        /// 密码文字
        /// </summary>
        private void PasswordText()
        {
            int lenght = m_text.text.Length;
            string content = "";
            for (int i = 0; i < lenght; i++)
            {
                content += "*";
            }
            m_text.text = content;
        }
        /// <summary>
        /// 文本文字
        /// </summary>
        private void TextText()
        {
            m_text.text = text;
        }
        /// <summary>
        /// 昵称文字
        /// </summary>
        private void NameText()
        {
            m_text.text = text;
            float temp_Width = m_text.preferredWidth;
            
            if (temp_Width > m_Rect.rect.width)
            {
                float width = temp_Width / text.Length;
                int sum = (int)(m_Rect.rect.width / width);
                string content = text.Substring(0, sum);
                m_text.text = content + "...";
                try
                {
                    while (m_text.preferredWidth > m_Rect.rect.width)
                    {
                        sum--;
                        content = text.Substring(0, sum);
                        m_text.text = content + "...";
                    }
                }
                catch (Exception e)
                {
                    m_text.text = text;
                    Debug.LogError(e.Message);
                }
            }
            else
            {
                 m_text.text = text;
            }
        }
        /// <summary>
        /// 金币文字
        /// </summary>
        private void MoneyText()
        {
            float Sum = 0;
            float.TryParse(text, out Sum);
            if (Mathf.Abs(Sum) < 10000)
            {
                m_text.text = ((long)Sum).ToString();
            }
            else if ((Mathf.Abs(Sum) >= 10000) && (Mathf.Abs(Sum) < 100000000))
            {
                float a = Sum / 10000.0f;
                if (Isfloat)
                {
                    if(Sum % 10000 >=50&& Sum % 10000 <= 9950)
                        m_text.text = string.Format("{0:N2}万", a);
                    else
                        m_text.text = string.Format("{0:N0}万", a);
                }
                else
                    m_text.text = string.Format("{0}万", (long)a);
            }
            else if ((Mathf.Abs(Sum) >= 100000000) && (Mathf.Abs(Sum) < 1000000000000))
            {
                float a = Sum / 100000000.0f;
                if (Isfloat)
                    if (Sum % 100000000.0f >= 10000*50&& Sum % 10000 <= 10000*9950)
                        m_text.text = string.Format("{0:N2}亿", a);
                    else
                        m_text.text = string.Format("{0:N0}亿", a);
                else
                    m_text.text = string.Format("{0}亿", (long)a);
            }
            else if (Mathf.Abs(Sum) >= 1000000000000)
            {
                float a = Sum / 100000000.0f;
                int idx = 0;
                while (true)
                {
                    idx++;
                    a = a / 10.0f;
                    if (Mathf.Abs((float)a) < 10)
                        break;
                }
                if (Isfloat)
                    m_text.text = string.Format("{0:N2}^{1}亿", a, idx);
                else
                    m_text.text = string.Format("{0}^{1}亿", (long)a, idx);
            }
        }
    }

}