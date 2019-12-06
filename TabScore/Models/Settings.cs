using System;
using System.Data.Odbc;
using System.Collections.Generic;

namespace TabScore.Models
{
    public enum SettingName
    {
        ShowResults,
        ShowPercentage,
        EnterLeadCard,
        ValidateLeadCard,
        ShowRanking,
        EnterResultsMethod,
        ShowHandRecord,
        NumberEntryEachRound,
        NameSource
    }

    public class SettingType
    {
        public readonly SettingName name;
        public readonly string databaseName;
        public readonly bool defaultValueBool;
        public readonly int defaultValueInt;

        public SettingType(SettingName name, string DbName, bool defaultValue)
        {
            this.name = name;
            databaseName = DbName;
            defaultValueBool = defaultValue;
        }

        public SettingType(SettingName name, string DbName, int defaultValue)
        {
            this.name = name;
            databaseName = DbName;
            defaultValueInt = defaultValue;
        }
    }

    public static class Settings
    {
        public static readonly List<SettingType> settingsList = new List<SettingType>()
        {
            new SettingType(SettingName.ShowResults,          "ShowResults",              true),
            new SettingType(SettingName.ShowPercentage,       "ShowPercentage",           true),
            new SettingType(SettingName.EnterLeadCard,        "LeadCard",                 true),
            new SettingType(SettingName.ValidateLeadCard,     "BM2ValidateLeadCard",      true),
            new SettingType(SettingName.ShowRanking,          "BM2Ranking",               1),
            new SettingType(SettingName.EnterResultsMethod,   "EnterResultsMethod",       1),
            new SettingType(SettingName.ShowHandRecord,       "BM2ViewHandRecord",        true),
            new SettingType(SettingName.NumberEntryEachRound, "BM2NumberEntryEachRound",  true),
            new SettingType(SettingName.NameSource,           "BM2NameSource",            0)
        };

        public static T GetSetting<T>(string DB, SettingName settingName)
        {
            T settingValue = default (T);
            SettingType setting = settingsList.Find(x => x.name.Equals(settingName));
            using (OdbcConnection connection = new OdbcConnection(DB))
            {
                connection.Open();
                object queryResult = null;
                string SQLString = $"SELECT {setting.databaseName} FROM Settings";
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                try
                {
                    ODBCRetryHelper.ODBCRetry(() =>
                    {
                        queryResult = cmd.ExecuteScalar();
                    });
                }
                finally
                {
                    cmd.Dispose();
                }
                if (settingValue is bool)
                {
                    if (queryResult == null)
                    {
                        settingValue = (T)(object)setting.defaultValueBool;
                    }
                    else
                    {
                        settingValue = (T)(object)Convert.ToBoolean(queryResult);
                    }
                }
                else
                {
                    if (queryResult == null)
                    {
                        settingValue = (T)(object)setting.defaultValueInt;
                    }
                    else
                    {
                        settingValue = (T)(object)Convert.ToInt32(queryResult);
                    }
                }
            }
            return settingValue;
        }
    }
}

