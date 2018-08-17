namespace ALEXXPCLPTools
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.panelMain = new System.Windows.Forms.Panel();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.splitContainerRight = new System.Windows.Forms.SplitContainer();
            this.splitContainerTextArea = new System.Windows.Forms.SplitContainer();
            this.treeViewMain = new System.Windows.Forms.TreeView();
            this.listBoxMain = new System.Windows.Forms.ListBox();
            this.groupBoxStringNodes = new System.Windows.Forms.GroupBox();
            this.groupBoxStringArea = new System.Windows.Forms.GroupBox();
            this.groupBoxPanel = new System.Windows.Forms.GroupBox();
            this.groupBoxTop = new System.Windows.Forms.GroupBox();
            this.groupBoxBottom = new System.Windows.Forms.GroupBox();
            this.textBoxTop = new System.Windows.Forms.TextBox();
            this.textBoxBottom = new System.Windows.Forms.TextBox();
            this.panelMain.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            this.splitContainerRight.Panel1.SuspendLayout();
            this.splitContainerRight.Panel2.SuspendLayout();
            this.splitContainerRight.SuspendLayout();
            this.splitContainerTextArea.Panel1.SuspendLayout();
            this.splitContainerTextArea.Panel2.SuspendLayout();
            this.splitContainerTextArea.SuspendLayout();
            this.groupBoxStringNodes.SuspendLayout();
            this.groupBoxStringArea.SuspendLayout();
            this.groupBoxTop.SuspendLayout();
            this.groupBoxBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(784, 24);
            this.menuStripMain.TabIndex = 0;
            // 
            // statusStripMain
            // 
            this.statusStripMain.Location = new System.Drawing.Point(0, 540);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(784, 22);
            this.statusStripMain.TabIndex = 1;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.splitContainerMain);
            this.panelMain.Controls.Add(this.toolStripMain);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 24);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(784, 516);
            this.panelMain.TabIndex = 2;
            // 
            // toolStripMain
            // 
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(784, 25);
            this.toolStripMain.TabIndex = 0;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 25);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerLeft);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerRight);
            this.splitContainerMain.Size = new System.Drawing.Size(784, 491);
            this.splitContainerMain.SplitterDistance = 261;
            this.splitContainerMain.TabIndex = 1;
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLeft.Name = "splitContainerLeft";
            this.splitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.groupBoxPanel);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.groupBoxStringArea);
            this.splitContainerLeft.Size = new System.Drawing.Size(261, 491);
            this.splitContainerLeft.SplitterDistance = 87;
            this.splitContainerLeft.TabIndex = 0;
            // 
            // splitContainerRight
            // 
            this.splitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRight.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRight.Name = "splitContainerRight";
            // 
            // splitContainerRight.Panel1
            // 
            this.splitContainerRight.Panel1.Controls.Add(this.groupBoxStringNodes);
            // 
            // splitContainerRight.Panel2
            // 
            this.splitContainerRight.Panel2.Controls.Add(this.splitContainerTextArea);
            this.splitContainerRight.Size = new System.Drawing.Size(519, 491);
            this.splitContainerRight.SplitterDistance = 173;
            this.splitContainerRight.TabIndex = 0;
            // 
            // splitContainerTextArea
            // 
            this.splitContainerTextArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTextArea.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTextArea.Name = "splitContainerTextArea";
            this.splitContainerTextArea.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTextArea.Panel1
            // 
            this.splitContainerTextArea.Panel1.Controls.Add(this.groupBoxTop);
            // 
            // splitContainerTextArea.Panel2
            // 
            this.splitContainerTextArea.Panel2.Controls.Add(this.groupBoxBottom);
            this.splitContainerTextArea.Size = new System.Drawing.Size(342, 491);
            this.splitContainerTextArea.SplitterDistance = 216;
            this.splitContainerTextArea.TabIndex = 0;
            // 
            // treeViewMain
            // 
            this.treeViewMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewMain.Location = new System.Drawing.Point(3, 17);
            this.treeViewMain.Name = "treeViewMain";
            this.treeViewMain.Size = new System.Drawing.Size(255, 380);
            this.treeViewMain.TabIndex = 0;
            // 
            // listBoxMain
            // 
            this.listBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMain.FormattingEnabled = true;
            this.listBoxMain.ItemHeight = 12;
            this.listBoxMain.Location = new System.Drawing.Point(3, 17);
            this.listBoxMain.Name = "listBoxMain";
            this.listBoxMain.Size = new System.Drawing.Size(167, 460);
            this.listBoxMain.TabIndex = 0;
            // 
            // groupBoxStringNodes
            // 
            this.groupBoxStringNodes.Controls.Add(this.listBoxMain);
            this.groupBoxStringNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxStringNodes.Location = new System.Drawing.Point(0, 0);
            this.groupBoxStringNodes.Name = "groupBoxStringNodes";
            this.groupBoxStringNodes.Size = new System.Drawing.Size(173, 491);
            this.groupBoxStringNodes.TabIndex = 0;
            this.groupBoxStringNodes.TabStop = false;
            this.groupBoxStringNodes.Text = "字符串列表";
            // 
            // groupBoxStringArea
            // 
            this.groupBoxStringArea.Controls.Add(this.treeViewMain);
            this.groupBoxStringArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxStringArea.Location = new System.Drawing.Point(0, 0);
            this.groupBoxStringArea.Name = "groupBoxStringArea";
            this.groupBoxStringArea.Size = new System.Drawing.Size(261, 400);
            this.groupBoxStringArea.TabIndex = 0;
            this.groupBoxStringArea.TabStop = false;
            this.groupBoxStringArea.Text = "文档结构";
            // 
            // groupBoxPanel
            // 
            this.groupBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPanel.Location = new System.Drawing.Point(0, 0);
            this.groupBoxPanel.Name = "groupBoxPanel";
            this.groupBoxPanel.Size = new System.Drawing.Size(261, 87);
            this.groupBoxPanel.TabIndex = 0;
            this.groupBoxPanel.TabStop = false;
            this.groupBoxPanel.Text = "控制面板";
            // 
            // groupBoxTop
            // 
            this.groupBoxTop.Controls.Add(this.textBoxTop);
            this.groupBoxTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTop.Location = new System.Drawing.Point(0, 0);
            this.groupBoxTop.Name = "groupBoxTop";
            this.groupBoxTop.Size = new System.Drawing.Size(342, 216);
            this.groupBoxTop.TabIndex = 0;
            this.groupBoxTop.TabStop = false;
            this.groupBoxTop.Text = "中文文本内容";
            // 
            // groupBoxBottom
            // 
            this.groupBoxBottom.Controls.Add(this.textBoxBottom);
            this.groupBoxBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxBottom.Location = new System.Drawing.Point(0, 0);
            this.groupBoxBottom.Name = "groupBoxBottom";
            this.groupBoxBottom.Size = new System.Drawing.Size(342, 271);
            this.groupBoxBottom.TabIndex = 0;
            this.groupBoxBottom.TabStop = false;
            this.groupBoxBottom.Text = "英文文本内容";
            // 
            // textBoxTop
            // 
            this.textBoxTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTop.Location = new System.Drawing.Point(3, 17);
            this.textBoxTop.Multiline = true;
            this.textBoxTop.Name = "textBoxTop";
            this.textBoxTop.Size = new System.Drawing.Size(336, 196);
            this.textBoxTop.TabIndex = 0;
            // 
            // textBoxBottom
            // 
            this.textBoxBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxBottom.Location = new System.Drawing.Point(3, 17);
            this.textBoxBottom.Multiline = true;
            this.textBoxBottom.Name = "textBoxBottom";
            this.textBoxBottom.Size = new System.Drawing.Size(336, 251);
            this.textBoxBottom.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "MainForm";
            this.Text = "Alex.XP的ARMA2语言文档汉化辅助工具";
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            this.splitContainerLeft.ResumeLayout(false);
            this.splitContainerRight.Panel1.ResumeLayout(false);
            this.splitContainerRight.Panel2.ResumeLayout(false);
            this.splitContainerRight.ResumeLayout(false);
            this.splitContainerTextArea.Panel1.ResumeLayout(false);
            this.splitContainerTextArea.Panel2.ResumeLayout(false);
            this.splitContainerTextArea.ResumeLayout(false);
            this.groupBoxStringNodes.ResumeLayout(false);
            this.groupBoxStringArea.ResumeLayout(false);
            this.groupBoxTop.ResumeLayout(false);
            this.groupBoxTop.PerformLayout();
            this.groupBoxBottom.ResumeLayout(false);
            this.groupBoxBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerLeft;
        private System.Windows.Forms.SplitContainer splitContainerRight;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.SplitContainer splitContainerTextArea;
        private System.Windows.Forms.TreeView treeViewMain;
        private System.Windows.Forms.GroupBox groupBoxStringNodes;
        private System.Windows.Forms.ListBox listBoxMain;
        private System.Windows.Forms.GroupBox groupBoxPanel;
        private System.Windows.Forms.GroupBox groupBoxStringArea;
        private System.Windows.Forms.GroupBox groupBoxTop;
        private System.Windows.Forms.TextBox textBoxTop;
        private System.Windows.Forms.GroupBox groupBoxBottom;
        private System.Windows.Forms.TextBox textBoxBottom;
    }
}