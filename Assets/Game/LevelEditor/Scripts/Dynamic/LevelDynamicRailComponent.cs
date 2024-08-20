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
          ptemp.transformPos = new Vector3(temp.x * CompSize - Width / 2 + posOff.x, 0, 0);
          pMesh.CombineMeshAddByGrid(temp, ptemp);
          break;
        case LevelDynamicComponentType.Arc:
        case LevelDynamicComponentType.Bizer:
        case LevelDynamicComponentType.Spiral:
          {
            var scaledLen = 0.0f;
            switch (Type)
            {
              case LevelDynamicComponentType.Arc:
                //圆弧的分段由半径决定
                ptemp.lengthScale = Mathf.Max(Mathf.Floor(Mathf.Abs(arcRadius / 4)), 1);
                scaledLen = CompSize * ptemp.lengthScale;
                zl = Mathf.Floor(arcLength / scaledLen);
                break;
              case LevelDynamicComponentType.Bizer:
                //贝塞尔的分段由长度决定
                ptemp.lengthScale = Mathf.Max(Mathf.Floor(Mathf.Abs(bizerLength / 10)), 1);
                scaledLen = CompSize * ptemp.lengthScale;
                zl = Mathf.Floor(bizerLength / scaledLen);
                break;
              case LevelDynamicComponentType.Spiral:
                //圆弧的分段由半径决定
                ptemp.lengthScale = Mathf.Max(Mathf.Floor(Mathf.Abs(spiralRadius / 3)), 1);
                scaledLen = CompSize * ptemp.lengthScale;
                zl = Mathf.Floor(spiralLength / scaledLen);
                break;
            }
            for (temp.z = 0; temp.z < zl; temp.z++)
            {
              temp.left = temp.x == 0;
              temp.top = temp.z == zl - 1;
              temp.right = temp.x == xl - 1;
              temp.bottom = temp.z == 0;
              ptemp.transformPos = new Vector3(temp.x * CompSize - Width / 2 + posOff.x, 0, temp.z * scaledLen);
              pMesh.CombineMeshAddByGrid(temp, ptemp);
            }
            break;
          }
        default:
          posOff.z = 0;
          base.DoGenerateBaseMesh(xl, zl, posOff, temp, ptemp);
          break;
      }
    }
  } 
}