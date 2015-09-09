﻿using System;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ssi.TrackTruck.Web.Utils
{
    public sealed class JsonDotNetValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
                return null;

            var reader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            var bodyText = reader.ReadToEnd();

            return String.IsNullOrEmpty(bodyText) ? null : new DictionaryValueProvider<object>(JsonConvert.DeserializeObject<ExpandoObject>(bodyText, new ExpandoObjectConverter()), CultureInfo.CurrentCulture);
        }
    }
}