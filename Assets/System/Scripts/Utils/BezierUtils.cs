using UnityEngine;

namespace Ballance2.Utils
{
  public static class BezierUtils
  {
    /// <summary>
    /// 获取某向量的垂直向量
    /// </summary>
    public static Vector3 GetVerticalDir(Vector3 _dir)
    {
      //（_dir.x,_dir.z）与（？，1）垂直，则_dir.x * ？ + _dir.z * 1 = 0
      if (_dir.x == 0)
      {
        return new Vector3(1, 0, 0);
      }
      else
      {
        return new Vector3(-_dir.z / _dir.x, 0, 1).normalized;
      }
    }
    /// <summary>
    /// 一阶贝塞尔曲线
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 Bezier(Vector3 p0, Vector3 p1, float t)
    {
      return (1 - t) * p0 + t * p1;
    }
    /// <summary>
    /// 二阶贝塞尔曲线
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
      Vector3 p0p1 = (1 - t) * p0 + t * p1;
      Vector3 p1p2 = (1 - t) * p1 + t * p2;
      Vector3 temp = (1 - t) * p0p1 + t * p1p2;
      return temp;
    }
    /// <summary>
    /// 二阶贝塞尔曲线近似长度
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="pointCount"></param>
    /// <returns></returns>
    public static float BezierLength(Vector3 p0, Vector3 p1, Vector3 p2, int pointCount = 30)
    {
      if (pointCount < 2)
        return 0;
      //取点 默认 30个
      float length = 0.0f;
      Vector3 lastPoint = Bezier(p0, p1, p2, 0.0f / (float)pointCount);
      for (int i = 1; i <= pointCount; i++)
      {
        Vector3 point = Bezier(p0, p1, p2, (float)i / (float)pointCount);
        length += Vector3.Distance(point, lastPoint);
        lastPoint = point;
      }
      return length;
    }
    /// <summary>
    /// 三阶贝塞尔曲线
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
      Vector3 aa = a + (b - a) * t;
      Vector3 bb = b + (c - b) * t;
      Vector3 cc = c + (d - c) * t;

      Vector3 aaa = aa + (bb - aa) * t;
      Vector3 bbb = bb + (cc - bb) * t;
      return aaa + (bbb - aaa) * t;
    }
    /// <summary>
    /// 三阶贝塞尔曲线斜率
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 BezierTangent(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
      float u = 1 - t;
      float uu = u * u;
      float tu = t * u;
      float tt = t * t;

      Vector3 P = p0 * 3 * uu * (-1.0f);
      P += p1 * 3 * (uu - 2 * tu);
      P += p2 * 3 * (2 * tu - tt);
      P += p3 * 3 * tt;

      //返回单位向量
      return P.normalized;
    }
    /// <summary>
    /// 三阶贝塞尔曲线近似长度
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="pointCount"></param>
    /// <returns></returns>
    public static float BezierLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int pointCount = 30)
    {
      if (pointCount < 2)
        return 0;
      //取点 默认 30个
      float length = 0.0f;
      Vector3 lastPoint = Bezier(p0, p1, p2, p3, 0.0f / (float)pointCount);
      for (int i = 1; i <= pointCount; i++)
      {
        Vector3 point = Bezier(p0, p1, p2, p3, (float)i / (float)pointCount);
        length += Vector3.Distance(point, lastPoint);
        lastPoint = point;
      }
      return length;
    }
  }
}