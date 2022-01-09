
const csharp = require('csharp');

const GameErrorChecker = csharp.Ballance2.Services.Debug.GameErrorChecker;
const GameError = csharp.Ballance2.Services.Debug.GameError;
const GameManager = csharp.Ballance2.Services.GameManager;

/**
 * 加载模块
 * @param {string} entryCode 
 * @param {string} packageName 
 * @returns {boolean}
 * @keep
 */
ballance.internal['SystemLoadPackage'] = function SystemLoadPackage(entryCode, packageName) {

  //引用
  const enteyRet = require(`__${packageName}__/${entryCode}`);
  if(typeof enteyRet == "undefined") {
    GameErrorChecker.SetLastErrorAndLog(GameError.ExecutionFailed, "SystemLoadPackage " + packageName, `EntryCode: ${entryCode} does not return init structure.`);
    return false;
  }

  /**
   * @type {csharp.Ballance2.Services.GamePackageManager}
   */
  const GamePackageManager = GameManager.Instance.GetSystemService("GamePackageManager");

  //加载包
  const package = GamePackageManager.FindRegisteredPackage(packageName);
  if(!package) {
    GameErrorChecker.SetLastErrorAndLog(GameError.ExecutionFailed, "SystemLoadPackage " + packageName, `Not found package: ${packageName}.`);
    return false;
  }
  
  if(typeof enteyRet.OnLoad === 'function')
    package.PackageEntry.OnLoad = enteyRet.OnLoad;
  if(typeof enteyRet.OnBeforeUnLoad === 'function')
    package.PackageEntry.OnBeforeUnLoad = enteyRet.OnBeforeUnLoad;
  if(typeof enteyRet.Version === 'number')
    package.PackageEntry.Version = enteyRet.Version;

  return true;
}