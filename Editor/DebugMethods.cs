using UnityEngine;
using UnityEditor;
using UnityEditor.DebugTools;

[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour), true)]
public class DebugMethods : Editor
{
    private MethodInvoker invoker = new MethodInvoker();
    private DebugMethodGUI gui;

    public DebugMethods()
    {
        gui = new DebugMethodGUI(invoker);
    }

    public override void OnInspectorGUI()
    {
        // Draw the normal inspector first
        base.OnInspectorGUI();

        gui.DrawInspector(targets);
    }

    protected void OnDisable()
    {
        gui.ClearContinuous();
    }
}
