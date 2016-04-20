using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using SeoulKwachun.Entity;
using SeoulKwachun.Manager;

namespace SeoulKwachun.Frm
{
    public partial class ReservationFrm : Form
    {
        private string _cookie;
        private readonly WebBrowser _wb = new WebBrowser();
        readonly Worker.Worker _worker = new Worker.Worker();
        /// <summary>
        /// 
        /// </summary>
        public ReservationFrm()
        {
            InitializeComponent();
            //gbIndivisual.Enabled = false;
            
            _wb.DocumentCompleted += wb_DocumentCompleted;
            _wb.ScriptErrorsSuppressed = true;
            
            gbSelect.Enabled = false;

            lbLocation.DisplayMember = "text";
            lbLocation.ValueMember = "value";

            lbSchedule.DisplayMember = "text";
            lbSchedule.ValueMember = "value";

            lbLocation.Enabled = false;
            lbSchedule.Enabled = false;
            lbFreeDay.Enabled = false;
            dateTimePicker1.Enabled = false;

            _worker.ProgressChanged += worker_ProgressChanged;
            _worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            Util.Logging(txtLog, e.Url.ToString());

            if (e.Url.ToString().Contains("login.web"))
            {
                if (_wb.Document != null)
                {
                    _wb.Document.GetElementById("userid")
                        .SetAttribute("value", txtId.Text);
                    _wb.Document.GetElementById("userpwd")
                        .SetAttribute("value", TxtPassword.Text);

                    _wb.Document.Forms["login_Form"].InvokeMember("submit");
                }
            }
            else if (e.Url.ToString() == "http://yeyak.seoul.go.kr/main.web")
            {
                gbIndivisual.Enabled = true;

                gbSelect.Enabled = true;
                lbLocation.Enabled = true;

                var locations = HttpManager.GetLocation();

                lbLocation.Items.Clear();
                foreach (var split in locations.Select(location => location.Split('|')))
                {
                    lbLocation.Items.Add(new { value = split[0], text = split[1] });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            _wb.Navigate("https://yeyak.seoul.go.kr/reservation/login/login.web", null, null, "User-Agent: Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartReservation_Click(object sender, EventArgs e)
        {
            if (_wb.Document != null) _cookie = _wb.Document.Cookie;

            if (cbAsync.CanFocus)
            {

                if (_worker.IsBusy)
                {
                    _worker.CancelAsync();
                    btnStartReservation.Text = @"중지 증...";
                    btnStartReservation.Enabled = false;
                }
                else
                {
                    var reservations =
                        (from object item in cbReservation.CheckedItems
                            select new Reservation
                            {
                                ReservationDateTime = DateTime.Parse(item.ToString().Split('|')[0].Trim()),
                                SvcId = item.ToString().Split('|')[1].Trim()
                            }).ToList();


                    _worker.Interval = (int) numInterval.Value;
                    _worker.Reservations = reservations;
                    _worker.Cookie = _cookie;
                    _worker.RunWorkerAsync();

                    btnStartReservation.Text = @"중지";
                }
            }
            else
            {
                if (btnStartReservation.Text == @"중지")
                {
                    btnStartReservation.Text = @"시작";
                }
                else
                {
                    btnStartReservation.Text = @"중지";
                    SyncReservation();
                }
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SyncReservation()
        {
            var reservations =
                       (from object item in cbReservation.CheckedItems
                        select new Reservation
                        {
                            ReservationDateTime = DateTime.Parse(item.ToString().Split('|')[0].Trim()),
                            SvcId = item.ToString().Split('|')[1].Trim()
                        }).ToList();

            foreach (var reservation in reservations)
            {
                if (btnStartReservation.Text == @"시작") break;

                var reservationDateTime = reservation.ReservationDateTime;
                var svcId = reservation.SvcId;

                var freeDates = HttpManager.GetFreeSite(reservationDateTime.Year.ToString("0000"),
                    reservationDateTime.Month.ToString("00"), svcId);

                var findDate = freeDates.Where(x => x == reservationDateTime.ToString("yyyyMMdd"));
                
                string message;

                if (findDate.Any())
                {
                    var rsvReqKey = HttpManager.GetRsvReq(_cookie, svcId);

                    HttpManager.Step0(_cookie, svcId, rsvReqKey);

                    HttpManager.Step1_1(_cookie);

                    HttpManager.Step2(_cookie);

                    var stepRsvtm = HttpManager.Step2_1(_cookie, svcId, reservationDateTime.ToString("yyyyMMdd"));

                    var mixData = HttpManager.Step2_2(reservationDateTime, stepRsvtm, _cookie);
                    var dcStdSn = mixData.Split('|')[0];
                    var feeTgt = mixData.Split('|')[1];

                    HttpManager.Step3_1(_cookie, svcId, feeTgt, dcStdSn);
                    var mixString = HttpManager.Step3_2(_cookie, svcId, feeTgt, dcStdSn);

                    var array = mixString.Split('|');

                    HttpManager.Step4_1(_cookie);
                    var zipcode = array[4].Substring(0, 3) + "-" + array[4].Substring(3, 3);
                    HttpManager.Step4_2(_cookie, svcId, zipcode);


                    if (btnStartReservation.Text == @"중지") break;
                    var result = HttpManager.Step4_3(array[0], array[1], array[2], array[3], zipcode, array[5],
                        _cookie);

                    
                    if (result)
                    {
                        message = string.Format("{0} {1} 예약 - {2}", "성공", reservationDateTime.ToString("MM-dd"),
                            DateTime.Now.ToString("MM-dd HH:mm:ss"));

                        GcmManager.SendNotification("과천" + message, "캠핑예약");
                        Util.Logging(txtLog, message);
                        break;
                    }

                    message = string.Format("{0} {1} 예약 - {2}", "실패", reservationDateTime.ToString("MM-dd"),
                        DateTime.Now.ToString("MM-dd HH:mm:ss"));
                    Util.Logging(txtLog, message);
                }
                else
                {
                    message = string.Format("{0} {1} 실패 - {2}", "성공", reservationDateTime.ToString("MM-dd"),
                           DateTime.Now.ToString("MM-dd HH:mm:ss"));
                    Util.Logging(txtLog, message);
                }

                Thread.Sleep(1000 * (int)numInterval.Value);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddReservation_Click(object sender, EventArgs e)
        {
            var svcId = lbSchedule.SelectedItem.ToString().Substring("value = ", ",");
            var datetime = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            cbReservation.Items.Add(datetime + " | " + svcId);
            cbReservation.SetItemCheckState(cbReservation.Items.Count - 1, CheckState.Checked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            cbReservation.Items.RemoveAt(cbReservation.SelectedIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {

            var schedules = HttpManager.GetSchedule(lbLocation.SelectedItem.ToString().Substring("= ", ","));

            lbSchedule.Items.Clear();
            foreach (string schedule in schedules)
            {
                var split = schedule.Split('|');
                lbSchedule.Items.Add(new { value = split[0], text = split[1] });
            }

            lbSchedule.Enabled = true;
            lbFreeDay.Enabled = false;
            dateTimePicker1.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            var text1 = lbSchedule.SelectedItem.ToString().Substring("장", "월").Trim();

            text1 = text1.Replace("-", "~");

            lbFreeDay.Items.Clear();
            foreach (var text in text1.Split('~'))
            {
                var freeDates = HttpManager.GetFreeSite(DateTime.Now.Year.ToString("0000"),
                    int.Parse(text).ToString("00"),
                    lbSchedule.SelectedItem.ToString().Substring("= ", ","));

                foreach (var freeDate in freeDates)
                {
                    int year = int.Parse(freeDate.Substring(0, 4));
                    int month = int.Parse(freeDate.Substring(4, 2));
                    int day = int.Parse(freeDate.Substring(6, 2));
                    lbFreeDay.Items.Add( new DateTime(year, month, day).ToLongDateString());
                }
            }

            lbFreeDay.Enabled = true;
            dateTimePicker1.Enabled = true;
        }



        #region worker

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (txtLog.Lines.Length <= 30)
            {
                Util.Logging(txtLog, e.UserState.ToString());
            }
            else
            {
                txtLog.Clear();
                Util.Logging(txtLog, e.UserState.ToString());
            }
        }

        private void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Util.Logging(txtLog, "완료~!!");
            btnStartReservation.Enabled = true;
            btnStartReservation.Text = @"시작";
        }

        #endregion
    }
}

