using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RealmComponent))]
// ReSharper disable once UnusedMember.Global
// ReSharper disable once CheckNamespace
public class RealmComponentEditor : Editor
{
	// ReSharper disable once UnusedMember.Global
	// ReSharper disable once InconsistentNaming
	protected void OnSceneGUI()
	{
		var component = target as RealmComponent;
		if(ReferenceEquals(null, component))
		{
			return;
		}

		foreach(var pair in component.Statuses)
		{
			// var rect = GUI.skin.label.CalcSize(new GUIContent(pair.Value.Title));
			Handles.Label(pair.Value.Position, pair.Value.Title, GUI.skin.label);
		}
	}
}