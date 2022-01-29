using Microsoft.Xna.Framework;
using MonogameToolkit.Engine.Components.actors.characters.AI_data;
using MonogameToolkit.Engine.Components.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameToolkit.Engine.Components.actors.characters.animations_data
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
    class Animator
    {
        private Point position1;

        protected int texture_index { get; set; }
        protected float chrono_move { get; set; }

        protected AI_manager Ai_mgr { get; set; }

        protected int tile_size { get; set; }


        public Animator(int __tile_size)
        {
            tile_size = __tile_size;
        }

    
       

        public virtual AI_manager Get_AI()
        {
           return Ai_mgr;
        }

        public virtual int Get_Texture_Index()
        {
            return texture_index;
        }

        public virtual (int current_anim, int number_frames, float speed_anim) Get_Animation(bad_guy_action _behaviour)
        {
            return (0, 0, 0);
        }

        public void Set_Texture_Index(int value)
        {
            texture_index = value;
        }

        public virtual int Get_Life()
        {
            return 0;
        }

        public virtual bad_guy_action Get_Default_Action()
        {
            return bad_guy_action.idle;
        }

        public virtual void Update(ref Grid_Game_Object[] _grid_game,  Point _this_actor_position,  Point player_position)
        {
            if(Ai_mgr!=null)
            {
                Ai_mgr.Update(ref _grid_game, _this_actor_position,  player_position);
            }
        }

       

        public virtual void Move(ref Grid_Game_Object[] _grid_game)
        {

        }

       
      
    }
}
