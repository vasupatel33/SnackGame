using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float streerSpeed;
    [SerializeField] int Gap;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] GameObject parent, gameOverPanel;
    int scoreValue;

    [SerializeField] GameObject PlayerPref;
    [SerializeField] List<GameObject> AllFood;
    //[SerializeField] List<GameObject> EatenFood;
    List<GameObject> PlayerBody = new List<GameObject>();
    List<Vector3> PlayerPosition = new List<Vector3>();
    int val = 0;

    private void Start()
    {
        while (val < 20)
        {
            FoodGenerate();
            val++;
        }
        //val = 15;
    }
    private void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        float Direction = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * streerSpeed * Direction * Time.deltaTime);

        PlayerPosition.Insert(0, transform.position);
        int index = 0;
        foreach (var body in PlayerBody)
        {
            Vector3 point = PlayerPosition[Mathf.Clamp(index * Gap, 0, PlayerPosition.Count - 1)];

            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * moveSpeed * Time.deltaTime;

            body.transform.LookAt(point);

            index++;
        }
    }
    void GrowSnake()
    {
        GameObject body = Instantiate(PlayerPref);
        PlayerBody.Add(body);
    }
    //int value;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "food")
        {
            val--;
            Destroy(other.gameObject);
            scoreValue++;
            ScoreText.text = scoreValue.ToString();
            GrowSnake();
            while (val < 15)
            {
                Debug.Log("Before val = "+val);
                FoodGenerate();
                val++;
                Debug.Log("After val = "+val);
            }

        }
        if(other.gameObject.tag == "wall")
        {
            Debug.Log("Wall detect in Triggerr");
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
    }
    public void RetryButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
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
}