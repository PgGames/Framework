using PG.SQLite;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




namespace PGServer
{
    public class GamesServer
    {

        private SQLiteDate m_SQLData;
        //数据库地址
        private string m_SQL_Path;
        //数据库名称
        private string m_SQL_Name = "/LoginServer.db";

        public void Init()
        {
            m_SQL_Path = Application.streamingAssetsPath;
            if (!Directory.Exists(m_SQL_Path))
                Directory.CreateDirectory(m_SQL_Path);
            m_SQLData = new SQLiteDate("data source=" + m_SQL_Path + m_SQL_Name);

            CreateTable();
        }


        #region 创建表格

        /// <summary>
        /// 创建表格
        /// </summary>
        private void CreateTable()
        {
            CreateUserTable();
        }

        /// <summary>
        /// 创建用户战斗信息表
        /// </summary>
        protected void CreateUserTable()
        {
            if (!m_SQLData.ExistsTable("UserCombatInfo"))
            {
                //创建用户信息表
                //表的结构
                /*
                 * 表的字段      |  字段含义          字段类型
                 * ---------------------------------------
                 * UserID        |  用户标识    |       int(表的主键)
                 * EXP           |  经验值      |       long(默认为0，)
                 * Lv            |  等级        |       long(默认为1，)
                 * JOB           |  职业        |       byte(默认为0，无职业)
                 * 
                 * //装备属性（活动属性）
                 * 
                 * ATK           |  攻击力      |       long(默认为0，)
                 * DEF           |  防御力      |       long(默认为0，)
                 * HP            |  生命值      |       long(默认为0，)
                 * MP            |  魔法值      |       long(默认为0，)
                 * 
                 * //基础属性（固定属性+已分配的自由属性）
                 * 
                 * POWER        |力量属性（每级自动加1）| 1力量 = 10     攻击
                 * HABITUS      |体质属性（每级自动加1）| 1体质 = 100    HP
                 * SPIRIT       |精神属性（每级自动加1）| 1精神 = 50     MP
                 * AGILITY      |敏捷属性（每级自动加1）| 1敏捷 = 10     防御
                 * 
                 * //自由属性（每级获得3个自由属性点，初始属性点有10个）
                 * ATTRIBUTE    |自由属性（每级自动加3）| 用于分配至基础属性上得到属性加成
                 */
                m_SQLData.CreateTable("UserCombatInfo", new string[] {
                    "UserID",
                    "EXP",
                    "Lv",
                    "JOB",

                    "ATK",
                    "DEF",
                    "HP",
                    "MP",

                    "POWER",
                    "HABITUS",
                    "SPIRIT",
                    "AGILITY",

                    "ATTRIBUTE",
                }, new string[] {
                    "INTEGER PRIMARY KEY",
                    "INTEGER DEFAULT 0",
                    "INTEGER DEFAULT 1",
                    "INTEGER DEFAULT 0",
                    //装备属性
                    "INTEGER DEFAULT 0",
                    "INTEGER DEFAULT 0",
                    "INTEGER DEFAULT 0",
                    "INTEGER DEFAULT 0",
                    //基础属性
                    "INTEGER DEFAULT 1",
                    "INTEGER DEFAULT 1",
                    "INTEGER DEFAULT 1",
                    "INTEGER DEFAULT 1",
                    //自由属性
                    "INTEGER DEFAULT 10",
                });
            }
        }
        /// <summary>
        /// 创建职业信息表
        /// </summary>
        protected void CreateJobTable()
        {
            if (!m_SQLData.ExistsTable("UserJobInfo"))
            {
                /*
                 * 表的字段      |  字段含义          字段类型
                 * ---------------------------------------
                 * JobID         | 职业标识 |       byte(表的主键,自增)
                 * 
                 * POWER         |力量成长（10为满值）|byte(默认为0)
                 * HABITUS       |体质成长（10为满值）|byte(默认为0)
                 * SPIRIT        |精神成长（10为满值）|byte(默认为0) 
                 * AGILITY       |敏捷成长（10为满值）|byte(默认为0)
                 */
                m_SQLData.CreateTable("UserJobInfo", new string[] {
                    "JobID",
                    "POWER",
                    "HABITUS",
                    "SPIRIT",
                    "AGILITY",
                }, new string[] {
                    "INTEGER NOT NULL PRIMARY KEY",
                    "INTEGER DEFAULT 0",
                    "INTEGER DEFAULT 0",
                    "INTEGER DEFAULT 0",
                    "INTEGER DEFAULT 0",
                });
                m_SQLData.InsertInfo("UserJobInfo", "(POWER,HABITUS,SPIRIT,AGILITY) VALUES (9,9,8,5)");         //战士
                m_SQLData.InsertInfo("UserJobInfo", "(POWER,HABITUS,SPIRIT,AGILITY) VALUES (7,10,7,8)");        //肉
                m_SQLData.InsertInfo("UserJobInfo", "(POWER,HABITUS,SPIRIT,AGILITY) VALUES (8,6,10,3)");        //法师
                m_SQLData.InsertInfo("UserJobInfo", "(POWER,HABITUS,SPIRIT,AGILITY) VALUES (10,6,6,3)");        //弓手
            }
        }
        #endregion
        
    }
}