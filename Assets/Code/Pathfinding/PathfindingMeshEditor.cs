//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(PathfindingMesh))]
//[CanEditMultipleObjects]
//public class PathfindingMeshEditor : Editor
//{
//    int newLength;
//    string arrayName = "Grounds";

//    void OnEnable()
//    {
//        PathfindingMesh myTarget = (PathfindingMesh)target;
//        newLength = myTarget.grounds.Length;
//    }

//    public override void OnInspectorGUI()
//    {
//        PathfindingMesh myTarget = (PathfindingMesh)target;
//        EditorGUILayout.FloatField("PlayerRadius", 0.5f);

        
//        newLength = EditorGUILayout.IntField(arrayName, newLength);


//        if (GUI.GetNameOfFocusedControl() != arrayName ||
//            GUI.GetNameOfFocusedControl() == arrayName && (
//                Event.current.keyCode == KeyCode.KeypadEnter ||
//                Event.current.keyCode == KeyCode.Return
//            ))
//        {
//            if (newLength != myTarget.grounds.Length)
//            {
//                GameObject[] newArray = new GameObject[newLength];

//                for (var i = 0; i < newLength && i < myTarget.grounds.Length; i++)
//                {
//                    newArray[i] = myTarget.grounds[i];
//                }

//                myTarget.grounds = newArray;
//            }
//        }



//        for (var i = 0; i < myTarget.grounds.Length; i++)
//        {
//            myTarget.grounds[i] = (GameObject)EditorGUILayout.ObjectField(myTarget.grounds[i], typeof(GameObject), true);
//        }



//        if (GUILayout.Button("Bake"))
//        {

//        }
//    }
//}
