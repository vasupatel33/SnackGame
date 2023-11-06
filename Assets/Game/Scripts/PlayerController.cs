using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float streerSpeed;
    [SerializeField] int Gap;
    [SerializeField] TextMeshProUGUI ScoreTextGamePanel, ScoreTextGameOverPanel, HighScoreTxt;
    [SerializeField] GameObject parent, gameOverPanel, pausePanel;

    [SerializeField] AudioClip ClickSound, GameOverSound, foodSound;
    int scoreValue;

    [SerializeField] GameObject PlayerPref;
    [SerializeField] List<GameObject> AllFood;
    //[SerializeField] List<GameObject> EatenFood;
    List<GameObject> PlayerBody = new List<GameObject>();
    List<Vector3> PlayerPosition = new List<Vector3>();
    int val = 0;
    int HighScoreVal;
    public Joystick joystick;

    private void Start()
    {
        HighScoreVal = PlayerPrefs.GetInt("HighScorePref",0);
        while (val < 20)
        {
            FoodGenerate();
            val++;
        }
    }
    private Vector3 lastMoveDirection = Vector3.zero;

    private void Update()
    {
        Debug.Log("val upd is = " + HighScoreVal);

        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        // Calculate the movement based on joystick input
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // If there is new input, update the last move direction
        if (moveDirection != Vector3.zero)
        {
            lastMoveDirection = moveDirection;
        }

        // Apply movement based on the last input direction
        transform.Translate(lastMoveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Rotate the player based on the last input direction
        if (lastMoveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(lastMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, streerSpeed * Time.deltaTime);
        }

        PlayerPosition.Insert(0, transform.position);
        int index = 0;
        foreach (var body in PlayerBody)
        {
            Vector3 point = PlayerPosition[Mathf.Clamp(index * Gap, 0, PlayerPosition.Count - 1)];

            Vector3 moveDirection1 = point - body.transform.position;
            body.transform.position += moveDirection1 * moveSpeed * Time.deltaTime;

            body.transform.LookAt(point);

            index++;
        }
    }

    //private void Update()
    //{
    //    Debug.Log("val upd is = "+HighScoreVal);
    //    //transform.position += transform.forward * moveSpeed * Time.deltaTime;
    //    //float Direction = Input.GetAxis("Horizontal");
    //    //transform.Rotate(Vector3.up * streerSpeed * Direction * Time.deltaTime);


    //    float horizontalInput = joystick.Horizontal;
    //    float verticalInput = joystick.Vertical;

    //    // Calculate the movement based on joystick input
    //    Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

    //    // Apply movement
    //    transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

    //    //transform.position += new Vector3(horizontalInput, 0, verticalInput).normalized;

    //    // Rotate the player based on the movement direction
    //    if (moveDirection != Vector3.zero)
    //    {
    //        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, streerSpeed * Time.deltaTime);
    //    }



    //    PlayerPosition.Insert(0, transform.position);
    //    int index = 0;
    //    foreach (var body in PlayerBody)
    //    {
    //        Vector3 point = PlayerPosition[Mathf.Clamp(index * Gap, 0, PlayerPosition.Count - 1)];

    //        Vector3 moveDirection1 = point - body.transform.position;
    //        body.transform.position += moveDirection1 * moveSpeed * Time.deltaTime;

    //        body.transform.LookAt(point);

    //        index++;
    //    }
    //}
    
    void GrowSnake()
    {
        GameObject body = Instantiate(PlayerPref);
        PlayerBody.Add(body);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "food")
        {
            Common.instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(foodSound);
            val--;
            Destroy(other.gameObject);
            scoreValue++;
            
            ScoreTextGamePanel.text = scoreValue.ToString();
            GrowSnake();
            while (val < 20)
            {
                Debug.Log("Before val = "+val);
                FoodGenerate();
                val++;
                Debug.Log("After val = "+val);
            }

        }
        if(other.gameObject.tag == "wall")
        {
            Common.instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(GameOverSound);
            ScoreTextGameOverPanel.text = scoreValue.ToString();//2
                
            HighScoreTxt.text = HighScoreVal.ToString();
            if(scoreValue > HighScoreVal)
            {
                HighScoreVal = scoreValue;
                PlayerPrefs.SetInt("HighScorePref", HighScoreVal);
                HighScoreTxt.text = HighScoreVal.ToString();
            }
            Debug.Log("Wall detect in Triggerr");
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
    }
    
    public void FoodGenerate()
    {
        Debug.Log("Function called");
        int Generatefood = Random.Range(0, AllFood.Count);
        float xPos = Random.Range(-23f, 23.5f);
        float ZPos = Random.Range(-17.5f, 19f);
        Vector3 spawnPos = new Vector3(xPos, -0.3f, ZPos);
        GameObject game = Instantiate(AllFood[Generatefood], spawnPos, Quaternion.identity ,parent.transform);
        //EatenFood.Add(game);
    }
    public void OnPauseBtn()
    {
        Common.instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void OnPausePanelCloseBtn()
    {
        Common.instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
    public void OnExitBtnClicked()
    {
        Common.instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void RetryButton()
    {
        Common.instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
  
}