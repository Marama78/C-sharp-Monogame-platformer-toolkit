using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameToolkit.Engine.Components.actors.characters.animations_data;
using MonogameToolkit.Engine.Components.shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonogameToolkit.Engine
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
    interface IAnimations
    {
        public Rectangle rectframe { get; set; }
        public float speed_anim { get; set; }
        public int number_frames { get; set; }
        public int texture_index { get; set; }
    }

    interface Itexture
    {
        public Texture2D texture_object { get; set; }

        public string texture_name { get; set; }
    }
   interface IFx
    {
        public Rectangle position { get; set; }
        public int rect_size { get; set; }
        public float fading { get; set; }
        public float auto_remove { get; set; }
        public int to_remove { get; set; }
        public float vertical_force { get; set; }
        public float horizontal_force { get; set; }
        public float gravity { get; set; }
        public Color color { get; set; }

    }

    interface IBad_guys
    {
        public int tile_size { get; set; }

        public SpriteEffects flip_horizontal { get; set; }

        public int loading_state { get; set; }

        public Animator anime_manager { get; set; }
        public int hit_counter { get; set; }
        public float chrono_Fx { get; set; }
        public Random rand { get; set; }
        public Horizontal_Direction_LookAt look_at_H { get; set; }
        public SpriteEffects sprite_effect { get; set; }
        public int life { get; set; }
        public int attack_points { get; set; }
        public int frameW { get; set; }
        public int frameH { get; set; }
        public Rectangle rectposition { get; set; }
        public Rectangle colider { get; set; }
        public Rectangle rectframe { get; set; }
        public float chrono_anim { get; set; }
        public float speed_anim { get; set; }
        public int number_frames { get; set; }
        public int current_anim { get; set; }
        public int texture_index { get; set; }
        public bad_guy_action current_action { get; set; }
        public bad_guy_action next_action { get; set; }
        public bad_guy_action old_action { get; set; }

    }

    interface IButons
    {
        public string str_content { get; set; }

        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }
        public Rectangle ColideBox { get; set; }
        public menu_editor name_enum { get; set; }
        public Color default_color { get; set; }
        public Color current_color { get; set; }
        public int texture_ID { get; set; }

        public int colide_state { get; set; }

    }


    interface IStructTiles
    {
        public XMLDataBase.bad_guy_units unit_to_print { get; set; }

        public colider_optimized is_optimized { get; set; }
        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }
        public Rectangle colideBox { get; set; }
        public string ID { get; set; }
        public Color default_color { get; set; }
        public Color current_color { get; set; }
        public int texture_ID { get; set; }

        script_type logic_value { get; set; }

        public int grid_ID { get; set; }
        public int grid_line { get; set; }
        public int grid_column { get; set; }

      
    }
    interface Iactor
    {
        public action_player old_action_player { get; set; }

        public Rectangle ladder_rectposition { get; set; }

        public trigger_state is_touching_ladder { get; set; }

        public trigger_state trigger_ladder_up { get; set; }
        public trigger_state trigger_ladder_center { get; set; }
        public trigger_state trigger_ladder_down { get; set; }

        public trigger_state trigger_colide_up_left { get; set; }
        public trigger_state trigger_colide_up_center { get; set; }
        public trigger_state trigger_colide_up_right { get; set; }

        public trigger_state trigger_colide_left_up { get; set; }
        public trigger_state trigger_colide_left_midle { get; set; }
        public trigger_state trigger_colide_left_down { get; set; }

        public trigger_state trigger_colide_right_up { get; set; }

        public trigger_state trigger_colide_right_midle { get; set; }
        public trigger_state trigger_colide_right_down { get; set; }
        public trigger_state trigger_colide_down_left { get; set; }
        public trigger_state trigger_colide_down_center { get; set; }
        public trigger_state trigger_colide_down_right { get; set; }


        public action_player next_action { get; set; }
        public action_player current_action_player { get; set; }
        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }
        public Rectangle colideBox { get; set; }

        public string ID { get; set; }

        public Color default_color { get; set; }
        public Color current_color { get; set; }

        public int numberframes { get; set; }
        public int current_anim { get; set; }
        public int frame_width { get; set; }

        public int frame_height { get; set; }

        public int object_state { get; set; }

        public SpriteEffects flip_horizontal { get; set; }
        public float chronoAnim { get; set; }
        public float speedAnim { get; set; }

        public bool toRemove { get; set; }
    }

    interface IObject
    {
        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }
        public Rectangle colideBox { get; set; }

        public string ID { get; set; }

        public Color default_color { get; set; }
        public Color current_color { get; set; }

        public int numberframes { get; set; }
        public int current_anim { get; set; }
        public int frame_width { get; set; }

        public int frame_height { get; set; }

        public int object_state { get; set; }

        public SpriteEffects flip_horizontal { get; set; }
        public float chronoAnim { get; set; }
        public float speedAnim { get; set; }

        public bool toRemove { get; set; }
    }
    

    interface Ienemi
    {
        public int texture_index { get; set; }

        public int life { get; set; }

        public Rectangle rect_position { get; set; }
        public Rectangle rect_frame { get; set; }

        public int attack_damage { get; set; }
        public float chrono_anim { get; set; }
        public float speed_anim { get; set; }
        public int number_of_frames { get; set; }
        public int current_anim { get; set; }
        public int next_anim { get; set; }
        public int frameW { get; set; }

        public int frameH { get; set; }
    }

   
    interface IColider
    {
        public Rectangle colideBox { get; set; }
     
        public int grid_ID { get; set; }
        public int grid_line { get; set; }
        public int grid_column { get; set; }
    }

    interface IGameObjectToDraw
    {
        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }
    }

     interface ISimple_texture
    {
        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }

        public Color color { get; set; }

        public Texture2D texture2D { get; set; }
    }



     interface ICamera
    {
        public Rectangle rectposition { get; set; }

    }


}
