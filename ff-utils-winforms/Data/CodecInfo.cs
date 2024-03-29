﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.Data
{
    class CodecInfo
    {
        public string Name;
        public string FriendlyName;
        public string EncoderName;
        public string[] Presets;
        public int PresetDef = 0;
        public string PInfo = "";
        public string[] ColorFormats;
        public int ColorFormatDef = 0;
        public int QMin;
        public int QMax;
        public int QDefault = -1;
        public string QInfo = "";

        public CodecInfo()
        {
            
        }

        public CodecInfo(string name, string frName, string[] presets, int presetDef, string[] fmts, int fmtDef, int qMin, int qMax, int qDef, string qInfo = "", string pInfo = "")
        {
            Name = name;
            FriendlyName = frName;
            Presets = presets;
            PresetDef = presetDef;
            ColorFormats = fmts;
            ColorFormatDef = fmtDef;
            QMin = qMin;
            QMax = qMax;
            QDefault = qDef;
            QInfo = qInfo;
            PInfo = pInfo;
        }
    }
}
