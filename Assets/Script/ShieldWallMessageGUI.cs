using UnityEngine;
using System.Collections;

public class ShieldWallMessageGUI : MonoBehaviour {

	public Texture2D guiTextImg;
	
	
	void Start(){
		gameObject.SetActive(false);	
	}
	void OnGUI(){
		GUI.Label(new Rect(Screen.width/2,Screen.height/2-100,400,200),guiTextImg);

		
	}
	
}
