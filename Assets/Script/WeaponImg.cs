using UnityEngine;
using System.Collections;

public class WeaponImg : MonoBehaviour {

	public Texture2D weaponImage;
	public float life=1.0f;
	
	void OnGUI(){
		GUI.Label (new Rect (10,30,48,48), weaponImage);
	}
	
	void SetImage(Texture2D newImage) {
		weaponImage = newImage;
	}
}
