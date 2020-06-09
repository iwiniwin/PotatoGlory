using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Generator))]  // 用于标识要自定义哪个脚本的inspector窗口
public class GeneratorEditor : Editor
{
    // 判断是否显示折叠框
    private bool m_ShowPrefabs;
    private Generator m_Generator;
    private List<GameObject> m_PrefabList = null;

    public override void OnInspectorGUI(){  // 绘制Inspecotr窗口的代码是在这个函数中执行的
        m_Generator = (Generator)target;  // target是Editor类提供的成员变量
        // 显示GenerateDelay参数
        m_Generator.GenerateDelay = EditorGUILayout.FloatField(new GUIContent("Generate Delay", "多久之后开始实例化预设对象"), m_Generator.GenerateDelay);
        // 显示RandomGenerateInterval参数
        m_Generator.RandomGenerateInterval = EditorGUILayout.Toggle(new GUIContent("Random Generate Interval", "是否使用随机时间间隔来实例化预设对象"), m_Generator.RandomGenerateInterval);
        if(m_Generator.RandomGenerateInterval){
            m_Generator.MinGenerateInterval = EditorGUILayout.FloatField(new GUIContent("Min Generate Interval", "实例化预设对象的最短时间间隔"), m_Generator.MinGenerateInterval);
            m_Generator.MaxGenerateInterval = EditorGUILayout.FloatField(new GUIContent("Max Generate Interval", "实例化预设对象的最长时间间隔"), m_Generator.MaxGenerateInterval);
            // 确保MaxGenerateInterval的数值比MinGenerateInterval大
            if(m_Generator.MaxGenerateInterval < m_Generator.MinGenerateInterval){
                m_Generator.MaxGenerateInterval = m_Generator.MinGenerateInterval;
            }
        }else{
            // 显示Generate Interval参数
            m_Generator.GenerateInterval = EditorGUILayout.FloatField(new GUIContent("Generate Interval", "实例化预设对象的固定时间间隔"), m_Generator.GenerateInterval);
        }

        // 显示RandomGeneratePositionX参数
        m_Generator.RandomGeneratePositionX = EditorGUILayout.Toggle(new GUIContent("Random Generate Position X", "是否在随机的X坐标上实例化预设对象"), m_Generator.RandomGeneratePositionX);
        if(m_Generator.RandomGeneratePositionX){
            m_Generator.MinGeneratePositionX = EditorGUILayout.FloatField(new GUIContent("Min Generate Position X", "实例化预设对象的最小X坐标"), m_Generator.MinGeneratePositionX);
            m_Generator.MaxGeneratePositionX = EditorGUILayout.FloatField(new GUIContent("Max Generate Position X", "实例化预设对象的最大X坐标"), m_Generator.MaxGeneratePositionX);
            if(m_Generator.MaxGeneratePositionX < m_Generator.MinGeneratePositionX){
                m_Generator.MaxGeneratePositionX = m_Generator.MinGeneratePositionX;
            }
        }

        m_Generator.RandomGeneratePositionY = EditorGUILayout.Toggle(new GUIContent("Random Generate Position Y", "是否在随机的Y坐标上实例化预设对象"), m_Generator.RandomGeneratePositionY);
        if(m_Generator.RandomGeneratePositionY){
            m_Generator.MinGeneratePositionY = EditorGUILayout.FloatField(new GUIContent("Min Generate Position Y", "实例化预设对象的最小Y坐标"), m_Generator.MinGeneratePositionY);
            m_Generator.MaxGeneratePositionY = EditorGUILayout.FloatField(new GUIContent("Max Generate Position Y", "实例化预设对象的最大Y坐标"), m_Generator.MaxGeneratePositionY);
            if(m_Generator.MaxGeneratePositionY < m_Generator.MinGeneratePositionY){
                m_Generator.MaxGeneratePositionY = m_Generator.MinGeneratePositionY;
            }
        }

        m_Generator.PrefabOrientation = (Orientation)EditorGUILayout.EnumPopup(new GUIContent("Prefab Orientation", "预设对象的朝向"), m_Generator.PrefabOrientation);

        if(m_PrefabList == null){
            m_PrefabList = new List<GameObject>(m_Generator.Prefabs);
        }

        // 绘制折叠框，设置prefab参数
        m_ShowPrefabs = EditorGUILayout.Foldout(m_ShowPrefabs, new GUIContent("Prefabs", "预设对象"));
        if(m_ShowPrefabs){
            // 缩进
            EditorGUI.indentLevel ++;
            for(int i = 0; i < m_Generator.Prefabs.Length; i ++){
                EditorGUILayout.BeginHorizontal();

                // 绘制元素赋值框
                m_Generator.Prefabs[i] = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Prefab", "预设对象"), m_Generator.Prefabs[i], typeof(GameObject), false);

                if(GUILayout.Button("Remove")){
                    m_PrefabList.RemoveAt(i);
                    m_Generator.Prefabs = m_PrefabList.ToArray();
                }
                EditorGUILayout.EndHorizontal();
            }
            if(GUILayout.Button("Add")){
                m_PrefabList.Add(null);
                m_Generator.Prefabs = m_PrefabList.ToArray();
            }

            // 取消缩进
            EditorGUI.indentLevel --;
        }

        // 当值改变时，将target标记为已修改
        if(GUI.changed){
            EditorUtility.SetDirty(target);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
