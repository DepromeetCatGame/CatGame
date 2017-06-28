using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Input helper.
/// SOURCE: http://answers.unity3d.com/answers/956579/view.html
/// </summary>
///

namespace Core
{
    public static class InputHelper
    {
        private static TouchCreator lastFakeTouch;

        public static List<Touch> GetTouches()
        {
            List<Touch> touches = new List<Touch>();
            touches.AddRange(Input.touches);
            // Uncomment if you want it only to allow mouse swipes in the Unity Editor
#if UNITY_EDITOR
            if (lastFakeTouch == null)
            {
                lastFakeTouch = new TouchCreator();
            }
            if (Input.GetMouseButtonDown(0))
            {
                lastFakeTouch.phase = TouchPhase.Began;
                lastFakeTouch.deltaPosition = new Vector2(0, 0);
                lastFakeTouch.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                lastFakeTouch.fingerId = 0;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                lastFakeTouch.phase = TouchPhase.Ended;
                Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                lastFakeTouch.deltaPosition = newPosition - lastFakeTouch.position;
                lastFakeTouch.position = newPosition;
                lastFakeTouch.fingerId = 0;
            }
            else if (Input.GetMouseButton(0))
            {
                lastFakeTouch.phase = TouchPhase.Moved;
                Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                lastFakeTouch.deltaPosition = newPosition - lastFakeTouch.position;
                lastFakeTouch.position = newPosition;
                lastFakeTouch.fingerId = 0;
            }
            else
            {
                lastFakeTouch = null;
            }
            if (lastFakeTouch != null)
            {
                touches.Add(lastFakeTouch.Create());
            }
            // Uncomment if you want it only to allow mouse swipes in the Unity Editor
#endif

            return touches;
        }
    }

    /// <summary>
    /// Touch creator.
    /// BASED ON: http://answers.unity3d.com/answers/801637/view.html
    /// </summary>
    public class TouchCreator
    {
        private static BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        private static Dictionary<string, FieldInfo> fields;

        private object touch;

        public float deltaTime { get { return ((Touch)touch).deltaTime; } set { fields["m_TimeDelta"].SetValue(touch, value); } }
        public int tapCount { get { return ((Touch)touch).tapCount; } set { fields["m_TapCount"].SetValue(touch, value); } }
        public TouchPhase phase { get { return ((Touch)touch).phase; } set { fields["m_Phase"].SetValue(touch, value); } }
        public Vector2 deltaPosition { get { return ((Touch)touch).deltaPosition; } set { fields["m_PositionDelta"].SetValue(touch, value); } }
        public int fingerId { get { return ((Touch)touch).fingerId; } set { fields["m_FingerId"].SetValue(touch, value); } }
        public Vector2 position { get { return ((Touch)touch).position; } set { fields["m_Position"].SetValue(touch, value); } }
        public Vector2 rawPosition { get { return ((Touch)touch).rawPosition; } set { fields["m_RawPosition"].SetValue(touch, value); } }

        public Touch Create()
        {
            return (Touch)touch;
        }

        public TouchCreator()
        {
            touch = new Touch();
        }

        static TouchCreator()
        {
            fields = new Dictionary<string, FieldInfo>();
            foreach (var f in typeof(Touch).GetFields(flags))
            {
                fields.Add(f.Name, f);
                //Debug.Log("name: " + f.Name); // Use this to find the names of hidden private fields
            }
        }
    }
}