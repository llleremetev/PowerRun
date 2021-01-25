using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : MonoBehaviour
{
    public static SwipeManager instance;
    public GameObject player;
    float heroPos = 0;

    // Jump

    public Vector3 jump;
    public float jumpForce = 2.5f;
    public bool isGrounded = true;
    public Rigidbody heroRB;

    
    public enum Direction { Left, Right, Up, Down};

    public bool[] swipe = new bool[4];

    Vector2 startTouch;
    bool touchMoved;
    Vector2 swipeDelta;

    const float SWIPE_THRESHOLD = 50;

    public delegate void MoveDelegate(bool[] swipes);
    public MoveDelegate MoveEvent;

    public delegate void ClickDelegate(Vector2 pos );
    public ClickDelegate ClickEvent;


    Vector2 TouchPosition() { return (Vector2)Input.mousePosition; }
    bool TouchBegan() { return Input.GetMouseButtonDown(0); }
    bool TouchEnded() { return Input.GetMouseButtonUp(0); }
    bool GetTouch() { return Input.GetMouseButton(0); }
    void Awake()
    {
        instance = this;
        heroRB = player.GetComponent<Rigidbody>();
        jump = new Vector3(0, 2.5f, 0);
    }

    void Update()
    {

        if(TouchBegan())
        {
            startTouch = TouchPosition();
            touchMoved = true;
        }
       else if(TouchEnded() && touchMoved == true)
        {
            SendSwipe();
            touchMoved = false;
        }

        swipeDelta = Vector2.zero;
        if (touchMoved && GetTouch())
        {
            swipeDelta = TouchPosition() - startTouch;
        }

        if(swipeDelta.magnitude > SWIPE_THRESHOLD)
        {
            if(Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                swipe[(int)Direction.Left] = swipeDelta.x < 0;
                swipe[(int)Direction.Right] = swipeDelta.x > 0;
            }

            else
            {
                swipe[(int)Direction.Down] = swipeDelta.y < 0;
                swipe[(int)Direction.Up] = swipeDelta.y > 0;
            }
            SendSwipe();
        }
    }

    void SendSwipe()
    {

        if  (swipe[0])
        {
            if (heroPos > -1.5f)
            {
                
                heroPos-= 1.5f;
                player.transform.position = new Vector3(heroPos, player.transform.position.y, player.transform.position.z);
                Debug.Log(player.transform.position);
            }
        }

        else if (swipe[1])
        {
            if(heroPos < 1.5f)
            {
                heroPos+= 1.5f;
                player.transform.position = new Vector3(heroPos, player.transform.position.y, player.transform.position.z);
            }
        }

        else if (swipe[2] && isGrounded)
        {
            
                heroRB.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                Debug.Log("Jump");
            
        }
            else if (swipe[3])
        {
            if (isGrounded == false)
            {
                heroRB.AddForce(-jump * jumpForce, ForceMode.Impulse);
                Debug.Log("Roll");
            }

            if (isGrounded)
            {
                Debug.Log("Slide");
                
            }

        }
        else
        {
            Debug.Log("Click");
            ClickEvent?.Invoke(TouchPosition());
        }
        Reset();
    }




    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        touchMoved = false;
        for (int i=0;i<4;i++) { swipe[i] = false; }
    }
}
