using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    public static float speed = 6;


    [SerializeField] private Transform groundCheck;

    [SerializeField] private LayerMask groundMask;

    [SerializeField] private CharacterController controller;

    private float gravity = -9.81f;
    private float groundDistance = 0.4f;
    private float currentLerpTime = 0;
    private float lerpTime = 15;
    private float cameraZDistance;
    private float rotationValue = 0;

    private bool isGrounded;
    private bool isArrivedFinishPoint = false;

    private Animator animator;

    private EchoEffect echoEffect;

    private Vector3 velocity;

    #endregion

    #region Start and Update Methods
    void Start()
    {
        cameraZDistance = Camera.main.WorldToScreenPoint(transform.position).z;
        animator = GetComponent<Animator>();
        echoEffect = GetComponent<EchoEffect>();
    }


    void Update()
    {
        if(transform.position.y < -30 )
        {
            GameManager.Instance.GameOver();
        }
        
        if (Input.GetMouseButton(0) && GameManager.Instance.gameStarted && isArrivedFinishPoint)
        {
            animator.SetTrigger("IsRunning");
        }

        if (GameManager.Instance.gameStarted && !isArrivedFinishPoint)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            
            if (isGrounded && velocity.y < 0)
            {
                animator.SetTrigger("IsRunning");
                velocity.y = -2;
            }

             
            
                Vector3 move = transform.forward;
                controller.Move(move * speed * Time.deltaTime);
                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime);
                currentLerpTime += Time.deltaTime;
                Vector3 ScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
                Vector3 NewWorldPosition = Camera.main.ScreenToWorldPoint(ScreenPosition);
                rotationValue = NewWorldPosition.x * 5;
                Quaternion target = Quaternion.Euler(transform.rotation.x, Mathf.Clamp(rotationValue, -30, 30), transform.rotation.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, target, currentLerpTime / lerpTime);
            
           
        }

    }

    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "tile" && GameManager.Instance.gameStarted)
        {
            StartCoroutine(FallTiles(other.gameObject));
            //echo effect should work only while the character is flipping.
            //we don't know where the character will fall exactly so we need to check if the character's in the ground
            if (echoEffect.enabled == true)
                echoEffect.enabled = false;
            Destroy(other.transform.gameObject, 4);
        }
        if (other.tag == "final")
        {
            
            isArrivedFinishPoint = true;
            
            GameManager.Instance.LevelCompleted = true;
            GameManager.Instance.CompleteLevel();
        }

        if (other.tag == "jumperButton")
        {
            velocity.y = 6;
            animator.SetTrigger("IsFlipping");
            echoEffect.enabled = true;
            StartCoroutine(disableEchoEffect());

            if (other.transform.gameObject != null)
                Destroy(other.transform.gameObject, 2);
        }
    }

    #region Private Methods
    private IEnumerator FallTiles(GameObject other)
    {
      
        yield return new WaitForSeconds(0.05f);
        if (other.transform.gameObject.GetComponent<Rigidbody>() == null)
        {
            other.transform.gameObject.AddComponent<Rigidbody>();
            
        }

        other.transform.gameObject.GetComponent<Collider>().enabled = false;
    }

    //in order to disable echo effect, even if character falls down through all the layers
    private IEnumerator disableEchoEffect()
    {
        yield return new WaitForSeconds(3);
        echoEffect.enabled = false;

    }
    #endregion


}
