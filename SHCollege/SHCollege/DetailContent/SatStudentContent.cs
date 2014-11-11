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

        int _SatSerNoLen = 8;
        int _SatClassSeatNoLen = 5;
        

        public SatStudentContent()
        {
            InitializeComponent();
            _SHSATStudentDict = new Dictionary<string, UDT_SHSATStudent>();
            _ChangeListener = new ChangeListener();
            sidList = new List<string>();
            _errorP = new ErrorProvider();
            this.Group = "大學繁星學測報名序號";
            _bgWorker = new BackgroundWorker();
            _bgWorker.DoWork += _bgWorker_DoWork;
            _bgWorker.RunWorkerCompleted += _bgWorker_RunWorkerCompleted;

            // 加入控制項變動檢查                        
            _ChangeListener.Add(new TextBoxSource(txtSatClassSeatNo));            
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
            _SHSATStudentDict = UDTTransfer.GetSATStudentByStudentIDListDict(sidList);
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

            txtSatClassSeatNo.Text = "";            
            txtSATSerNo.Text = "";
            if (_SHSATStudentDict.Count > 0)
            {
                txtSATSerNo.Text = _SHSATStudentDict[PrimaryKey].SatSerNo;
                txtSatClassSeatNo.Text = _SHSATStudentDict[PrimaryKey].SatClassSeatNo;
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

        private bool ChkData()
        {
            bool pass = true;
            
            if (string.IsNullOrEmpty(txtSATSerNo.Text))
            {
                _errorP.SetError(txtSATSerNo, "報名序號不能空白!");
                pass = false;
            }
            else
            {
                int x;
                if (int.TryParse(txtSATSerNo.Text, out x) == false)
                {
                    _errorP.SetError(txtSATSerNo, "報名序號必須數字!");
                    pass = false;
                }

                if (txtSATSerNo.Text.Length != _SatSerNoLen)
                {
                    _errorP.SetError(txtSATSerNo, "報名序號必須" + _SatSerNoLen + "碼!");
                    pass = false;
                }
            }            

            if (string.IsNullOrEmpty(txtSatClassSeatNo.Text))
            {
                _errorP.SetError(txtSatClassSeatNo, "學測班級不能空白!");
                pass = false;
            }
            else
            {
                int x;
                if (int.TryParse(txtSatClassSeatNo.Text, out x) == false)
                {
                    _errorP.SetError(txtSatClassSeatNo, "學測班級座號必須數字!");
                    pass = false;
                }

                if (txtSatClassSeatNo.Text.Length != _SatClassSeatNoLen)
                {
                    _errorP.SetError(txtSatClassSeatNo, "學測班級座號必須" + _SatClassSeatNoLen + "碼!");
                    pass = false;
                }
            }
            
            return pass;
        }



        protected override void OnSaveButtonClick(EventArgs e)
        {
            if (ChkData())
            {
                string IDNumber = "", StudentNumber = "", SATClassName = "", SATSeatNo = "", SATSerNo = "",SATClassSeatNo="";

                IDNumber = _StudRec.IDNumber;
                StudentNumber = _StudRec.StudentNumber;
                SATSerNo = txtSATSerNo.Text;
                SATClassSeatNo = txtSatClassSeatNo.Text;
                if (_StudRec.Class != null)
                    SATClassName = _StudRec.Class.Name;
                
                if (_SHSATStudentDict.ContainsKey(PrimaryKey))
                {
                    _SHSATStudentDict[PrimaryKey].IDNumber = IDNumber;
                    _SHSATStudentDict[PrimaryKey].StudentNumber = StudentNumber;
                    _SHSATStudentDict[PrimaryKey].SatSeatNo = SATSeatNo;
                    _SHSATStudentDict[PrimaryKey].SatClassName = SATClassName;
                    _SHSATStudentDict[PrimaryKey].SatSerNo = SATSerNo;
                    _SHSATStudentDict[PrimaryKey].SatClassSeatNo = SATClassSeatNo;
                    _SHSATStudentDict[PrimaryKey].Save();
                }
                else
                {
                    UDT_SHSATStudent newData = new UDT_SHSATStudent();
                    newData.RefStudentID = PrimaryKey;
                    newData.StudentNumber = StudentNumber;
                    newData.IDNumber = IDNumber;
                    newData.SatClassName = SATClassName;
                    newData.SatSeatNo = SATSeatNo;
                    newData.SatSerNo = SATSerNo;
                    newData.SatClassSeatNo = SATClassSeatNo;
                    newData.Save();
                }

                this.CancelButtonVisible = false;
                this.SaveButtonVisible = false;
                eh(this, EventArgs.Empty);
                _ChangeListener.Reset();
                _ChangeListener.ResumeListen();
            }    
        }

        private void txtSATSerNo_TextChanged(object sender, EventArgs e)
        {
            _errorP.SetError(txtSATSerNo, "");
        }

        private void txtSatClassSeatNo_TextChanged(object sender, EventArgs e)
        {
            _errorP.SetError(txtSatClassSeatNo, "");
        }        
    }
}
