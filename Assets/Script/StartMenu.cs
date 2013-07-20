using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {
	
	public bool loading = false;
	public bool showControl = false;
	public Texture2D mainMenuBG;
	public Texture2D startGameBG;
	public Texture2D controlsBG;
	
	void OnGUI(){
		if(!loading){
			if(!showControl){
				GUI.BeginGroup(new Rect(Screen.width/2-200,Screen.height/2-200,500,600));
				GUI.Label(new Rect(0,0,500,200),mainMenuBG);
				if(GUI.Button(new Rect(100,120,200,50),"Start Game")){
					Application.LoadLevel("level1");
					loading = true;
				}
				if(GUI.Button(new Rect(100,200,200,50),"Controls")){
					showControl=true;
				}
				GUI.EndGroup();
			}
			else{
				GUI.BeginGroup(new Rect(Screen.width/2-200,Screen.height/2-300,400,600));
				GUI.Label(new Rect(0,0,400,600),controlsBG);
				if(GUI.Button(new Rect(30,30,50,30),"Back")){
					showControl=false;
				}
				GUI.EndGroup();
			}
				
		}
		else
			GUI.Box(new Rect(Screen.width/2-50,Screen.height/2-20,100,40),"Loading...");
	}
}
