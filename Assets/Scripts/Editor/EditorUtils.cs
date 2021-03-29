using UnityEngine;
using UnityEditor;

public class EditorUtils : Editor
{
    //This code is released under the MIT license: https://opensource.org/licenses/MIT
    [MenuItem("Window/Fold all")]
    static void UnfoldSelection()
    {
        EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
        var hierarchyWindow = EditorWindow.focusedWindow;
        var expandMethodInfo = hierarchyWindow.GetType().GetMethod("SetExpandedRecursive");
        foreach (GameObject root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            expandMethodInfo.Invoke(hierarchyWindow, new object[] { root.GetInstanceID(), false });
        }
    }
}