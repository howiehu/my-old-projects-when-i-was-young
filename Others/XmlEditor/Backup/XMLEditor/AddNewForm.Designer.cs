namespace XmlEditor
{
    partial class AddNewForm
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
            this.ultraButtonOK = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonCancel = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTextEditor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraButtonOK
            // 
            this.ultraButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ultraButtonOK.Location = new System.Drawing.Point(12, 61);
            this.ultraButtonOK.Name = "ultraButtonOK";
            this.ultraButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ultraButtonOK.TabIndex = 0;
            this.ultraButtonOK.Text = "确定";
            // 
            // ultraButtonCancel
            // 
            this.ultraButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButtonCancel.Location = new System.Drawing.Point(205, 61);
            this.ultraButtonCancel.Name = "ultraButtonCancel";
            this.ultraButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ultraButtonCancel.TabIndex = 1;
            this.ultraButtonCancel.Text = "取消";
            // 
            // ultraLabel
            // 
            this.ultraLabel.AutoSize = true;
            this.ultraLabel.Location = new System.Drawing.Point(12, 12);
            this.ultraLabel.Name = "ultraLabel";
            this.ultraLabel.Size = new System.Drawing.Size(140, 16);
            this.ultraLabel.TabIndex = 2;
            this.ultraLabel.Text = "请输入新节点的关键字：";
            // 
            // ultraTextEditor
            // 
            this.ultraTextEditor.Location = new System.Drawing.Point(12, 34);
            this.ultraTextEditor.Name = "ultraTextEditor";
            this.ultraTextEditor.Size = new System.Drawing.Size(268, 21);
            this.ultraTextEditor.TabIndex = 3;
            // 
            // AddNewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 95);
            this.Controls.Add(this.ultraTextEditor);
            this.Controls.Add(this.ultraLabel);
            this.Controls.Add(this.ultraButtonCancel);
            this.Controls.Add(this.ultraButtonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AddNewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加节点";
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton ultraButtonOK;
        private Infragistics.Win.Misc.UltraButton ultraButtonCancel;
        private Infragistics.Win.Misc.UltraLabel ultraLabel;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditor;
    }
}