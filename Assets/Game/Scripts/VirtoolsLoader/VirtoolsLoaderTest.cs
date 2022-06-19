using Ballance2;
using UnityEngine;

public class VirtoolsLoaderTest : MonoBehaviour
{
  void Start()
  {
		VirtoolsLoader.SetDllDirectoryA(@"E:\Programming\GameProjects\Ballance2\VirtoolsNMOLoader\Debug");
		VirtoolsLoader.LoadLibraryA(@"E:\Programming\GameProjects\Ballance2\VirtoolsNMOLoader\Debug\VirtoolsNMOLoader.dll");
		
		if (VirtoolsLoader.Init(@"E:\Programming\GameProjects\Ballance2\VirtoolsNMOLoader\Debug\CK2.dll")) {
			VirtoolsLoader.LoadNMOToScense(@"E:\Programming\GameProjects\Ballance2\VirtoolsNMOLoader\Debug\1.NMO", MaterialCallback, TextureCallback);
    }
  }

  Texture TextureCallback(string texName)
  {
    return null;
  }
  Material MaterialCallback (string matName) 
  {
  	return null;
  }
  void Update()
  {

  }
}
