
using System.Text;

namespace Ballance2.Services.JSService {
  class JSCodePresolve {
    public static string PrePresolveChunkCode(string code, string package, string refPath) {
      StringBuilder sb = new StringBuilder();
      sb.Append("(function() { const require = function(str) {return ballance.internal.SystemRequire('" + package + "/','" + refPath + "/',str);} ");
      sb.AppendLine(code);
      sb.Append("})()");
      return sb.ToString();
    }
  }
}