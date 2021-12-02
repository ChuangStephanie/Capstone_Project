using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    // Camera Controller lives as a parent object to Camera.
    // Rotation of the Parent object preserves to distance from Camera to focal point.

    private bool isMouseDown = false;
    private bool isFocused = false;
    private float mouseHorizontal = 0f;
    private float rotationOffset = 0f;
    private float horizontalOffset = 0f;
    private float verticalOffset = 0f;
    private Quaternion initialRotation = Quaternion.identity;

    [Header("Rotation")]
    [Tooltip("")]
    [SerializeField] private GameObject sceneTarget = null;
    [Tooltip("Rotation Bounds in absolute angles")]
    [SerializeField] private Vector2 rotationBounds = Vector2.zero;
    [Tooltip("Speed as a percentage of default (1)")]
    [SerializeField] private float rotationSpeed = 0.5f;

    [Header("Zoom")]
    [SerializeField] private GameObject myCamera = null;
    [Tooltip("Bounds in Local Space, Max Extent and Min Extent (0 is center of the room)")]
    [SerializeField] private Vector2 zoomBounds = Vector2.zero;
    [Tooltip("Zoom Level to switch from Room Focus to Character Focus")]
    [SerializeField] private float zoomBreakpoint = -4f;
    [Tooltip("Speed as a percentage of default (1)")]
    [SerializeField] private float zoomSpeed = 0.5f;

    [Header("UI")]
    [SerializeField] private Slider mySlider = null;

    private void Awake()
    {
        rotationOffset = transform.rotation.eulerAngles.y;
        horizontalOffset = myCamera.transform.localPosition.x;
        verticalOffset = myCamera.transform.localPosition.y;
        initialRotation = myCamera.transform.localRotation;
    }

    #region Event Listeners
    private void OnEnable()
    {
        // When this object is Enabled, begin listening to the GameManger Events
        GameManager.MousePressed += OnMousePressed;
        GameManager.MouseReleased += OnMouseReleased;
        GameManager.MouseScroll += OnMouseScroll;
        mySlider.onValueChanged.AddListener(delegate { ZoomToValue(mySlider.value); });
    }
    private void OnDisable()
    {
        // When this object is Disabled, remove this from the GameManager Events listener list
        GameManager.MousePressed -= OnMousePressed;
        GameManager.MouseReleased -= OnMouseReleased;
        GameManager.MouseScroll -= OnMouseScroll;
    }
    #endregion

    private void Update()
    {
        if(isMouseDown)
        {
            RotateAroundTarget();
        }

        if(isFocused && sceneTarget != null)
        {
            MoveAroundTarget();
        }
    }

    private void OnMousePressed()
    {
        // Start Tracking Mouse Movement
        isMouseDown = true;

        // Initialize Mouse's Starting Position when Pressed
        mouseHorizontal = Input.mousePosition.x;
    }

    private void OnMouseReleased()
    {
        isMouseDown = false;
    }

    private void OnMouseScroll(float scroll)
    {
        float zoomTarget = myCamera.transform.localPosition.z + (scroll * zoomSpeed);
        zoomTarget = Mathf.Clamp(zoomTarget, zoomBounds.x, zoomBounds.y);
        myCamera.transform.localPosition = new Vector3(horizontalOffset, verticalOffset, zoomTarget);
        FocusCheck(zoomTarget);
    }

    private void FocusCheck(float zoomTarget)
    {
        isFocused = zoomTarget > zoomBreakpoint;

        if (!isFocused)
            myCamera.transform.localRotation = initialRotation;
    }

    private void RotateAroundTarget()
    {
        // Get difference of Current mouse position from Start mouse position.
        // Constrain difference to the upper and lower Rotation Bounds.
        // Rotate Camera to bounded difference orientation.

        float rotationTarget = (Input.mousePosition.x - mouseHorizontal) * rotationSpeed;
        rotationOffset += rotationTarget;
        rotationOffset = Mathf.Clamp(rotationOffset, rotationBounds.x, rotationBounds.y);
        transform.rotation = Quaternion.Euler(new Vector3(0, rotationOffset, 0));
        mouseHorizontal = Input.mousePosition.x;
    }

    private void MoveAroundTarget()
    {
        myCamera.transform.LookAt(sceneTarget.transform);
    }

    private void ZoomToValue(float value)
    {
        float targetValue = Mathf.Lerp(zoomBounds.x, zoomBounds.y, value);
        myCamera.transform.localPosition = new Vector3(horizontalOffset, verticalOffset, targetValue);
        FocusCheck(targetValue);
    }
}
