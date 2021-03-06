﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Noodle.IO
{
    /// <summary>
    /// Converts back and forth between string and path
    /// </summary>
    public class PathConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            var valueString = value as string;
            return valueString != null ? new Path(valueString) : base.ConvertFrom(context, culture, value);
        }
    }
}
