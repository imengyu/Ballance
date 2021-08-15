using System.Collections;
using System.Collections.Generic;
using Ballance2.Sys.UI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.Small
{
    public class ScrollCaption : MonoBehaviour {

        public Text TextHeader;
        public Text TextContent;
        public UIFadeManager FadeManager;
        public GameObject PanelImages;
        
        [SerializeField]
        [TextArea(0, 5)]
        public List<string> Titles = new List<string>();
        [SerializeField]
        [TextArea(0, 10)]
        public List<string> Contents = new List<string>();

        public float PageShowTime = 5.0f;

        public int currIndex = 0;
        public int currTick = 0;
        public bool currFadeing = true;
        public Image nextShowImage = null;
        public Image currentShowImage = null;

        private void Start() {
            GetNext(true);
            for (int i = 0; i < PanelImages.transform.childCount; i++)
                PanelImages.transform.GetChild(i).gameObject.SetActive(false);
        }

        private void GetNext(bool start = false) {
            currFadeing = true;

            var isLast = false; 

            if(start)  currIndex = 0;
            else {
                if(currIndex < Titles.Count - 1) currIndex++;
                else { currIndex = 0; isLast = true;  }
            }

            var title = Titles[currIndex];
            var content = currIndex < Contents.Count ? Contents[currIndex] : "";
            if(title.StartsWith("-"))
                nextShowImage = PanelImages.transform.Find("Image" + title.Substring(1)).GetComponent<Image>();

            currTick = (int)((PageShowTime * (content.Length / 100 + 1)) * 60);
            StartCoroutine(HideAndShow(title, content, start, isLast));
        }

        private IEnumerator HideAndShow(string title, string content, bool noHide, bool isLast) {
            currFadeing = true;
            if(!noHide) {
                if(currentShowImage != null) {
                    FadeManager.AddFadeOut(currentShowImage, 0.5f, false);
                    currentShowImage = null;
                } else {
                    FadeManager.AddFadeOut(TextHeader, 0.5f, false);
                    FadeManager.AddFadeOut(TextContent, 0.5f, false);
                }
                yield return new WaitForSeconds(0.5f);
            } else {
                if(currentShowImage != null) {
                    currentShowImage.gameObject.SetActive(false);
                    currentShowImage = null;
                }
            }

            if(isLast)
                yield return new WaitForSeconds(1.5f);

            if(nextShowImage != null) {
                FadeManager.AddFadeIn(nextShowImage, 0.5f);
                currentShowImage = nextShowImage;
                nextShowImage = null;
            }
            else {
                TextHeader.text = title;
                TextContent.text = content;
                yield return new WaitForSeconds(0.2f);
                TextContent.rectTransform.sizeDelta = new Vector2(TextContent.rectTransform.sizeDelta.x, TextContent.preferredHeight);
                FadeManager.AddFadeIn(TextHeader, 0.5f);
                FadeManager.AddFadeIn(TextContent, 0.5f);
            }
            yield return new WaitForSeconds(0.5f);
            currFadeing = false;
        }

        private void Update() {
            if(!currFadeing) {
                if(currTick > 0) 
                    currTick--;
                else
                    GetNext();
            }
        } 

        private void OnEnable() {
            GetNext(true);
        }
    }
}



