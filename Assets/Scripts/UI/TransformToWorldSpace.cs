using UnityEngine;

public class TransformToWorldSpace : MonoBehaviour
{
    public Camera mainCamera;

    public bool updatePosition;

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

        PlaceElement(rectTransform, desiredGameObject.transform.position);
    }

    public void Update()
    {
        if (updatePosition)
            PlaceElement(rectTransform, desiredGameObject.transform.position);
    }
}