using LAS_file;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
            paintChart();
            dgvload(); //заполнение таблицы. временно откл, работает
            //dataGridView1.DataSource = GetDataTable(hd.dataValue, hd.dataValue_Count);
            
        }

        DataTable table;
        private BindingSource bindingSource = new BindingSource();


        //работа с таблицей
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

            // заполняем таблицу
            for (int i = 0; i < length2; i++)
            {
                table.Rows.Add(table.NewRow());
                for (int j = 0; j < length1; j++)
                {
                    if (j == 1)
                    {
                        //перевожу Unix Timestamp в представление типа DateTime
                        DateTime dateTime = ConvertFromUnixTimestamp(double.Parse(hd.dataValue[i]._dataValue[j], CultureInfo.InvariantCulture));
                        //теперь привожу полученную переменную даты и времени в текст, вида "дд.мм.гггг чч:мм:сс"
                        string sDT = dateTime.ToString("G", CultureInfo.CreateSpecificCulture("de-DE"));
                        //кладу полученное значение в таблицу
                        table.Rows[i][j + 1] = sDT;
                        continue;
                    }
                    table.Rows[i][j + 1] = (string)hd.dataValue[i]._dataValue[j];
                }
            }

            bindingSource.DataSource = table;
            dataGridView1.DataSource = bindingSource;
        }

        //Конвертирование Unix Timestamp в DateTime
        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        //Обратное конвертирование DateTime в Unix Timestamp
        static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }



        private void paintChart()
        {
            int length2 = hd.dataValue.Length;
            for (int i = 0; i < length2; i++)
            {
                chart1.Series[0].Points.AddXY(double.Parse(hd.dataValue[i]._dataValue[3], CultureInfo.InvariantCulture), i ); // 
                chart1.Series[1].Points.AddXY(double.Parse(hd.dataValue[i]._dataValue[2], CultureInfo.InvariantCulture), i); //
                chart1.Series[2].Points.AddXY(double.Parse(hd.dataValue[i]._dataValue[4], CultureInfo.InvariantCulture), i); //
            }
            //chart1.Series[0].Points.AddXY(0, 5);
            //chart1.Series[0].Points.AddXY(1, 7);
            //chart1.Series[0].Points.AddXY(2, 2);
            //chart1.Series[0].Points.AddXY(3, 6);
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
