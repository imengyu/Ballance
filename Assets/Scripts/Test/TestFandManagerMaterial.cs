using System.Collections;
using System.Collections.Generic;
using Ballance2.Sys;
using Ballance2.Sys.Services;
using UnityEngine;

namespace Ballance2
{
    public class TestFandManagerMaterial : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Wait());
        }
        IEnumerator Wait() {
            yield return new WaitForSeconds(3);
            GameManager.Instance.GetSystemService<GameUIManager>().UIFadeManager.AddFadeOut(gameObject, 4, false, null);
        }
    }
}
