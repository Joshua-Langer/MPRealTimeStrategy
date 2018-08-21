using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCamera : MonoBehaviour
{
    // vars
    private Transform m_transform;
    public bool useFixedUpdate = false;

    //movement vars
    public float screenEdgeMovementSpeed = 50f;
    public float panningSpeed = 50f;
   // public float mouseRotationSpeed = 10f;

    //Height vars
    public bool autoHeight = true;
    public LayerMask groundMask = -1;
    public float maxHeight = 10f;
    public float minHeight = 15f;
    public float heightDampening = 5f;
    public float scrollWheelZoomingSensitivity = 300f;
    private float zoomPos = 0;

    //Map limit
    public bool limitMap = true;
    public float limitX = 50f; //x
    public float limitY = 50f; //z

    //Input
    public bool useScreenEdgeInput = true;
    public bool usePanning = true;
    public bool useScrollwheelZooming = true;
    public bool useMouseRotation = true;
    public float screenEdgeBorder = 25f;
    public KeyCode panningKey = KeyCode.Mouse1; //Middle
    //public KeyCode mouseRotationKey = KeyCode.Mouse1; //Right
    public string zoomingAxis = "Mouse ScrollWheel";

    private Vector2 MouseInput
    {
        get { return Input.mousePosition; }
    }

    private float ScrollWheel
    {
        get { return Input.GetAxis(zoomingAxis); }
    }

    private Vector2 MouseAxis
    {
        get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); }
    }

    private float DistanceToGround()
    {
        Ray ray = new Ray(m_transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, groundMask.value))
            return (hit.point - m_transform.position).magnitude;
        return 0f;
    }

    void Awake()
    {
        m_transform = transform;
    }

    void Update()
    {
        if (!useFixedUpdate)
            CameraUpdate();
    }

    void FixedUpdate()
    {
        if (useFixedUpdate)
            CameraUpdate();
    }

    void CameraUpdate()
    {
        Move();
        HeightCalculation();
        //Rotation();
        LimitPosition();
    }

    void Move()
    {
        if (useScreenEdgeInput)
        {
            Vector3 desiredMove = new Vector3();

            Rect leftRect = new Rect(0, 0, screenEdgeBorder, Screen.height);
            Rect rightRect = new Rect(Screen.width - screenEdgeBorder, 0, screenEdgeBorder, Screen.height);
            Rect upRect = new Rect(0, Screen.height - screenEdgeBorder, Screen.width, screenEdgeBorder);
            Rect downRect = new Rect(0, 0, Screen.width, screenEdgeBorder);

            desiredMove.x = leftRect.Contains(MouseInput) ? -1 : rightRect.Contains(MouseInput) ? 1 : 0;
            desiredMove.z = upRect.Contains(MouseInput) ? 1 : downRect.Contains(MouseInput) ? -1 : 0;

            desiredMove *= screenEdgeMovementSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = m_transform.InverseTransformDirection(desiredMove);

            m_transform.Translate(desiredMove, Space.Self);
        }

        if (usePanning && Input.GetKey(panningKey) && MouseAxis != Vector2.zero)
        {
            Vector3 desiredMove = new Vector3(-MouseAxis.x, 0, -MouseAxis.y);

            desiredMove *= panningSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = m_transform.InverseTransformDirection(desiredMove);

            m_transform.Translate(desiredMove, Space.Self);
        }
    }

    void HeightCalculation()
    {
        float distanceToGround = DistanceToGround();
        if (useScrollwheelZooming)
            zoomPos += ScrollWheel * Time.deltaTime * scrollWheelZoomingSensitivity;
       

        zoomPos = Mathf.Clamp01(zoomPos);

        float targetHeight = Mathf.Lerp(minHeight, maxHeight, zoomPos);
        float difference = 0;

        if (distanceToGround != targetHeight)
            difference = targetHeight - distanceToGround;

        m_transform.position = Vector3.Lerp(m_transform.position,
            new Vector3(m_transform.position.x, targetHeight + difference, m_transform.position.z), Time.deltaTime * heightDampening);
    }

    /*  Likely to be removed.
    void Rotation()
    {
        if (useMouseRotation && Input.GetKey(mouseRotationKey))
            m_transform.Rotate(Vector3.up, -MouseAxis.x * Time.deltaTime * mouseRotationSpeed, Space.World);
    }
    */

    void LimitPosition()
    {
        if (!limitMap)
            return;
        m_transform.position = new Vector3(Mathf.Clamp(m_transform.position.x, -limitX, limitX), m_transform.position.y, Mathf.Clamp(m_transform.position.z, -limitY, limitY));
    }

}
