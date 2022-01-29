using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameToolkit.Engine.Components.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameToolkit.Engine.Components.actors.characters.AI_data
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
    class AI_manager
    {
       protected int positionX, positionY, distance_player, detection_circle;
       protected int new_positionX, new_positionY, tile_size;

        protected bad_guy_action current_action;
        protected int speed = 2;

        protected int loader_state = 0;

        protected SpriteEffects flip_horizontal;

        protected int starter_loader;
        protected int delta_horizontal;

        public AI_manager(int tile_size)
        {
            detection_circle = tile_size * 5;
            tile_size = tile_size;
        }

        public virtual void Set_Current_Action(bad_guy_action value)
        {
            current_action = value;
        }

        public SpriteEffects Get_flip_horizontal()
        {
            return flip_horizontal;
        }

        public virtual int Get_Starter_loader()
        {
            return starter_loader;
        }

        public Point Get_New_Position()
        {
            return new Point(new_positionX, new_positionY);
        }

        public virtual int Get_Distance_Player(Point player_position)
        {
            int deltaX = (player_position.X - positionX);
            int deltaY = (player_position.Y - positionY);

            return (int)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

        }

        public bad_guy_action Get_Current_Action()
        {
            return current_action;
        }


        public virtual void Update(ref Grid_Game_Object[] _grid_game, Point _this_actor_position, Point player_position)
        {

           
            positionX = _this_actor_position.X;
            positionY = _this_actor_position.Y;

         
            
        
        }

        public virtual void Update_chase(ref Grid_Game_Object[] _grid_game, Point _this_actor_position, Point player_position)
        {
           
        }

      

        public virtual int Try_Distance_Player(Point player_position, int positionX, int positionY)
        {
            int deltaX = (player_position.X - positionX);
            int deltaY = (player_position.Y - positionY);

            return (int)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

        }

       
    }
}
