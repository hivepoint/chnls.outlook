﻿using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using chnls.Model;
using Newtonsoft.Json;

namespace chnls.Service
{
    partial class PropertiesService
    {
        private readonly Timer _timerWrite = new Timer();
        private const int TimerInterval = 500;
        private string _preferencesFile;

        protected void PropertiesDirty()
        {
            lock (_timerWrite)
            {
                if (_timerWrite.Enabled)
                {
                    return;
                }
                _timerWrite.Enabled = true;
            }
        }

        private void LoadProperties()
        {
            _timerWrite.Tick += timerWrite_Tick;
            _timerWrite.Interval = TimerInterval;
            try
            {
                var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                   "ChnlsAddIn");
                Directory.CreateDirectory(folder);
                _preferencesFile = Path.Combine(folder, "ChnlsAddIn.properties.json");

                if (File.Exists(_preferencesFile))
                {
                    var reader = File.OpenText(_preferencesFile);

                    var text = "";
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        text += line;
                    }
                    reader.Close();
                    if (text.Trim().Length > 0)
                    {
                        LoggingService.Debug("Properties file:\r\n" + text);
                        try
                        {
                            Properties = JsonConvert.DeserializeObject<ChnlsProperties>(text);
                        }
                        catch (Exception ex)
                        {
                            LoggingService.Error("Failed to load properties:" + _preferencesFile, ex);
                        }
                    }
                }
            }
            finally
            {
                if (null == Properties)
                {
                    Properties = new ChnlsProperties();
                    PropertiesDirty();
                    LoggingService.Info("Missing properties, initializing to default");
                }
                switch (Properties.BaseUrl)
                {
                    case Constants.UrlChnlsBeta:
                        break;
                    case Constants.UrlChnlsDev:
                        break;
                    case Constants.UrlChnlsProduction:
                        break;
                    default:
                        Properties.BaseUrl = Constants.UrlChnlsProduction;
                        break;
                }
            }
        }

        private void timerWrite_Tick(object sender, EventArgs e)
        {
            lock (_timerWrite)
            {
                _timerWrite.Stop();
            }
            string json;
            lock (Properties)
            {
                json = JsonConvert.SerializeObject(Properties,
                   Formatting.Indented,
                   new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            var writer = new StreamWriter(_preferencesFile, false, Encoding.UTF8);
            writer.WriteLine(json);
            writer.Flush();
            writer.Close();
            LoggingService.Info("Write properties: \n" + json);
        }
    }
}