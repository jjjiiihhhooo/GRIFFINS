//////////////////////////////////////////////////////
// MK Toon URP Standard Unlit Editor        		//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

#if UNITY_EDITOR

namespace MK.Toon.Editor.URP
{
    internal class StandardUnlitEditor : MK.Toon.Editor.UnlitEditorBase
    {
        public StandardUnlitEditor() : base(RenderPipeline.Universal) { }
    }
}
#endif