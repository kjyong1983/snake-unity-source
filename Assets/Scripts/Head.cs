using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//클래스 헤드
public class Head : MonoBehaviour {

    private int width = 8;
    private int height = 4;

    private List<GameObject> tails;
    public GameObject tail;
    public GameObject food;
    public Vector2 direction;
    private float moveUnit = 1;

    private bool isMouse = false;

    private float dirX;
    private float dirY;
    private Vector2 lastDirection;
    private Vector3 lastPosition;

    private float timer = 0;
    private float timerThreshold = 1f;//difficulty

    void Start () {
        tails = new List<GameObject>();

        for (int i = 0; i < 2; i++)
        {
            GameObject newTail = Instantiate(tail, transform.position, Quaternion.identity);
            tails.Add(newTail);
            tails[i].transform.position = new Vector2(transform.position.x, transform.position.y - (i + 1) * moveUnit);
        }

        direction = Vector2.up;
        lastDirection = direction;
        GenerateFood();

	}
	
	void Update () {

        if (Input.anyKey)
        {
            if (Input.GetMouseButton(0))
                isMouse = true;
            else
                isMouse = false;
        }

        if (isMouse)
        {
            GetMouseInput();
        }
        else
        {
            GetMoveKey();
        }

        timer += Time.deltaTime;

        if (timer > timerThreshold)
        {
            Move();
            timer = 0;
        }

        if (Mathf.Abs(transform.position.x) > width || Mathf.Abs(transform.position.y) > height)
        {
            Destroy(gameObject);
            GameManager.isDead = true;
        }

        //debug
        if (Input.GetKeyDown(KeyCode.F))
        {
            GenerateFood();
        }

    }

    void GetMouseInput()
    {
        var mouseDirection = FindObjectOfType<MouseDrag>().direction;

        SetDirection(mouseDirection.x, mouseDirection.y);
    }

    void GetMoveKey()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            SetDirection(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            SetDirection(1, 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SetDirection(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            SetDirection(0, -1);
        }

    }

    void SetDirection(float x, float y)
    {
        //가속
        //마우스는 해당없음...
        //if (lastDirection.x == x && lastDirection.y == y)
        //{
        //    Debug.Log("dd");
        //    Move();
        //    timer = 0;
        //}

        if (lastDirection.x == -x && lastDirection.y == y)
        {
            return;
        }
        if (lastDirection.x == x && lastDirection.y == -y)
        {
            return;
        }

        direction = new Vector2(x, y);
    }

    void Move()
    {
        transform.position += (Vector3)direction * moveUnit;

        for (int i = tails.Count-1; i >= 0; i--)
        {
            if (i == 0)
            {
                tails[i].transform.position = lastPosition;
            }
            else
            {
                tails[i].transform.position = tails[i - 1].transform.position;
            }

        }

        lastDirection = direction;
        lastPosition = transform.position;

    }

    void GenerateFood()
    {
        Vector3 randomLocation = RandomizeLocation();

        while (CheckFoodLocation(randomLocation))
        {
            randomLocation = RandomizeLocation();
        } 
        GameObject newFood = Instantiate(food, randomLocation, Quaternion.identity);
    }

    Vector3 RandomizeLocation()
    {
        return new Vector3(Random.Range(-width, width+1), Random.Range(-height, height+1));
    }

    private bool CheckFoodLocation(Vector3 randomLocation)
    {
        for (int i = 0; i < tails.Count; i++)
        {
            if (tails[i].transform.position == randomLocation || transform.position == randomLocation)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            GameObject newTail = Instantiate(tail, tails[tails.Count-1].transform.position, Quaternion.identity);
            tails.Add(newTail);
            GenerateFood();

            timerThreshold *= 0.9f;

            GameManager.score = tails.Count - 2;
            FindObjectOfType<GameManager>().UpdateScore();
        }

        if (other.gameObject.CompareTag("Player"))
        {

            Destroy(gameObject);
        }
    }

}

