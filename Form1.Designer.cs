namespace AutoCompleteDataGridViewWidgets
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.autoCompleteDataGridView1 = new AutoCompleteDataGridViewWidgets.AutoCompleteDataGridView();
            this.SuspendLayout();
            // 
            // autoCompleteDataGridView1
            // 
            this.autoCompleteDataGridView1.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.autoCompleteDataGridView1.Location = new System.Drawing.Point(16, 24);
            this.autoCompleteDataGridView1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.autoCompleteDataGridView1.Name = "autoCompleteDataGridView1";
            this.autoCompleteDataGridView1.Size = new System.Drawing.Size(788, 507);
            this.autoCompleteDataGridView1.TabIndex = 0;
            this.autoCompleteDataGridView1.Load += new System.EventHandler(this.autoCompleteDataGridView1_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 546);
            this.Controls.Add(this.autoCompleteDataGridView1);
            this.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private AutoCompleteDataGridView autoCompleteDataGridView1;
    }
}

