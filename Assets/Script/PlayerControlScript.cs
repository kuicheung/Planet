using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Require these components when using this script
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CharacterController))]
public class PlayerControlScript : MonoBehaviour
{				
	
	public float animSpeed = 1.5f;				// a public setting for overall animator animation speed
	public float lookSmoother = 3f;				// a smoothing setting for camera motion
	public bool useCurves;						// a setting for teaching purposes to show use of curves

	public GameObject bullets;
	public GameObject iceBullet;
	public GameObject granades;
	public GameObject guiBar;
	public GameObject mainCam;
	public GameObject pistolModel;
	public GameObject uziModel;
	public GameObject mixGunModel;
	public GameObject grenadeLauncherModel;
	public GameObject iceGunModel;
	public List<GameObject> followers;
	public Texture2D PP7Image;
	public Texture2D PP7SelectedImage;
	public Texture2D headShot;
	public bool firing = false;
	public bool switching = false;
	public bool jumping = false;
	public float jumpingTime;
	public float maxJumpTime = 0.5f;
	public float speed = 10;	
	public AudioClip pp7Sound;	
	public bool playingSound = false;
	public float turnSpeed = 1;
	public float jumpSpeed = 50;
	public float gravity = 0.5f;
	public float h;
	public float v;
	public float s;
	public int life = 100;
	public Vector3 initPos;
	public Quaternion initRotation;
	public int maxLife = 100;
	public int weaponIndex = 0;
	
	private List<Weapon> weapons;
	private List<string> weaponsName;
	private Animator anim;							// a reference to the animator on the character
	private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer
	private AnimatorStateInfo layer2CurrentState;	// a reference to the current state of the animator, used for layer 2
	private CharacterController controller;					// a reference to the capsule collider of the character
	private GameObject currentGun;
	
	static int idleState = Animator.StringToHash("Base Layer.Idle");	
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");			// these integers are references to our animator's states
	static int jumpState = Animator.StringToHash("Base Layer.Jump");	
	

	void Start ()
	{
		// initialising reference variables
		anim = GetComponent<Animator>();					  
		controller = GetComponent<CharacterController>();	
		initPos = transform.position;
		initRotation = transform.rotation;
		weapons = new List<Weapon>();
		weaponsName = new List<string>();
		weapons.Add(new Weapon(PP7Image,PP7SelectedImage,10f,1f,0.8f,false,bullets,"PP7",pp7Sound));
		weaponsName.Add("PP7");		
		if (anim.layerCount >= 2)
			anim.SetLayerWeight(1, 1);
		uziModel.SetActive(false);
		mixGunModel.SetActive(false);
		grenadeLauncherModel.SetActive(false);
		iceGunModel.SetActive(false);
		currentGun=pistolModel;
	}
	
	
	void FixedUpdate ()
	{
		h = Input.GetAxis("Horizontal");				// setup h variable as our horizontal input axis
		v = Input.GetAxis("Vertical");				// setup v variables as our vertical input axis
		s = Input.GetAxis("Sideway");			
		if((s>0.1f||s<-0.1f)&&(v<0.1&&v>-0.1)){
			anim.SetFloat("Speed",.5f);
		}
		else
			anim.SetFloat("Speed", Mathf.Clamp(v,-1f,0.7f));
		anim.SetFloat("Direction",s);
													// set our animator's float parameter 'Direction' equal to the horizontal input axis		
		anim.speed = animSpeed;								// set the speed of our animator to the public variable 'animSpeed'
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// set our currentState variable to the current state of the Base Layer (0) of animation		
		if (anim.layerCount == 2)
			layer2CurrentState = anim.GetCurrentAnimatorStateInfo(1);	// set our layer2CurrentState variable to the current state of the second Layer (1) of animation

		//Jumping
		if(controller.isGrounded){
			if(Input.GetButtonDown("Jump")){
				jumping = true;
				jumpingTime = Time.time;
				anim.SetBool("Jump",true);
			}
		}
		else{
			anim.SetBool("Jump", false);				
			}
		
		if(Input.GetButtonUp("Jump"))
			jumping = false;
		
		if(Input.GetButton("Jump")&&(jumping==true)&&(Time.time-jumpingTime<maxJumpTime))
		{
			controller.Move(new Vector3(0,jumpSpeed*Time.deltaTime,0));
		}
		
		if(!switching&&Input.GetKeyDown(KeyCode.Tab)){
			StartCoroutine(SwitchWeapon(weaponIndex+1));					
		}
		
	
		
		
		
		//Moving character
		Vector3 moveDirection = new Vector3(s*speed*Time.deltaTime,-gravity*Time.deltaTime,v*speed*Time.deltaTime);
		transform.Rotate(0,h*Time.deltaTime*turnSpeed,0);
		moveDirection = transform.TransformDirection(moveDirection);
		controller.Move(moveDirection);
	}
	
	void Update(){
		//Firing weapon
		Weapon weapon = weapons[weaponIndex];
		if(Input.GetButton("Fire1")&&(GUIUtility.hotControl==0)){			
			anim.SetBool("Shoot",true);
			if(!firing){			
				StartCoroutine(Fire(weapon.getFiringSpeed(),weapon.getSound(),weapon.getAmmo()));
			}
		}
		else
			anim.SetBool("Shoot",false);
	}
	
	IEnumerator Fire (float firingSpeed, AudioClip sound,GameObject ammo) {	
		RaycastHit[] hits;
		hits = Physics.RaycastAll(mainCam.transform.position,mainCam.transform.forward).OrderBy(h=>h.distance).ToArray();
		int i = 0;		
		while(hits[i].collider.name=="Player"||hits[i].collider.tag=="Friendly")		
			i++;			
		Vector3 cameraLookAtPos = hits[i].point;
		Vector3 startPos = currentGun.transform.position+transform.forward*2;//transform.position+transform.up*2+transform.forward*2;
		GameObject bullet = (GameObject) Instantiate(ammo,startPos,transform.rotation);
		bullet.SendMessage("Fire",cameraLookAtPos);
		if(!playingSound)
			StartCoroutine(GunSound (sound));
		firing = true;
		yield return new WaitForSeconds(firingSpeed);
		firing = false;		
	}
	
	IEnumerator GunSound(AudioClip sound) {
		playingSound = true;
		AudioSource.PlayClipAtPoint(sound,gameObject.transform.position);
		yield return new WaitForSeconds(.3f);
		playingSound = false;
	}

	
	IEnumerator SwitchWeapon (int wIndex) {	
		switching = true;
		weaponIndex=wIndex;
		if(weaponIndex>=weapons.Count)
			weaponIndex=0;
		string wepName = weapons[weaponIndex].getName();
		if(wepName.Equals("PP7")||wepName.Equals("IceGun")){
			currentGun.SetActive(false);
			currentGun=pistolModel;
			currentGun.SetActive(true);
		}
		if(wepName.Equals("IceGun")){
			currentGun.SetActive(false);
			currentGun=iceGunModel;
			currentGun.SetActive(true);
		}
		else if(wepName.Equals("UZI")){
			currentGun.SetActive(false);
			currentGun=uziModel;
			currentGun.SetActive(true);
		}
		else if(wepName.Equals("IceGrenadeLauncher")){
			currentGun.SetActive(false);
			currentGun=mixGunModel;
			currentGun.SetActive(true);
		}
		else if(wepName.Equals("GrenadeLauncher")){
			currentGun.SetActive(false);
			currentGun=grenadeLauncherModel;
			currentGun.SetActive(true);
		}
		yield return new WaitForSeconds(0.1f);
		switching = false;
	}
	
	void ChangeLife(int amountToChange){
		life += amountToChange;
		life = Mathf.Clamp(life,0,100);
		guiBar.SendMessage("SetLife",life/100f);
		if(life<=0)
			StartCoroutine("Dead");
	}
	
	void Dead() {
		transform.position = initPos;
		transform.rotation = initRotation;
		life = maxLife;
		guiBar.SendMessage("SetLife",life/100f);
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
			enemy.SendMessage("Reset");
		}
		foreach(GameObject friendly in GameObject.FindGameObjectsWithTag("Friendly")){
			friendly.SendMessage("Reset");
		}
	}
	
	void AddWeapon(Weapon weapon){
		if(!weaponsName.Contains(weapon.getName())){
			weapons.Add(weapon);
			weaponsName.Add(weapon.getName());
			guiBar.SendMessage("AddWeaponImage",weapon);
			foreach(GameObject follower in followers)
				follower.SendMessage("AddWeapon",weapon);
		}
	}
	
	public void GetWeapons(GameObject friendly){
		followers.Add(friendly);
		friendly.SendMessage("AddWeapons",weapons);	
		guiBar.SendMessage("AddPlayer",friendly);
	}
	
	
	public void MoveTo(Vector3 pos){
		return;	
	}
	public void UnSelect(){
		return;	
	}
}
