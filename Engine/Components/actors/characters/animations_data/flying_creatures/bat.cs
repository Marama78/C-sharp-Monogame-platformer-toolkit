using Microsoft.Xna.Framework;
using MonogameToolkit.Engine.Components.actors.characters.AI_data;
using MonogameToolkit.Engine.Components.actors.characters.AI_data.flying_creature;
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
    class Bat :Animator
    {
        public Bat(int __tile_size) : base(__tile_size)
        {
            Ai_mgr = new Flying_AI_model1(tile_size);

        }


        public override (int current_anim, int number_frames, float speed_anim) Get_Animation(bad_guy_action _behaviour)
        {
            ///---------- attribuer l'index de la texture ----
            texture_index = 0;

            switch(_behaviour)
            {
                case bad_guy_action.idle:
                   return (current_anim: 0, number_frames: 4, speed_anim: 0.2f);

                case bad_guy_action.hit:
                    return (current_anim: 1, number_frames: 8, speed_anim: 0.4f);

                case bad_guy_action.dead:
                    return (current_anim: 2, number_frames: 8, speed_anim: 0.2f);

                case bad_guy_action.sleep:
                    return (current_anim: 3, number_frames: 7, speed_anim: 0.2f);
            }


            return (0, 0, 0);
        }

        public override int Get_Life()
        {
            return 20;
        }

        public override bad_guy_action Get_Default_Action()
        {
            return bad_guy_action.sleep;
        }



      
        public override int Get_Texture_Index()
        {
            return 0;
        }

        public override void Move(ref Grid_Game_Object[] _grid_game)
        {
            base.Move(ref _grid_game);
        }

       

      
    }
}
