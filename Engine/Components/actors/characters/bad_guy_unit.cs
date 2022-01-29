using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameToolkit.Engine.Components.actors.characters.AI_data;
using MonogameToolkit.Engine.Components.actors.characters.animations_data;
using MonogameToolkit.Engine.Components.shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameToolkit.Engine.Components.actors.characters
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
    class bad_guy_unit : IBad_guys
    {
        public  SpriteEffects flip_horizontal { get; set; }

        public Animator anime_manager { get; set; }

        public Rectangle colider { get; set; }
        public Rectangle default_colider { get; set; }

        public int texture_index { get; set; }

        public float chrono_Fx { get; set; }

        public Random rand { get; set; }

        public List<particles_FX> list_particles { get; set; }

        public Horizontal_Direction_LookAt look_at_H { get; set; }
        public SpriteEffects sprite_effect { get; set; }
        public int life { get; set; }
        public int attack_points { get; set; }
        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }
        public float chrono_anim { get; set; }
        public float speed_anim { get; set; }
        public int number_frames { get; set; }
        public int current_anim { get; set; }
        public bad_guy_action current_action { get; set; }
        public bad_guy_action old_action { get; set; }
        public bad_guy_action next_action { get; set; }
        public int frameW { get; set; }
        public int frameH { get; set; }
        public int hit_counter { get; set; }

        public int loading_state { get; set; }

        public int tile_size { get; set; }

        public bad_guy_unit()
        {
            this.rand = new Random();
            this.list_particles = new List<particles_FX>();
           
        }


        public void Set_Behaviour_Object(Animator _type)
        {
            //------------------------------
            this.anime_manager = new Animator(tile_size);
            this.anime_manager = _type;

            Set_Life();
            current_action = anime_manager.Get_Default_Action();

            loading_state = anime_manager.Get_AI().Get_Starter_loader();
        }

        public void Set_Life()
        {
            life = anime_manager.Get_Life();
        }

       
        public Rectangle Get_New_Rectangle_Position(Rectangle target, int x, int y)
        {
            return new Rectangle(target.X + x, target.Y + y, target.Width, target.Height);
        }


        public int Get_Hit(ref int deltaX, ref int deltaY, int damage_points )
        {
            Point ping = new Point(deltaX, deltaY);

            bad_guy_action temp_action = new bad_guy_action();

            if(this.colider.Contains(ping) && current_action!=bad_guy_action.dead)
            {
                int value = this.life;
                value -= damage_points;

                if(value > 0)
                {
                    this.chrono_anim = 0;

                    temp_action = bad_guy_action.hit;

                }
                else
                {
                    this.chrono_anim = 0;

                    temp_action = bad_guy_action.dead;
                    Update_Animation();
                   
                }
                this.life = value;

                this.current_action = temp_action;
                return 10;
            }

            return 0;
        }


       public void Update_Animation()
        {
            this.number_frames = anime_manager.Get_Animation(current_action).number_frames;
            this.speed_anim = anime_manager.Get_Animation(current_action).speed_anim;
            this.current_anim = anime_manager.Get_Animation(current_action).current_anim;
            this.chrono_anim = 0;

        }


        public Point Get_Point_Position()
        {
            int size = rectposition.Width / 2;

            return new Point(rectposition.X + size, rectposition.Y + size);
        }

      public List<particles_FX> Get_particles()
        {
            List<particles_FX> result = new List<particles_FX>();

            bool looping = true;

            while(looping)
            {
                foreach(particles_FX item in list_particles)
                {
                    result.Add(item);
                }
                looping = false;
            }

            list_particles.Clear();
            return result;
        }

        public void Update(ref Grid_Game_Object[] _grid_game,  Point player_position)
        {

            if (loading_state > 0)
            {

              ///  anime_manager.Get_AI().Update(ref _grid_game,ref Get_Point_Position() , ref player_position);

                chrono_anim += speed_anim;

                if (life > 0)
                {
                    if (current_action == bad_guy_action.hit)
                    {
                        chrono_Fx -= 0.2f;
                        if (chrono_Fx < 0)
                        {
                            if (hit_counter > 2)
                            {
                                current_action = bad_guy_action.idle;
                                Update_Animation();
                                hit_counter = 0;
                            }
                            else if (list_particles.Count < 20)
                            {
                                hit_counter++;
                                Hit(8, Color.OrangeRed);
                                Update_Animation();
                            }

                            chrono_Fx = 1;
                        }
                    }
                    else
                    {
                       //f (current_action != bad_guy_action.idle)
                        //
                            current_action = anime_manager.Get_AI().Get_Current_Action();
                          
                       //

                        if (current_action != old_action)
                        {
                            Update_Animation();
                        }

                        anime_manager.Update(ref _grid_game,  Get_Point_Position(),  player_position);

                       
                        anime_manager.Get_AI().Update_chase(ref _grid_game, Get_Point_Position(), player_position);
                        rectposition = new Rectangle(anime_manager.Get_AI().Get_New_Position().X, anime_manager.Get_AI().Get_New_Position().Y, rectposition.Width, rectposition.Height);
                        colider = Clone_Rectangle_Get_Original_Size(default_colider, rectposition);


                    }


                    if (chrono_anim >= number_frames)
                    {

                        if(current_action == bad_guy_action.rise)
                        {
                            current_action = bad_guy_action.idle;
                            anime_manager.Get_AI().Set_Current_Action(bad_guy_action.idle);
                            Update_Animation();
                        }

                        chrono_anim = 0;
                    }
                }
                else if (life <= 0 && life > -100)
                {
                    current_action = bad_guy_action.dead;
                    Update_Animation();
                    chrono_anim = 0;
                    life = -100;
                }
                else if (life <= -100)
                {

                    if (chrono_anim >= number_frames)
                    {
                        colider = new Rectangle(0, 0, 0, 0);
                        list_particles.Clear();
                        life -= 10000;
                    }
                    else
                    {
                        Hit(2, Color.Black);
                    }

                }

                if (current_action != bad_guy_action.none)
                {
                    int positionX = (int)chrono_anim * frameW;
                    int positionY = current_anim * frameH;

                    rectframe = new Rectangle(positionX, positionY, frameW, frameH);
                  
                }
                else
                {
                    rectframe = new Rectangle(0, 0, 0, 0);
                    colider = new Rectangle(0, 0, 0, 0);
                }
                old_action = current_action;
            }
            else
            {
                anime_manager.Update(ref _grid_game, Get_Point_Position(), player_position);

                loading_state = anime_manager.Get_AI().Get_Starter_loader();
            }
        }

        public Rectangle Clone_Rectangle_Get_Original_Size(Rectangle original, Rectangle target)
        {


            return new Rectangle(target.X, target.Y, original.Width,original.Height);
        }
        public void Hit(int total, Color _colorization)
        {

            for (int i = 0; i < total; i++)
            {
                int forceX = rand.Next(0, 20);
                int forceY = rand.Next(0, 20);
                int gravity = rand.Next(2, 5);

                int rect_size = rand.Next(2, 5);

                int left_or_right = rand.Next(10, 100);

                particles_FX new_fx = new particles_FX();

                new_fx.rect_size = rect_size;
                new_fx.fading = 1;

                if (left_or_right < 50)
                    new_fx.horizontal_force = forceX;
                else
                    new_fx.horizontal_force = -forceX;

                new_fx.vertical_force = forceY;

                new_fx.gravity = -gravity;

                new_fx.color = _colorization;

                new_fx.position = new Rectangle(this.rectposition.X + this.rectposition.Width/2, this.rectposition.Y + this.rectposition.Height / 2, rect_size, rect_size);

                list_particles.Add(new_fx);
            }
        }

       
      

        public void Draw(SpriteBatch _sprite_batch,ref Texture2D[] _texture_array, ref int delta_cameraX, ref int delta_cameraY) //ref Texture2D[] _texture_array,
        {
            Rectangle temp = Get_New_Rectangle_Position(rectposition, delta_cameraX, delta_cameraY);


            _sprite_batch.Draw(_texture_array[anime_manager.Get_Texture_Index()], temp, rectframe,Color.White,0,new Vector2(0,0),anime_manager.Get_AI().Get_flip_horizontal(),0);

        }
    }
}
