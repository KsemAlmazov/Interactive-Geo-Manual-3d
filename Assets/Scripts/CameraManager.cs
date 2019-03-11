using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{

    private static readonly float PanSpeed = 20f;
    private static readonly float ZoomSpeedTouch = 0.2f;
    private static readonly float ZoomSpeedMouse = 5f;

    private static readonly float[] BoundsX = new float[] { -20f, 200f };
    private static readonly float[] BoundsZ = new float[] { -100f, 15f };
    private static readonly float[] ZoomBounds = new float[] { 30f, 90f };

    private Camera cam;

    private Vector3 lastPanPosition;
    private int panFingerId; // Только mobile режим

    private bool wasZoomingLastFrame; // Только mobile режим
    private Vector2[] lastZoomPositions; // Только mobile режим

    public bool CanMoveCamera = true;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (CanMoveCamera == true)
        {
            if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                HandleTouch();
            }
            else {
            HandleMouse();
            }
        }
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Паннинг (свайп)
                wasZoomingLastFrame = false;

                // Если считалось нажатие, сохраняем позицию и ID пальца
                // В противном случае, если ID пальца не совпадает, пропускаем нажатие
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // Масштабирование
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else {
                    // Масштабирование базируется на дистанции между новыми позициями и сравнивается 
                    // с дистанциями между старыми позициями
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, ZoomSpeedTouch);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    void HandleMouse()
    {
        // При нажатии мыши, сохраняем позицию
        // Если мышь уже нажата - двигаем камеру
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Проверяем прокрутку колеса для того, чтобы отдалить или приблизить камеру
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Считаем насколько передвинуть камеру
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);

        // Производим передвижение с помощью Translate
        transform.Translate(move, Space.World);

        // Проверяем, находится ли камера внутри границ
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;

        // Сохраняем позицию в кэш
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }
}