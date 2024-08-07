﻿using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(RedDotComponent))]
    public static class RedDotHelper
    {
        /// <summary>
        /// 增加逻辑红点节点
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="parent"></param>
        /// <param name="target"></param>
        /// <param name="isNeedShowNum"></param>
        public static void AddRedDotNode(Scene scene, string parent, string target,bool isNeedShowNum)
        {
            RedDotComponent RedDotComponent = scene.GetComponent<RedDotComponent>();
            if (RedDotComponent == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(parent) && !RedDotComponent.RedDotNodeParentsDict.ContainsKey(parent))
            {
                Log.Warning("Runtime动态添加的红点，其父节点是新节点： " + parent);
            }

            RedDotComponent.AddRedDotNode(parent, target, isNeedShowNum);
        }
        
        /// <summary>
        /// 移除逻辑红点
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="target"></param>
        /// <param name="isRemoveView"></param>
        public static void RemoveRedDotNode(Scene scene, string target, bool isRemoveView = true)
        {
            RedDotComponent RedDotComponent = scene.GetComponent<RedDotComponent>();
            if (RedDotComponent == null)
            {
                return;
            }

            RedDotComponent.RemoveRedDotNode(target);
            if (isRemoveView)
            {
                RedDotComponent.RemoveRedDotView(target, out RedDotMonoView redDotMonoView);
            }
        }
        
        
        /// <summary>
        /// 增加红点节点显示层
        /// </summary>
        /// <param name="ZoneScene"></param>
        /// <param name="target"></param>
        /// <param name="monoView"></param>
        public static void AddRedDotNodeView(Scene scene, string target, GameObject gameObject,Vector3 RedDotScale,Vector2 PositionOffset )
        {
            RedDotComponent RedDotComponent = scene.GetComponent<RedDotComponent>();
            if (RedDotComponent == null)
            {
                return;
            }
            RedDotMonoView monoView = gameObject.GetComponent<RedDotMonoView>()??gameObject.AddComponent<RedDotMonoView>();
            monoView.RedDotScale = RedDotScale;
            monoView.PositionOffset = PositionOffset;
            RedDotComponent.AddRedDotView(target, monoView);
        }
        
        
        /// <summary>
        /// 增加红点节点显示层
        /// </summary>
        /// <param name="ZoneScene"></param>
        /// <param name="target"></param>
        /// <param name="monoView"></param>
        public static void AddRedDotNodeView(Scene scene, string target, RedDotMonoView monoView)
        {
            RedDotComponent RedDotComponent = scene.GetComponent<RedDotComponent>();
            if (RedDotComponent == null)
            {
                return;
            }
            RedDotComponent.AddRedDotView(target, monoView);
        }

        /// <summary>
        /// 移除红点节点显示层
        /// </summary>
        /// <param name="ZoneScene"></param>
        /// <param name="target"></param>
        /// <param name="monoView"></param>
        public static void RemoveRedDotView(Scene scene, string target, out RedDotMonoView monoView)
        {
            monoView = null;
            RedDotComponent RedDotComponent = scene?.GetComponent<RedDotComponent>();
            if (RedDotComponent == null)
            {
                return;
            }

            RedDotComponent.RemoveRedDotView(target, out monoView);
        }
        
        /// <summary>
        /// 隐藏逻辑红点
        /// </summary>
        /// <param name="ZoneScene"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool HideRedDotNode(Scene scene, string target)
        {
            RedDotComponent redDotComponent = scene.GetComponent<RedDotComponent>();
            if (redDotComponent == null)
            {
                return false;
            }
            return redDotComponent.HideRedDotNode(target);
        }
        
        /// <summary>
        /// 显示逻辑红点
        /// </summary>
        /// <param name="ZoneScene"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool ShowRedDotNode(Scene scene, string target)
        {
            if (IsLogicAlreadyShow(scene, target))
            {
                return false;
            }
            RedDotComponent redDotComponent = scene.GetComponent<RedDotComponent>();
            if (redDotComponent == null)
            {
                return false;
            }
            return redDotComponent.ShowRedDotNode(target);
        }
        
        /// <summary>
        /// 逻辑红点是否已经处于显示状态
        /// </summary>
        /// <param name="ZoneScene"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsLogicAlreadyShow(Scene scene, string target)
        {
            RedDotComponent redDotComponent = scene.GetComponent<RedDotComponent>();
            if (redDotComponent == null)
            {
                Log.Error("redDotComponent is not exist!");
                return false;
            }

            if (!redDotComponent.RedDotNodeRetainCount.ContainsKey(target))
            {
                return false;
            }
            
            return redDotComponent.RedDotNodeRetainCount[target] >= 1;
        }
        
        /// <summary>
        /// 刷新红点显示层的文本数量
        /// </summary>
        /// <param name="zoneScene"></param>
        /// <param name="target"></param>
        /// <param name="Count"></param>
        public static void RefreshRedDotViewCount(Scene scene, string target, int Count)
        {
            RedDotComponent redDotComponent = scene.GetComponent<RedDotComponent>();
            if (redDotComponent == null)
            {
                return;
            }
            redDotComponent.RefreshRedDotViewCount(target,Count);
        }
    }
}