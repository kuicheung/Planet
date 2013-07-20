using UnityEngine;
using System.Collections;

public class MousePositionOnScreen : MonoBehaviour {
	
	public GameObject particle;
	public RaycastHit hitInfo;
	public Collider selected;
	public Vector3 worldPosition;

	// Update is called once per frame
	
	void Start() {
		selected = GameObject.Find("Player").collider;
	}
	void Update () {
		
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast (ray,out hitInfo,100f)) {
            worldPosition = hitInfo.point;
			if(Input.GetButtonDown("Fire1")){
				if(hitInfo.collider.tag=="Friendly"){
					if(selected.collider.name != hitInfo.collider.name){
						selected.SendMessage("UnSelect",0f);
						selected = hitInfo.collider;						
						selected.SendMessage("Select");
					}
				}
				else{
					selected.SendMessage("MoveTo",worldPosition);
					selected = GameObject.Find("Player").collider;
				}
			}
			/*else if(Input.GetButtonDown("Fire2")){
				selected.SendMessage("UnSelect");
				selected = GameObject.Find("Player").collider;
			}*/
           // Instantiate (particle, hitInfo.point, transform.rotation);
        }
   	// }
	}
}
