using UnityEngine;
using System.Collections;

public class HeroCamera : MonoBehaviour
{
	public static HeroCamera instance = null;
	public LayerMask collisionLayers = -1;
    public float heroHeight = 2.0f;
    public float heroDistance = 5.0f;
    public float minDistance = 2.0f;
    public float maxDistance = 10.0f;
	public int zoomRate = 200;
	public float zoomDampening = 5.0f;
	public float xSpeed = 200.0f;
	public float ySpeed = 200.0f;
	public bool invertMouseY = false;
    public float rotationDampening = 3.0f;
	public float positionDampening = 3.0f;
	public float offsetFromWall = 0.1f;
	public float fpsCamDist = -0.15f;
	
	public bool useIdleOrbit = true;
	
	public enum CameraState
	{
		FirstPerson,
		ThirdPerson,
		Orbit
	}
	public CameraState camState = CameraState.FirstPerson;
	
	public Transform cam = null;
	
	public Transform hero = null;
	public Transform headBone = null;
	public int minAngleY = -50;
	public int maxAngleY = 60;
    float xAngl = 0.0f;
    float yAngl = 0.0f;
    float curDist;
    float desDist;
    float finalDist;

	public VariableJoystick joystick;
	public VariableJoystick moveJoystick;
	public CamInput camInput;
	float fHeadBoneInitHeight = 0;
	bool bCanRotate = false;
	[System.Serializable]
	public class CamInput
	{
		public float mX = 0;
		public float mY = 0;
		public float mSW = 0;
		public bool doFPS = false;
		public bool do3rdP = false;
		public bool doOrbit = false;
		public bool doLShift = false;
	}

    private void Awake()
    {
		instance = this;
    }

    //=================================================================================================================o
    void Start ()
    {
		cam = Camera.main.transform;
		Vector3 angls = cam.eulerAngles;
    	xAngl = angls.y;
    	yAngl = angls.x;

		curDist = heroDistance;
    	desDist = heroDistance;
    	finalDist = heroDistance;

		fHeadBoneInitHeight = headBone.localPosition.y;

    }
	//=================================================================================================================o
	
	//=================================================================================================================o
    void LateUpdate ()
    {
		if (headBone == null || hero == null)
			return;

		/*if(HeroCtrl.instance.isMovingTarget)
        {
			Vector3 forward = HeroCtrl.instance.moveTargetTransform.position - cam.transform.position;
			forward.Normalize();

			Quaternion camRotation = Quaternion.LookRotation(forward);
			HeroCamera.instance.cam.rotation = camRotation;
			//HeroCamera.instance.cam.rotation = Quaternion.Slerp(HeroCamera.instance.cam.rotation, camRotation, 5 * Time.deltaTime);
		}*/
		//if (MainScreen.instance.curTransformDialog != null && MainScreen.instance.curTransformDialog.GetVisible())
		//	return;

		// Cached Input
		camInput.doFPS = Input.GetKeyDown ("1");
		camInput.do3rdP = Input.GetKeyDown ("2");
		camInput.doOrbit = Input.GetKeyDown("3");
		camInput.doLShift = Input.GetKey (KeyCode.LeftShift);
		camInput.mX = Input.GetAxis ("Mouse X");
		camInput.mY = Input.GetAxis ("Mouse Y");
		camInput.mSW = Input.GetAxis ("Mouse ScrollWheel");

		/*// 1,2,3 buttons for switching camera modi
		if (camInput.doFPS) 
		{
    		// FirstPerson
    		cam.GetComponent<Camera>().fieldOfView = 80.0f;
    		camState = CameraState.FirstPerson;
    	}
		else if (camInput.do3rdP)
		{
    		// ThirdPerson
			cam.GetComponent<Camera>().fieldOfView = 70.0f;
    		camState = CameraState.ThirdPerson;
    	}		*/

		bCanRotate = false;
		if (moveJoystick.Horizontal != 0 || moveJoystick.Vertical != 0)
			UIManager.instance.bBusy = true;

		if ((!UIDragDropItem.bDraggingNow && !UIManager.instance.bBusy && Input.GetMouseButton(0)) || Input.GetMouseButton(1) || joystick.Horizontal != 0 || joystick.Vertical != 0)
		{
			/*if (joystick.Horizontal != 0)
				camInput.mX = joystick.Horizontal;

			if (joystick.Vertical != 0)
				camInput.mY = joystick.Vertical;
			*/
			bCanRotate = true;
		}
		// Camera states
		switch (camState)
		{
			case CameraState.FirstPerson:
				FirstPerson();
				break;
			case CameraState.ThirdPerson:
				ThirdPerson();
				break;
			case CameraState.Orbit:
				Orbit();
				break;
		}

		UIManager.instance.bBusy = false;
	}

	//FPS Camera of Hero
	void FirstPerson ()
	{
		if(bCanRotate)
        {
			// Horizontal
			xAngl += camInput.mX * xSpeed * Time.deltaTime;
			// Vertical
			yAngl = ClampAngle(yAngl, minAngleY, maxAngleY);
		}
		// Apply Y-mouse axis

		// Desired distance
		desDist = fpsCamDist;
		// Camera rotation
    	Quaternion camRot = Quaternion.Euler (yAngl, xAngl, 0);
		// Camera position
		//Vector3 camPos = headBone.position - (cam.forward * desDist) - (cam.up * -heroHeight /4);
		Vector3 camPos = headBone.position - (cam.up * -heroHeight / 4);

		if(bCanRotate)
        {
			if (invertMouseY)
				yAngl += Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
			else
				yAngl -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

			cam.rotation = camRot;
		}
		


		// Apply position and rotation
		//cam.rotation = Quaternion.Lerp(cam.rotation, camRot, rotationDampening * Time.deltaTime);		
		//cam.position = Vector3.Lerp(cam.position, camPos, positionDampening * Time.deltaTime);
		cam.position = camPos;
		hero.eulerAngles = new Vector3(hero.eulerAngles.x, xAngl, hero.eulerAngles.z);
	}
	//=================================================================================================================o
	void ThirdPerson ()
	{
		// Desired distance via mouse wheel
		desDist = heroDistance;
		desDist -= camInput.mSW * Time.deltaTime * zoomRate * Mathf.Abs (desDist);
		desDist = Mathf.Clamp (desDist, minDistance, maxDistance);
		finalDist = desDist;

		// Horizontal smooth rotation
		//xAngl += camInput.mX * xSpeed * Time.deltaTime;
		xAngl = hero.transform.eulerAngles.y;
		// Vertical angle limitation
		yAngl = ClampAngle (yAngl, minAngleY, maxAngleY);
    	// Camera rotation
    	Quaternion camRot = Quaternion.Euler (yAngl, xAngl, 0);
    	// Camera height
    	Vector3 headPos = new Vector3 (0, -heroHeight /1.2f, 0);
    	// Camera position
    	Vector3 camPos = hero.position - (camRot * Vector3.forward * desDist + headPos);
		
		// Recalculate hero position
		Vector3 trueHeroPos = new Vector3 (hero.position.x, hero.position.y + heroHeight, hero.position.z);
		
		// Check for collision with Linecast
		RaycastHit hit;
		bool isOk = false;
		if ( Physics.Linecast (trueHeroPos, camPos - Vector3.up + Vector3.forward, out hit, collisionLayers.value)) // slightly behind and below the camera
		{
			// Final distance
			finalDist = Vector3.Distance (trueHeroPos, hit.point) - offsetFromWall;
			isOk = true;
		}
		
		// Lerp current distance if not corrected
		if ( !isOk || ( finalDist > curDist ) )
			curDist = Mathf.Lerp (curDist, finalDist, Time.deltaTime * zoomDampening);
		else
			curDist = finalDist;
		
		// Clamp current distance
		//curDist = Mathf.Clamp (curDist, minDistance, maxDistance);
		
		// Recalculate camera position
		camPos = hero.position - (camRot * Vector3.forward * curDist + headPos);
		
		// Left shift = no y rotation
		if(!camInput.doLShift)
		{
			// Apply Y-mouse axis
			if(invertMouseY)
			    yAngl += Input.GetAxis ("Mouse Y") * ySpeed * Time.deltaTime;
			else
			    yAngl -= Input.GetAxis ("Mouse Y") * ySpeed * Time.deltaTime;
		}


		// Apply position and rotation
		cam.rotation = Quaternion.Lerp(cam.rotation, camRot, rotationDampening * Time.deltaTime);
		cam.position = Vector3.Lerp(cam.position, camPos, positionDampening * Time.deltaTime);
	}
	//=================================================================================================================o
	void Orbit ()
	{
		// Desired distance via mouse wheel
		desDist -= camInput.mSW * Time.deltaTime * zoomRate * Mathf.Abs (desDist);
		desDist = Mathf.Clamp (desDist, minDistance, maxDistance);
		finalDist = desDist;
		
		// Horizontal smooth rotation
		xAngl += camInput.mX * xSpeed * 0.02f;
		// Vertical angle limitation
    	yAngl = ClampAngle (yAngl, minAngleY, maxAngleY);
		
		// Camera rotation
    	Quaternion camRot = Quaternion.Euler (yAngl, xAngl, 0);
    	// Camera height
    	Vector3 headPos = new Vector3 (0, -heroHeight /0.8f, 0);
    	// Camera position
    	Vector3 camPos = hero.position - (camRot * Vector3.forward * desDist + headPos);
		
		// Recalculate hero position
		Vector3 trueHeroPos = new Vector3 (hero.position.x, hero.position.y + heroHeight, hero.position.z);
		
		// Check if there is something between camera and character
		RaycastHit hit;
		bool isOk = false;
		if ( Physics.Linecast (trueHeroPos, camPos, out hit, collisionLayers.value))
		{
			// Final distance
			finalDist = Vector3.Distance (trueHeroPos, hit.point) - offsetFromWall;
			isOk = true;
		}
		
		// Lerp current distance if not corrected
		if ( !isOk || ( finalDist > curDist ) )
			curDist = Mathf.Lerp (curDist, finalDist, Time.deltaTime * zoomDampening);
		else
			curDist = finalDist;
		
		// Clamp current distance
		//curDist = Mathf.Clamp (curDist, minDistance, maxDistance);
		
		// Recalculate camera position
		camPos = hero.position - (camRot * Vector3.forward * curDist + headPos);
		
		// Left shift = no y rotation
		if(!camInput.doLShift)
		{
			// Apply Y-mouse axis
			if(invertMouseY)
			    yAngl += Input.GetAxis ("Mouse Y") * ySpeed * Time.deltaTime;
			else
			    yAngl -= Input.GetAxis ("Mouse Y") * ySpeed * Time.deltaTime;
		}


		// Apply position and rotation
		cam.rotation = Quaternion.Lerp(cam.rotation, camRot, rotationDampening * Time.deltaTime);
		cam.position = Vector3.Lerp(cam.position, camPos, positionDampening * Time.deltaTime);
	}
	//=================================================================================================================o
	
	public void SetCamerState(CameraState state)
    {
		camState = state;
		switch(camState)
        {
			case CameraState.FirstPerson:
				cam.GetComponent<Camera>().fieldOfView = 80.0f;
				break;
			case CameraState.ThirdPerson:
				cam.GetComponent<Camera>().fieldOfView = 70.0f;
				break;
        }
	}

	public void ChangeHeadBonePosition(float height)
    {
		headBone.localPosition = new Vector3(headBone.localPosition.x, fHeadBoneInitHeight + height * 2, headBone.localPosition.z);
    }
	// Clamp angle at 360deg
	static float ClampAngle ( float angle, float min, float max )
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}

	public void InitHeroCam()
    {
		cam = Camera.main.transform;
		Vector3 camPos = headBone.position - (cam.up * -heroHeight / 4);
		cam.rotation = transform.rotation;
		cam.position = camPos;

		yAngl = cam.eulerAngles.x;
		xAngl = cam.eulerAngles.y;
	}

	public void InitAngle(Vector3 targetAngle)
    {
		if (targetAngle.x > 180)
			yAngl = targetAngle.x - 360;
		else
			yAngl = targetAngle.x;
		yAngl = ClampAngle(yAngl, minAngleY, maxAngleY);
		xAngl = targetAngle.y;
	}
	//=================================================================================================================o
}
