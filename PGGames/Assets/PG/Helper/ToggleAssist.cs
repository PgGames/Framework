/**********************************************
 * 脚本类型：辅助脚本
 * 创建时间：2017/3/3
 * 创 建 人：彭钢
 * 功能描述：应用于UGUI中当开关的抬起状态由多个组成时运用该脚本协助Toggle完成Toggle 的抬起事件
 **********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[AddComponentMenu("Help/Toggle")]
[RequireComponent(typeof(Toggle))]
//[RequireComponent(typeof(Image))]
public class ToggleAssist : MonoBehaviour {
    private Toggle toggle;
    public GameObject[] ValueTrue;
    public GameObject[] ValueFalse;

    private List<UnityAction<bool>> list;
    
    void Awake()
    {
        list = new List<UnityAction<bool>>();
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(SetGraphicActive);
        toggle.onValueChanged.AddListener(onValueChanged);
    }
    void Start()
    {
        if (ValueTrue == null)
        {
            Debug.LogError("ValueTrue be incapable of is Null");
            return;
        }
        SetGraphicActive(toggle.isOn);
    }
    private void SetGraphicActive(bool active)
    {
		if (ValueTrue != null) 
		{
			for (int i = 0; i < ValueTrue.Length; i++) 
			{
				if (ValueTrue [i] != null) 
				{
					ValueTrue[i].SetActive(active);
				}
			}
		}
		if (ValueFalse != null) 
		{
			for (int i = 0; i < ValueFalse.Length; i++) {
				if (ValueFalse [i] != null) 
				{
					ValueFalse[i].SetActive(!active);
				}
			}
		}
    }
    public void AddListener(UnityAction<bool> action)
    {
        list.Add(action);
    }
    protected void onValueChanged(bool ison)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i](ison);
        }
    }
}
