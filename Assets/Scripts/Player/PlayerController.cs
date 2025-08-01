
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // enable other scripts to access this script
    public static PlayerController instance;


    // player's movement speed
    public float moveSpeed = 12;

    public float gravityModifier = 12f;

    public float jumpPower = 12f;

    // player's run speed
    public float runSpeed = 12f;

    // reference to player character controller component
    public CharacterController characterController;

    // player input values
    private Vector3 moveInput;

    // reference to the camera rig position transform component
    public Transform cameraRigPosition;

    // mouse movement sensitivity
    public float mouseSensitivity;

    // mouse movement reversal flags
    public bool invertX;
    public bool invertY;


    // if player can jump / double jump
    private bool canJump;
    private bool canDoubleJump;


    public Transform groundCheckPoint;

    public LayerMask whatIsGround;

    // reference to player animator component
    public Animator anim;

    //public GameObject bullet;

    // reference to the player weapon fire position transform component
    public Transform firePoint;
  

    public List<Gun> allGuns = new List<Gun>();

    public List<Gun> unlockableGuns = new List<Gun>();

    public Gun activeGun;

    // currently active weapon index
    public int currentGun;

    // reference to the weapon aim adjustment position transform component
    public Transform weaponAimAdjustmentPoint;

    // reference to the player's weapon holder transform component
    public Transform gunHolder;

    // weapon position
    private Vector3 gunStartPosition;

    // weapon aim adjustment speed
    public float weaponAimAdjustmentSpeed = 2f;

    // reference to player weapon muzzle flash
    public GameObject muzzleFlash;

    // reference to player footstep audio
    public AudioSource footstepFast;
    public AudioSource footstepSlow;

    // how high the player bounces if the walk onto a bounce pad
    private float bounceAmount;

    // if the player can bounce
    private bool bounce;




    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentGun--;

        SwitchGun();

        gunStartPosition = gunHolder.localPosition;
    }


    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }


    private void PlayerInput()
    {
        // if the pause screen is not active
        // and
        // the game level is not ending
        if (!UIController.uiController.pauseScreen.activeInHierarchy && !GameController.instance.levelEnding)
        {
            MovePlayer();

            CameraControl();

            WeaponControl();


            anim.SetFloat("moveSpeed", moveInput.magnitude);

            anim.SetBool("onGround", canJump);
        }
    }


    private void MovePlayer()
    {
        // temporary store for player's 'y' axis movement
        // if the player jumps
        float yAxis = moveInput.y;

        // get player's vertical 'z' axis movement
        Vector3 verticleMovement = transform.forward * Input.GetAxis("Vertical");

        // get player's horizontal 'x' axis movement
        Vector3 horizontalMoveement = transform.right * Input.GetAxis("Horizontal");

        // set the player movement input
        moveInput = horizontalMoveement + verticleMovement;

        // normalise the player input value
        moveInput.Normalize();

        // if the player presses the 'left shift' key
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // player is running
            moveInput *= runSpeed;
        }

        // otherwise
        else
        {
            // player is walking
            moveInput *= moveSpeed;
        }

        // set the gravity to simulate the moon's gravity
        ///Physics.gravity = new Vector3(0, -1.62f, 0);

        moveInput.y = yAxis;

        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        if (characterController.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;

        if (canJump)
        {
            canDoubleJump = false;
        }

        //Handle Jumping
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            moveInput.y = jumpPower;

            canDoubleJump = true;

            AudioManager.instance.PlaySFX(8);
        }

        else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            moveInput.y = jumpPower;

            canDoubleJump = false;

            // play the jump sound
            AudioManager.instance.PlaySFX(8);
        }


        if (bounce)
        {
            bounce = false;

            moveInput.y = bounceAmount;

            canDoubleJump = true;
        }


        // move the player
        characterController.Move(moveInput * Time.deltaTime);
    }


    private void CameraControl()
    {
        // get camera rotation input
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        // if the 'invertx' flag is set
        if (invertX)
        {
            // reverse camera movement along the 'y' axis
            mouseInput.x = -mouseInput.x;
        }

        // if the 'inverty' flag is set
        if (invertY)
        {
            // reverse camera movement along the 'x' axis
            mouseInput.y = -mouseInput.y;
        }

        // rotate the player around the 'y' axis - camera rotation
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        // rotate the camera around the 'x' axis - camera tilt
        cameraRigPosition.rotation = Quaternion.Euler(cameraRigPosition.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));
    }


    private void WeaponControl()
    {
        // disable the weapon muzzle flash
        muzzleFlash.SetActive(false);


        // single shots //
        if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0)
        {
            RaycastHit hit;

            if (Physics.Raycast(cameraRigPosition.position, cameraRigPosition.forward, out hit, 50f))
            {
                if (Vector3.Distance(cameraRigPosition.position, hit.point) > 2f)
                {
                    firePoint.LookAt(hit.point);
                }
            }

            else
            {
                firePoint.LookAt(cameraRigPosition.position + (cameraRigPosition.forward * 30f));
            }



            //Instantiate(bullet, firePoint.position, firePoint.rotation);

            // fire the weapon
            FireShot();
        }


        // repeating shots //
        if (Input.GetMouseButton(0) && activeGun.canAutoFire)
        {
            if (activeGun.fireCounter <= 0)
            {
                FireShot();
            }
        }

        // if the player presses the 'tab' key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // select another weapon
            SwitchGun();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CameraController.instance.ZoomIn(activeGun.zoomAmount);
        }

        if (Input.GetMouseButton(1))
        {
            gunHolder.position = Vector3.MoveTowards(gunHolder.position, weaponAimAdjustmentPoint.position, weaponAimAdjustmentSpeed * Time.deltaTime);
        }

        else
        {
            gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition, gunStartPosition, weaponAimAdjustmentSpeed * Time.deltaTime);
        }



        if (Input.GetMouseButtonUp(1))
        {
            CameraController.instance.ZoomOut();
        }
    }


    // fire the weapon
    public void FireShot()
    {
        // if the currently acive weapon has ammo
        if (activeGun.currentAmmo > 0)
        {
            // decrease the weapon's ammo by one round
            activeGun.currentAmmo--;

            // create a bullet at the weapon's 'fire point'
            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);

            // set the weapon's fire rate
            activeGun.fireCounter = activeGun.fireRate;

            // update the weapon's ui
            UIController.uiController.ammoSlider.value = activeGun.currentAmmo;
            UIController.uiController.ammoText.text = activeGun.currentAmmo + " / " + activeGun.maximumAmmo;

            // show the weapon's muzzle flash
            muzzleFlash.SetActive(true);
        }
    }


    // if the player presses the 'tab' key
    // switch to another weapon
    public void SwitchGun()
    {
        // deactivate the currently active weapon
        activeGun.gameObject.SetActive(false);

        // increment the 'current gun' index
        currentGun++;

        // if the 'current gun' index is greater than or equal to 
        // then number of available weapons
        if (currentGun >= allGuns.Count)
        {
            // set the 'current gun' index to zero
            currentGun = 0;
        }

        // make the currently active weapon the selected weapon
        activeGun = allGuns[currentGun];

        // activate the weapon
        activeGun.gameObject.SetActive(true);

        // update the selected weapon's ui
        UIController.uiController.ammoSlider.maxValue = activeGun.maximumAmmo;
        UIController.uiController.ammoSlider.value = activeGun.currentAmmo;
        UIController.uiController.ammoText.text = activeGun.currentAmmo + " / " + activeGun.maximumAmmo;

        // set the 'fire position' of the selected weapon
        firePoint.position = activeGun.firepoint.position;
    }


    public void AddGun(string gunToAdd)
    {
        bool gunUnlocked = false;

        if (unlockableGuns.Count > 0)
        {
            for (int i = 0; i < unlockableGuns.Count; i++)
            {
                if (unlockableGuns[i].gunName == gunToAdd)
                {
                    gunUnlocked = true;

                    allGuns.Add(unlockableGuns[i]);

                    unlockableGuns.RemoveAt(i);

                    i = unlockableGuns.Count;
                }
            }
            
        }

        if (gunUnlocked)
        {
            currentGun = allGuns.Count - 2;

            SwitchGun();
        }
    }


    public void Bounce(float bounceForce)
    {
        bounceAmount = bounceForce;

        bounce = true;
    }


} // end of class
