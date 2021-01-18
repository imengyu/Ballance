using System;
using System.Collections.Generic;

namespace Ballance2.Utils
{
    /*
     * 字符串分割器
     */

    [SLua.CustomLuaClass]
    /// <summary>
    /// 字符串分割器
    /// </summary>
    public class StringSpliter
    {
        /// <summary>
        /// 创建字符串分割器
        /// </summary>
        /// <param name="s">要分隔的字符串</param>
        /// <param name="sp">分隔依据</param>
        public StringSpliter(string s, char sp)
        {
            if (s.Contains(sp.ToString()))
            {
                buf = s.Split(sp);
                count = buf.Length;
            }
            else
            {
                buf = new string[] { s };
                count = 1;
            }
        }
        /// <summary>
        /// 创建字符串分割器
        /// </summary>
        /// <param name="s">要分隔的字符串</param>
        /// <param name="sp">分隔依据</param>
        /// <param name="excludeQuotes">是否忽略英文引号（"）</param>
        public StringSpliter(string s, char sp, bool excludeQuotes)
        {
            if (s.Contains(sp.ToString()))
            {
                if (excludeQuotes)
                {
                    string[] bufbuf = s.Split(new char[] { sp }, StringSplitOptions.RemoveEmptyEntries);
                    count = bufbuf.Length;
                    if (count > 0)
                    {
                        bool excludeing = false;
                        string excludeingCache = null;
                        List<string> cache = new List<string>();
                        for (int i = 0; i < count; i++)
                        {
                            if (bufbuf[i].StartsWith("\"") && !excludeing)
                            {
                                excludeing = true;
                                excludeingCache = bufbuf[i];
                            }
                            else if (bufbuf[i].EndsWith("\""))
                            {
                                if (excludeing)
                                {
                                    excludeingCache += " " + bufbuf[i];
                                    cache.Insert(cache.Count, excludeingCache);
                                    excludeingCache = null;
                                    excludeing = false;
                                }
                                else cache.Insert(cache.Count, bufbuf[i]);
                            }
                            else
                            {
                                if (excludeing) excludeingCache += " " + bufbuf[i];
                                else cache.Insert(cache.Count, bufbuf[i]);
                            }
                        }

                        buf = cache.ToArray();
                        cache.Clear();
                        cache = null;
                        bufbuf = null;
                    }
                }
                else
                {
                    buf = s.Split(sp);
                    count = buf.Length;
                }
            }
            else
            {
                buf = new string[] { s };
                count = 1;
            }
        }
        /// <summary>
        /// 创建字符串分割器
        /// </summary>
        /// <param name="s">要分隔的字符串</param>
        /// <param name="sp">分隔依据</param>
        /// <param name="sp2">分隔依据2</param>
        public StringSpliter(string s, char sp, char sp2)
        {
            if (s.Contains(sp.ToString()))
            {
                buf = s.Split(new char[] { sp, sp2 }, StringSplitOptions.RemoveEmptyEntries);
                count = buf.Length;
            }
            else
            {
                buf = new string[] { s };
                count = 1;
            }
        }

        private string[] buf = null;
        private int count = 0;

        /// <summary>
        /// 分割是否成功
        /// </summary>
        public bool Success { get { return count != 0; } }
        /// <summary>
        /// 分割结果个数
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }
        /// <summary>
        /// 分割结果
        /// </summary>
        public string[] Result { get { return buf; } }

    }

}