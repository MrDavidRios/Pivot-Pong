using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    //Transforms
    private Transform _ball;
    
    //Floats
    public float paddleSpeed;
    public float triggerDistance;
    
    //Booleans
    private bool _followMouse = true;
    public bool followMouseEnabled;
    
    //Other
    private Camera _mainCamera;

    private void Awake()
    {
        _ball = GameObject.Find("Ball").transform;
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        var pos = transform.position;
        float xPos = pos.x;

        Vector2 currentMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        _followMouse = !(Vector2.Distance(pos, currentMousePosition) > triggerDistance);

        if (_followMouse)
        {
            if (followMouseEnabled)
            {
                //Follow Cursor
                if (currentMousePosition.y >= -4.5 && currentMousePosition.y <= 4.5)
                    transform.position = Vector2.MoveTowards(transform.position, currentMousePosition, paddleSpeed / 100);

                //Lock paddle to y-axis
                transform.position = new Vector2(xPos, transform.position.y);
            }
            else
            {
                //Follow Ball
                transform.position = Vector2.MoveTowards(transform.position, _ball.position, paddleSpeed / 100);

                //Lock paddle to y-axis
                transform.position = new Vector2(xPos, transform.position.y);
            }
        }
        else
        {
            //Follow Ball
            transform.position = Vector2.MoveTowards(transform.position, _ball.position, paddleSpeed / 100);

            //Lock paddle to y-axis
            transform.position = new Vector2(xPos, transform.position.y);
        }
    }
}