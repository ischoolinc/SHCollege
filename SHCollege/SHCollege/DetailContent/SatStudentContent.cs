using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SHCollege.DAO;
using Campus.Windows;

namespace SHCollege.DetailContent
{
    [FISCA.Permission.FeatureCode("SH_College_SATStudentContent", "大學繁星學測學生")]
    public partial class SatStudentContent : FISCA.Presentation.DetailContent
    {
        EventHandler eh;
        string EventCode = "SH_College_SATStudentContent";

        Dictionary<string, UDT_SHSATStudent> _SHSATStudentDict;
        List<string> sidList;
        BackgroundWorker _bgWorker;
        private ChangeListener _ChangeListener;
        K12.Data.StudentRecord _StudRec;
        bool _isBusy = false;
        ErrorProvider _errorP;

        public SatStudentContent()
        {
            InitializeComponent();
            _SHSATStudentDict = new Dictionary<string, UDT_SHSATStudent>();
            _ChangeListener = new ChangeListener();
            sidList = new List<string>();
            _errorP = new ErrorProvider();
            this.Group = "大學繁星學測學生";
            _bgWorker = new BackgroundWorker();
            _bgWorker.DoWork += _bgWorker_DoWork;
            _bgWorker.RunWorkerCompleted += _bgWorker_RunWorkerCompleted;

            // 加入控制項變動檢查                        
            _ChangeListener.Add(new TextBoxSource(txtSatClassName));
            _ChangeListener.Add(new TextBoxSource(txtSatSeatNo));
            _ChangeListener.Add(new TextBoxSource(txtSATSerNo));
            _ChangeListener.StatusChanged += _ChangeListener_StatusChanged;
            //啟動更新事件
            eh = FISCA.InteractionService.PublishEvent(EventCode);
        }

        void _ChangeListener_StatusChanged(object sender, ChangeEventArgs e)
        {
            this.CancelButtonVisible = (e.Status == ValueStatus.Dirty);
            this.SaveButtonVisible = (e.Status == ValueStatus.Dirty);
        }

        void _bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_isBusy)
            {
                _isBusy = false;
                _bgWorker.RunWorkerAsync();
                return;
            }
            LoadData();
        }

        void _bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _SHSATStudentDict = UDTTransfer.GetSATStudentByStudentIDList(sidList);
            _StudRec = K12.Data.Student.SelectByID(PrimaryKey);
        }

        protected override void OnPrimaryKeyChanged(EventArgs e)
        {
            this.Loading = true;
            this.CancelButtonVisible = false;
            this.SaveButtonVisible = false;
            sidList.Clear();
            sidList.Add(PrimaryKey);
            _BGRun();
        }

        private void LoadData()
        {
            _ChangeListener.SuspendListen();

            txtSatClassName.Text = "";
            txtSatSeatNo.Text = "";
            txtSATSerNo.Text = "";
            if (_SHSATStudentDict.Count > 0)
            {
                txtSATSerNo.Text = _SHSATStudentDict[PrimaryKey].SatSerNo;
                txtSatSeatNo.Text = _SHSATStudentDict[PrimaryKey].SatSeatNo;
                txtSatClassName.Text = _SHSATStudentDict[PrimaryKey].SatClassName;
            }

            _ChangeListener.Reset();
            _ChangeListener.ResumeListen();
            this.Loading = false;
        }

        private void _BGRun()
        {
            if (_bgWorker.IsBusy)
                _isBusy = true;
            else
                _bgWorker.RunWorkerAsync();
        }

        protected override void OnCancelButtonClick(EventArgs e)
        {
            this.CancelButtonVisible = false;
            this.SaveButtonVisible = false;
            LoadData();
        }

        protected override void OnSaveButtonClick(EventArgs e)
        {
            string IDNumber = "", StudentNumber = "", SATClassName = "", SATSeatNo = "",SATSerNo="";

            IDNumber = _StudRec.IDNumber;
            StudentNumber = _StudRec.StudentNumber;
            SATSerNo = txtSATSerNo.Text;
            SATClassName = txtSatClassName.Text;
            SATSeatNo = txtSatSeatNo.Text;
            
            // 檢查當已是高關懷
            if (_SHSATStudentDict.ContainsKey(PrimaryKey))
            {
           
                    

                    _SHSATStudentDict[PrimaryKey].Save();

            }
            else
            {
                UDT_SHSATStudent newData = new UDT_SHSATStudent();
                newData.StudentNumber = StudentNumber;
             
                newData.Save();
            }
            this.CancelButtonVisible = false;
            this.SaveButtonVisible = false;
            eh(this, EventArgs.Empty);
            _ChangeListener.Reset();
            _ChangeListener.ResumeListen();
        }

        private void txtSATSerNo_TextChanged(object sender, EventArgs e)
        {
            _errorP.SetError(txtSatSeatNo, "");
        }

        private void txtSatClassName_TextChanged(object sender, EventArgs e)
        {
            _errorP.SetError(txtSatClassName, "");
        }

        private void txtSatSeatNo_TextChanged(object sender, EventArgs e)
        {
            _errorP.SetError(txtSatSeatNo, "");
        }
        
    }
}
