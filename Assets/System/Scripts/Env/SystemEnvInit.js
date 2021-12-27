const CS = require('csharp');

/**
 * 自动处理require路径，以方便require模块包中的代码。
 * @param {string} package 当前包名
 * @param {string} refPath 当前代码参考路径
 * @param {string} str require名称
 * @returns 
 */
const SystemRequire = function(package, refPath, str) {
  //替换反斜杠
  str = str.replace(/\\/g, "/");

  if(str.indexOf('__/') > 0 )
    return require(str);
  if(str.indexOf('purets/') == 0)
    return require(str);

  //处理相对路径
  if(str.indexOf('./') >= 0)
     return require(`__${package}__/` +  str);
  if(str.indexOf('/') > 0)
    return require(`__${package}__/` + str);
  
  return require(str);
} 

const GamePackageManager = CS.Ballance2.Services.GamePackageManager;
const GameManager = CS.Ballance2.Services.GameManager;

console.log('Test!!!');
console.log(GamePackageManager);
console.log(GameManager);
console.log(GameManager.Instance.GetSystemService("GamePackageManager"));


