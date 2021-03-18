using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoCompleteDataGridViewWidgets
{
    using System.Diagnostics;
    using System.Reflection;
    public partial class AutoCompleteDataGridView : UserControl
    {
        private bool changed { get; set; } = false;
        public AutoCompleteDataGridView()
        {
            InitializeComponent();
            keyValueGridView.Width = Math.Min(mainDataGridView.Width,keyValueWidth);
            
        }

        private void keyValueGridView_VisibleChanged(object sender, EventArgs e)
        {
            if (!keyValueGridView.Visible) return;
            //debug Information
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name},row={mainDataGridView.CurrentCell?.RowIndex},column={mainDataGridView.CurrentCell?.ColumnIndex}");
            //debug Information
            if(mainDataGridView.CurrentCell?.OwningColumn.Name == primaryKey)
            {
                var cur_ = mainDataGridView.CurrentCell;
                var rect = mainDataGridView.GetCellDisplayRectangle(cur_.ColumnIndex, cur_.RowIndex, false);
                int x = rect.Left + mainDataGridView.Left;
                int y = rect.Bottom + mainDataGridView.Top;
                if (y + keyValueGridView.Height > mainDataGridView.Bottom)
                {
                    y -= keyValueGridView.Height + rect.Height;
                }
                keyValueGridView.Location = new Point(x, y);
            }
        }

        private void mainDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            mainDataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            //debug Information
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name},row={mainDataGridView.CurrentCell?.RowIndex},column={mainDataGridView.CurrentCell?.ColumnIndex}");
            //debug Information
            if (mainDataGridView.CurrentCell.OwningColumn.Name == primaryKey)
            {
                if (!(e.Control is DataGridViewTextBoxEditingControl)) throw new Exception("Primary Key Column's control must be TextBox");
                var textBox = e.Control as DataGridViewTextBoxEditingControl;
                keyValueGridView.Visible = true;
                keyValueGridView.Height = keyValueHeightScale * textBox.Height;
            } else
            {
                keyValueGridView.Visible = false;
            }
        }

        private void mainDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            keyValueGridView.Visible = false;
            if(changed)
            {
                //debug Information
                Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name},row={mainDataGridView.RowCount},column={mainDataGridView.ColumnCount}");
                //debug Information
                var cur_ = mainDataGridView.CurrentCell;
                var rowIndex = mainDataGridView.RowCount;
                var colIndex = mainDataGridView.ColumnCount;
                if(cur_.ColumnIndex + 1 == colIndex)
                {
                    if (cur_.RowIndex == rowIndex) return;
                    mainDataGridView.CurrentCell = mainDataGridView[0, cur_.RowIndex];
                }else
                {
                    mainDataGridView.CurrentCell = mainDataGridView[cur_.ColumnIndex+1,Math.Max(cur_.RowIndex - 1,0)];
                }
                changed = false;
                mainDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;

            } else
            {
                mainDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                mainDataGridView.EditMode = DataGridViewEditMode.EditOnKeystroke;
            }
        }

        private void mainDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //debug Information
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name},row={mainDataGridView.CurrentCell?.RowIndex},column={mainDataGridView.CurrentCell?.ColumnIndex}");
            //debug Information
            if(mainDataGridView.CurrentCell != null)
                changed = true;
        }

        private void mainDataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show($"您確定要刪除第{e.Row.Index+1}列資料嗎？", "詢問", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        }
    }
}
