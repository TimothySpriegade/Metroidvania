#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


namespace EventSystem.Tester
{
    [CustomEditor(typeof(EventTester))]
    public class EventTestingInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var eventTester = (EventTester)target;


            if (GUILayout.Button("Invoke"))
            {
                eventTester.Invoke();
            }
        }
    }
}
#endif