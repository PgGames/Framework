using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Xml;
using System.Text;
using System.IO;


public class CustomFont : EditorWindow
{

    private Font font;
    private TextAsset xmlText;

    [MenuItem("Window/CustomFont")]
    static void AddWindow()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 500, 500);
        CustomFont window = (CustomFont)EditorWindow.GetWindowWithRect(typeof(CustomFont), wr, true, "自定义字体");
        window.Show();

    }

    // Use this for initialization
    void Start()
    {

    }

    void OnGUI()
    {
        font = EditorGUILayout.ObjectField("字体", font, typeof(Font), true) as Font;
        xmlText = EditorGUILayout.ObjectField("文字XML配置", xmlText, typeof(TextAsset), true) as TextAsset;

        if (GUILayout.Button("创建字体", GUILayout.Width(200)))
        {
            this.CreateFont();
        }
    }

    void CreateFont()
    {
        XmlDocument _doc = new XmlDocument();
        byte[] _array = Encoding.ASCII.GetBytes(xmlText.text);
        MemoryStream _stream = new MemoryStream(_array);
        _doc.Load(_stream);

        XmlNode _font = _doc.SelectSingleNode("font");
        XmlElement _common = (XmlElement)_font.SelectSingleNode("common");

        float _scaleW = float.Parse(_common.GetAttribute("scaleW"));
        float _scaleH = float.Parse(_common.GetAttribute("scaleH"));

        XmlNode _chars = _font.SelectSingleNode("chars");
        XmlNodeList _charsList = _chars.ChildNodes;

        CharacterInfo[] _infos = new CharacterInfo[_charsList.Count];
        for (int i = 0; i < _charsList.Count; i++)
        {
            XmlElement _element = (XmlElement)_charsList[i];
            CharacterInfo _characterInfo = new CharacterInfo();
            _characterInfo.index = int.Parse(_element.GetAttribute("id"));

            float _x = float.Parse(_element.GetAttribute("x"));
            float _y = float.Parse(_element.GetAttribute("y"));

            int _width = int.Parse(_element.GetAttribute("width"));
            int _height = int.Parse(_element.GetAttribute("height"));

            int _xadvance = int.Parse(_element.GetAttribute("xadvance"));

            _characterInfo.uv = new Rect(_x / _scaleW, 1 - (_y + _height) / _scaleH, _width / _scaleW, _height / _scaleH);
            //_characterInfo.uvBottomLeft = new Vector2(_x / _scaleW, 1 - (_y + _height) / _scaleH);
            //_characterInfo.uvBottomRight = new Vector2(_width / _scaleW, _height / _scaleH);
            _characterInfo.vert = new Rect(0, 0, _width, -_height);
            //_characterInfo.minX = 0;
            //_characterInfo.maxX = 0;
            //_characterInfo.minY = _width;
            //_characterInfo.maxY = -_height;
            _characterInfo.width = _xadvance;
            //Debug.Log(_characterInfo.uv);
            _infos[i] = _characterInfo;
        }
        font.characterInfo = _infos;
        EditorUtility.SetDirty(font);
    }
}

//作者：重装机霸
//链接：https://www.jianshu.com/p/675288dd4368
//來源：简书
//简书著作权归作者所有，任何形式的转载都请联系作者获得授权并注明出处。