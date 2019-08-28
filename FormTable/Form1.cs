using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace FormTable
{
    public partial class Form1 : Form
    {
        LAS_reader hd = new LAS_reader();

        public Form1()
        {
            InitializeComponent();
            //var hd = new LAS_reader();
            hd.LoadFile("gti.las");
            dgvload();
            //dataGridView1.DataSource = GetDataTable(hd.dataValue, hd.dataValue_Count);
            
        }

        DataTable table;
        private BindingSource bindingSource = new BindingSource();


        private void dgvload()
        {
            table = new DataTable();

            DataColumn c = table.Columns.Add("Ключ", typeof(String));
            c.AutoIncrement = true;
            c.AutoIncrementSeed = 0;
            c.AutoIncrementStep = 1;

            for (int i = 0; i<hd.CurveInfo_Count;i++)
            {
                table.Columns.Add(hd.CurveInfo[i]._name, typeof(string));
            }

            table.PrimaryKey = new DataColumn[] { table.Columns[0] };

            int length1 = hd.CurveInfo_Count;
            int length2 = hd.dataValue.Length;
            //for (int i = 0; i < length1; i++)
            //    table.Columns.Add();
            for (int i = 0; i < length2; i++)
            {
                table.Rows.Add(table.NewRow());
                for (int j = 0; j < length1; j++)
                    table.Rows[i][j+1] = hd.dataValue[i]._dataValue[j];
            }

            bindingSource.DataSource = table;
            dataGridView1.DataSource = bindingSource;
        }

        private void addRowinTable()
        {
            int i = 0;
            while (i<hd.dataValue_Count)
            {
                //Object[] newRow = (object[])hd.dataValue[i]._dataValue;
            }

            //table.LoadDataRow(newRow, false);
        }

        DataTable GetDataTable(LAS_reader.logDataValue[] array, int count)
        {
            DataTable table = new DataTable();
            int length1 = array[0]._dataValue.Count;
            int length2 = array.Length;
            for (int i = 0; i < length1; i++)
                table.Columns.Add();
            for (int i = 0; i < length2; i++)
            {
                table.Rows.Add(table.NewRow());
                for (int j = 0; j < length1; j++)
                    table.Rows[i][j] = array[i]._dataValue[j];
            }
            return table;
        }
    }
}
