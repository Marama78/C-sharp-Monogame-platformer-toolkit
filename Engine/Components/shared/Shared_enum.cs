using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public enum triggers
    {
        show,hide,
    }
    public enum bad_guy_action
    {
        none,
        idle,
        walk,
        attack,
        hit,
        dead,
        rise,
        sleep,
    }
    public enum command_btn
    {
        is_hidden,
        is_show,
    }
    public enum colider_optimized
    {
        off,
        on,
    }
    public enum editor_loader
    {
        none,
        set_the_arrays,
        is_now_operating,
    }
    public enum action
    {
        nothing,
        print_tile,
        get_tile,
        get_logic,
        print_logic,
    }

    public enum active_window
    {
        none,
        tiles,
        script,
    }

    public enum hover_btn
    {
        none,
        btn_files,
        btn_script,
        btn_tiles,
        btn_save,
        btn_erase,
        btn_quit,
        window_script,
        window_tiles,
        panel_command,
    }

    public enum menu_editor
    {
        none,
        Files,
        script,
        tiles,
        quit,
        save,
        erase,
        nothing,
    }

    public enum script_type
    {
        none,//0
        add_colider,//1
        player_start_position,//2
        go_to_nex_level,//3
        add_ladder,//4
        add_coin,//5
        add_go_through,//6
        Hit_the_player,//7
        enter_gate_Index,//8
        exit_gate_Index,//9
        delete_logic_tile,
        go_through,
        flying_bat,
        flying_peach,
        zombie,
    }



    public enum trigger_state
    {
        off,
        on,
    }

    public enum player_colider_states
    {
        touch_left,
        touch_right,
        touch_up,
        touch_down,
        touch_center,
    }

    public enum player_colider_objects
    {
        up_L,
        up_M,
        up_R,
        left_U,
        left_M,
        left_D,
        right_U,
        right_M,
        right_D,
        down_L,
        down_M,
        down_R,
        center
    }

    public enum game_state
    {
        editing_mode,
        loading,
        looping,
        end_looping,
    }

    public enum action_player
    {
        none,
        idle,
        walk,
        climb,
        jump_up,
        jump_fall,
        attack,
        hit,
        dead,
        invoke,
    }

    public enum Horizontal_Direction_LookAt
    {
        go_to_left,
        go_to_right,
        none,
    }

    public enum Vertical_Direction_LookAt
    {
        look_at_up,
        look_at_down,
        none,
    }
  

  

    





   /* interface ISimple_texture
    {
        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }

        public Color color { get; set; }

        public Texture2D texture2D { get; set; }
    }



    interface ICamera
    {
        public Rectangle rectposition { get; set; }

    }*/


}
