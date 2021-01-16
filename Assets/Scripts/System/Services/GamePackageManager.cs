using Ballance2.System.Bridge;
using Ballance2.System.Debug;
using Ballance2.System.Package;
using Ballance2.System.Res;
using Ballance2.Utils;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GamePackageManager.cs
* 
* 用途：
* 框架包管理器，用于管理每个功能包的加载与释放
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-17 创建
*
*/

namespace Ballance2.System.Services
{
    /// <summary>
    /// 框架包管理器
    /// </summary>
    [SLua.CustomLuaClass]
    public class GamePackageManager : GameService
    {
        private static readonly string TAG = "GamePackageManager";

        public GamePackageManager() : base(TAG) {}

        public override void Destroy()
        {
            UnLoadAllPackages();

            registeredPackages.Clear();
            packagesLoadStatus.Clear();
            loadedPackages.Clear();
            registeredPackages = null;
            packagesLoadStatus = null;
            loadedPackages = null;

            GameManager.GameMediator.UnRegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_LOAD_FAILED);
            GameManager.GameMediator.UnRegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_LOAD_SUCCESS);
            GameManager.GameMediator.UnRegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_REGISTERED);
            GameManager.GameMediator.UnRegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_UNLOAD);
        }
        public override bool Initialize()
        {
            GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_LOAD_FAILED);
            GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_LOAD_SUCCESS);
            GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_REGISTERED);
            GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_UNLOAD);
            return true;
        }

        private const int PACKAGE_DEFAULT = 0;
        private const int PACKAGE_REGISTING = 1;
        private const int PACKAGE_LOADING = 2;
        private const int PACKAGE_LOAD_SUCCESS = 3;
        private const int PACKAGE_LOAD_FAILED = 4;
        private const int PACKAGE_UNLOAD_WAITING = 5;

        private Dictionary<string, int> packagesLoadStatus = new Dictionary<string, int>();

        private Dictionary<string, GamePackage> registeredPackages = new Dictionary<string, GamePackage>();
        private Dictionary<string, GamePackage> loadedPackages = new Dictionary<string, GamePackage>();

        /// <summary>
        /// 注册包
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <param name="load">是否立即加载</param>
        /// <returns>返回是否加载成功。要获得错误代码，请获取 <see cref="GameErrorChecker.LastError"/></returns>
        public async Task<bool> RegisterPackage(string packageName)
        {
            if (registeredPackages.ContainsKey(packageName))
            {
                Log.W(TAG, "Package {0} already registered!", packageName);
                return true;
            }

            //路径转换
            string realPackagePath = GamePathManager.GetResRealPath("package", packageName + ".ballance");
            string realPackagePathInCore = GamePathManager.GetResRealPath("core", packageName + ".ballance");
            if (File.Exists(realPackagePathInCore)) realPackagePath = realPackagePathInCore;
            else if (!File.Exists(realPackagePath))
            {
                Log.E(TAG, "Package {0} register failed because file {1} not found", packageName, realPackagePath);
                return false;
            }

            //设置正在加载
            packagesLoadStatus.Add(packageName, PACKAGE_REGISTING);

            GamePackage gamePackage;
            //判断文件类型
            if (FileUtils.TestFileIsZip(realPackagePath))
                gamePackage = new GameZipPackage();
            else if (FileUtils.TestFileIsAssetBundle(realPackagePath))
                gamePackage = new GameAssetBundlePackage();
            else
            {
                //文件格式不支持
                GameErrorChecker.LastError = GameError.NotSupportFileType;
                Log.E(TAG, "Package file type not support {0}", realPackagePath);
                return false;
            }

            //加载信息
            if(await gamePackage.LoadInfo(realPackagePath))
            {

                packagesLoadStatus.Remove(packageName);
                registeredPackages.Add(packageName, gamePackage);

                //通知事件
                GameManager.GameMediator.DispatchGlobalEvent(
                    GameEventNames.EVENT_PACKAGE_REGISTERED, "*", packageName);
                return true;
            }

            packagesLoadStatus.Remove(packageName);
            return false;
        }
        /// <summary>
        /// 查找已注册的模块
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <returns></returns>
        public GamePackage FindRegisteredPackage(string packageName)
        {
            registeredPackages.TryGetValue(packageName, out var outPackage);
            return outPackage;
        }
        /// <summary>
        /// 取消注册模块
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <param name="unLoadImmediately">是否立即卸载</param>
        /// <returns></returns>
        public bool UnRegisterPackage(string packageName, bool unLoadImmediately)
        {
            bool success = false;
            if (registeredPackages.ContainsKey(packageName))
            {
                registeredPackages.Remove(packageName);
                success = true;
            }
            if (packagesLoadStatus.ContainsKey(packageName))
                packagesLoadStatus.Remove(packageName);

            if (IsPackageLoaded(packageName))
                UnLoadPackage(packageName, unLoadImmediately);

            return success;
        }

        /// <summary>
        /// 获取包是否正在加载
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <returns></returns>
        public bool IsPackageLoading(string packageName)
        {
            return packagesLoadStatus.ContainsKey(packageName) && packagesLoadStatus[packageName] == PACKAGE_LOADING;
        }        
        /// <summary>
        /// 获取包是否正在注册
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <returns></returns>
        public bool IsPackageRegistering(string packageName)
        {
            return packagesLoadStatus.ContainsKey(packageName) && packagesLoadStatus[packageName] == PACKAGE_REGISTING;
        }
        /// <summary>
        /// 获取包是否已加载
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <returns></returns>
        public bool IsPackageLoaded(string packageName)
        {
            return loadedPackages.ContainsKey(packageName); 
        }

        /// <summary>
        /// 加载模块
        /// </summary>
        /// <param name="packageName">模块包名</param>
        /// <returns>返回加载是否成功</returns>
        public async Task<bool> LoadPackage(string packageName)
        {
            if(!StringUtils.IsPackageName(packageName))
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.InvalidPackageName, TAG, "Invalid packageName {0}", packageName);
                return false;
            }
            if (IsPackageLoaded(packageName))
            {
                Log.W(TAG, "Package {0} already loaded!", packageName);
                return true;
            }
            if (IsPackageLoading(packageName))
            {
                Log.W(TAG, "Package {0} is loading!", packageName);
                return false;
            }

            //注册包
            GamePackage package = FindRegisteredPackage(packageName);
            if (package == null)
            {
                if(!await RegisterPackage(packageName))
                {
                    string err = string.Format("Package {0} could not load, because RegisterPackage failed",
                        packageName);

                    Log.E(TAG, err);

                    //通知事件
                    GameManager.GameMediator.DispatchGlobalEvent(
                        GameEventNames.EVENT_PACKAGE_LOAD_FAILED, "*", packageName, err);
                    return false;
                }

                package = FindRegisteredPackage(packageName);
            }

            if (packagesLoadStatus.ContainsKey(packageName)) packagesLoadStatus[packageName] = PACKAGE_LOADING;
            else packagesLoadStatus.Add(packageName, PACKAGE_LOADING);

            //加载依赖
            List<GamePackageDependencies> dependencies = package.BaseInfo.Dependencies;
            if (dependencies.Count > 0)
            {
                GamePackageDependencies dependency;
                GamePackage dependencyPackage;

                //加载依赖
                for (int i = 0; i < dependencies.Count; i++)
                {
                    dependency = dependencies[i];
                    if (!IsPackageLoaded(dependency.Name))
                    {
                        bool loadSuccess = await LoadPackage(dependency.Name);
                        if(!loadSuccess)
                        {
                            string err = string.Format("Package {0} load failed because a dependency {1}/{2} " +
                               "load failed",
                               packageName, dependency.Name, dependency.MinVersion);

                            Log.E(TAG, err);
                            //通知事件
                            GameManager.GameMediator.DispatchGlobalEvent(
                                GameEventNames.EVENT_PACKAGE_LOAD_FAILED, "*", packageName, err);

                            packagesLoadStatus[packageName] = PACKAGE_LOAD_FAILED;
                            return false;
                        }
                    }
                }
                //检查依赖版本
                for (int i = 0; i < dependencies.Count; i++)
                { 
                    dependency = dependencies[i];
                    
                    if (IsPackageLoaded(dependency.Name)) {
                        dependencyPackage = loadedPackages[dependency.Name];
                        if (dependencyPackage.PackageVersion < dependency.MinVersion)
                        {
                            string err = string.Format("Package {0} load failed because dependency {1} {2} " +
                                "less than required version {3}",
                                packageName, dependency.Name, dependencyPackage.PackageVersion,
                                dependency.MinVersion);

                            Log.E(TAG, err);
                            //通知事件
                            GameManager.GameMediator.DispatchGlobalEvent(
                                GameEventNames.EVENT_PACKAGE_LOAD_FAILED, "*", packageName, err);

                            packagesLoadStatus[packageName] = PACKAGE_LOAD_FAILED;
                            return false;
                        }
                        //添加依赖计数
                        dependencyPackage.DependencyRefCount++;
                    }
                }
            }

            //加载
            if(!await package.LoadPackage())
            {
                packagesLoadStatus[packageName] = PACKAGE_LOAD_FAILED;

                string err = string.Format("Package {0} load failed {1}",
                        packageName, GameErrorChecker.GetLastErrorMessage());

                Log.E(TAG, err);
                //通知事件
                GameManager.GameMediator.DispatchGlobalEvent(
                    GameEventNames.EVENT_PACKAGE_LOAD_FAILED, "*", packageName, err);
                return false;
            }

            packagesLoadStatus[packageName] = PACKAGE_LOAD_SUCCESS;
            loadedPackages.Add(packageName, package);

            //通知事件
            GameManager.GameMediator.DispatchGlobalEvent(
                GameEventNames.EVENT_PACKAGE_LOAD_SUCCESS, "*", packageName, package);

            return true;
        }
        /// <summary>
        /// 卸载模块
        /// </summary>
        /// <param name="packageName">模块包名</param>
        /// <param name="unLoadImmediately">
        /// 是否立即卸载，如果为false，此模块
        /// 将等待至依赖它的模块全部卸载之后才会卸载
        /// </param>
        /// <returns>返回是否成功</returns>
        public bool UnLoadPackage(string packageName, bool unLoadImmediately)
        {
            GamePackage package = FindPackage(packageName);
            if (package == null)
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.NotLoad, TAG,
                    "Can not unload package " + packageName + " because it isn't load! ");
                return false;
            }
            if (package.UnLoadWhenDependencyRefNone)
                return true;

            //如果不是立即卸载并且依赖引用计数大于0
            if (!unLoadImmediately && package.DependencyRefCount > 0)
            {
                package.UnLoadWhenDependencyRefNone = true;
                return true;
            }

            //依赖计数处理，在这里当前模块卸载了，所以要将他们的依赖计数减一
            List<GamePackageDependencies> dependencies = package.BaseInfo.Dependencies;
            GamePackage dependencyPackage;
            for (int i = 0; i < dependencies.Count; i++)
            {
                if (IsPackageLoaded(dependencies[i].Name))
                {
                    dependencyPackage = loadedPackages[dependencies[i].Name];
                    dependencyPackage.DependencyRefCount--;
                    if (dependencyPackage.DependencyRefCount <= 0
                        && dependencyPackage.UnLoadWhenDependencyRefNone)
                        UnLoadPackage(dependencyPackage.PackageName, true);
                }
            }

            //卸载

            //通知事件
            GameManager.GameMediator.DispatchGlobalEvent(
                GameEventNames.EVENT_PACKAGE_UNLOAD, "*", packageName, package);

            package.Destroy();
            packagesLoadStatus.Remove(packageName);
            loadedPackages.Remove(packageName);

            return true;
        }
        /// <summary>
        /// 查找已加载的模块
        /// </summary>
        /// <param name="packageName">模块包名</param>
        /// <returns></returns>
        public GamePackage FindPackage(string packageName)
        {
            loadedPackages.TryGetValue(packageName, out var outPackage);
            return outPackage;
        }

        private void UnLoadAllPackages()
        {
            foreach(string key in loadedPackages.Keys)
                UnLoadPackage(key, true);
        }
    }
}
