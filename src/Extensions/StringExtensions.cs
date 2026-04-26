/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Text.RegularExpressions;

namespace InventorySimulator;

public static partial class StringExtensions
{
    extension(string self)
    {
        public string ApplyColors()
        {
            return self.Replace("{", "[").Replace("}", "]");
        }

        public string StripColors()
        {
            return ColorTag().Replace(self, "");
        }
    }

    [GeneratedRegex(@"\{.*?\}")]
    private static partial Regex ColorTag();
}
