﻿using System;
using System.Text;
using System.Xml;
using NewLife.CommonEntity;
using NewLife.Serialization;
using NewLife.Xml;
using NewLife.Log;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Test
{
    /// <summary>
    /// 序列化测试
    /// </summary>
    public static class SerialTest
    {
        /// <summary>
        /// 开始
        /// </summary>
        public static void Start()
        {
            //OldBinaryTest();
            BinaryTest();
        }

        static void OldBinaryTest()
        {
            TraceStream ts = new TraceStream();
            ts.UseConsole = true;

            BinaryFormatter bf = new BinaryFormatter();

            Administrator entity = GetDemo();

            bf.Serialize(ts, entity);

            Byte[] buffer = ts.ToArray();
            Console.WriteLine(BitConverter.ToString(buffer));

            bf = new BinaryFormatter();
            ts.Position = 0;
            entity = bf.Deserialize(ts) as Admin;
            Console.WriteLine(entity != null);
        }

        /// <summary>
        /// 二进制序列化测试
        /// </summary>
        public static void BinaryTest()
        {
            TraceStream ts = new TraceStream();
            ts.UseConsole = true;

            BinaryWriterX writer = new BinaryWriterX();
            writer.Stream = ts;
            //writer.IsLittleEndian = false;
            //writer.EncodeInt = true;
            writer.Settings.DateTimeFormat = SerialSettings.DateTimeFormats.Seconds;
            writer.SplitGenericType = true;
            writer.Settings.IgnoreName = false;
            writer.Settings.IgnoreType = false;

            Administrator entity = GetDemo();

            writer.WriteObject(entity);

            Byte[] buffer = writer.ToArray();
            Console.WriteLine(BitConverter.ToString(buffer));

            BinaryReaderX reader = new BinaryReaderX();
            reader.Stream = writer.Stream;
            reader.Stream.Position = 0;
            //reader.EncodeInt = true;
            reader.SplitGenericType = true;
            reader.Settings = writer.Settings;

            Administrator admin = new Admin();
            Object obj = admin;
            reader.ReadObject(null, ref obj);
            Console.WriteLine(obj != null);

            //reader.ReadObject(typeof(Administrator));
        }

        /// <summary>
        /// Xml序列化测试
        /// </summary>
        public static void XmlTest()
        {
            TraceStream ts = new TraceStream();
            ts.UseConsole = true;

            XmlWriterX writer = new XmlWriterX();
            writer.Stream = ts;
            writer.MemberAsAttribute = false;
            writer.IgnoreDefault = false;

            Administrator entity = GetDemo();

            writer.WriteObject(entity);

            writer.Flush();
            Byte[] buffer = writer.ToArray();
            Console.WriteLine(Encoding.UTF8.GetString(buffer));

            #region 测试
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            writer.Stream.Position = 0;
            XmlReader xr = XmlReader.Create(writer.Stream, settings);
            while (xr.Read())
            {
                Console.WriteLine("{0}, {1}={2}", xr.NodeType, xr.Name, xr.Value);
            }
            #endregion

            XmlReaderX reader = new XmlReaderX();
            reader.Stream = writer.Stream;
            reader.Stream.Position = 0;
            reader.MemberAsAttribute = writer.MemberAsAttribute;
            reader.IgnoreDefault = writer.IgnoreDefault;

            Administrator admin = new Admin();
            Object obj = admin;
            reader.ReadObject(null, ref obj);
            Console.WriteLine(obj != null);
        }

        public static void JsonTest()
        {
            Console.Clear();

            TraceStream ts = new TraceStream();
            //ts.UseConsole = true;

            JsonWriter writer = new JsonWriter();
            writer.Stream = ts;

            Administrator entity = GetDemo();

            writer.WriteObject(entity);

            Console.WriteLine(writer.ToString());

            NewLife.IO.Json json = new NewLife.IO.Json();
            Console.WriteLine(json.Serialize(entity));

            JsonReader reader = new JsonReader();
            reader.Stream = writer.Stream;
            reader.Stream.Position = 0;

            Administrator admin = new Admin();
            Object obj = admin;
            reader.ReadObject(null, ref obj);
            Console.WriteLine(obj != null);
        }

        static Administrator GetDemo()
        {
            Admin entity = new Admin();
            entity.ID = 123;
            entity.Name = "nnhy";
            entity.DisplayName = "大石头";
            entity.Logins = 65535;
            entity.LastLogin = DateTime.Now;
            entity.SSOUserID = 555;

            Department dp = new Department();
            dp.ID = 1;
            dp.Name = "部门一";

            Department dp2 = new Department();
            dp2.ID = 2;
            dp2.Name = "部门二";

            entity.DP1 = dp;
            entity.DP2 = dp2;
            entity.DP3 = dp;

            entity.DPS = new Department[] { dp, dp2, dp };

            entity.LPS = new List<Department>(entity.DPS);

            entity.PPS = new Dictionary<string, Department>();
            entity.PPS.Add("aa", dp);
            entity.PPS.Add("bb", dp2);

            entity.SPS = new SortedList<string, Department>(entity.PPS);

            return entity;
        }

        [Serializable]
        class Admin : Administrator
        {
            private Department _DP1;
            /// <summary>属性说明</summary>
            public Department DP1
            {
                get { return _DP1; }
                set { _DP1 = value; }
            }

            private Department _DP2;
            /// <summary>属性说明</summary>
            public Department DP2
            {
                get { return _DP2; }
                set { _DP2 = value; }
            }

            private Department _DP3;
            /// <summary>属性说明</summary>
            public Department DP3
            {
                get { return _DP3; }
                set { _DP3 = value; }
            }

            private Department[] _DPS;
            /// <summary>属性说明</summary>
            public Department[] DPS
            {
                get { return _DPS; }
                set { _DPS = value; }
            }

            private List<Department> _LPS;
            /// <summary>属性说明</summary>
            public List<Department> LPS
            {
                get { return _LPS; }
                set { _LPS = value; }
            }

            private Dictionary<String, Department> _PPS;
            /// <summary>字典</summary>
            public Dictionary<String, Department> PPS
            {
                get { return _PPS; }
                set { _PPS = value; }
            }

            private SortedList<String, Department> _SPS;
            /// <summary>属性说明</summary>
            public SortedList<String, Department> SPS
            {
                get { return _SPS; }
                set { _SPS = value; }
            }
        }

        [Serializable]
        class Department
        {
            private Int32 _ID;
            /// <summary>属性说明</summary>
            public Int32 ID
            {
                get { return _ID; }
                set { _ID = value; }
            }

            private String _Name;
            /// <summary>属性说明</summary>
            public String Name
            {
                get { return _Name; }
                set { _Name = value; }
            }

            public override string ToString()
            {
                return String.Format("{0}, {1}", ID, Name);
            }
        }
    }
}