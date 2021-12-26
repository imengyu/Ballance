const csharp = require('csharp');

const GamePackageManager = csharp.Ballance2.Services.GamePackageManager;
const GameManager = csharp.Ballance2.Services.GameManager;

console.log('Test!!!');
console.log(GamePackageManager);
console.log(GameManager);
console.log(GameManager.Instance.GetSystemService("GamePackageManager"));
