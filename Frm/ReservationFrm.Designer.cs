namespace SeoulKwachun.Frm
{
    partial class ReservationFrm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLogin = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.TxtPassword = new System.Windows.Forms.TextBox();
            this.gbIndivisual = new System.Windows.Forms.GroupBox();
            this.lbLocation = new System.Windows.Forms.ListBox();
            this.gbSelect = new System.Windows.Forms.GroupBox();
            this.lbFreeDay = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbSchedule = new System.Windows.Forms.ListBox();
            this.labal1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddReservation = new System.Windows.Forms.Button();
            this.gbReservation = new System.Windows.Forms.GroupBox();
            this.cbReservation = new System.Windows.Forms.CheckedListBox();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnStartReservation = new System.Windows.Forms.Button();
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.cbAsync = new System.Windows.Forms.CheckBox();
            this.gbIndivisual.SuspendLayout();
            this.gbSelect.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbReservation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.gbLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(194, 18);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(79, 50);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "로그인";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(52, 20);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(220, 21);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "아이디 : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "비밀번호 : ";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(72, 20);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(116, 21);
            this.txtId.TabIndex = 12;
            // 
            // TxtPassword
            // 
            this.TxtPassword.Location = new System.Drawing.Point(72, 47);
            this.TxtPassword.Name = "TxtPassword";
            this.TxtPassword.Size = new System.Drawing.Size(116, 21);
            this.TxtPassword.TabIndex = 13;
            // 
            // gbIndivisual
            // 
            this.gbIndivisual.Controls.Add(this.txtId);
            this.gbIndivisual.Controls.Add(this.TxtPassword);
            this.gbIndivisual.Controls.Add(this.btnLogin);
            this.gbIndivisual.Controls.Add(this.label2);
            this.gbIndivisual.Controls.Add(this.label1);
            this.gbIndivisual.Location = new System.Drawing.Point(12, 12);
            this.gbIndivisual.Name = "gbIndivisual";
            this.gbIndivisual.Size = new System.Drawing.Size(285, 79);
            this.gbIndivisual.TabIndex = 14;
            this.gbIndivisual.TabStop = false;
            this.gbIndivisual.Text = "정보";
            // 
            // lbLocation
            // 
            this.lbLocation.FormattingEnabled = true;
            this.lbLocation.ItemHeight = 12;
            this.lbLocation.Location = new System.Drawing.Point(52, 20);
            this.lbLocation.Name = "lbLocation";
            this.lbLocation.Size = new System.Drawing.Size(220, 40);
            this.lbLocation.TabIndex = 15;
            this.lbLocation.SelectedIndexChanged += new System.EventHandler(this.lbLocation_SelectedIndexChanged);
            // 
            // gbSelect
            // 
            this.gbSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbSelect.Controls.Add(this.lbFreeDay);
            this.gbSelect.Controls.Add(this.label4);
            this.gbSelect.Controls.Add(this.label3);
            this.gbSelect.Controls.Add(this.lbSchedule);
            this.gbSelect.Controls.Add(this.labal1);
            this.gbSelect.Controls.Add(this.lbLocation);
            this.gbSelect.Location = new System.Drawing.Point(12, 97);
            this.gbSelect.Name = "gbSelect";
            this.gbSelect.Size = new System.Drawing.Size(285, 271);
            this.gbSelect.TabIndex = 16;
            this.gbSelect.TabStop = false;
            this.gbSelect.Text = "조회";
            // 
            // lbFreeDay
            // 
            this.lbFreeDay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbFreeDay.FormattingEnabled = true;
            this.lbFreeDay.ItemHeight = 12;
            this.lbFreeDay.Location = new System.Drawing.Point(53, 136);
            this.lbFreeDay.Name = "lbFreeDay";
            this.lbFreeDay.Size = new System.Drawing.Size(220, 124);
            this.lbFreeDay.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "가능 :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "일정 :";
            // 
            // lbSchedule
            // 
            this.lbSchedule.FormattingEnabled = true;
            this.lbSchedule.ItemHeight = 12;
            this.lbSchedule.Location = new System.Drawing.Point(52, 66);
            this.lbSchedule.Name = "lbSchedule";
            this.lbSchedule.Size = new System.Drawing.Size(220, 64);
            this.lbSchedule.TabIndex = 17;
            this.lbSchedule.SelectedIndexChanged += new System.EventHandler(this.lbSchedule_SelectedIndexChanged);
            // 
            // labal1
            // 
            this.labal1.AutoSize = true;
            this.labal1.Location = new System.Drawing.Point(9, 20);
            this.labal1.Name = "labal1";
            this.labal1.Size = new System.Drawing.Size(37, 12);
            this.labal1.TabIndex = 16;
            this.labal1.Text = "지역 :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "예약 :";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.btnAddReservation);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 375);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 76);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            // 
            // btnAddReservation
            // 
            this.btnAddReservation.Location = new System.Drawing.Point(196, 48);
            this.btnAddReservation.Name = "btnAddReservation";
            this.btnAddReservation.Size = new System.Drawing.Size(75, 23);
            this.btnAddReservation.TabIndex = 22;
            this.btnAddReservation.Text = "추가";
            this.btnAddReservation.UseVisualStyleBackColor = true;
            this.btnAddReservation.Click += new System.EventHandler(this.btnAddReservation_Click);
            // 
            // gbReservation
            // 
            this.gbReservation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbReservation.Controls.Add(this.cbAsync);
            this.gbReservation.Controls.Add(this.cbReservation);
            this.gbReservation.Controls.Add(this.numInterval);
            this.gbReservation.Controls.Add(this.btnDelete);
            this.gbReservation.Controls.Add(this.btnStartReservation);
            this.gbReservation.Location = new System.Drawing.Point(303, 12);
            this.gbReservation.Name = "gbReservation";
            this.gbReservation.Size = new System.Drawing.Size(578, 171);
            this.gbReservation.TabIndex = 23;
            this.gbReservation.TabStop = false;
            this.gbReservation.Text = "예약";
            // 
            // cbReservation
            // 
            this.cbReservation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbReservation.FormattingEnabled = true;
            this.cbReservation.Location = new System.Drawing.Point(6, 12);
            this.cbReservation.Name = "cbReservation";
            this.cbReservation.Size = new System.Drawing.Size(566, 116);
            this.cbReservation.TabIndex = 15;
            // 
            // numInterval
            // 
            this.numInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numInterval.Location = new System.Drawing.Point(446, 143);
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(44, 21);
            this.numInterval.TabIndex = 14;
            this.numInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(6, 142);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "선택삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnStartReservation
            // 
            this.btnStartReservation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartReservation.Location = new System.Drawing.Point(497, 142);
            this.btnStartReservation.Name = "btnStartReservation";
            this.btnStartReservation.Size = new System.Drawing.Size(75, 23);
            this.btnStartReservation.TabIndex = 12;
            this.btnStartReservation.Text = "예약시작";
            this.btnStartReservation.UseVisualStyleBackColor = true;
            this.btnStartReservation.Click += new System.EventHandler(this.btnStartReservation_Click);
            // 
            // gbLog
            // 
            this.gbLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLog.Controls.Add(this.txtLog);
            this.gbLog.Location = new System.Drawing.Point(303, 189);
            this.gbLog.Name = "gbLog";
            this.gbLog.Size = new System.Drawing.Size(578, 262);
            this.gbLog.TabIndex = 24;
            this.gbLog.TabStop = false;
            this.gbLog.Text = "로그";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(6, 20);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(566, 232);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // cbAsync
            // 
            this.cbAsync.AutoSize = true;
            this.cbAsync.Location = new System.Drawing.Point(380, 144);
            this.cbAsync.Name = "cbAsync";
            this.cbAsync.Size = new System.Drawing.Size(60, 16);
            this.cbAsync.TabIndex = 16;
            this.cbAsync.Text = "비동기";
            this.cbAsync.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 455);
            this.Controls.Add(this.gbLog);
            this.Controls.Add(this.gbReservation);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbSelect);
            this.Controls.Add(this.gbIndivisual);
            this.Name = "Form1";
            this.Text = "과천 캠프장";
            this.gbIndivisual.ResumeLayout(false);
            this.gbIndivisual.PerformLayout();
            this.gbSelect.ResumeLayout(false);
            this.gbSelect.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbReservation.ResumeLayout(false);
            this.gbReservation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.gbLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox TxtPassword;
        private System.Windows.Forms.GroupBox gbIndivisual;
        private System.Windows.Forms.ListBox lbLocation;
        private System.Windows.Forms.GroupBox gbSelect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbSchedule;
        private System.Windows.Forms.Label labal1;
        private System.Windows.Forms.ListBox lbFreeDay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAddReservation;
        private System.Windows.Forms.GroupBox gbReservation;
        private System.Windows.Forms.CheckedListBox cbReservation;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnStartReservation;
        private System.Windows.Forms.GroupBox gbLog;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.CheckBox cbAsync;
    }
}

