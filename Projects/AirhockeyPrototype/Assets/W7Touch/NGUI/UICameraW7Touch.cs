using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UICamera))]
public class UICameraW7Touch : MonoBehaviour {
    private UICamera uiCamera;
    public bool useGestures;
    public float scrollSpeed = 10;
    public float twistSpeed = 10;
    public bool useRawTouches;

    Dictionary<GameObject, List<W7TouchEvent>> touchEventsByGameObject = new Dictionary<GameObject, List<W7TouchEvent>>();

    public class W7TouchEvent {
        public W7Touch vwTouch;

        public bool pressed;
        public bool unpressed;

        public int touchId;
        public UICamera.MouseOrTouch touch;
    }

    void Awake() {
        uiCamera = GetComponent<UICamera>();
    }

    void Update () {
        ProcessInput();
    }

    void ProcessInput() {
        if (!enabled) return;

        touchEventsByGameObject.Clear();

        for (int i = 0; i < W7TouchManager.GetTouchCount(); ++i) {
            W7Touch touch = W7TouchManager.GetTouch(i);
            UICamera.currentTouchID = (int)touch.Id;
            UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID);

            bool pressed = (touch.Phase == TouchPhase.Began);
            bool unpressed = (touch.Phase == TouchPhase.Canceled) || (touch.Phase == TouchPhase.Ended);

            if (pressed) {
                UICamera.currentTouch.delta = Vector2.zero;
            } else {
                // Although input.deltaPosition can be used, calculating it manually is safer (just in case)
                UICamera.currentTouch.delta = touch.Position - UICamera.currentTouch.pos;
            }

            UICamera.currentTouch.pos = touch.Position;
            UICamera.currentTouch.current = UICamera.Raycast(UICamera.currentTouch.pos, ref UICamera.lastHit) ? UICamera.lastHit.collider.gameObject : UICamera.fallThrough;
            UICamera.lastTouchPosition = UICamera.currentTouch.pos;

            // We don't want to update the last camera while there is a touch happening
            if (pressed) UICamera.currentTouch.pressedCam = UICamera.currentCamera;
            else if (UICamera.currentTouch.pressed != null) UICamera.currentCamera = UICamera.currentTouch.pressedCam;

            // If the touch has ended, remove it from the list
            if (unpressed) UICamera.RemoveTouch(UICamera.currentTouchID);

            // multitouch
            W7TouchEvent ft = new W7TouchEvent();
            ft.vwTouch = touch;
            ft.touchId = UICamera.currentTouchID;
            ft.touch = UICamera.currentTouch;
            ft.pressed = pressed;
            ft.unpressed = unpressed;

            if (UICamera.currentTouch.current == null) continue;

            List<W7TouchEvent> list;
            if (touchEventsByGameObject.TryGetValue(UICamera.currentTouch.current, out list)) {
                list.Add(ft);
            } else {
                touchEventsByGameObject[UICamera.currentTouch.current] = new List<W7TouchEvent>();
                touchEventsByGameObject[UICamera.currentTouch.current].Add(ft);
            }

            UICamera.currentTouch = null;
        }

        foreach (var touchEventsInGameObject in touchEventsByGameObject) {
            GameObject target = touchEventsInGameObject.Key;
            List<W7TouchEvent> touchEvents = touchEventsInGameObject.Value;
            List<W7TouchEvent> uniqueTouchEvents = new List<W7TouchEvent>();
            {
                Dictionary<int, W7TouchEvent> touchEventsByID = new Dictionary<int, W7TouchEvent>();
                touchEvents.ForEach(te => {
                    if (!te.unpressed) {
                        touchEventsByID[te.touchId] = te;
                    }
                });
                uniqueTouchEvents = new List<W7TouchEvent>(touchEventsByID.Values);
            }
            //Debug.Log(target.name + " " + uniqueTouchEvents.Count);
            if (uniqueTouchEvents.Count == 1) {
                foreach (var touchEvent in touchEvents) {
                    UICamera.currentTouch = touchEvent.touch;
                    uiCamera.ProcessTouch(touchEvent.pressed, touchEvent.unpressed);
                }
            }
            if (useGestures) {
                if (uniqueTouchEvents.Count == 2) {
                    Vector2 delta1 = uniqueTouchEvents[0].touch.delta;
                    Vector2 delta2 = uniqueTouchEvents[1].touch.delta;
                    Vector2 pos1 = uniqueTouchEvents[0].touch.pos;
                    Vector2 pos2 = uniqueTouchEvents[1].touch.pos;

                    UICamera.currentTouch = uniqueTouchEvents[0].touch;
                    float dot = Vector2.Dot(delta1.normalized, delta2.normalized);
                    if (dot <= -0.7) {
                        float distance = Vector3.Distance(pos1, pos2);
                        float distanceOld = Vector3.Distance(pos1 - delta1, pos2 - delta2);
                        float delta = ((distance - distanceOld) / 2) / scrollSpeed;
                        if (Mathf.Abs(delta) > Mathf.Epsilon) {
                            target.SendMessage("OnScroll", delta, SendMessageOptions.DontRequireReceiver);
                        }
                    } else if (dot >= 0.7) {
                        Vector2 deltaAvg = (delta1 + delta2) / 2;
                        if (deltaAvg.sqrMagnitude > Mathf.Epsilon) {
                            int touchID = UICamera.currentTouchID;
                            UICamera.currentTouchID = -2;
                            target.SendMessage("OnDrag", deltaAvg, SendMessageOptions.DontRequireReceiver);
                            UICamera.currentTouchID = touchID;
                        }
                    }
                    
                    //Twist
                    Vector2 posOld1 = pos1 - delta1;
                    Vector2 posOld2 = pos2 - delta2;

                    Vector2 dir = (pos2 - pos1).normalized;
                    Vector2 oldDir = (posOld2 - posOld1).normalized;

                    float deltaAngle = Vector2.Angle(oldDir, dir);
                    if (deltaAngle > Mathf.Epsilon) {
                        int sign = (Vector3.Cross(oldDir, dir).z < 0) ? 1 : -1;
                        target.SendMessage("OnTwist", sign * deltaAngle / twistSpeed, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            if (useRawTouches) {
                target.SendMessage("OnTouch", touchEvents.ToArray(), SendMessageOptions.DontRequireReceiver);
            }
            UICamera.currentTouch = null;
        }
    }

    private bool AllTouchesUp(List<W7Touch> touches) {
        int count = 0;

        foreach (W7Touch touch in touches) {
            if (touch.Phase == TouchPhase.Ended || touch.Phase == TouchPhase.Canceled)
                count++;
        }

        if (count == touches.Count)
            return true;
        else
            return false;
    }
}
