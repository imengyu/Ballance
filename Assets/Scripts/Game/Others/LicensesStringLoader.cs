using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2
{
    public class LicensesStringLoader : MonoBehaviour
    {
        [SerializeField]
        public List<string> names = new List<string>();

        void Start()
        {
            LoadStrings();
        }
        void LoadStrings()
        {
            StringBuilder sb = new StringBuilder();
            Text text = GetComponent<Text>();
            foreach(string name in names) {
                var con = Resources.Load<TextAsset>("ThirdPartAllLincnse/" + name + ".LICENSE");
                if(con != null) {

                    sb.AppendLine("");
                    sb.AppendLine("------------------");
                    sb.AppendLine(name);
                    sb.AppendLine("------------------");
                    sb.AppendLine("");

                    sb.AppendLine(con.text);
                }
            }
            text.text = sb.ToString();
        }
    }
}