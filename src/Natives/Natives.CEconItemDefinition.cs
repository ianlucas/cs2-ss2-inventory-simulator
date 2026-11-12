/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace InventorySimulator;

public static partial class Natives
{
    public static readonly int CEconItemDefinition_m_nDefIndex = GetOffset(
        "CEconItemDefinition::m_nDefIndex"
    );

    public static readonly int CEconItemDefinition_m_pszDefinitionName = GetOffset(
        "CEconItemDefinition::m_pszDefinitionName"
    );

    public static readonly int CEconItemDefinition_m_iDefaultLoadoutSlot = GetOffset(
        "CEconItemDefinition::m_iDefaultLoadoutSlot"
    );
}
