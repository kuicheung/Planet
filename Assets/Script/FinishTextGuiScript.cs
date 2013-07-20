using UnityEngine;
using System.Collections;

public class FinishTextGuiScript : MonoBehaviour {

	public Texture2D guiTextImg;
	public bool finish=false;
	public GameObject target;
	
	
	void Start(){
		gameObject.SetActive(false);	
	}
	void OnGUI(){
			if(target.activeSelf)
				target.SetActive(false);
			GUI.Label(new Rect(Screen.width/2-200,Screen.height/2-300,400,600),guiTextImg);
			if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2,200,50),"Return Home")){
				Application.LoadLevel("startScreen");	
			}
	}
	

}
