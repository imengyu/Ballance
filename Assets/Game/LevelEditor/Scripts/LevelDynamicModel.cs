using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicModel
  {
    public LevelDynamicModelSource SourceType;
    public string SourcePath;
    public string SourceEmbed;

  }
  public enum LevelDynamicModelSource
  {
    Game,
    Package,
    Embed,
  }
}
