﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YooAsset;

namespace ET
{

    // 用于字符串转换，减少GC
    [FriendOf(typeof(ResourcesComponent))]
    public static class AssetBundleHelper
    {
        public static string StringToAB(this string value)
        {
            // string result =  $"Assets/Bundles/UI/Dlg/{value}.prefab";
            string result = $"{value}.prefab";
            return result;
        }

    }


    /// <summary>
    /// 远端资源地址查询服务类
    /// </summary>
    public class RemoteServices : IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }
        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }
        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }

    public class ResourcesComponent : Singleton<ResourcesComponent>, ISingletonAwake
    {
        private ResourcePackage defaultPackage;
        private EPlayMode playMode;
        public void Awake()
        {
            YooAssets.Initialize();
            BetterStreamingAssets.Initialize();
            // this.LoadGlobalConfigAsync().Coroutine();
        }

        protected override void Destroy()
        {
            YooAssets.Destroy();
        }
        
        private IEnumerator LoadGlobalConfig()
        {
            AssetHandle handler = YooAssets.LoadAssetAsync<GlobalConfig>("GlobalConfig");
            yield return handler;
            GlobalConfig.Instance = handler.AssetObject as GlobalConfig;
            handler.Release();
            defaultPackage.UnloadUnusedAssets();
        }

        private async ETTask LoadGlobalConfigAsync()
        {
            AssetHandle handler = YooAssets.LoadAssetAsync<GlobalConfig>("GlobalConfig");
            await handler;
            GlobalConfig.Instance = handler.AssetObject as GlobalConfig;
            handler.Release();
            defaultPackage.UnloadUnusedAssets();
        }

        public async ETTask RestartAsync()
        {
            await this.LoadGlobalConfigAsync();
        }

        public async ETTask CreatePackageAsync(string packageName, bool isDefault = false)
        {
            defaultPackage = YooAssets.TryGetPackage(packageName);
            if(this.defaultPackage == null)
                defaultPackage = YooAssets.CreatePackage(packageName);
            if (isDefault)
            {
                YooAssets.SetDefaultPackage(defaultPackage);
            }

            EPlayMode ePlayMode = Define.PlayMode;

            // 编辑器下的模拟模式
            switch (ePlayMode)
            {
                case EPlayMode.EditorSimulateMode:
                    {
                        EditorSimulateModeParameters createParameters = new();
                        createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild("ScriptableBuildPipeline", packageName);
                        await defaultPackage.InitializeAsync(createParameters).Task;
                        break;
                    }
                case EPlayMode.OfflinePlayMode:
                    {
                        OfflinePlayModeParameters createParameters = new();
                        createParameters.DecryptionServices = new FileOffsetDecryption();
                        await defaultPackage.InitializeAsync(createParameters).Task;
                        break;
                    }
                case EPlayMode.HostPlayMode:
                    {
                        string defaultHostServer = GetHostServerURL();
                        string fallbackHostServer = GetHostServerURL();
                        HostPlayModeParameters createParameters = new();
                        createParameters.BuildinQueryServices = new GameQueryServices();
                        createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                        createParameters.DecryptionServices = new FileOffsetDecryption();
                        await defaultPackage.InitializeAsync(createParameters).Task;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await this.LoadGlobalConfigAsync();

            return;

            string GetHostServerURL()
            {
                //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
                string hostServerIP = "http://127.0.0.1";
                string appVersion = "v1.0";

#if UNITY_EDITOR
                if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                {
                    return $"{hostServerIP}/CDN/Android/{appVersion}";
                }
                else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
                {
                    return $"{hostServerIP}/CDN/IPhone/{appVersion}";
                }
                else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
                {
                    return $"{hostServerIP}/CDN/WebGL/{appVersion}";
                }

                return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
            if (Application.platform == RuntimePlatform.Android)
            {
                return $"{hostServerIP}/CDN/Android/{appVersion}";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return $"{hostServerIP}/CDN/IPhone/{appVersion}";
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return $"{hostServerIP}/CDN/WebGL/{appVersion}";
            }

            return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
            }

        }

        public void DestroyPackage(string packageName)
        {
            ResourcePackage package = YooAssets.GetPackage(packageName);
            package.UnloadUnusedAssets();
        }

        /// <summary>
        /// 主要用来加载dll config aotdll，因为这时候纤程还没创建，无法使用ResourcesLoaderComponent。
        /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
        /// </summary>
        public T LoadAssetSync<T>(string location) where T : UnityEngine.Object
        {
            AssetHandle handle = YooAssets.LoadAssetSync<T>(location);
            T t = (T)handle.AssetObject;
            handle.Release();
            return t;
        }

        public byte[] LoadRawFile(string location)
        {
            RawFileHandle handle = YooAssets.LoadRawFileSync(location);
            return handle.GetRawFileData();
        }

        public T LoadAsset<T>(string location) where T : UnityEngine.Object
        {
            // self.AssetsOperationHandles.TryGetValue(location, out AssetOperationHandle handle);
            AssetHandle handle;
            // if (handle == null)
            {
                handle = YooAssets.LoadAssetSync<T>(location);
                // self.AssetsOperationHandles[location] = handle;
            }

            return handle.AssetObject as T;
        }

        public async ETTask<byte[]> LoadRawFileAsync(string location)
        {
            RawFileHandle handle = YooAssets.LoadRawFileAsync(location);
            await handle.Task;
            return handle.GetRawFileData();
        }

        /// <summary>
        /// 主要用来加载dll config aotdll，因为这时候纤程还没创建，无法使用ResourcesLoaderComponent。
        /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
        /// </summary>
        public async ETTask<T> LoadAssetAsync<T>(string location) where T : UnityEngine.Object
        {
            AssetHandle handle = YooAssets.LoadAssetAsync<T>(location);
            await handle.Task;
            T t = (T)handle.AssetObject;
            handle.Release();
            return t;
        }

        /// <summary>
        /// 主要用来加载dll config aotdll，因为这时候纤程还没创建，无法使用ResourcesLoaderComponent。
        /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
        /// </summary>
        public async ETTask<Dictionary<string, T>> LoadAllAssetsAsync<T>(string location) where T : UnityEngine.Object
        {
            AllAssetsHandle allAssetsOperationHandle = YooAssets.LoadAllAssetsAsync<T>(location);
            await allAssetsOperationHandle.Task;
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            foreach (UnityEngine.Object assetObj in allAssetsOperationHandle.AllAssetObjects)
            {
                T t = assetObj as T;
                dictionary.Add(t.name, t);
            }
            allAssetsOperationHandle.Release();
            return dictionary;
        }
    }
}