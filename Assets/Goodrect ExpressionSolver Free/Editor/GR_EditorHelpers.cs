using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class GR_EditorHelpers : Editor
{
    private GUIStyle boxBG;
    private GUIStyle header;

    static public void StringField(ref string val, string title, bool multiline = false, bool inline = false)
    {
        GUIStyle textAreastyle = new GUIStyle(EditorStyles.textArea);
        textAreastyle.wordWrap = true;

        if (inline) EditorGUILayout.BeginHorizontal();
        if (title.Length > 0)
            EditorGUILayout.PrefixLabel(title);
        if (multiline)
            val = EditorGUILayout.TextArea(val, textAreastyle);
        else
            val = EditorGUILayout.TextField(val);
        if (inline) EditorGUILayout.EndHorizontal();
    }

    static public Texture2D ColorTexture(float r, float g, float b, float a)
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(1, 1, new Color(r, g, b, a));
        tex.filterMode = FilterMode.Point;
        tex.Apply();

        return tex;
    }

    static public void GameObjectField(ref GameObject gameObject, string title)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(title);
        gameObject = (GameObject)EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
    }

    static public void RangeFieldInt(ref int val, string title, int min, int max)
    {
        EditorGUILayout.LabelField(title);
        val = EditorGUILayout.IntSlider(val, min, max);
    }

    static public void CurveField(ref AnimationCurve curve, string title)
    {
        if (curve == null)
            curve = AnimationCurve.Linear(0, 0, 1, 1);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(title);
        curve = EditorGUILayout.CurveField(curve);
        EditorGUILayout.EndHorizontal();
    }

    static public void BeginGroup(string name, GUIStyle groupStyle, GUIStyle headerStyle)
    {
        EditorGUILayout.BeginVertical(groupStyle);
        EditorGUILayout.LabelField(name, headerStyle);
        EditorGUILayout.Space();
    }

    static public void EndGroup()
    {
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }

    static public void DrawLogo(Texture[] t)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(60));
        EditorGUI.DrawRect(r, Color.clear);
        r.height -= 10;
        r.width = r.height * t[0].width / t[0].height;

        GUI.DrawTexture(r, EditorGUIUtility.isProSkin ? t[1]:t[0], ScaleMode.ScaleToFit, true);
    }

    static public void AlignVCenter(bool end = false)
    {
        if(end)
        {
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
        }
    }
}
#endif