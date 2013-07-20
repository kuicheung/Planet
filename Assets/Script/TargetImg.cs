using UnityEngine;
using System.Collections;

public class TargetImg : MonoBehaviour {

	public Texture2D targetImage;
	
	void OnGUI(){
		GUI.Label (new Rect ((Screen.width-124)/2,(Screen.height-128)/2,128,128), targetImage);
	}
}
