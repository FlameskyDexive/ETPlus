﻿
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

using System.Text;
using ET;
using UnityEngine.UI;

public partial class UICodeSpawner
{
    static public void SpawnLoopItemCode(GameObject gameObject)
    {
        if (gameObject.GetComponent<LayoutElement>() == null)
        {
            Log.Error("哦吼！ 生成Item UI代码失败");
            Log.Error($"请不要犯贱，请给{gameObject.name}加上LayoutElement组件,并根据预设物宽高设置好组件的Preferred Width和Preferred Height 参数");
            Log.Error("请不要大着脸质疑EUI的循环列表有BUG,对于你这种不认真上课的菜鸡,你踏马的哪来的脸问出这种傻逼问题？");
            return;
        }
        
        
        
        Path2WidgetCachedDict?.Clear();
        Path2WidgetCachedDict = new Dictionary<string, List<Component>>();
        FindAllWidgets(gameObject.transform, "");
        SpawnCodeForScrollLoopItemBehaviour(gameObject);
        SpawnCodeForScrollLoopItemViewSystem(gameObject);
        AssetDatabase.Refresh();
    }
    
    static void SpawnCodeForScrollLoopItemViewSystem(GameObject gameObject)
    {
        if (null == gameObject)
        {
            return;
        }
        string strDlgName = gameObject.name;

        string strFilePath = Application.dataPath + "/Scripts/HotfixView/Client/Demo/UIItemBehaviour";

        if ( !System.IO.Directory.Exists(strFilePath) )
        {
            System.IO.Directory.CreateDirectory(strFilePath);
        }
        strFilePath     = Application.dataPath + "/Scripts/HotfixView/Client/Demo/UIItemBehaviour/" + strDlgName + "ViewSystem.cs";
        StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);

        StringBuilder strBuilder = new StringBuilder();
        strBuilder.AppendLine()
            .AppendLine("using UnityEngine;");
        strBuilder.AppendLine("using UnityEngine.UI;");
        strBuilder.AppendLine("namespace ET.Client");
        strBuilder.AppendLine("{");
        
        
        strBuilder.AppendFormat("\t[EntitySystemOf(typeof(Scroll_{0}))]\n",strDlgName);
        strBuilder.AppendFormat("\tpublic static partial class Scroll_{0}ViewSystem \r\n", strDlgName);
        strBuilder.AppendLine("\t{");
        
        strBuilder.AppendLine("\t\t[EntitySystem]");
        strBuilder.AppendFormat("\t\tprivate static void Awake(this Scroll_{0} self )",strDlgName);
        strBuilder.AppendLine("\n\t\t{");
        
        strBuilder.AppendLine("\t\t}\n");
        
        
        strBuilder.AppendLine("\t\t[EntitySystem]");
        strBuilder.AppendFormat("\t\tprivate static void Destroy(this Scroll_{0} self )",strDlgName);
        strBuilder.AppendLine("\n\t\t{");
        
        strBuilder.AppendFormat("\t\t\tself.DestroyWidget();\r\n");

        strBuilder.AppendLine("\t\t}");
        strBuilder.AppendLine("\t}");
        strBuilder.AppendLine("}");
        
        sw.Write(strBuilder);
        sw.Flush();
        sw.Close();
    }
    
    
    static void SpawnCodeForScrollLoopItemBehaviour(GameObject gameObject)
    {
        if (null == gameObject)
        {
            return;
        }
        string strDlgName = gameObject.name;

        string strFilePath = Application.dataPath + "/Scripts/ModelView/Client/Demo/UIItemBehaviour";

        if ( !System.IO.Directory.Exists(strFilePath) )
        {
            System.IO.Directory.CreateDirectory(strFilePath);
        }
        strFilePath     = Application.dataPath + "/Scripts/ModelView/Client/Demo/UIItemBehaviour/" + strDlgName + ".cs";
        StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);

        StringBuilder strBuilder = new StringBuilder();
        strBuilder.AppendLine()
            .AppendLine("using UnityEngine;");
        strBuilder.AppendLine("using UnityEngine.UI;");
        strBuilder.AppendLine("namespace ET.Client");
        strBuilder.AppendLine("{");
        strBuilder.AppendLine("\t[EnableMethod]");
        strBuilder.AppendFormat("\tpublic  class Scroll_{0} : Entity,IAwake,IDestroy,IUIScrollItem<Scroll_{1}>,IUILogic \r\n", strDlgName,strDlgName)
            .AppendLine("\t{");
        
        strBuilder.AppendLine("\t\tpublic long DataId {get;set;}");
        
        strBuilder.AppendLine("\t\tprivate bool isCacheNode = false;");
        strBuilder.AppendLine("\t\tpublic void SetCacheMode(bool isCache)");
        strBuilder.AppendLine("\t\t{");
        strBuilder.AppendLine("\t\t\tthis.isCacheNode = isCache;");
        strBuilder.AppendLine("\t\t}\n");
        strBuilder.AppendFormat("\t\tpublic Scroll_{0} BindTrans(Transform trans)\r\n",strDlgName);
        strBuilder.AppendLine("\t\t{");
        strBuilder.AppendLine("\t\t\tthis.uiTransform = trans;");
        CreateESUIReleaseCode(ref strBuilder, gameObject.transform);
        strBuilder.AppendLine("\t\t\treturn this;");
        strBuilder.AppendLine("\t\t}\n");
        
        CreateWidgetBindCode(ref strBuilder, gameObject.transform);
        CreateDestroyWidgetCode(ref strBuilder,true);
        CreateDeclareCode(ref strBuilder);
        
        strBuilder.AppendLine("\t\tpublic Transform uiTransform = null;");
        
        strBuilder.AppendLine("\t}");
        strBuilder.AppendLine("}");
        
        sw.Write(strBuilder);
        sw.Flush();
        sw.Close();
    }
    
    public static void CreateESUIReleaseCode(ref StringBuilder strBuilder, Transform transRoot)
    {
        foreach (KeyValuePair<string, List<Component>> pair in Path2WidgetCachedDict)
        {
	        foreach (var info in pair.Value)
	        {
		        Component widget = info;
				if (pair.Key.StartsWith(CommonUIPrefix))
				{
					var subUIClassPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(widget);
					if (subUIClassPrefab==null)
					{
						Debug.LogError($"公共UI找不到所属的Prefab! {pair.Key}");
						continue;
					}
                    strBuilder.AppendFormat("\t\t\tthis.{0}?.Dispose();\r\n",pair.Key );
				}
	        }
        }
    }
    
    


}
