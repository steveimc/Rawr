using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(MenuButton))]
public class MenuButtonEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		//MenuButton t = (MenuButton) target;
	}
}