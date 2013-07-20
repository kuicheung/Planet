using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {
	
	public bool loading = false;
	public bool menuOn=false;
	public bool showControl=false;
	public Texture2D controlsBG;
	public GameObject pointer;
	public GUISkin skin;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
			if(!menuOn)
				menuOn=true;
	}
	
	void OnGUI(){
		if(!loading){
			if(!showControl){
				if(menuOn){
					GUI.skin=skin;
					if(pointer.activeSelf)
						pointer.SetActive(false);
					GUI.BeginGroup(new Rect(Screen.width/2-200,Screen.height/2-200,500,600));
						if(GUI.Button(new Rect(100,120,200,50),"Resume")){
							menuOn=false;	
							pointer.SetActive(true);
						}
						if(GUI.Button(new Rect(100,200,200,50),"Controls")){
							showControl=true;
						}
						if(GUI.Button(new Rect(100,280,200,50),"Quit")){
							Application.LoadLevel("startScreen");
						}
						GUI.EndGroup();
				
				}
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
