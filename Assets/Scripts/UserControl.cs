using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserControl : MonoBehaviour
{
    public Camera GameCamera;
    public float PanSpeed = 10.0f;
    public GameObject Marker;

    private Unit m_Selected = null;

    private void Start()
    {
        Marker.SetActive(false);
    }

    private void Update()
    {
        HandleCameraMovement();
        HandleMouseSelection();
        MarkerHandling();
    }

    // =============================
    // CAMERA MOVEMENT (WASD + Arrows)
    // =============================
    private void HandleCameraMovement()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float horizontal = 0f;
        float vertical = 0f;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            horizontal -= 1f;

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            horizontal += 1f;

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            vertical += 1f;

        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            vertical -= 1f;

        Vector2 move = new Vector2(horizontal, vertical);
        if (move.sqrMagnitude > 1f)
            move.Normalize();

        GameCamera.transform.position +=
            new Vector3(move.y, 0f, -move.x) * PanSpeed * Time.deltaTime;
    }

    // =============================
    // MOUSE INTERACTION
    // =============================
    private void HandleMouseSelection()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            Ray ray = GameCamera.ScreenPointToRay(mouse.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                m_Selected = hit.collider.GetComponentInParent<Unit>();

                var uiInfo = hit.collider.GetComponentInParent<UIMainScene.IUIInfoContent>();
                UIMainScene.Instance.SetNewInfoContent(uiInfo);
            }
        }
        else if (m_Selected != null && mouse.rightButton.wasPressedThisFrame)
        {
            Ray ray = GameCamera.ScreenPointToRay(mouse.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var building = hit.collider.GetComponentInParent<Building>();

                if (building != null)
                    m_Selected.GoTo(building);
                else
                    m_Selected.GoTo(hit.point);
            }
        }
    }

    // =============================
    // MARKER HANDLING
    // =============================
    void MarkerHandling()
    {
        if (m_Selected == null && Marker.activeInHierarchy)
        {
            Marker.SetActive(false);
            Marker.transform.SetParent(null);
        }
        else if (m_Selected != null && Marker.transform.parent != m_Selected.transform)
        {
            Marker.SetActive(true);
            Marker.transform.SetParent(m_Selected.transform, false);
            Marker.transform.localPosition = Vector3.zero;
        }
    }
}
