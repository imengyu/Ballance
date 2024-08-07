using UnityEngine;
using Newtonsoft.Json;
using System;

namespace Ballance2.Utils
{
  /// <summary>
  /// 使Json.Net可以正确序列化或反序列化Unity中的Vector数据
  /// </summary>
  public class VectorConverter : JsonConverter
  {
    public override bool CanRead => true;
    public override bool CanWrite => true;
    public override bool CanConvert(Type objectType)
    {
      return typeof(Vector2) == objectType ||
      typeof(Vector2Int) == objectType ||
      typeof(Vector3) == objectType ||
      typeof(Vector3Int) == objectType ||
      typeof(Vector4) == objectType;
    }
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      return objectType switch
      {
        var t when t == typeof(Vector2) => JsonConvert.DeserializeObject<Vector2>(serializer.Deserialize(reader).ToString()),
        var t when t == typeof(Vector2Int) => JsonConvert.DeserializeObject<Vector2Int>(serializer.Deserialize(reader).ToString()),
        var t when t == typeof(Vector3) => JsonConvert.DeserializeObject<Vector3>(serializer.Deserialize(reader).ToString()),
        var t when t == typeof(Vector3Int) => JsonConvert.DeserializeObject<Vector3Int>(serializer.Deserialize(reader).ToString()),
        var t when t == typeof(Vector4) => JsonConvert.DeserializeObject<Vector4>(serializer.Deserialize(reader).ToString()),
        _ => throw new Exception("Unexpected Error Occurred"),
      };
    }
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteStartObject();
      switch (value)
      {
        case Vector2 v:
          writer.WritePropertyName("x");
          writer.WriteValue(v.x);
          writer.WritePropertyName("y");
          writer.WriteValue(v.y);
          break;
        case Vector2Int v:
          writer.WritePropertyName("x");
          writer.WriteValue(v.x);
          writer.WritePropertyName("y");
          writer.WriteValue(v.y);
          break;
        case Vector3 v:
          writer.WritePropertyName("x");
          writer.WriteValue(v.x);
          writer.WritePropertyName("y");
          writer.WriteValue(v.y);
          writer.WritePropertyName("z");
          writer.WriteValue(v.z);
          break;
        case Vector3Int v:
          writer.WritePropertyName("x");
          writer.WriteValue(v.x);
          writer.WritePropertyName("y");
          writer.WriteValue(v.y);
          writer.WritePropertyName("z");
          writer.WriteValue(v.z);
          break;
        case Vector4 v:
          writer.WritePropertyName("x");
          writer.WriteValue(v.x);
          writer.WritePropertyName("y");
          writer.WriteValue(v.y);
          writer.WritePropertyName("z");
          writer.WriteValue(v.z);
          writer.WritePropertyName("w");
          writer.WriteValue(v.w);
          break;
        default:
          throw new Exception("Unexpected Error Occurred");
      }
      writer.WriteEndObject();
    }
  }
  }