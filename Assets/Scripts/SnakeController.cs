using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float bodySpeed;
    [SerializeField] private float steerSpeed;
    [SerializeField] private GameObject  bodyPrefab;
    [SerializeField] private Canvas  DeadPanel;
    private Rigidbody rb;
    private bool canmove = true;
    private float candie = 3.0f;
    private float candieTimer;
    private bool ableDie;
    private bool die;
    private int point=0;
    private bool firstSnake = false;

    private int gap = 10;
    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> positionHistory = new List<Vector3>();
    
    public StickController Joystic;
    
    void Awake()
    {
        if(Joystic != null)
        {
            Joystic.StickChanged += MoveStick_StickChanged;
        }

        if (transform.position == new Vector3(4, 1, 0))
        {
            firstSnake = true;
        }
    }

    private Vector2 MoveStickPos = Vector2.zero;

    private void MoveStick_StickChanged(object sender, StickEventArgs e)
    {
        MoveStickPos = e.Position;
    }
    void Start()
    {
        
        
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        
        //positionHistory.Insert(0, transform.position);
        InvokeRepeating("UpdatePositionHistory", 0f, 0.01f);

        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // Get Input for axis
        float h = MoveStickPos.x; // Input.GetAxis("Horizontal");
        candieTimer += Time.deltaTime;
        if (candieTimer > candie)
        {
            ableDie = true;
        }
        if(canmove)
        {
            //move forward
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                //steer
                float steerDirection = Input.GetAxis("Horizontal");
                transform.Rotate(Vector3.up * h * steerSpeed * Time.deltaTime);
            }
            
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                if (firstSnake)
                {
                    float steerDirection = 0;
                    if (Input.GetKey(KeyCode.A))
                    {
                        steerDirection = -1;
                    } 
                    if (Input.GetKey(KeyCode.D))
                    {
                        steerDirection = 1;
                    } 
                    transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);
                }
                else
                {
                    float steerDirection = 0;
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        steerDirection = -1;
                    } 
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        steerDirection = 1;
                    } 
                    transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);
                }
            }
           
        
            int index = 0;
            foreach (GameObject body in bodyParts)
            {
                Vector3 point = positionHistory[Math.Min(index*10, positionHistory.Count-1)];
                Vector3 moveDirection = point - body.transform.position;
                body.transform.position += moveDirection * bodySpeed * Time.deltaTime;

                body.transform.LookAt(point);
            
                index++;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GrowSnake();
            }
        }

        if (die)
        {
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(1);
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void points()
    {
        point++;
        if (PlayerPrefs.GetInt("BestScore") < point)
        {
            PlayerPrefs.SetInt("BestScore", point);
        }
    }

    public void freeze()
    {
        canmove = false;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        DeadPanel.gameObject.SetActive(true);
        die = true;
    }
    
    void UpdatePositionHistory()
    {
        Debug.Log("UpdatePositionHistory");
        // Añadir la posición actual al inicio de la lista
        positionHistory.Insert(0, transform.position);

        // Si la lista excede el número máximo de posiciones, elimina la última
        if (positionHistory.Count > 500)
        {
            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
    }

    public void GrowSnake()
    {
        GameObject body = Instantiate(bodyPrefab);
        bodyParts.Add(body);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("body"))
        {
            if (ableDie)
            {
                freeze();
            }
        }
        if (other.CompareTag("snake"))
        {
            if (ableDie)
            {
                freeze();   
            }
        }
    }
}
