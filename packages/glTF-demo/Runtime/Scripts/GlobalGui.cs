// Copyright 2020-2022 Andreas Atteneder
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using UnityEngine;

#if UNITY_IMGUI
public static class GlobalGui
{
    public static float screenFactor;
    public static float listWidth = 200;
    public static float barHeightWidth = 25;
    public static float listItemHeight = 25;
    public static float buttonWidth = 50;

    static bool initialized;

    public static void Init() {
        if(initialized) return;

        screenFactor = Mathf.Max( 1, Mathf.Floor( Screen.dpi / 100f ));

        var fontSize =  Mathf.RoundToInt(14 * screenFactor);

        var guiStyle = GUI.skin.button;
        guiStyle.fontSize = fontSize;

        guiStyle = GUI.skin.toggle;
        guiStyle.fontSize = fontSize;

        guiStyle = GUI.skin.label;
        guiStyle.fontSize = fontSize;

        guiStyle = GUI.skin.textField;
        guiStyle.fontSize = fontSize;

        barHeightWidth *= screenFactor;
        buttonWidth *= screenFactor;
        listWidth *= screenFactor;
        listItemHeight *= screenFactor;

        guiStyle = GUI.skin.verticalScrollbar;
        guiStyle.fixedWidth = barHeightWidth;

        guiStyle = GUI.skin.verticalScrollbarThumb;
        guiStyle.fixedWidth = barHeightWidth;

        guiStyle = GUI.skin.verticalScrollbarUpButton;
        guiStyle.fixedWidth = barHeightWidth;
        guiStyle.fixedHeight = 0;

        guiStyle = GUI.skin.verticalScrollbarDownButton;
        guiStyle.fixedWidth = barHeightWidth;
        guiStyle.fixedHeight = 0;

        initialized = true;
    }
}
#endif
