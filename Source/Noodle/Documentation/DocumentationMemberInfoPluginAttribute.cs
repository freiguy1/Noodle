﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noodle.Documentation
{
    /// <summary>
    /// Derived <see cref="DocumentationMemberInfoPlugin"/> types must be decoriated with this attibute to be found by the plugin system
    /// </summary>
    public class DocumentationMemberInfoPluginAttribute : Plugins.BasePluginAttribute
    {
    }
}
