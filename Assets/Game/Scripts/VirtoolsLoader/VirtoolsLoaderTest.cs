using Ballance2;
using Ballance2.Package;
using UnityEngine;

public class VirtoolsLoaderTest : MonoBehaviour
{
  void Start()
  {
    if (VirtoolsLoader.Init(@"D:\Code\GitHub\Ballance\VirtoolsNMOLoader\Debug\CK2.dll"))
      VirtoolsLoader.LoadNMOToScense(@"D:\Code\GitHub\Ballance\VirtoolsNMOLoader\Debug\Level_01.NMO", GamePackage.GetCorePackage());
  }
  void Update()
  {

  }
}
