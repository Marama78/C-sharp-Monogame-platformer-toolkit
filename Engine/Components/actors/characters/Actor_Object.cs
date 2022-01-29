using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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


    public class Actor_Object : Iactor
    {
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
        public action_player old_action_player { get; set; }

        public float chronoAnim { get; set; }
        public float speedAnim { get; set; }
        public Point collide_UP_LEFT { get; set; }
        public Point collide_UP_MIDDLE { get; set; }
        public Point collide_UP_RIGHT { get; set; }
        public Point collide_DOWN_LEFT { get; set; }
        public Point collide_DOWN_MIDDLE { get; set; }
        public Point collide_DOWN_RIGHT { get; set; }
        public Point collide_LEFT { get; set; }
        public Point collide_LEFT_UP { get; set; }
        public Point collide_LEFT_DOWN { get; set; }
        public Point collide_RIGHT { get; set; }
        public Point collide_RIGHT_UP { get; set; }
        public Point collide_RIGHT_DOWN { get; set; }

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

        private Texture2D spriteTex;
       public Point[] coliders_triggers;
        public int vertical_fix_pos;
        public int horizontal_fix_pos;

        public int life;
        public bool toRemove { get; set; }

        public void Set_Coliders(int horizontal_size, int vertical_size, int horizontal_centering, int vertical_centering)
        {
            coliders_triggers = new Point[13];
            int delta_horizontal_centering = -rectposition.Width/4;
            int espacement_horizontal = rectposition.Width / 8;
    

            int width = rectposition.Width;
            int height = rectposition.Height;
            int middle_width = width / 2;
            int middle_height = height / 2;
            vertical_centering =  rectposition.Height / 10;
            horizontal_centering =  rectposition.Width / 6;
            collide_UP_LEFT = new Point( - delta_horizontal_centering + espacement_horizontal,   width/6);
            collide_UP_MIDDLE = new Point(  middle_width,   width / 6);
            collide_UP_RIGHT = new Point(  width + delta_horizontal_centering - espacement_horizontal,   width / 6);

            collide_DOWN_LEFT = new Point(  - delta_horizontal_centering - espacement_horizontal-8,   height+8);
            collide_DOWN_MIDDLE = new Point(  middle_width,  height+8);
            collide_DOWN_RIGHT = new Point(  width + delta_horizontal_centering + espacement_horizontal+8,   height+8);

            collide_LEFT_UP = new Point( - delta_horizontal_centering,   vertical_centering + width / 6);
            collide_LEFT = new Point( - delta_horizontal_centering,   middle_height);
            collide_LEFT_DOWN = new Point( - delta_horizontal_centering,   height - vertical_centering);

            collide_RIGHT_UP = new Point(width + delta_horizontal_centering,   vertical_centering + width / 6);
            collide_RIGHT = new Point(width + delta_horizontal_centering,   middle_height);
            collide_RIGHT_DOWN = new Point(width + delta_horizontal_centering,   height - vertical_centering);

            Point center = new Point(width/2,height/2);

            coliders_triggers[0] = collide_UP_LEFT;
            coliders_triggers[1] = collide_UP_MIDDLE;
            coliders_triggers[2] = collide_UP_RIGHT;

            coliders_triggers[3] = collide_DOWN_LEFT;
            coliders_triggers[4] = collide_DOWN_MIDDLE;
            coliders_triggers[5] = collide_DOWN_RIGHT;

            coliders_triggers[6] = collide_LEFT_UP;
            coliders_triggers[7] = collide_LEFT;
            coliders_triggers[8] = collide_LEFT_DOWN;

            coliders_triggers[9] = collide_RIGHT_UP;
            coliders_triggers[10] = collide_RIGHT;
            coliders_triggers[11] = collide_RIGHT_DOWN;

            coliders_triggers[12] = center;
        }

        public Point Get_Colider_by_Name(player_colider_objects colider,int deltaX, int deltaY)
        {
            int positionX = GetColidersIndex(colider).X + deltaX- rectposition.Width / 2;
            int positionY = GetColidersIndex(colider).Y + deltaY- rectposition.Height / 2;

            return new Point(positionX + rectposition.X, positionY + rectposition.Y); 
        }


        public Point Get_Colider_by_Index(int index, int deltaX, int deltaY)
        {
            int positionX = coliders_triggers[index].X + deltaX - rectposition.Width / 2;
            int positionY = coliders_triggers[index].Y + deltaY - rectposition.Height / 2;


            return new Point(positionX + rectposition.X, positionY+rectposition.Y);
        }

        public void SetPlayer_ColideBox(int x, int y,int width, int height, int vertical_fixed_position)
        {
            vertical_fix_pos = vertical_fixed_position;
             colideBox = new Rectangle(coliders_triggers[5], coliders_triggers[11]);// new Rectangle( x , y + vertical_fixed_position, width, height);
        }

        public Point GetColidersIndex(player_colider_objects colider)
        {
            switch(colider)
            {
                case player_colider_objects.up_L:
                    return coliders_triggers[0];

               case player_colider_objects.up_M:
                    return coliders_triggers[1];


                case player_colider_objects.up_R:
                    return coliders_triggers[2];

                case player_colider_objects.down_L:
                    return coliders_triggers[3];


                case player_colider_objects.down_M:
                    return coliders_triggers[4];


                case player_colider_objects.down_R:
                    return coliders_triggers[5];

                case player_colider_objects.left_U:
                    return coliders_triggers[6];


                case player_colider_objects.left_M:
                    return coliders_triggers[7];


                case player_colider_objects.left_D:
                    return coliders_triggers[8];


                case player_colider_objects.right_U:
                    return coliders_triggers[9];


                case player_colider_objects.right_M:
                    return coliders_triggers[10];


                case player_colider_objects.right_D:
                    return coliders_triggers[11];


                case player_colider_objects.center:
                    return coliders_triggers[12];

            }

            return new Point(0, 0);
        }
        public void SetTexture(Texture2D _texture)
        {
            spriteTex = _texture;
        }
        public void SetPlayer_rectPosition(int positionX, int positionY, int size_width, int size_height, int vertical_fixed_position)
        {
            vertical_fix_pos = vertical_fixed_position;

            rectposition = new Rectangle(positionX, positionY + vertical_fixed_position, size_width, size_height);
        }

        public void Set_RectFrame(int _frame_width, int _frame_height)
        {
            rectframe = new Rectangle(0, current_anim*_frame_height, _frame_width, frame_height);
            frame_width = _frame_width;
            frame_height = _frame_height;
        }

        public void ChangeAnimation(action_player _action)
        {

            if (old_action_player != _action)
            {
                int value = Get_Animations_Params(_action).curent_animations;
                float speed = Get_Animations_Params(_action).speed_anim;
                int numb_frames = Get_Animations_Params(_action).total_frames;



                speedAnim = speed;
                numberframes = numb_frames;
                current_anim = value;
                next_action = _action;
                current_action_player = _action;

                chronoAnim = 0;
                old_action_player = _action;
            }
        }

        public (int curent_animations,int total_frames, float speed_anim) Get_Animations_Params( action_player _action)
        {
            int _current_anim;
            int numb_frames;
            float speed;


            switch (_action)
            {
                case action_player.idle:
                    _current_anim = 0;
                    numb_frames = 8;
                    speed = 0.1f;
                    return (_current_anim, numb_frames, speed);

                case action_player.walk:

                    _current_anim = 1;
                    numb_frames = 6;
                    speed = 0.2f;
                    return (_current_anim, numb_frames, speed);


                case action_player.jump_up:

                    _current_anim = 2;
                    numb_frames = 4;
                    speed = 0.1f;
                    return (_current_anim, numb_frames, speed);


                case action_player.jump_fall:

                    _current_anim = 3;
                    numb_frames = 4;
                    speed = 0.1f;
                    return (_current_anim, numb_frames, speed);


                case action_player.climb:

                    _current_anim = 4;
                    numb_frames = 4;
                    speed = 0.02f;
                    return (_current_anim, numb_frames, speed);


                case action_player.attack:

                    _current_anim = 5;
                    numb_frames = 7;
                    speed = 0.4f;
                    return (_current_anim, numb_frames, speed);


            }

            return (-10, -10, -10);
        }
     


        public void Update()
        {
           
            chronoAnim += speedAnim;


            if (chronoAnim >= numberframes)
            {
                    chronoAnim = 0;
            }

            rectframe = new Rectangle(
                        (int)chronoAnim * frame_width, 
                        current_anim * frame_height, 
                        frame_width, 
                        frame_height);
         
        }
    }
}
