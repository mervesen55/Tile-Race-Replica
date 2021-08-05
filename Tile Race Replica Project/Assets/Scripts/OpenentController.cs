using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenentController : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    private RaycastHit hit;
    private float rotationValue = 10;
    private Animator animator;
    private float timeRemaining = 0.5f;
    private Vector3 velocity;
    private CharacterController controller;
    private bool isGrounded;
    private bool isArrivedFinishPoint = false;
    private float gravity = -9.81f;
    private float groundDistance = 0.4f;
    private EchoEffect echoEffect;
    private float speed = 7.5f;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        echoEffect = GetComponent<EchoEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        if(transform.position.y < -30 && !GameManager.instance.levelCompleted)
        {
            gameObject.SetActive(false);
            transform.position = new Vector3(0, 0, 0);
            gameObject.SetActive(true);
        }
        if (Input.GetMouseButton(0) && GameManager.instance.gameStarted && (!isArrivedFinishPoint &&  !GameManager.instance.levelCompleted))
        {

            animator.SetTrigger("IsRunning");

        }

       
        if (GameManager.instance.gameStarted && (!isArrivedFinishPoint && !GameManager.instance.levelCompleted))
        {
         
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                animator.SetTrigger("IsRunning");
                velocity.y = -2;
            }
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z), transform.forward, out hit, 3))
            {
              
              
                //if (hit.collider.gameObject.tag == "endOfLayer")
                //{
                    
                //    animator.SetTrigger("IsJumping");
                //    //transform.position += transform.forward * Time.deltaTime * PlayerController.speed;
                //    //echoEffect.enabled = true;
                //    GetComponent<Rigidbody>().velocity = new Vector3(0, 10, 1);
                //    //StartCoroutine(disableEchoEffect());
                //}
                //if(hit.collider.gameObject.tag == "wall")
                //{
                //    transform.rotation = Quaternion.Euler(transform.rotation.x, Mathf.Clamp(rotationValue, -20, 20), transform.rotation.z);
                //}
                //else
                {

                    // transform.position += transform.forward * Time.deltaTime * PlayerController.speed; //character and openents' speed need to be the same
                    Vector3 move = transform.forward;
                    controller.Move(move * PlayerController.speed * Time.deltaTime);
                }
            }
            else
            {
                
                if(timeRemaining <= 0)
                {
                    int direction = Random.Range(-1, 1);
                    if (direction == 0)
                    {
                        direction = 1;
                    }
                    rotationValue += 8;
                    timeRemaining = 0.5f;
                    transform.rotation = Quaternion.Euler(transform.rotation.x, Mathf.Clamp(rotationValue * direction, -20, 20), transform.rotation.z);

                }
                else
                {
                    //transform.position += transform.forward * Time.deltaTime * PlayerController.speed;
                    Vector3 move = transform.forward;
                    controller.Move(move * speed * Time.deltaTime);
                }
                
               
                
            }



        }
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "tile" && GameManager.instance.gameStarted)
        {
            if (echoEffect.enabled == true)
                echoEffect.enabled = false;
            StartCoroutine(FallTiles(other.gameObject));
            Destroy(other.transform.gameObject, 4);
        }
        if (other.tag == "jumperButton")
        {
            velocity.y = 6;
            animator.SetTrigger("IsFlipping");
            echoEffect.enabled = true;
            //GetComponent<Rigidbody>().velocity = new Vector3(0, 10, 4);
            StartCoroutine(disableEchoEffect());
            if (other.transform.gameObject != null)
                Destroy(other.transform.gameObject, 2);
            //other.transform.parent.tag = "tile";
        }
        if (other.tag == "final")
        {
            //Destroy(gameObject);
            isArrivedFinishPoint = true;
            //animator.SetTrigger("IsWaiting");
        }
    }
  
    private IEnumerator FallTiles(GameObject other)
    {

        yield return new WaitForSeconds(0.05f);
        if (other.transform.gameObject.GetComponent<Rigidbody>() == null)
        {
            other.transform.gameObject.AddComponent<Rigidbody>();

        }

        other.transform.gameObject.GetComponent<Collider>().enabled = false;
    }
    private IEnumerator ResetRotation()
    {
        yield return new WaitForSeconds(1);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private IEnumerator disableEchoEffect()
    {
        yield return new WaitForSeconds(3);
        echoEffect.enabled = false;

    }
}
