using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed, steerSpeed;
    [SerializeField] int Gap;
    [SerializeField] GameObject PlayerPref;
    [SerializeField] List<GameObject> AllFood;
    [SerializeField] List<GameObject> GeneratedFoodItem;
    [SerializeField] private TextMeshProUGUI ScoreText;

    int scoreVal;
    List<GameObject> PlayerBody = new List<GameObject>();
    List<Vector3> PlayerPosition = new List<Vector3>();
    private void Start()
    {
        ScoreText.text = 0.ToString();
    }
    void Update()
    {
        //float Vinput = Input.GetAxis("Vertical");
        //Player.transform.Translate(Hinput * Time.deltaTime * 6, 0, Vinput*Time.deltaTime * 6);
        transform.position += transform.forward * playerSpeed * Time.deltaTime;
        float Hinput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up *Hinput * steerSpeed * Time.deltaTime);

        PlayerPosition.Insert(0, transform.position);
        int index = 0;
        foreach(var body in PlayerBody)
        {
            Debug.Log("Foreach called");
            Vector3 point = PlayerPosition[Mathf.Clamp(index * Gap, 0, PlayerPosition.Count - 1)];

            Vector3 Direction = point - body.transform.position;
            body.transform.position += Direction * playerSpeed * Time.deltaTime ;

            body.transform.LookAt(point);
            index++;
        }
        //StartCoroutine(GenerateFood());
    }
    float xVal;
    float ZVal;
    //IEnumerator GenerateFood()
    //{
    //    if(GeneratedFoodItem.Count < 10)
    //    {
    //        xVal = Random.Range(-8f, 8f);
    //        ZVal = Random.Range(-8f, 8f);
    //        int foodIndex =  Random.Range(0, AllFood.Count);
    //        GameObject GeneratedFood = AllFood[foodIndex];
    //        GameObject FoodGenerate = Instantiate(GeneratedFood, new Vector3(xVal, 0.65f, ZVal),Quaternion.identity);
    //        GeneratedFoodItem.Add(FoodGenerate);
    //    }
    //    yield return new WaitForSeconds(0.3f);
    //    //GameObject food = Instantiate();
    //}
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision detect");
        if (other.gameObject.tag == "food")
        {
            Debug.Log("Collision playedd");
            GrowUpPlayer();
            Destroy(other.gameObject);
            scoreVal++;
            ScoreText.text = scoreVal.ToString();
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Truggerrrr detect");
    //    if (other.gameObject.tag == "food")
    //    {
    //        Debug.Log("Triggerrr playedd");
    //        GrowUpPlayer();
    //        Destroy(other.gameObject);
    //        scoreVal++;
    //        ScoreText.text = scoreVal.ToString();
    //    }
    //}
    private void GrowUpPlayer()
    {
        Debug.Log("Grow player called");
        GameObject gameObject = Instantiate(PlayerPref);
        PlayerBody.Add(gameObject);
    }
}
