using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameToolkit.Engine.Components.shared
{
    /*  Copyright 2022 Dan EBB 

   Permission is hereby granted, free of charge, to any person obtaining a 
   copy of this software and associated documentation files (the "Software"), 
   to deal in the Software without restriction, including without limitation 
   the rights to use, copy, modify, merge, publish, distribute, sublicense, 
   and/or sell copies of the Software, and to permit persons to whom the 
   Software is furnished to do so, subject to the following conditions:

   The above copyright notice and this permission notice shall be included 
   in all copies or substantial portions of the Software.

   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
   EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
   OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
   WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF 
   OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.*/
    public static class Level_Params
    {
        ///*******************************
        /// cherger manuellement une map
        ///*******************************
        public static int current_level;
        public static int gaming_state;

        public static int tile_size;
        public static int line_size;
    }


    public struct particles_FX : IFx
    {
        public float auto_remove { get; set; }

        public Rectangle position { get; set; }
        public int rect_size { get; set; }
        public float fading { get; set; }
        public int to_remove { get; set; }
        public float vertical_force { get; set; }
        public float horizontal_force { get; set; }
        public float gravity { get; set; }
        public Color color { get; set; }
    }

    public struct simple_texture : ISimple_texture
    {
        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }
        public Color color { get; set; }
        public Texture2D texture2D { get; set; }

    }

    public struct Camera : ICamera
    {
        public Rectangle rectposition { get; set; }

    }

    public class ColideBox : IColider
    {
        public colider_optimized is_optimized { get; set; }

        public Rectangle colideBox { get; set; }

        public int grid_ID { get; set; }
        public int grid_line { get; set; }
        public int grid_column { get; set; }
    }

    public struct Grid_Game_Object : IStructTiles
    {
        public colider_optimized is_optimized { get; set; }

        public XMLDataBase.bad_guy_units unit_to_print { get; set; }
        public int grid_ID { get; set; }
        public int grid_line { get; set; }
        public int grid_column { get; set; }

        /// ---- read the params for the tiles ----
        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }
        public Rectangle colideBox { get; set; }
        public string ID { get; set; }
        public Color default_color { get; set; }
        public Color current_color { get; set; }
        public int texture_ID { get; set; }

        /// ---- read the params for the logic ----
        public int logic_value_index { get; set; }

        public script_type logic_value { get; set; }
    }

    public struct Button : IButons
    {
        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }
        public Rectangle ColideBox { get; set; }
        public menu_editor name_enum { get; set; }
        public Color default_color { get; set; }
        public Color current_color { get; set; }
        public int texture_ID { get; set; }

        public int colide_state { get; set; }
        public string str_content { get; set; }
    }

   

    public struct ChoosenTile
    {
        public int rectframeX { get; set; }
        public int rectframeY { get; set; }
       
    }
}
