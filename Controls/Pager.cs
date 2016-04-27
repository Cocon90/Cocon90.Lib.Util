using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cocon90.Lib.Util.Controls
{
    /// <summary>
    /// 分页控件
    /// </summary>
    public partial class Pager : UserControl
    {
        /// <summary>
        /// 构建一个分页组件
        /// </summary>
        public Pager()
        {
            InitializeComponent();
            TotalRecord = 0;
            PageSize = 0;
            PageNumber = 1;
        }
        private int pageNumber = 1, totalRecord = 0, pageSize = 10;
        public delegate void PagedNumberChanged(int newPageNumber, int pageTotal);
        public event PagedNumberChanged OnPagedNumberChanged;
        /// <summary>
        ///获取或设置 总记录数
        /// </summary>
        public int TotalRecord { get { return totalRecord; } set { totalRecord = value; PageNumber = PageNumber; } }
        /// <summary>
        ///获取或设置 每页的数据量
        /// </summary>
        public int PageSize { get { return pageSize; } set { pageSize = value; PageNumber = PageNumber; } }
        /// <summary>
        ///获取或设置 当前页数 当前是第几页(初始为第1页)
        /// </summary>
        public int PageNumber
        {
            get { if (pageNumber >= getPageTotal()) { pageNumber = getPageTotal(); return pageNumber; } else if (pageNumber <= 0) { return 1; } else { return pageNumber; } }
            set
            {
                var total = getPageTotal();
                pageNumber = getNormalPageNum(value);
                if (pageNumber == 1)
                { //第一页
                    disableButton();
                    if (total > 1) { nextPageBtn.Enabled = lastPageBtn.Enabled = true; }
                    setStatus(total, 1);
                }
                else if (pageNumber == total)
                {//最后一页
                    disableButton();
                    if (total > 1)
                    { fristPageBtn.Enabled = provPageBtn.Enabled = true; }
                    setStatus(total, total);
                }
                else
                {//界于第1页与最后一页之间
                    fristPageBtn.Enabled = provPageBtn.Enabled = nextPageBtn.Enabled = lastPageBtn.Enabled = true;
                    setStatus(total, pageNumber);
                }

            }
        }
        /// <summary>
        ///获取 页总数 分页后的页总数
        /// </summary>
        public int getPageTotal()
        {
            if (PageSize <= 0 || TotalRecord <= 0 || PageSize >= TotalRecord) return 1;
            else return (TotalRecord % PageSize == 0 ? TotalRecord / PageSize : ((TotalRecord / PageSize) + 1));
        }
        private int getNormalPageNum(int value)
        {
            var total = getPageTotal();
            if (value >= total) { return total; } else if (value <= 0) { return 1; } else { return value; }
        }
        private void disableButton()
        {
            lastPageBtn.Enabled = nextPageBtn.Enabled = fristPageBtn.Enabled = provPageBtn.Enabled = false;//上页、首页不可用
        }
        private void setStatus(int total, int num)
        {
            infoLabel.Text = String.Format("第{0}页（共{1}页）", num, total);
        }
        private void fristPageBtn_Click(object sender, EventArgs e)
        {
            this.PageNumber = 1;
            invokeEvent();
        }

        private void provPageBtn_Click(object sender, EventArgs e)
        {
            this.PageNumber--;
            invokeEvent();
        }

        private void nextPageBtn_Click(object sender, EventArgs e)
        {
            this.PageNumber++;
            invokeEvent();
        }

        private void lastPageBtn_Click(object sender, EventArgs e)
        {
            this.PageNumber = getPageTotal();
            invokeEvent();
        }

        private void invokeEvent()
        {
            if (lastPageNumber != PageNumber)
            {
                if (OnPagedNumberChanged != null) { OnPagedNumberChanged(PageNumber, getPageTotal()); lastPageNumber = PageNumber; }
            }
        }
        private int lastPageNumber = 1;

        private Button fristPageBtn;
        public Button provPageBtn;
        public Button nextPageBtn;
        public Button lastPageBtn;
        public Label infoLabel;
        private void InitializeComponent()
        {
            this.fristPageBtn = new System.Windows.Forms.Button();
            this.provPageBtn = new System.Windows.Forms.Button();
            this.nextPageBtn = new System.Windows.Forms.Button();
            this.lastPageBtn = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // fristPageBtn
            // 
            this.fristPageBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.fristPageBtn.Location = new System.Drawing.Point(19, 4);
            this.fristPageBtn.Name = "fristPageBtn";
            this.fristPageBtn.Size = new System.Drawing.Size(75, 28);
            this.fristPageBtn.TabIndex = 100;
            this.fristPageBtn.Text = "首  页";
            this.fristPageBtn.UseVisualStyleBackColor = true;
            this.fristPageBtn.Click += new System.EventHandler(this.fristPageBtn_Click);
            // 
            // provPageBtn
            // 
            this.provPageBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.provPageBtn.Location = new System.Drawing.Point(100, 4);
            this.provPageBtn.Name = "provPageBtn";
            this.provPageBtn.Size = new System.Drawing.Size(75, 28);
            this.provPageBtn.TabIndex = 101;
            this.provPageBtn.Text = "上一页";
            this.provPageBtn.UseVisualStyleBackColor = true;
            this.provPageBtn.Click += new System.EventHandler(this.provPageBtn_Click);
            // 
            // nextPageBtn
            // 
            this.nextPageBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.nextPageBtn.Location = new System.Drawing.Point(181, 4);
            this.nextPageBtn.Name = "nextPageBtn";
            this.nextPageBtn.Size = new System.Drawing.Size(75, 28);
            this.nextPageBtn.TabIndex = 102;
            this.nextPageBtn.Text = "下一页";
            this.nextPageBtn.UseVisualStyleBackColor = true;
            this.nextPageBtn.Click += new System.EventHandler(this.nextPageBtn_Click);
            // 
            // lastPageBtn
            // 
            this.lastPageBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lastPageBtn.Location = new System.Drawing.Point(262, 4);
            this.lastPageBtn.Name = "lastPageBtn";
            this.lastPageBtn.Size = new System.Drawing.Size(75, 28);
            this.lastPageBtn.TabIndex = 103;
            this.lastPageBtn.Text = "尾  页";
            this.lastPageBtn.UseVisualStyleBackColor = true;
            this.lastPageBtn.Click += new System.EventHandler(this.lastPageBtn_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(456, 0);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(132, 36);
            this.infoLabel.TabIndex = 4;
            this.infoLabel.Text = "第1页（共1页）";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Pager
            // 
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.lastPageBtn);
            this.Controls.Add(this.nextPageBtn);
            this.Controls.Add(this.provPageBtn);
            this.Controls.Add(this.fristPageBtn);
            this.Name = "Pager";
            this.Size = new System.Drawing.Size(597, 36);
            this.ResumeLayout(false);

        }



    }
}
