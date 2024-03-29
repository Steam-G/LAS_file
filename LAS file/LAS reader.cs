﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LAS_file
{
    public class LAS_reader
    {
        //Работа с файлом
        //public string filepath;
        
        
        public void LoadFile(string filepath)
        {
            List<string> strInfo = new List<string>();
            List<string> strLogData = new List<string>();
            
            string s;
            int nRazdel = 0;

            using (var f = new StreamReader(filepath, Encoding.GetEncoding(866)))
            {
                var dataF = f;

                s = f.ReadLine();
                while (!f.EndOfStream)
                {
                    // определим текущий раздел данных
                    if (getNRazdel(s) < 255) nRazdel = getNRazdel(s);
                    
                    //if (s == "~ASCII Log Data") valstr = true;
                    s = f.ReadLine();
                    if (nRazdel == 1)
                    {
                        if (s.StartsWith("~")) continue; // строки заголовков пропускаем
                        if (s.StartsWith("#")) continue; // закомментированные строки надо пропускать

                        // что-нибудь делаем с прочитанной строкой s
                        createWell(s);
                    }

                    if (nRazdel == 2)
                    {
                        if (s.StartsWith("~")) continue; // строки заголовков пропускаем
                        if (s.StartsWith("#")) continue; // закомментированные строки надо пропускать

                        // что-нибудь делаем с прочитанной строкой s
                        createCurveInfo(s);
                    }
                    if (nRazdel == 4)
                    {
                        if (s.StartsWith("~")) continue; // строки заголовков пропускаем
                        if (s.StartsWith("#")) continue; // закомментированные строки надо пропускать

                        // что-нибудь делаем с прочитанной строкой s
                        readLogData(s);
                        //strLogData.Add(s);
                    }
                } 
            }
        }

        private int getNRazdel(string s)
        {
            //List<string> lasLabelRazdel = new List<string>() { "~W", "~C", "~O", "~A" };
            if (s.StartsWith("~W")) return 1;
            if (s.StartsWith("~C")) return 2;
            if (s.StartsWith("~O")) return 3;
            if (s.StartsWith("~A")) return 4;
            return 255;
        }
        

        //-------------------------------------------------
        public struct headWell
        {
            public string _name;
            public string _units;
            public string _data;
            public string _description;
        }

        public headWell[] well;
        public int well_Count = 0;
        public void createWell(string stroka)
        {
            if (stroka.IndexOf(' ') != -1)
            {

                Array.Resize<headWell>(ref well, well_Count + 1);
                well[well_Count]._name = stroka.Substring(0, stroka.IndexOf('.'));
                well[well_Count]._units = stroka.Substring(stroka.IndexOf('.') + 1, stroka.IndexOf(' ', stroka.IndexOf('.')) - stroka.IndexOf('.') - 1);
                well[well_Count]._data = stroka.Substring(stroka.IndexOf(' ', stroka.IndexOf('.') + 1), stroka.IndexOf(':') - stroka.IndexOf(' ', stroka.IndexOf('.') + 1));
                well[well_Count]._description = stroka.Substring(stroka.IndexOf(':') + 1, stroka.Length - stroka.IndexOf(':') - 1);

                //чистка пробелов вначале и в конце строк
                well[well_Count]._name = well[well_Count]._name.TrimStart(' ');
                well[well_Count]._name = well[well_Count]._name.TrimEnd(' ');
                well[well_Count]._units = well[well_Count]._units.TrimStart(' ');
                well[well_Count]._units = well[well_Count]._units.TrimEnd(' ');
                well[well_Count]._data = well[well_Count]._data.TrimStart(' ');
                well[well_Count]._data = well[well_Count]._data.TrimEnd(' ');
                well[well_Count]._description = well[well_Count]._description.TrimStart(' ');
                well[well_Count]._description = well[well_Count]._description.TrimEnd(' ');

                well_Count++;
            }
        }


        /// <summary>
        /// #--- НАЗВАНИЯ КРИВЫХ ---# - название кривой/ед.измерения/информация/описание кривой
        /// </summary>
        public struct headCurveInfo
        {
            public string _name;
            public string _units;
            public string _data;
            public string _description;
        }

        public headCurveInfo[] CurveInfo;
        public int CurveInfo_Count = 0;
        /// <summary>
        /// Заполняет массив данными о НАЗВАНИИ КРИВЫХ из файла LAS, расположенные в блоке ~Curve
        /// </summary>
        /// <param name="stroka">Принимает текстовую строку из файла, данные будут добавлены в массив CurveInfo[]</param>
        public void createCurveInfo(string stroka)
        {
            if (stroka.IndexOf(' ') != -1)
            {

                Array.Resize<headCurveInfo>(ref CurveInfo, CurveInfo_Count + 1);
                CurveInfo[CurveInfo_Count]._name = stroka.Substring(0, stroka.IndexOf('.'));
                CurveInfo[CurveInfo_Count]._units = stroka.Substring(stroka.IndexOf('.') + 1, stroka.IndexOf(' ', stroka.IndexOf('.')) - stroka.IndexOf('.') - 1);
                CurveInfo[CurveInfo_Count]._data = stroka.Substring(stroka.IndexOf(' ', stroka.IndexOf('.') + 1), stroka.IndexOf(':') - stroka.IndexOf(' ', stroka.IndexOf('.') + 1));
                CurveInfo[CurveInfo_Count]._description = stroka.Substring(stroka.IndexOf(':') + 1, stroka.Length - stroka.IndexOf(':') - 1);

                //чистка пробелов вначале и в конце строк
                CurveInfo[CurveInfo_Count]._name = CurveInfo[CurveInfo_Count]._name.TrimStart(' ');
                CurveInfo[CurveInfo_Count]._name = CurveInfo[CurveInfo_Count]._name.TrimEnd(' ');
                CurveInfo[CurveInfo_Count]._units = CurveInfo[CurveInfo_Count]._units.TrimStart(' ');
                CurveInfo[CurveInfo_Count]._units = CurveInfo[CurveInfo_Count]._units.TrimEnd(' ');
                CurveInfo[CurveInfo_Count]._data = CurveInfo[CurveInfo_Count]._data.TrimStart(' ');
                CurveInfo[CurveInfo_Count]._data = CurveInfo[CurveInfo_Count]._data.TrimEnd(' ');
                CurveInfo[CurveInfo_Count]._description = CurveInfo[CurveInfo_Count]._description.TrimStart(' ');
                CurveInfo[CurveInfo_Count]._description = CurveInfo[CurveInfo_Count]._description.TrimEnd(' ');

                CurveInfo_Count++;
            }
        }

        public struct logDataValue
        {
            public List<string> _dataValue;
        }

        public logDataValue[] dataValue;
        public int dataValue_Count = 0;

        public void readLogData(string stroka)
        {
            if (stroka.IndexOf(' ') != -1)
            {
                Array.Resize<logDataValue>(ref dataValue, dataValue_Count + 1);
                List<string> dtVal = new List<string>();

                for (int i = 0; i < CurveInfo_Count; i++)
                {
                    dtVal.Add(stroka.Substring(1, stroka.IndexOf(' ')));
                }

                string[] words = stroka.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                dataValue[dataValue_Count]._dataValue = new List<string>();
                dataValue[dataValue_Count]._dataValue.AddRange(words);

                dataValue_Count++;
            }
        }
    
}
}
