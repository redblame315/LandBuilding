//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

[AddComponentMenu("NGUI/Examples/Drag and Drop Item (Example)")]
public class ExampleDragDropItem : UIDragDropItem
{
	/// <summary>
	/// Prefab object that will be instantiated on the DragDropSurface if it receives the OnDrop event.
	/// </summary>

	public GameObject prefab;

	/// <summary>
	/// Drop a 3D game object onto the surface.
	/// </summary>

	protected override void OnDragDropRelease (GameObject surface)
	{
		if (surface != null)
		{
			ExampleDragDropSurface dds = surface.GetComponent<ExampleDragDropSurface>();

			if (dds != null)
			{
				GameObject child = NGUITools.AddChild(dds.gameObject, prefab);

				child.name = prefab.name + "_" + Mathf.RoundToInt(Time.time * 1000);
				child.layer = LayerMask.NameToLayer("Object");
				Transform trans = child.transform;
				trans.position = UICamera.lastHit.point;

				if (dds.rotatePlacedObject)
				{
					trans.rotation = Quaternion.LookRotation(UICamera.lastHit.normal) * Quaternion.Euler(90f, 0f, 0f);
				}

				if(child.tag == "NormalObject")
				{
					MainScreen.instance.normalObjectInfoDialog.SetTarget(trans, prefab.name);
				}
				else if(child.tag == "ImageObject")
					MainScreen.instance.imageObjectInfoDialog.SetTarget(trans, prefab.name);
				else if(child.tag == "VideoObject")
					MainScreen.instance.videoObjectInfoDialog.SetTarget(trans, prefab.name);
				// Destroy this icon as it's no longer needed
				NGUITools.Destroy(gameObject);

				// Save to firebase db
				
				return;
			}
		}
		base.OnDragDropRelease(surface);
	}
}
