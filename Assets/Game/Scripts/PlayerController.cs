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
    [SerializeField] TextMeshProUGUI GameOverTxt;
    int scoreValue;

    [SerializeField] GameObject PlayerPref;
    List<GameObject> PlayerBody = new List<GameObject>();
    List<Vector3> PlayerPosition = new List<Vector3>();

    private void Start()
    {
        //GrowSnake();
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
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "food")
        {
            Destroy(other.gameObject);
            scoreValue++;
            ScoreText.text = scoreValue.ToString();
            GrowSnake();
        }
        if(other.gameObject.tag == "wall")
        {
            GameOverTxt.gameObject.SetActive(true);
        }
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }
}
