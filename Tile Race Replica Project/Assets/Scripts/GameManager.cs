using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    [SerializeField] private GameObject[] unSortedCharacters;
    [SerializeField] private GameObject[] crowns;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject TappedTooSoonText;
    [SerializeField] private GameObject greenLight;
    [SerializeField] private GameObject Cup;
    [SerializeField] private GameObject Rank;
    [SerializeField] private GameObject Result;
    [SerializeField] private GameObject gameOverGUI;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private int NumberOfCharacters;
    [HideInInspector] public bool levelCompleted = false;
    //public bool gameOver = false;



    [HideInInspector] public bool gameStarted = false;
    public static GameManager instance;
    public int PlayerRant;

    // Start is called before the first frame update
    void Start()
    {
        
        NumberOfCharacters = characters.Length;
        instance = this;
  
    }

    // Update is called once per frame
    void Update()
    {

        if(greenLight.gameObject.activeInHierarchy && Input.GetMouseButtonDown(0) && !gameStarted)
        {
            gameStarted = true;
            panel.transform.gameObject.SetActive(false);
        }
        else if(!greenLight.gameObject.activeInHierarchy && Input.GetMouseButtonDown(0) && !gameStarted)
        {
            TappedTooSoonText.SetActive(true);
            StartCoroutine(disableTappedtooSoonText());
        }

        if (gameStarted && !levelCompleted)
        {
            Sort(characters); // i sort characters from biggest transform.z to smalest transform.z

            for(int i =0; i <5; i++)
            {
                //in order to find the player's rank and use it in RankTextController to disable player's rank as a text
              if( characters[i].tag == "Player")
                {
                    PlayerRant = i+1; //i starts with 0 so i 
                }
              //To find winner's crown I created 3 array. one of them will be sorted, the other one will be unsorted characters and the last one is crown array.
              //unsorted characters array's and crown array's indexes are matching. so i can find which one i should activate or deactivate
              if(unSortedCharacters[i] == characters[0])
                {
                    crowns[i].SetActive(true);
                }
              else
                {
                    crowns[i].SetActive(false);
                }
            }
            
         
        }
       
    }

    // This is bubble sort. Sorting from biggest to smallest 
    private void Sort(GameObject[] array)
    {
        int i = 0, j;
        GameObject value;    
        while(i < NumberOfCharacters)
        {
            j = NumberOfCharacters - 1;
            while(j >= 1)
            {
                if(array[j-1].transform.position.z < array[j].transform.position.z)
                {
                    value = array[j];
                    array[j] = array[j - 1];
                    array[j - 1] = value;
                }
                j--;
            }
            i++;
        }
        
    }
    private IEnumerator disableTappedtooSoonText()
    {
        yield return new WaitForSeconds(0.5f);
        TappedTooSoonText.SetActive(false);

    }
    //change its name please. it doesn't sound like a function name but a bool name
    public void LevelCompleted()
    {
        //Cup.SetActive(true);
        //cinemachineVirtualCamera.m_Follow = Cup.transform;
        //cinemachineVirtualCamera.m_LookAt = Cup.transform;
        characters[0].gameObject.transform.position = new Vector3(0.51f, -28, 93.86f);
        characters[0].gameObject.transform.rotation =  Quaternion.Euler(0, 180, 0);
        characters[0].GetComponent<Animator>().SetTrigger("DidWin");


        characters[1].SetActive(false);
        characters[1].gameObject.transform.position = new Vector3(-1.92f, -28.42f, 93.92f);
        characters[1].transform.rotation = Quaternion.Euler(0, 180, 0);
        characters[1].GetComponent<Animator>().SetTrigger("DidLose");
        characters[1].SetActive(true);

        characters[2].SetActive(false);
        characters[2].gameObject.transform.position = new Vector3(2.34f, -28.79f, 93.92f);
        characters[2].gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        characters[2].GetComponent<Animator>().SetTrigger("DidLose");
        characters[2].SetActive(true);



        Rank.SetActive(false);
        Result.SetActive(true);
    }

    public void GameOver()
    {
        gameOverGUI.SetActive(true);
        cinemachineVirtualCamera.m_Follow = null;
        cinemachineVirtualCamera.m_LookAt = null;
        Rank.SetActive(false);
    }
   public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
