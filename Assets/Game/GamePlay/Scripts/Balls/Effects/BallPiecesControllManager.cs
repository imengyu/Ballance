using System.Collections;
using Ballance2.Services;
using Ballance2.Utils;
using UnityEngine;

namespace Ballance2.Game.GamePlay.Balls
{
  /// <summary>
  /// 球碎片抛出和回收器接口
  /// </summary>
  public interface IBallPiecesControllManager
  {
    /// <summary>
    /// 开始抛出碎片
    /// </summary>
    /// <param name="data">碎片数据集</param>
    /// <param name="pos">抛出的位置</param>
    /// <param name="minForce">推动最小力</param>
    /// <param name="maxForce">推动最大力</param>
    /// <param name="timeLive">碎片存活时间（秒）</param>
    void ThrowPieces(BallPiecesData data, Vector3 pos,float minForce, float maxForce, float timeLive);
    /// <summary>
    /// 回收碎片
    /// </summary>
    /// <param name="data">碎片数据集</param>
    void ResetPieces(BallPiecesData data);
  }

  /// <summary>
  /// 默认的球碎片抛出和回收器。提供默认的球碎片抛出和回收效果控制。
  /// </summary>
  public class BallPiecesControllManager : MonoBehaviour, IBallPiecesControllManager
  {
    private const string TAG = "BallPiecesControllManager";

    /// <summary>
    /// 开始抛出碎片
    /// </summary>
    /// <param name="data">碎片数据集</param>
    /// <param name="pos">抛出的位置</param>
    /// <param name="minForce">推动最小力</param>
    /// <param name="maxForce">推动最大力</param>
    /// <param name="timeLive">碎片存活时间（秒）</param>
    public void ThrowPieces(
      BallPiecesData data, 
      Vector3 pos,
      float minForce, 
      float maxForce, 
      float timeLive)
    {
      var FadeManager = GameUIManager.Instance.UIFadeManager;
      var parent = data.parent;

      //去除物理
      foreach (var body in data.bodys) {
        if (body.IsPhysicalized)
          body.UnPhysicalize(true);
      }

      parent.SetActive(true);

      //还原初始状态
      ObjectStateBackupUtils.RestoreObjectAndChilds(parent);
      //设置位置
      data.parent.transform.position = pos;
      data.throwed = true;

      //清除上一次的延时
      if (data.delayHideTimerID > 0)
        GameManager.Instance.DeleteTimer(data.delayHideTimerID);
      if (data.fadeOutTimerID > 0)
        GameManager.Instance.DeleteTimer(data.fadeOutTimerID);

      //渐变未完成，需要强制清除正在运行的渐变
      if (data.fadeObjects.Count > 0) 
      {
        foreach(var value in data.fadeObjects) {
          value.ResetTo(1);
          value.Delete();
        }
        data.fadeObjects.Clear();
      }
      else
      {
        //快速显示
        for (int i = 0; i < parent.transform.childCount; i++)
          FadeManager.AddFadeIn(parent.transform.GetChild(i).gameObject, 0.1f, null);
      }

      //延时消失
      data.delayHideTimerID = GameTimer.Delay(timeLive * 1000, () => {
        data.delayHideTimerID = 0;
        ResetPieces(data);
      });

      //物理化并且抛出碎片
      StartCoroutine(PhysicsAndThrowPeices(data, minForce, maxForce));
    }

    /// <summary>
    /// 回收碎片
    /// </summary>
    /// <param name="data">碎片数据集</param>
    public void ResetPieces(BallPiecesData data)
    {
      var FadeManager = GameUIManager.Instance.UIFadeManager;
      var parent = data.parent;  
      //清除上一次的延时
      if (data.delayHideTimerID > 0)
        GameTimer.DeleteDelay(data.delayHideTimerID);

      if (data.fadeObjects.Count > 0)
        data.fadeObjects.Clear();

      //渐变淡出隐藏其材质
      for (int i = 0; i < parent.transform.childCount; i++)
        data.fadeObjects.Add(FadeManager.AddFadeOut(parent.transform.GetChild(i).gameObject, 3, true, null));

      data.throwed = false;

      if (data.fadeOutTimerID > 0)
        GameTimer.DeleteDelay(data.fadeOutTimerID);

      //延时
      data.fadeOutTimerID = GameTimer.Delay(2990, () => {
        
        if (data.fadeObjects.Count > 0)
          data.fadeObjects.Clear();
  
        //去除物理
        foreach (var body in data.bodys) 
          body.UnPhysicalize(true);

        parent.SetActive(false); //隐藏
      });
    }

    //物理化并且抛出碎片
    private IEnumerator PhysicsAndThrowPeices(BallPiecesData data, float minForce, float maxForce) {
      var count = 0;
      foreach (var body in data.bodys)
      {
        //防止一下抛出太多碎片
        count++;
        if (count > 3) 
        {
          yield return new WaitForSeconds(0.01f);
          count = 0;
        }
        body.Physicalize(); //物理
      }
      foreach (var body in data.bodys)
      {
        count++;
        if (count > 3) 
        {
          yield return new WaitForSeconds(0.01f);
          count = 0;
        }

        var forceDir = body.transform.localPosition;
        body.gameObject.SetActive(true);
        forceDir.y += 2;
        forceDir.Normalize();
        body.Impluse(forceDir * CommonUtils.RandomFloat(minForce, maxForce)); //施加力
      }
    }
  }
}