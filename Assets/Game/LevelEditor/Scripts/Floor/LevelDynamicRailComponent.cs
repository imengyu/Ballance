using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  /// <summary>
  /// 线性动态轨道
  /// </summary>
  public class LevelDynamicRailComponent : LevelDynamicFloorBlockComponent
  {
    public LevelDynamicRailComponent()
    {
      CompSize = 0.5f;
      Width = 0.5f;
      IsRail = true;
    }

    //钢轨生成的特殊处理，钢轨需要尽可能节省分段
    protected override void DoGenerateBaseMesh(float xl, float zl, Vector3 posOff, LevelDynamicFloorBlockMaker.PreparedMeshGroupCombineByGrid temp, LevelDynamicFloorBlockMaker.PreparedMeshGroupCombineTraslateProps ptemp)
    {
      switch (Type)
      {
        case LevelDynamicComponentType.Strait:
          //直线只需要一个分段
          temp.top = true;
          temp.bottom = true;
          ptemp.lengthScale = zl;
          ptemp.transformPos = new Vector3(temp.x * CompSize - Width / 2, 0, -posOff.z) + posOff;
          pMesh.CombineMeshAddByGrid(temp, ptemp);
          break;
        case LevelDynamicComponentType.Arc:
          {
            //圆弧的分段由半径决定
            ptemp.lengthScale = Mathf.Max(Mathf.Floor(Mathf.Abs(arcRadius / 4)), 1);
            var scaledLen = CompSize * ptemp.lengthScale;
            zl = Mathf.Floor(arcLength / scaledLen);
            for (temp.z = 0; temp.z < zl; temp.z++)
            {
              temp.left = temp.x == 0;
              temp.top = temp.z == zl - 1;
              temp.right = temp.x == xl - 1;
              temp.bottom = temp.z == 0;
              ptemp.transformPos = new Vector3(temp.x * CompSize - Width / 2, 0, temp.z * scaledLen) + posOff;
              pMesh.CombineMeshAddByGrid(temp, ptemp);
            }
            break;
          }
        default:
          base.DoGenerateBaseMesh(xl, zl, posOff, temp, ptemp);
          break;
      }
    }
  } 
}