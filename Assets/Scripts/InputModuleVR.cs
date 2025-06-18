using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputModuleVR : PointerInputModule
{
    Camera playerCamera;

    Dictionary<int, PointerEventDataVR> cache = new();


    protected override void Start()
    {
        base.Start();
        playerCamera = Camera.main;
    }

    public override void Process()
    {
        if(VRManager.Instance)
        {
            if(VRManager.Instance.leftController)
            {
                HandleController(VRManager.Instance.leftController);
            }
        }

        if(VRManager.Instance)
        {
            if (VRManager.Instance.rightController)
            {
                HandleController(VRManager.Instance.rightController);
            }
        }
    }

    void HandleController(Controller controller)
    {
        if(!cache.TryGetValue(controller.pointerId, out var pointerData))
        {
            pointerData = new PointerEventDataVR(eventSystem) { pointerId = controller.pointerId };
            cache[controller.pointerId] = pointerData;
        }

        Vector2 lastPointerPosition = pointerData.position;
        pointerData.Reset();
        pointerData.controller = controller;
        pointerData.pointerId = controller.pointerId;
        pointerData.button = PointerEventData.InputButton.Left;

        if(controller.bRaycastHit)
        {
            pointerData.position = playerCamera.WorldToScreenPoint(controller.rayHitResult.point);
        }
        else
        {
            pointerData.position = playerCamera.WorldToScreenPoint(controller.GetMaxRayPosition());
        }

        pointerData.delta = pointerData.position - lastPointerPosition;
        //data.scrollDelta = scrollDelta;                      // for ScrollRects

        m_RaycastResultCache.Clear();
        eventSystem.RaycastAll(pointerData, m_RaycastResultCache);
        pointerData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);

        HandlePointerExitAndEnter(pointerData, pointerData.pointerCurrentRaycast.gameObject);

        if(controller.TriggerPressedThisFrame())
        {
            ProcessPress(pointerData, true, false);
        }

        if(controller.TriggerReleasedThisFrame())
        {
            ProcessPress(pointerData, false, true);
        }

        if(pointerData.IsPointerMoving() && pointerData.pointerDrag != null && pointerData.dragging)
        {
            ProcessDrag(pointerData);
        }

        // ---------- 7. Scroll wheel / joystick ----------
        //if (scrollDelta.sqrMagnitude > 0.0f)
        //    ExecuteEvents.ExecuteHierarchy(data.pointerCurrentRaycast.gameObject,
        //                                   data, ExecuteEvents.scrollHandler);
    }

    void ProcessPress(PointerEventData pointerData, bool pressed, bool released)
    {
        var hoveredObject = pointerData.pointerCurrentRaycast.gameObject;

        if(pressed)
        {
            pointerData.eligibleForClick = true;
            pointerData.delta = Vector2.zero;
            pointerData.dragging = false;
            pointerData.useDragThreshold = true;
            pointerData.pressPosition = pointerData.position;
            pointerData.pointerPressRaycast = pointerData.pointerCurrentRaycast;

            var newPressed = ExecuteEvents.ExecuteHierarchy(
                hoveredObject,
                pointerData,
                ExecuteEvents.pointerDownHandler) ?? ExecuteEvents.GetEventHandler<IPointerClickHandler>(hoveredObject);

            pointerData.pointerPress = newPressed;
            pointerData.rawPointerPress = hoveredObject;
            pointerData.clickTime = Time.unscaledTime;

            pointerData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(hoveredObject);
            if(pointerData.pointerDrag != null)
            {
                ExecuteEvents.Execute(pointerData.pointerDrag, pointerData, ExecuteEvents.initializePotentialDrag);
            }
        }

        if (released)
        {
            ExecuteEvents.Execute(pointerData.pointerPress, pointerData, ExecuteEvents.pointerUpHandler);

            var clickHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(hoveredObject);
            if (pointerData.pointerPress == clickHandler && pointerData.eligibleForClick)
                ExecuteEvents.Execute(pointerData.pointerPress, pointerData, ExecuteEvents.pointerClickHandler);
            else if (pointerData.pointerDrag != null && pointerData.dragging)
                ExecuteEvents.ExecuteHierarchy(hoveredObject, pointerData, ExecuteEvents.dropHandler);

            // drag cleanup
            pointerData.eligibleForClick = false;
            pointerData.pointerPress = null;
            pointerData.rawPointerPress = null;

            if (pointerData.pointerDrag != null && pointerData.dragging)
                ExecuteEvents.Execute(pointerData.pointerDrag, pointerData, ExecuteEvents.endDragHandler);

            pointerData.dragging = false;
            pointerData.pointerDrag = null;

            HandlePointerExitAndEnter(pointerData, hoveredObject);
        }
    }
}