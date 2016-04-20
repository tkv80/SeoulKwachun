using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using log4net;

namespace SeoulKwachun.Manager
{
    internal static class HttpManager
    {
        private const string ClassCode = "T502";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        ///     스텝1
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string GetRsvReq(string cookie, string svcId)
        {
            try
            {
                var httpWRequest =
                    (HttpWebRequest)
                        WebRequest.Create("http://yeyak.seoul.go.kr/reservation/getRsvReqKey.web?svcid=" + svcId);
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";

                httpWRequest.Method = "Get";
                httpWRequest.CookieContainer = new CookieContainer();

                SetCookie(cookie, httpWRequest);
                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream(), Encoding.GetEncoding("euc-kr"));

                string resultHtml = sr.ReadToEnd();

                Log.DebugFormat("{0} - {1}", MethodBase.GetCurrentMethod().Name, resultHtml);
                return resultHtml.Substring("rsvReqKey\":\"", "}").Replace("\"", "");
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
                return null;
            }
        }

        public static void Step0(string cookie, string svcId, string rsvReqKey)
        {
            try
            {
                var httpWRequest =
                    (HttpWebRequest)
                        WebRequest.Create(
                            string.Format("http://yeyak.seoul.go.kr/request.web?l=1&svcId={0}&rsvreqkey={1}", svcId,
                                rsvReqKey));
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";

                httpWRequest.Method = "Get";
                httpWRequest.CookieContainer = new CookieContainer();

                SetCookie(cookie, httpWRequest);
                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                string resultHtml = sr.ReadToEnd();
                Log.DebugFormat("{0} - {1}", MethodBase.GetCurrentMethod().Name, resultHtml);
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
            }
        }

        /// <summary>
        ///     스텝
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetLocation()
        {
            try
            {
                var httpWRequest =
                    (HttpWebRequest)
                        WebRequest.Create(
                            "http://yeyak.seoul.go.kr/reservation/quickrsv/getQuickRsvCodeListXml.web?upCode=SE00&type=class&step=3&areaCode=SE00&classCode=T502");
                httpWRequest.Method = "get";

                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                string resultHtml = sr.ReadToEnd();

                int startIndex = 0;
                string startText = "<code>";
                string endText = "</codenm>";

                var sites = new List<string>();
                while (true)
                {
                    try
                    {
                        startIndex = resultHtml.IndexOf(startText, startIndex + 1);
                        if (startIndex == -1)
                        {
                            break;
                        }
                        string resultText =
                            resultHtml.Substring(startIndex + startText.Length,
                                resultHtml.IndexOf(endText, startIndex) - startIndex - startText.Length)
                                .Trim();

                        if (resultText.Contains("</code><codenm>"))
                        {
                            resultText = resultText.Replace("</code><codenm>", "|");
                            sites.Add(resultText);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }


                return sites;
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
                return null;
            }
        }

        /// <summary>
        ///     스텝
        /// </summary>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetSchedule(string areaCode)
        {
            try
            {
                var httpWRequest =
                    (HttpWebRequest)
                        WebRequest.Create(
                            string.Format(
                                "http://yeyak.seoul.go.kr/reservation/quickrsv/getQuickRsvSvcListXml.web?classCode={0}&areaCode={1}",
                                ClassCode, areaCode))
                    ;
                httpWRequest.Method = "get";
                httpWRequest.CookieContainer = new CookieContainer();

                //SetCookie(cookie, httpWRequest);

                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                string resultHtml = sr.ReadToEnd();

                int startIndex = 0;
                string startText = "<svcid>";
                string endText = "</svcnm>";

                var sites = new List<string>();
                while (true)
                {
                    try
                    {
                        startIndex = resultHtml.IndexOf(startText, startIndex + 1);
                        if (startIndex == -1)
                        {
                            break;
                        }
                        string resultText =
                            resultHtml.Substring(startIndex + startText.Length,
                                resultHtml.IndexOf(endText, startIndex) - startIndex - startText.Length)
                                .Trim();

                        if (resultText.Contains("</svcid><svcnm>"))
                        {
                            resultText = resultText.Replace("</svcid><svcnm>", "|");
                            sites.Add(resultText);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }


                return sites;
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
                return null;
            }
        }

        /// <summary>
        ///     스텝
        /// </summary>
        /// <param name="month"></param>
        /// <param name="svsvcid"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetFreeSite(string year, string month, string svsvcid)
        {
            try
            {
                string parameter =
                    string.Format(
                        "year={0}&month={1}&rsvsvcid={2}&waitnum=0&usetimeunitcode=B403&usedaystdrcptday=&usedaystdrcpttime=&rsvdaystdrcptday=1&rsvdaystdrcpttime=24",
                        year, month, svsvcid);

                var httpWRequest =
                    (HttpWebRequest) WebRequest.Create("http://yeyak.seoul.go.kr/reservation/include/schedule.web");
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                httpWRequest.ContentType = "application/x-www-form-urlencoded";
                httpWRequest.KeepAlive = true;
                httpWRequest.Method = "Post";
                httpWRequest.ContentLength = parameter.Length;


                var sw = new StreamWriter(httpWRequest.GetRequestStream());
                sw.Write(parameter);
                sw.Close();

                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream(), Encoding.GetEncoding("euc-kr"));

                string resultHtml = sr.ReadToEnd();


                int startIndex = 0;
                string startText = "<usedate>";
                string endText = "</state>";

                var sites = new List<string>();
                while (true)
                {
                    try
                    {
                        startIndex = resultHtml.IndexOf(startText, startIndex + 1);
                        if (startIndex == -1)
                        {
                            break;
                        }
                        string resultText =
                            resultHtml.Substring(startIndex + startText.Length,
                                resultHtml.IndexOf(endText, startIndex) - startIndex - startText.Length)
                                .Trim();

                        if (resultText.Contains("</usedate><state>A"))
                        {
                            resultText = resultText.Replace("</usedate><state>A", "");
                            sites.Add(resultText);
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
                    }
                }

                return sites;
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="httpWRequest"></param>
        private static void SetCookie(string cookie, HttpWebRequest httpWRequest)
        {
            string[] cookieSplit = cookie.Split(';');
            httpWRequest.CookieContainer = new CookieContainer();

            foreach (string s in cookieSplit)
            {
                try
                {
                    if (s.Split('=').Length == 2 && !s.ToUpper().Contains("PATH"))
                    {
                        string key = s.Substring(0, s.IndexOf("="));
                        string value = s.Substring(s.IndexOf("=") + 1, s.Length - key.Length - 1).Trim();

                        httpWRequest.CookieContainer.Add(new Cookie(key.Trim(), value.Trim(), "/", httpWRequest.Host));
                    }
                }
                catch (Exception ex)
                {
                    Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
                }
            }

            //ttpWRequest.CookieContainer.Add(new Cookie("clickData",
            //    "%5B%7B%22svcid%22%3A%22S140428132601366234%22%2C%22img%22%3A%22/fileDownload.web%3Fp%3D/TB_SVCIMG/2014/04/28/S140428132601366234%26m%3Di%26n%3D8bKY4WP8eys8MxO5M511FedXbECE5c_200108%26on%3D%uCEA0%uD551%uC7A5%201.jpg%22%2C%22title%22%3A%22%uC11C%uC6B8%uB300%uACF5%uC6D0%20%uCEA0%uD551%uC7A5%206%7E7%uC6D4%28%uC8FC%uC911%29%20%uC57C%uC601%uC608%uC57D%22%7D%5D",
            //    "/", httpWRequest.Host));
        }

        /// <summary>
        ///     스텝1
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static void Step1_1(string cookie)
        {
            try
            {
                var httpWRequest =
                    (HttpWebRequest)
                        WebRequest.Create(
                            "https://yeyak.seoul.go.kr/request.web?execution=e1s1&_eventId_nextState&agreeYn=Y");
                httpWRequest.Accept = "text/html, application/xhtml+xml, */*";
                httpWRequest.Headers.Add("Accept-Language", "ko-KR");
                httpWRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                httpWRequest.Headers.Add("DNT", "1");
                httpWRequest.KeepAlive = true;

                httpWRequest.Method = "Get";
                httpWRequest.CookieContainer = new CookieContainer();

                SetCookie(cookie, httpWRequest);
                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                string resultHtml = sr.ReadToEnd();
                Log.DebugFormat("{0} - {1}", MethodBase.GetCurrentMethod().Name, resultHtml);
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
            }
        }

        /// <summary>
        ///     스텝2
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static void Step2(string cookie)
        {
            try
            {
                var httpWRequest =
                    (HttpWebRequest) WebRequest.Create("http://yeyak.seoul.go.kr/request.web?execution=e1s2");
                httpWRequest.Accept = "text/html, application/xhtml+xml, */*";
                httpWRequest.Headers.Add("Accept-Language", "ko-KR");
                httpWRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWRequest.Referer = "http://yeyak.seoul.go.kr/request.web?execution=e1s1";
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                httpWRequest.Headers.Add("DNT", "1");
                httpWRequest.KeepAlive = true;
                httpWRequest.Method = "Get";
                httpWRequest.CookieContainer = new CookieContainer();

                SetCookie(cookie, httpWRequest);
                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                string resultHtml = sr.ReadToEnd();
                Log.DebugFormat("{0} - {1}", MethodBase.GetCurrentMethod().Name, resultHtml);
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
            }
        }

        /// <summary>
        ///     스텝
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="svcId"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string Step2_1(string cookie, string svcId, string datetime)
        {
            try
            {
                string parameter =
                    string.Format(
                        "day={1}&rsvsvcid={0}&waitnum=0&rsvtmtypecode=TM01&usetimeunitcode=B403&usedaystdrcptday=&usedaystdrcpttime=&rsvdaystdrcptday=1&rsvdaystdrcpttime=24&act=I&mode=C&ispreview=",
                        svcId, datetime);

                var httpWRequest =
                    (HttpWebRequest) WebRequest.Create("http://yeyak.seoul.go.kr/reservation/include/rsvtm.web");
                httpWRequest.Accept = "text/html, application/xhtml+xml, */*";
                httpWRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWRequest.Headers.Add("Accept-Language", "ko-KR");
                httpWRequest.Referer = "http://yeyak.seoul.go.kr/request.web?execution=e1s2";
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                httpWRequest.Headers.Add("DNT", "1");
                httpWRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                httpWRequest.KeepAlive = true;
                httpWRequest.Method = "Post";
                httpWRequest.ContentLength = parameter.Length;
                httpWRequest.CookieContainer = new CookieContainer();

                SetCookie(cookie, httpWRequest);

                var sw = new StreamWriter(httpWRequest.GetRequestStream());
                sw.Write(parameter);
                sw.Close();

                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                string resultHtml = sr.ReadToEnd();
                Log.DebugFormat("{0} - {1}", MethodBase.GetCurrentMethod().Name, resultHtml);
                return resultHtml.Substring("<rsvunitsn>", "</rsvunitsn>");
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
                return null;
            }
        }

        /// <summary>
        ///     스텝2_1
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="rsvtm"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string Step2_2(DateTime datetime, string rsvtm, string cookie)
        {
            try
            {
                var httpWRequest =
                    (HttpWebRequest) WebRequest.Create(
                        string.Format(
                            "http://yeyak.seoul.go.kr/request.web?execution=e1s2&_eventId_nextState&day={0}&mthfxtrsvpossat=0&sday={1}&eday=undefined&sltYear={3}&sltMonth={4}&sltDay={5}&rsvunit={6}&&useday={0}&useDateView={1}&turnView={1}&cancelDateView={2}",
                            datetime.ToString("yyyyMMdd"),
                            datetime.ToString("yyyy.MM.dd"),
                            datetime.ToString("yyyy-MM-dd"),
                            datetime.Year,
                            datetime.Month.ToString("00"),
                            datetime.Day.ToString("00"),
                            rsvtm
                            )
                        );
                httpWRequest.Accept = "text/html, application/xhtml+xml, */*";
                httpWRequest.Headers.Add("Accept-Language", "ko-KR");
                httpWRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWRequest.Referer = "http://yeyak.seoul.go.kr/request.web?execution=e1s2";
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                httpWRequest.Headers.Add("DNT", "1");
                httpWRequest.KeepAlive = true;
                httpWRequest.Method = "Get";
                httpWRequest.CookieContainer = new CookieContainer();

                SetCookie(cookie, httpWRequest);
                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                string resultHtml = sr.ReadToEnd();
                Log.DebugFormat("{0} - {1}", MethodBase.GetCurrentMethod().Name, resultHtml);

                string dc_std_sn = "";
                if (resultHtml.IndexOf("name='dc_std_sn' value='") > 0)
                {
                    dc_std_sn = resultHtml.Substring("name='dc_std_sn' value='", "class").Replace("'", "");
                }

                string fee_tgt = resultHtml.Substring("name='fee_tgt' value='", "class").Replace("'", "");
                return dc_std_sn + "|" + fee_tgt;
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
                return null;
            }
        }

        /// <summary>
        ///     스텝3
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="svcId"></param>
        /// <param name="fee_tgt"></param>
        /// <param name="dc_std_sn"></param>
        /// <returns></returns>
        public static void Step3_1(string cookie, string svcId, string fee_tgt, string dc_std_sn)
        {
            try
            {
                var httpWRequest =
                    (HttpWebRequest) WebRequest.Create(
                        string.Format(
                            "https://yeyak.seoul.go.kr/reservation/include/getUseFee.web?payat=1&reqrsvunitval=1&reqtime=1&reqcnt=&reqcnt=&svcid={0}&rsvunitcnt=1&ttlBscUseAmt=&ttlBscUseDcExtrAmt=&ttlAddUseAmt=&ttlAddUseDcExtrAmt=&payAmt=&mthfxtrsvpossat=0&fee_tgtcnt=1&fee_tgt={1}&dc_std_sn={2}",
                            svcId, fee_tgt, dc_std_sn));
                httpWRequest.Accept = "text/html, application/xhtml+xml, */*";
                httpWRequest.Headers.Add("Accept-Language", "ko-KR");
                httpWRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWRequest.Referer = "http://yeyak.seoul.go.kr/request.web?execution=e1s2";
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                httpWRequest.Headers.Add("DNT", "1");
                httpWRequest.KeepAlive = true;
                httpWRequest.Method = "Get";
                httpWRequest.CookieContainer = new CookieContainer();

                SetCookie(cookie, httpWRequest);
                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                string resultHtml = sr.ReadToEnd();
                Log.DebugFormat("{0} - {1}", MethodBase.GetCurrentMethod().Name, resultHtml);
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
            }
        }

        /// <summary>
        ///     스텝3_1
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="svcId"></param>
        /// <param name="fee_tgt"></param>
        /// <param name="dc_std_sn"></param>
        /// <returns></returns>
        public static string Step3_2(string cookie, string svcId, string fee_tgt, string dc_std_sn)
        {
            try
            {
                var httpWRequest =
                    (HttpWebRequest)
                        WebRequest.Create(
                            string.Format(
                                "http://yeyak.seoul.go.kr/request.web?execution=e1s3&_eventId_nextStateForUsr&reqcnt=1&svcid={1}&rsvunitcnt=1&ttlBscUseAmt=15000&ttlBscUseDcExtrAmt=0&ttlAddUseAmt=0&ttlAddUseDcExtrAmt=0&payAmt=15000&mthfxtrsvpossat=0&fee_tgtcnt=1&fee_tgt={0}&dc_std_sn={2}&reqvaloneView=1 동&useFeeView=15,000 원&dcExtView=0 원&addFeeView=0 원&totalFeeView=15,000 원",
                                fee_tgt, svcId, dc_std_sn));
                httpWRequest.Accept = "text/html, application/xhtml+xml, */*";
                httpWRequest.Headers.Add("Accept-Language", "ko-KR");
                httpWRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWRequest.Referer = "http://yeyak.seoul.go.kr/request.web?execution=e1s3";
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                httpWRequest.Headers.Add("DNT", "1");
                httpWRequest.KeepAlive = true;
                httpWRequest.Method = "Get";
                httpWRequest.CookieContainer = new CookieContainer();

                SetCookie(cookie, httpWRequest);
                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                string resultHtml = sr.ReadToEnd();

                Log.DebugFormat("{0} - {1}", MethodBase.GetCurrentMethod().Name, resultHtml);

                string mixString = "";

                string bCode = resultHtml.Substring("indivgrpsectcode 	= \"", "\";");
                string id = resultHtml.Substring("  mmbid = \"", "\";");
                string name = resultHtml.Substring("  mmbnm = \"", "\";");
                string address = resultHtml.Substring("  adres = \"", "\";");
                string email = resultHtml.Substring("  email = \"", "\";");
                string phone = resultHtml.Substring("  hpno  = \"", "\";");
                string zipcode = resultHtml.Substring("var zipcode = \"", "\".");
                mixString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                    bCode, name, email, phone, zipcode, address);

                return mixString;
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
                return null;
            }
        }

        /// <summary>
        ///     스텝3_1
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static void Step4_1(string cookie)
        {
            try
            {
                var httpWRequest =
                    (HttpWebRequest)
                        WebRequest.Create("https://yeyak.seoul.go.kr/reservation/include/checkLogin.web");
                httpWRequest.Accept = "text/html, application/xhtml+xml, */*";
                httpWRequest.Headers.Add("Accept-Language", "ko-KR");
                httpWRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWRequest.Referer = "http://yeyak.seoul.go.kr/request.web?execution=e1s4";
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                httpWRequest.Headers.Add("DNT", "1");
                httpWRequest.KeepAlive = true;
                httpWRequest.Method = "Get";
                httpWRequest.CookieContainer = new CookieContainer();

                SetCookie(cookie, httpWRequest);
                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                var resultHtml= sr.ReadToEnd();
                Log.DebugFormat("{0} - {1}", MethodBase.GetCurrentMethod().Name, resultHtml);
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
            }
        }

        /// <summary>
        ///     스텝3_1
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="svcId"></param>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public static void Step4_2(string cookie, string svcId, string zipCode)
        {
            try
            {
                var httpWRequest =
                    (HttpWebRequest)
                        WebRequest.Create(
                            string.Format(
                                "https://yeyak.seoul.go.kr/reservation/include/checkAreaLim.web?svcid={0}&zipcode={1}",
                                svcId, zipCode));
                httpWRequest.Accept = "text/html, application/xhtml+xml, */*";
                httpWRequest.Headers.Add("Accept-Language", "ko-KR");
                httpWRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWRequest.Referer = "http://yeyak.seoul.go.kr/request.web?execution=e1s4";
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                httpWRequest.Headers.Add("DNT", "1");
                httpWRequest.KeepAlive = true;
                httpWRequest.Method = "Get";
                httpWRequest.CookieContainer = new CookieContainer();

                SetCookie(cookie, httpWRequest);
                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                var resultHtml = sr.ReadToEnd();
                Log.DebugFormat("{0} - {1}", MethodBase.GetCurrentMethod().Name, resultHtml);
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
            }
        }

        /// <summary>
        ///     스텝4
        /// </summary>
        /// <param name="address"></param>
        /// <param name="cookie"></param>
        /// <param name="bCode"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="zipcode"></param>
        /// <returns></returns>
        public static bool Step4_3(string bCode, string name, string email, string phone, string zipcode, string address,
            string cookie)
        {
            try
            {
                var httpWRequest =
                    (HttpWebRequest)
                        WebRequest.Create(
                            string.Format(
                                "http://yeyak.seoul.go.kr/request.web?execution=e1s4&_eventId_nextState&rsvlang=1&rsvtype=w&&indivgrpsectcode={0}&grpnm=&usrnm={1}&email={2}&smsrcv=N&emailrcv=N&hpno={3}&zipcode={4}&adres={5}",
                                bCode,
                                name,
                                email,
                                phone,
                                zipcode,
                                address
                                ));
                httpWRequest.Accept = "text/html, application/xhtml+xml, */*";
                httpWRequest.Headers.Add("Accept-Language", "ko-KR");
                httpWRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWRequest.Referer = "http://yeyak.seoul.go.kr/request.web?execution=e1s3";
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                httpWRequest.Headers.Add("DNT", "1");
                httpWRequest.KeepAlive = true;
                httpWRequest.Method = "Get";
                httpWRequest.CookieContainer = new CookieContainer();

                SetCookie(cookie, httpWRequest);
                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                var resultHtml = sr.ReadToEnd();
                Log.DebugFormat("{0} - {1}", MethodBase.GetCurrentMethod().Name, resultHtml);

                bool result = resultHtml.Contains("resevation.process.success");
                return result;
            }
            catch (Exception ex)
            {
                Util.ErrorLog(MethodBase.GetCurrentMethod(), ex, "에러");
                return false;
            }
        }
    }
}