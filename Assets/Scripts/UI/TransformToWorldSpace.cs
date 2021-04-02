using System.Collections;
using UnityEngine;

public class TransformToWorldSpace : MonoBehaviour
{
    public Camera mainCamera;

    public bool updatePosition;

    public bool multiplayer;

    public Vector3 positionOffset;

    public GameObject desiredGameObject;

    private RectTransform rectTransform;

    public void PlaceElement(RectTransform UIElement, Vector3 desiredPosition)
    {
        Vector2 viewportPoint = mainCamera.WorldToViewportPoint(desiredPosition + positionOffset);

        UIElement.anchorMin = viewportPoint;
        UIElement.anchorMax = viewportPoint;
    }

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (!multiplayer)
            PlaceElement(rectTransform, desiredGameObject.transform.position);
        else
            StartCoroutine(WaitForPlayerSpawn(rectTransform));
    }

    public void Update()
    {
        if (updatePosition && desiredGameObject != null)
            PlaceElement(rectTransform, desiredGameObject.transform.position);
    }

    private IEnumerator WaitForPlayerSpawn(RectTransform rectTransform)
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Paddle") != null);

        var paddles = GameObject.FindGameObjectsWithTag("Paddle");

        if (paddles.Length == 1) //Added with the intention of preventing null errors
            desiredGameObject = paddles[0];
        else
            desiredGameObject = paddles[MultiplayerPaddleSetup.paddleID - 1];
    }
}