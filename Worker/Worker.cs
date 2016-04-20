using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using SeoulKwachun.Entity;
using SeoulKwachun.Manager;

namespace SeoulKwachun.Worker
{
    class Worker : BackgroundWorker
    {
        public Worker()
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        public int Interval { private get; set; }
        public string Cookie { private get; set; }
        public List<Reservation> Reservations { private get; set; }
        
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            while (!CancellationPending)
            {
                foreach (var reservation in Reservations)
                {
                    var datetime = reservation.ReservationDateTime;
                    var svcId = reservation.SvcId;

                    var freeDates = HttpManager.GetFreeSite(datetime.Year.ToString("0000"),
                        datetime.Month.ToString("00"), svcId);

                    if (freeDates == null) continue;

                    var findDate = freeDates.Where(x => x == datetime.ToString("yyyyMMdd"));

                    string message;
                    
                    if (findDate.Any())
                    {

                        GcmManager.SendNotification("과천" + datetime.ToString("yyyy-MM-dd"), "빈자리" );

                        var rsvReqKey = HttpManager.GetRsvReq(Cookie, svcId);
                        Thread.Sleep(10);

                        HttpManager.Step0(Cookie, svcId, rsvReqKey);
                        Thread.Sleep(50);

                        HttpManager.Step1_1(Cookie);
                        Thread.Sleep(100);

                        HttpManager.Step2(Cookie);
                        Thread.Sleep(100);

                        var stepRsvtm = HttpManager.Step2_1(Cookie, svcId, datetime.ToString("yyyyMMdd"));
                        Thread.Sleep(100);

                        var mixData = HttpManager.Step2_2(datetime, stepRsvtm, Cookie);
                        var dcStdSn = mixData.Split('|')[0];
                        var feeTgt = mixData.Split('|')[1];

                        Thread.Sleep(100);
                        HttpManager.Step3_1(Cookie, svcId, feeTgt, dcStdSn);
                        var mixString = HttpManager.Step3_2(Cookie, svcId, feeTgt, dcStdSn);

                        var array = mixString.Split('|');

                        Thread.Sleep(100);
                        HttpManager.Step4_1(Cookie);
                        string zipcode = "-";
                        try
                        {
                            zipcode = array[4].Substring(0, 3) + "-" + array[4].Substring(3, 3);
                        }
                        catch
                        {
                            
                        }
                        

                        Thread.Sleep(100);
                        HttpManager.Step4_2(Cookie, svcId, zipcode);
                        

                        var result = HttpManager.Step4_3(array[0], array[1], array[2], array[3], zipcode, array[5],
                            Cookie);

                        if (result)
                        {
                            message = string.Format("{0} {1} - {2}", "성공", datetime.ToString("MM-dd"),
                                DateTime.Now.ToString("MM-dd HH:mm:ss"));

                            ReportProgress(0, message);

                            GcmManager.SendNotification("과천" + message, "캠핑예약");
                            CancelAsync();
                            break;
                        }
                        
                        message = string.Format("{0} {1} - {2}", "실패", datetime.ToString("MM-dd"),
                            DateTime.Now.ToString("MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        message = string.Format("{0} {1}  - {2}", "실패", datetime.ToString("yyyy-MM-dd"),
                            DateTime.Now.ToString("MM-dd HH:mm:ss"));
                    }

                    ReportProgress(0, message);

                    Thread.Sleep(100*Interval);
                }
            }
        }
    }
}
