using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace AutoCompleteDataGridViewWidgets
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void autoCompleteDataGridView1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder()
            {
                Provider = "Microsoft.Jet.OLEDB.4.0",
                DataSource = System.IO.Directory.GetCurrentDirectory()+"\\database.accdb",
            };
            builder.Add("Jet OLEDB:Engine Type", 5);
            var catalog = new ADOX.Catalog();
            var table = new ADOX.Table();
            table.Name = "DataTableSample";
            table.Columns.Append("Column");
            table.Columns.Append("Column1");
            table.Columns.Append("Column2");
            catalog.Create(builder.ConnectionString);
            catalog.Tables.Append(table);
            var connection = catalog.ActiveConnection as ADODB.Connection;
            if (connection != null)
                connection.Close();
        }
    }
}
