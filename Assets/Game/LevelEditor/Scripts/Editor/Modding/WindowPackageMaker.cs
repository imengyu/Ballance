using Ballance2.Config;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Utils;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Ballance2.Editor.Modding
{
  public class WindowPackageMaker : EditorWindow
  {
    public WindowPackageMaker()
    {
      titleContent = new GUIContent("创建 Ballance 模组包");
    }

    private string modPackageName = "com.yourname.packagename";
    private string modName = "";
    private string modAuthor = "";
    private string modIntroduction = "";
    private string modUserVersion = "1.0";
    private int modVersion = 1;
    private GamePackageType PackageType = GamePackageType.Module;

    private SerializedObject serializedObject;

    private GUIStyle groupBox = null;
    private bool error = false;

    private TextAsset template_PackageDef;

    private void OnEnable()
    {
      serializedObject = new SerializedObject(this);

      template_PackageDef = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Packages/template_PackageDef.xml");
    }

    private void OnDisable()
    {
      serializedObject = null;
    }

    private void OnGUI()
    {
      serializedObject.Update();

      if (groupBox == null)
        groupBox = GUI.skin.FindStyle("GroupBox");

      error = false;

      EditorGUI.BeginChangeCheck();

      EditorGUILayout.BeginVertical(groupBox);

      EditorGUILayout.Space(20);
      EditorGUILayout.HelpBox(new GUIContent("使用这个工具来快速生成一个模组包模板，生成文件会输出到 Assets/Packages 下。"), true);
      EditorGUILayout.Space(15);

      modPackageName = EditorGUILayout.TextField("模组包名", modPackageName);
      if (StringUtils.isNullOrEmpty(modPackageName))
      {
        EditorGUILayout.HelpBox("必须填写模组包名", MessageType.Error);
        error = true;
      }
      if (!StringUtils.IsPackageName(modPackageName))
      {
        EditorGUILayout.HelpBox("包名必须是 xxxx.xxx.xxx 格式", MessageType.Error);
        error = true;
      }

      modName = EditorGUILayout.TextField("模组名称", modName);
      modAuthor = EditorGUILayout.TextField("模组作者名字", modAuthor);
      modIntroduction = EditorGUILayout.TextField("模组简介文字", modIntroduction, GUILayout.Height(60));
      modVersion = EditorGUILayout.IntField("模组版本（默认1）", modVersion);
      modUserVersion = EditorGUILayout.TextField("模组版本（显示用户看）", modUserVersion);
      PackageType = (GamePackageType)EditorGUILayout.EnumPopup("模组类型", PackageType);

      EditorGUILayout.Space(20);

      if (GUILayout.Button("生成") && !error)
      {
        Make();
      }

      EditorGUILayout.EndVertical();

      if (EditorGUI.EndChangeCheck() && serializedObject != null)
        serializedObject.ApplyModifiedProperties();
    }

    private void Make()
    {
      string folderPath = GamePathManager.DEBUG_PACKAGE_FOLDER + "/" + modPackageName;
      if (Directory.Exists(folderPath))
      {
        if (!EditorUtility.DisplayDialog("提示", "指定包名模组 " + modPackageName + " 已经在： \n" +
            folderPath + "\n存在了，是否要替换？", "替换", "取消"))
          return;
      }

      DirectoryInfo directoryInfo = Directory.CreateDirectory(folderPath);
      if (directoryInfo == null)
      {
        EditorUtility.DisplayDialog("错误", "创建文件夹失败：\n" + folderPath, "确定");
        return;
      }

      XmlDocument xml = new XmlDocument();
      xml.LoadXml(template_PackageDef.text);

      XmlNode Package = xml.SelectSingleNode("Package");
      XmlNode BaseInfo = Package.SelectSingleNode("BaseInfo");
      XmlNode Name = BaseInfo.SelectSingleNode("Name");
      XmlNode Author = BaseInfo.SelectSingleNode("Author");
      XmlNode Introduction = BaseInfo.SelectSingleNode("Introduction");
      XmlNode VersionName = BaseInfo.SelectSingleNode("VersionName");

      Package.Attributes["name"].InnerText = modPackageName;
      Package.Attributes["version"].InnerText = modVersion.ToString();

      Name.InnerText = modName;
      Author.InnerText = modAuthor;
      Introduction.InnerText = modIntroduction;
      VersionName.InnerText = modUserVersion;

      XmlNode MinVersion = xml.SelectSingleNode("Package/Compatibility/MinVersion");
      XmlNode TargetVersion = xml.SelectSingleNode("Package/Compatibility/TargetVersion");

      MinVersion.InnerText = GameConst.GameBulidVersion.ToString();
      TargetVersion.InnerText = GameConst.GameBulidVersion.ToString();

      XmlNode PackageType = xml.SelectSingleNode("Package/Type");

      PackageType.InnerText = this.PackageType.ToString();

      xml.Save(folderPath + "/PackageDef.xml");

      if (this.PackageType == GamePackageType.Module)
      {
        string str = File.ReadAllText(GamePathManager.DEBUG_PACKAGE_FOLDER + "/template_PackageEntry.cs");
        str = str.Replace("ModNamespace", modPackageName);
        File.WriteAllText(folderPath + "/PackageEntry.cs", str);
        File.WriteAllText(folderPath + "/.gitignore", "bin/\nobj/\n.vs/\n.dll/\n!*.csproj");
        MakeCSproj(folderPath);
      }
      File.Copy(GamePathManager.DEBUG_PACKAGE_FOLDER + "/template_PackageLogo.png", folderPath + "/PackageLogo.png");

      AssetDatabase.Refresh();
      EditorUtility.DisplayDialog("提示", "生成模板成功！", "好的");

      Close();
    }

    private void MakeCSproj(string folderPath) {
      
      XmlDocument xml = new XmlDocument();
      xml.LoadXml(File.ReadAllText("./Assembly-CSharp.csproj"));

      var addedRes = false;

      foreach(XmlNode node in xml.DocumentElement.ChildNodes) {
        if (node.Name == "PropertyGroup") {
          foreach(XmlNode node1 in node.ChildNodes) {
            if (node1.Name == "ProjectGuid") {
              node1.InnerText = GUID.Generate().ToString();
            } else if (node1.Name == "AssemblyName") {
              node1.InnerText = modPackageName;
            } else if (node1.Name == "OutputPath") {
              node1.InnerText = ".dll";
            }
          }
        } else if (node.Name == "ItemGroup") {
          for (var i = node.ChildNodes.Count - 1; i >= 0; i--)
          {
            var node1 = node.ChildNodes[i];
            if (node1.Name == "Compile" || node1.Name == "None" || node1.Name == "ProjectReference") {
              node.RemoveChild(node1);
            }
          }

          if (!addedRes) {
            addedRes = true;
            
            var csNode = xml.CreateElement("Compile");
            var csInclude = xml.CreateAttribute("Include");
            csInclude.Value = "PackageEntry.cs";
            csNode.Attributes.Append(csInclude);

            var xmlNode = xml.CreateElement("None");
            var xmlNodeInclude = xml.CreateAttribute("Include");
            xmlNodeInclude.Value = "PackageDef.xml";
            xmlNode.Attributes.Append(xmlNodeInclude);

            var refACNode = xml.CreateElement("Reference");
            var refACNodeInclude = xml.CreateAttribute("Include");
            var refACNodeHintPath = xml.CreateElement("HintPath");
            refACNodeHintPath.InnerText = "..\\..\\..\\Library\\ScriptAssemblies\\Assembly-CSharp.dll";
            refACNodeInclude.Value = "Assembly-CSharp";
            refACNode.AppendChild(refACNodeHintPath);
            refACNode.Attributes.Append(refACNodeInclude);

            node.AppendChild(csNode);
            node.AppendChild(xmlNode);
            node.AppendChild(refACNode);
          }
        } 

      }

      xml.Save(folderPath + "/Package.csproj");
    }
  }
}
