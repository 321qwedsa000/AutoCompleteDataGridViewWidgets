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
        private bool first_time { get; set; } = true;
        public AutoCompleteDataGridView()
        {
            InitializeComponent();
            keyValueGridView.Width = Math.Min(mainDataGridView.Width,keyValueWidth);
            DataTable table = new DataTable();
            table.Columns.AddRange(new DataColumn[] { new DataColumn("Column3"), new DataColumn("Column4"), new DataColumn("Column5") });
            table.Rows.Add("Amy", "Abby", "Sandy");
            table.Rows.Add("Abby", "Amy", "Sandy");
            table.Rows.Add("Abbz", "Amy", "Sandy");
            table.Rows.Add("Abba", "Amy", "Sandy");
            table.Rows.Add("Abbb", "Amy", "Sandy");

            keyBindingSource.DataSource = table;
            keyValueGridView.DataSource = null;
            keyValueGridView.Columns.Clear();
            keyValueGridView.DataSource = keyBindingSource.DataSource;
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
            
            //debug Information
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name},row={mainDataGridView.CurrentCell?.RowIndex},column={mainDataGridView.CurrentCell?.ColumnIndex}");
            //debug Information
            if (mainDataGridView.CurrentCell.OwningColumn.Name == primaryKey)
            {
                if (!(e.Control is DataGridViewTextBoxEditingControl)) throw new Exception("Primary Key Column's control must be TextBox");
                var textBox = mainDataGridView.EditingControl as DataGridViewTextBoxEditingControl;
                if (textBox != null)
                {
                    textBox.TextChanged -= TextBox_TextChanged;
                    textBox.KeyPress -= TextBox_KeyPress;
                    textBox.TextChanged += TextBox_TextChanged;
                    textBox.KeyPress += TextBox_KeyPress;
                }
                keyValueGridView.Height = keyValueHeightScale * textBox.Height;
                textBox = null;
            } else
            {
                keyValueGridView.Visible = false;
            }
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            //debug Information
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name},row={mainDataGridView.CurrentCell?.RowIndex},column={mainDataGridView.CurrentCell.OwningColumn.Name == primaryKey}");
            //debug Information
            if (mainDataGridView.CurrentCell.OwningColumn.Name == primaryKey)
            {
                keyValueGridView.Visible = true;
                var textBox = (TextBox)sender;
                Debug.WriteLine(textBox.Text);
                var filterView = (DataTable)keyValueGridView.DataSource;
                var view = filterView?.DefaultView;
                Debug.WriteLine(view == null);
                if (view == null) return;
                view.Sort = $"{autocompleteKey} asc";
                view.RowFilter = $"{autocompleteKey} like '%{textBox.Text}%'";
                keyBindingSource.DataSource = view.ToTable();
            }
        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(mainDataGridView.CurrentCell.OwningColumn.Name == primaryKey)
            {
                Debug.WriteLine(e.KeyChar);
                if(e.KeyChar == '*')
                {
                    keyValueGridView.Focus();
                    keyValueGridView.Visible = true;
                    keyValueGridView.Focus();
                }
            }
        }

        private void mainDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            mainDataGridView.EndEdit();
            //debug Information
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name},row={mainDataGridView.CurrentCell?.RowIndex},column={mainDataGridView.CurrentCell?.ColumnIndex}");
            //debug Information
            keyValueGridView.Visible = false;
            if(first_time)
            {
                first_time = false;
                return;
            }
            if(changed)
            {
                mainDataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
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
                if(!mainDataGridView.CurrentCell.ReadOnly)
                    changed = false;
            } else
            {
                if(mainDataGridView.CurrentCell != null)
                mainDataGridView.EditMode = DataGridViewEditMode.EditOnKeystroke;
            }
        }

        private void mainDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //debug Information
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name},row={mainDataGridView.CurrentCell?.RowIndex},column={mainDataGridView.CurrentCell?.ColumnIndex}");
            //debug Information
           
            if (mainDataGridView.CurrentCell != null)
            {
                mainDataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
                changed = true;
            }
                
        }

        private void mainDataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show($"您確定要刪除第{e.Row.Index+1}列資料嗎？", "詢問", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        }

        private void mainDataGridView_EditModeChanged(object sender, EventArgs e)
        {
            if(mainDataGridView.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                mainDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            } else
            {
                mainDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void keyValueGridView_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyData)
            {
                case Keys.Enter:
                    mainDataGridView.CurrentCell.Value = keyValueGridView.SelectedRows[0].Cells[autocompleteKey].Value;
                    mainDataGridView.Focus();
                    keyValueGridView.Visible = false;

                    break;
                case Keys.Escape:
                    mainDataGridView.Focus();
                    keyValueGridView.Visible = false;
                    break;
            }
        }
    }
}
