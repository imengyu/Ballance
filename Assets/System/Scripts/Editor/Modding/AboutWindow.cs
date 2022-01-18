using UnityEditor;
using UnityEngine;

namespace Ballance2.Editor.Modding
{
    public class AboutWindow : EditorWindow
    {
        public AboutWindow()
        {
            titleContent = new GUIContent("关于 Ballance 项目");
        }

        [@MenuItem("Ballance/帮助", false, 104)]
        static void ShowHelp()
        {

        }
        [@MenuItem("Ballance/关于", false, 109)]
        static void ShowAbout()
        {
            EditorWindow.GetWindowWithRect(typeof(AboutWindow), new Rect(200, 150, 390, 500));
        }

        private Texture2D logo;
        private GUIStyle DefaultCenteredLargeText;
        private GUIStyle DefaultCenteredText;
        private GUIStyle LinkLabel;
        private GUIStyle ButtonMid;
        private GUIStyle OLbox;

        private void OnEnable()
        {
            logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/System/Textures/splash_app.bmp");
        }

        private void OnGUI()
        {
            if (OLbox == null) OLbox = GUI.skin.GetStyle("OL box");
            if (DefaultCenteredLargeText == null) DefaultCenteredLargeText = GUI.skin.GetStyle("DefaultCenteredLargeText");
            if (DefaultCenteredText == null) DefaultCenteredText = GUI.skin.GetStyle("DefaultCenteredText");
            if (LinkLabel == null) LinkLabel = GUI.skin.GetStyle("LinkLabel");
            if (ButtonMid == null) ButtonMid = GUI.skin.GetStyle("ButtonMid");

            EditorGUILayout.BeginVertical(OLbox);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(logo, GUILayout.Width(360));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(16);
            GUILayout.Label("Ballance Rebuild Project", DefaultCenteredLargeText);
            GUILayout.Label("Ballance 重制项目（Unity）", DefaultCenteredText);

            EditorGUILayout.Space(3);
            GUILayout.Label("这是一个使用 Unity 制作的 Ballance 重制项目，\n这是作者很早就想做的一个小小梦想，\n" +
                "作者就是一个普通学生，开发水平不足请谅解。\n你可以在项目github上获取更多信息。", DefaultCenteredText);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("项目地址：github.com/imengyu/Ballance2", LinkLabel))
                EditorUtility.OpenWithDefaultApp("https://github.com/imengyu/Ballance2");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(6);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Copyright © 2020 mengyu 版权所有，MIT 开源协议", LinkLabel))
                EditorUtility.OpenWithDefaultApp("https://imyzc.com/");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical(GUILayout.Width(270));
            if (GUILayout.Button("开发帮助", ButtonMid))
                ShowHelp();
            if (GUILayout.Button("关闭", ButtonMid))
                Close();
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
    }
}
