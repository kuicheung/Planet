using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuiBar : MonoBehaviour {
	public Texture2D bgImage;
	public Texture2D fgImage;
	public Texture2D startImage;
	public Texture2D playerImage;
	public Texture2D bossImage;
	public Texture2D reactorImage;
	public Texture2D selectedImage;
	public GameObject player;
	public List<Texture2D> weaponImages;
	public List<GameObject> players;
	public List<Texture2D> playerImages;
	public List<int> selectedList;
	public List<Texture2D> selectedImages;
	
	public GUISkin mySkin;
	public float life=1.0f;
	public float bossLife = 0.82f;
	public bool bossFight = false;
	
	void Start() {
		weaponImages=new List<Texture2D>();
		weaponImages.Add(startImage);
		selectedImages=new List<Texture2D>();
		selectedImages.Add(selectedImage);
		players=new List<GameObject>();
		players.Add(player);
		playerImages = new List<Texture2D>();
		playerImages.Add(playerImage);
		selectedList = new List<int>();
		selectedList.Add(0);
	}
	
	void OnGUI(){
		
		int y = 0;
		int outterCt = 0;
		foreach(GameObject p in players){
			GUI.skin = mySkin;
			// Create one Group to contain both images
			// Adjust the first 2 coordinates to place it somewhere else on-screen
			GUI.Label(new Rect(10,y,48,48), playerImages[outterCt]);
			
			if(outterCt==0){
				GUI.BeginGroup (new Rect (60,y+10,256,32));
		
				// Draw the background image
				GUI.Label (new Rect (0,0,256,32), bgImage);
		
					// Create a second Group which will be clipped
					// We want to clip the image and not scale it, which is why we need the second Group
					GUI.BeginGroup (new Rect (0,0,life * 256, 32));
		
					// Draw the foreground image
					GUI.Label (new Rect (0,0,256,32), fgImage);
					
					// End both Groups
					GUI.EndGroup ();
					GUI.Label (new Rect (0,0,256,32),life*100+"/100");
		
				GUI.EndGroup ();
			}
			
			int x = 0;
			int ct = 0;
			foreach(Texture2D img in weaponImages){	
				Texture2D wepImg=img;
				if(selectedList[outterCt]==ct)
					wepImg=selectedImages[ct];
				if(GUI.Button(new Rect (x+10,y+50,48,48), wepImg)){
					p.SendMessage("SwitchWeapon",ct);
					selectedList[outterCt]=ct;
				}
				x+=48;
				ct++;
			}
			y+=100;
			outterCt++;
		}
		
		if(bossFight&&bossLife>0.001){	
			GUI.Label(new Rect(350,0,48,48), bossImage);
		
			GUI.BeginGroup (new Rect (400,10,256,32));
		
				// Draw the background image
				GUI.Label (new Rect (0,0,256,32), bgImage);
		
					// Create a second Group which will be clipped
					// We want to clip the image and not scale it, which is why we need the second Group
				GUI.BeginGroup (new Rect (0,0,bossLife * 256, 32));
		
					// Draw the foreground image
					GUI.Label (new Rect (0,0,256,32), fgImage);
					
					// End both Groups
				GUI.EndGroup ();
				GUI.Label (new Rect (0,0,256,32),bossLife*500+"/500");
		
			GUI.EndGroup ();			
		}
			
	}
	
	void SetLife(float newLife) {
		life = newLife;
	}
	
	void SetBossLife(float newLife) {
		bossLife = newLife;
	}
	
	void StartBossFight(){
		bossFight=true;	
	}
	
	void SetBossImage(Texture2D newImage){
		bossImage = newImage;
	}
	
	void AddWeaponImage(Weapon weapon) {
		weaponImages.Add(weapon.getImage());
		selectedImages.Add(weapon.getSelectedImage());
	}
	
	void AddPlayer(GameObject obj){
		players.Add(obj);	
	}
	
	void AddPlayer(Texture2D playImg){
		playerImages.Add(playImg);
		selectedList.Add(0);
	}
	
	void StartReactor(){
		bossImage = reactorImage;
		bossFight=true;
	}
}
