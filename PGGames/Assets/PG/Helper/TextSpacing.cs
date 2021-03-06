/**********************************************
*	协助脚本：用于控制UGUI中text组件的字间距
*	创建人：彭钢
**********************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


[AddComponentMenu("UI/Effects/TextSpacing")]
public class TextSpacing : BaseMeshEffect
{
	public float _Text_Spacing = 1f;
	public override void ModifyMesh (VertexHelper vh)
	{
		if (!IsActive () || vh.currentVertCount == 0) 
		{
			return;
		}
		List<UIVertex> vertex = new List<UIVertex> ();
		vh.GetUIVertexStream (vertex);
		int indexCount = vh.currentIndexCount;
		UIVertex vt;
        for (int i = 6; i < indexCount; i++)
        {
            vt = vertex[i];
            vt.position += new Vector3(_Text_Spacing * (i / 6), 0, 0);
            vertex[i] = vt;
            if (i % 6 <= 2)
            {
                vh.SetUIVertex(vt, (i / 6) * 4 + i % 6);
            }
            if (i % 6 == 4)
            {
                vh.SetUIVertex(vt, (i / 6) * 4 + i % 6 - 1);
            }

        }
	}
}
