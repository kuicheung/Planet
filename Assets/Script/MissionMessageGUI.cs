using UnityEngine;
using System.Collections;

public class MissionMessageGUI : MonoBehaviour {

	public Texture2D guiTextImg;
	

	void OnGUI(){
		GUI.Label(new Rect(Screen.width/2,Screen.height/2-100,400,200),guiTextImg);
		if(GUI.Button(new Rect(Screen.width/2+250,Screen.height/2+10,80,40),"Close")){
			gameObject.SetActive(false);
		}
	}
	
}
