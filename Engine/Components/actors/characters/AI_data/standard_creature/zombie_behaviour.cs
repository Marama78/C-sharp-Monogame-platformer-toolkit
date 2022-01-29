using Microsoft.Xna.Framework;
using MonogameToolkit.Engine.Components.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameToolkit.Engine.Components.actors.characters.AI_data.standard_creature
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
    class zombie_behaviour : AI_manager
    {
        public zombie_behaviour(int tile_size) : base(tile_size)
        {
            flip_horizontal = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
            detection_circle = 10 * tile_size;
            speed = 4;
        }
        public override void Set_Current_Action(bad_guy_action value)
        {
            current_action = value;
        }
        public override int Get_Starter_loader()
        {
            return loader_state;
        }

        public override void Update(ref Grid_Game_Object[] _grid_game, Point _this_actor_position, Point player_position)
        {
            base.Update(ref _grid_game, _this_actor_position, player_position);

            if (loader_state == 0)
            {
                loader_state = 10;
                new_positionX = _this_actor_position.X;
                new_positionY = _this_actor_position.Y;
                current_action = bad_guy_action.none;
            }

            if (loader_state == 10)
            {
                distance_player = Get_Distance_Player(player_position);

                if (distance_player <= detection_circle)
                {
                    starter_loader = 2;
                    current_action = bad_guy_action.rise;
                    loader_state = 20;
                }
            }

            if(loader_state==20)
            {
                distance_player = Get_Distance_Player(player_position);
            }


        }


       

        public override void Update_chase(ref Grid_Game_Object[] _grid_game, Point _this_actor_position, Point player_position)
        {
            base.Update_chase(ref _grid_game, _this_actor_position, player_position);

            
                if (distance_player < detection_circle 
                && current_action!=bad_guy_action.rise
                && distance_player > detection_circle/4)
                {
                    current_action = bad_guy_action.walk;


                    if (player_position.X > _this_actor_position.X)
                    {
                        new_positionX += speed;
                        flip_horizontal = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        new_positionX -= speed;
                        flip_horizontal = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;

                    }

                }
              
            
         
        }

       
    }
}
