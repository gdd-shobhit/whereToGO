using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    class EditorTools : EditorWindow
    {
        Color color;
        [MenuItem("Essential Tools/Editing Tools")]
        static void ShowWindow()
        {
            EditorWindow.GetWindow<EditorTools>("Editing Tools");
        }

        private void OnGUI()
        {
            using(new EditorGUILayout.HorizontalScope())
            {
                color = EditorGUILayout.ColorField("Color", color);
                if (GUILayout.Button("Color it"))
                {
                    Colorize();
                }          
            }
            EditorGUILayout.LabelField("asd", GUI.skin.horizontalSlider);


        }

        private void Colorize()
        {
            Renderer renderer;
            foreach (GameObject obj in Selection.objects)
            {
                renderer = obj.GetComponent<Renderer>();
                if(renderer!= null)
                {
                    renderer.sharedMaterial.color = color;
                }
            }
        }
    }
}
