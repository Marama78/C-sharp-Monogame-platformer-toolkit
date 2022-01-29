using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonogameToolkit.Engine.Components.actors.characters;
using MonogameToolkit.Engine.Components.actors.characters.animations_data;
using MonogameToolkit.Engine.Components.services;
using MonogameToolkit.Engine.Components.shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MonogameToolkit.Engine.Scenes
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

    class Level_template : EditorTemplate
    {
        float chrono_hack_anim_coins = 0;
        Song song_bestonik;
        SoundEffect snd_piouw,snd_boing,snd_footstep,snd_coin,snd_wow;

        SoundEffect snd_explode1, snd_hit1;


        float chrono_sound = 0;

        
        Texture2D[] creatures_textures;
        Texture2D bat_toto;
        Texture2D tex_particles_FX;

        List<bad_guy_unit> list_bad_guy;

        Fx_Manager fx_mgr;

        public Level_template(Mainclass _main, ContentManager _content, SpriteBatch _spritebatch) : base(_main, _content, _spritebatch)
        {
            creatures_textures = new Texture2D[3];
        }

        public override void Load()
        {
            base.Load();

            song_bestonik = content.Load<Song>("bestonik");
            snd_piouw = content.Load<SoundEffect>("sounds\\snd_piouw");
            snd_boing = content.Load<SoundEffect>("sounds\\snd_boing");
            snd_footstep = content.Load<SoundEffect>("sounds\\snd_footstep");
            snd_coin = content.Load<SoundEffect>("sounds\\snd_coin");
            snd_wow = content.Load<SoundEffect>("sounds\\snd_wow");

            snd_hit1 = content.Load<SoundEffect>("sounds\\snd_hit1");
            snd_explode1 = content.Load<SoundEffect>("sounds\\snd_explode1");



            tex_particles_FX = content.Load<Texture2D>("fx\\particle");

            creatures_textures[0] = content.Load<Texture2D>("actors\\bat");
            creatures_textures[1] = content.Load<Texture2D>("actors\\peach");
            creatures_textures[2] = content.Load<Texture2D>("actors\\zombie");

            if (get_new_level_state == 0)
            {
               
            }

            ///********************
            ///add ennemi
            ///******************
            ///
            list_bad_guy = new List<bad_guy_unit>();

         
            ///********************
            ///add Fx manager
            ///******************
            fx_mgr = new Fx_Manager();
        }

        public void Add_Bad_guy(int positionX, int positionY, XMLDataBase.bad_guy_units _unit, int sizeW, int sizeH)
        {
            bad_guy_unit newbie = new bad_guy_unit();
            Make_bad_guy(positionX, positionY, _unit, ref newbie);
            newbie.rectposition = new Rectangle(positionX, positionY - sizeH, sizeW, sizeH);

            int colider_sizeW = sizeW + 24;
            int colider_sizeH = sizeH + 24;

            newbie.colider = new Rectangle(positionX + colider_sizeW / 2, positionY + colider_sizeH / 2 , colider_sizeW, colider_sizeH);
            newbie.default_colider = new Rectangle(positionX + colider_sizeW / 2, positionY + colider_sizeH / 2, colider_sizeW, colider_sizeH);
            newbie.frameW = 32;
            newbie.frameH = 32;
            newbie.anime_manager.Get_Default_Action();
            newbie.anime_manager.Set_Texture_Index(1);
            newbie.Update_Animation();
            list_bad_guy.Add(newbie);
        }
        public void Add_Bad_guy(int positionX, int positionY, XMLDataBase.bad_guy_units _unit)
        {
            bad_guy_unit newbie = new bad_guy_unit();
            Make_bad_guy(positionX, positionY, _unit,ref newbie);
            int size = tile_size;

            int colider_sizeW = size + 24;

            newbie.rectposition = new Rectangle(positionX, positionY, size, size);
            newbie.colider = new Rectangle(positionX + colider_sizeW / 2, positionY + colider_sizeW / 2, colider_sizeW, colider_sizeW);
            newbie.default_colider = new Rectangle(positionX + colider_sizeW / 2, positionY + colider_sizeW / 2, colider_sizeW, colider_sizeW);
            newbie.frameW = 32;
            newbie.frameH = 32;
            newbie.anime_manager.Get_Default_Action();
            newbie.anime_manager.Set_Texture_Index(1);
            newbie.Update_Animation();
            list_bad_guy.Add(newbie);
        }

        public void Make_bad_guy(int positionX, int positionY, XMLDataBase.bad_guy_units _unit , ref bad_guy_unit newbie)
        {
            switch(_unit)
            {
                case XMLDataBase.bad_guy_units.flying_bat:
                    Bat item = new Bat(tile_size);
                    newbie.Set_Behaviour_Object(item);
                    break;
                case XMLDataBase.bad_guy_units.flying_peach:
                    Peach item2 = new Peach(tile_size);
                    newbie.Set_Behaviour_Object(item2);
                    break;
                case XMLDataBase.bad_guy_units.walking_zombie:
                    zombie item3 = new zombie(tile_size);
                    newbie.Set_Behaviour_Object(item3); 
                    break;
            }
        }


        public void Load_Player()
        {
            my_player = new Actor_Object();
            my_player.current_color = Color.White * 1;
            my_player.current_anim = player_settings.current_anim_start;//0;
            ///----------- TAILLE DU RECTPOSITION --------------------
            int player_width = player_settings.player_sizeW;// 96;// 128;// 64;
            int player_height = player_settings.player_sizeW;//96;// 128;// 64;
            int fixed_vertical_position = 0;// 25;
            ///------------------------------------------------------------------
            /// Calculer le rectposition
            ///------------------------------------------------------------------

            my_player.SetPlayer_rectPosition(
                Start_player_positionX,
                   Start_player_positionY - 8,
                   player_width,
                   player_height,
                   fixed_vertical_position);
            my_player.Set_Coliders(0, 0, 0, 0);// -tile_size/2,5) ;

            ///------------------------------------------------------------------
            /// Calculer le colideBox
            ///------------------------------------------------------------------

            my_player.SetPlayer_ColideBox(
                Start_player_positionX,
                   Start_player_positionY,
                   player_width, player_height,
                   fixed_vertical_position);

            ///------------------------------------------------------------------
            /// paramètres pour les animations
            ///------------------------------------------------------------------
            my_player.rectframe = player_settings.rect_frame;
            my_player.frame_width = player_settings.rect_frame.Width;
            my_player.frame_height = player_settings.rect_frame.Height;
            my_player.numberframes = player_settings.number_frames_start;
            my_player.speedAnim = player_settings.speed_anim_start;
            my_player.object_state = 3;

            ///------------------------------------------------------------------
            /// placer les coliders
            ///------------------------------------------------------------------
            my_player.Set_Coliders(0, 0, 0, 0);
        }

        public void Load_Bad_Guys()
        {
            for(int i=0; i<my_grid_game.Length;i++)
            {
                int positionX= my_grid_game[i].rectposition.X;
                int positionY = my_grid_game[i].rectposition.Y;

                switch (my_grid_game[i].logic_value)
                {
                    case script_type.flying_bat:
                        Add_Bad_guy(positionX, positionY, XMLDataBase.bad_guy_units.flying_bat,64,64);
                        break;

                    case script_type.flying_peach:
                        Add_Bad_guy(positionX, positionY, XMLDataBase.bad_guy_units.flying_peach,40,40);
                        break;

                    case script_type.zombie:
                        Add_Bad_guy(positionX, positionY, XMLDataBase.bad_guy_units.walking_zombie,78,78);
                        break;
                }

            }
        }


        public Point Get_Player_Point()
        {
            int size = my_player.rectposition.Width / 2;

            return new Point(my_player.rectposition.X + size / 2, my_player.rectposition.Y + size / 2);

        }
        public override void Update()
        {
           
            base.Update();




            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && old_KB.IsKeyUp(Keys.Enter))
            {
                gaming_state++;
                if (gaming_state ==game_state.end_looping)
                {
                    gaming_state = game_state.editing_mode;
                    
                }
            }

            if (gaming_state == game_state.loading)
            {
                Create_Colider_Boxes();
                Create_Ladders();
                if (my_player != null)
                {
                    my_player = null;
                }

                if (list_tirs == null)
                {
                    list_tirs = new List<Actor_Object>();
                    list_tirs_for_Draw = new List<Actor_Object>();
                }
                else
                {
                    list_tirs.Clear();
                    list_tirs_for_Draw.Clear();
                }
                Reset_Camera_to_startPosition(Start_player_positionX, Start_player_positionY);

                Load_Player();
                Load_Bad_Guys();

                gaming_state = game_state.looping;
                MediaPlayer.Play(song_bestonik);
                MediaPlayer.Volume = 0.5f;
                MediaPlayer.IsRepeating = true;

            }
            else if (gaming_state == game_state.looping)
            {

                foreach (bad_guy_unit item in list_bad_guy)
                {
                    bad_guy_unit temp_unit = item;
                    
                    item.Update(ref my_grid_game, Get_Player_Point());

                    List<particles_FX> temp = item.Get_particles();
                    if (temp.Count > 0)
                    {
                        fx_mgr.Add_Particles(ref temp);
                    }

                    for (int i = list_tirs.Count - 1; i >= 0; i--)
                    {
                        ///--------------------------------
                        ///   Gérer les tirs du player
                        ///--------------------------------

                        int deltaX = list_tirs[i].colideBox.X;
                        int deltaY = list_tirs[i].colideBox.Y;

                        int shot = item.Get_Hit(ref deltaX, ref deltaY,10);

                        if (shot != 0 )
                        {
                            if (item.life > 0)
                            {
                                snd_hit1.Play();
                            }
                            else
                            {
                                snd_explode1.Play();
                            }
                            list_tirs.RemoveAt(i);
                            break;
                        }
                    }

                    if(item.colider.Contains(Get_Player_Point()))
                    {
                        my_player.life -= item.attack_points;
                        current_player_state = action_player.hit;
                    }

                }

                list_bad_guy.RemoveAll(item => item.life <= -5000);

                fx_mgr.Update(ref my_grid_game);

                ///------------------------------------------------------------------
                /// l'animation des pièces d'or se fait de manière totalement manuelle
                ///------------------------------------------------------------------
                chrono_hack_anim_coins += 0.1f;

                if (chrono_hack_anim_coins >= 4)
                {
                    chrono_hack_anim_coins = 0;
                }

                if(list_tirs != null && list_tirs.Count>0)
                list_tirs.RemoveAll(it => it.toRemove);


                my_player.Update();

                Actor_Object temp_player = my_player;

            


                Get_Coins();



                ///------------------------------------------------------------------
                /// on gère le pooling et on limite les tirs du joueur
                ///------------------------------------------------------------------
                if (list_tirs.Count > 10)
                {
                    list_tirs.RemoveAt(0);
                }

                chrono_tir_player += 0.1f;

              

                List<Actor_Object> list_temp_tirs = new List<Actor_Object>();
                int index = 0;

                while (index <= list_tirs.Count-1)
                {
                    foreach (Actor_Object it in list_tirs)
                    {
                        Actor_Object temp = it;
                        Move_Tir_Player(ref temp);
                        list_temp_tirs.Add(temp);
                        index++;
                    }
                }

                list_tirs.Clear();
                list_tirs = list_temp_tirs;

                    foreach (ColideBox it in list_colide_boxes)
                    {
                        Rectangle temp =  it.colideBox;

                        Get_Tir_Colision(ref temp, ref list_tirs);
                    }

             

                list_coin_temp.RemoveAll(it => it.object_state == 10);


                Update_list_colide_box_in_the_world();
                Horizontal_Moves(ref current_player_state, ref temp_player);
                Vertical_Moves(ref current_player_state, ref temp_player);

                if (current_KB.IsKeyDown(Keys.Space))
                {
                    if (chrono_tir_player > 1)
                    {
                        chrono_tir_player = 0;
                        Attack(ref current_player_state);
                        current_player_state = action_player.attack;
                    }
                }

                float speed_anim_ladder = 0.3f;

                switch (current_player_state)
                {

                    case action_player.idle:
                        ///--------------------------------------------------
                        /// Idle
                        ///--------------------------------------------------

                        if (temp_player.trigger_colide_down_center == trigger_state.on)
                        {
                            temp_player.ChangeAnimation(action_player.idle);
                            current_player_state = action_player.idle;
                        }
                        break;

                    case action_player.walk:
                        ///--------------------------------------------------
                        /// Run
                        ///--------------------------------------------------
                        if (old_player_state != action_player.walk)
                        {
                            temp_player.ChangeAnimation(action_player.walk);
                        }
                        current_player_state = action_player.walk;
                   
                        chrono_sound+=0.2f;
                        if(chrono_sound > temp_player.numberframes/2)
                        {
                            snd_footstep.Play(volume:0.3f, pitch:-0.78f, pan:0);
                            chrono_sound = 0;
                        }
 
                        break;

                    case action_player.jump_up:
                        ///--------------------------------------------------
                        /// jump_up
                        ///--------------------------------------------------
                        temp_player.ChangeAnimation(action_player.jump_up);
                        current_player_state = action_player.jump_up;

                        break;

                    case action_player.jump_fall:
                        ///--------------------------------------------------
                        /// falling
                        ///--------------------------------------------------
                        temp_player.ChangeAnimation(action_player.jump_fall);
                        current_player_state = action_player.jump_fall;
                        chrono_sound += 0.2f;
                        if (chrono_sound > 1)
                        {
                            snd_wow.Play(volume: 0.8f, pitch: 1f, pan: 0);
                            chrono_sound = 0;
                        }
                        break;

                    case action_player.climb:
                        ///--------------------------------------------------
                        /// ladder_up
                        ///--------------------------------------------------
                        speed_anim_ladder = 0.3f;
                        current_player_state = action_player.climb;
                        temp_player.ChangeAnimation(action_player.climb);
                      
                        if (vertical_player_lookAt == Vertical_Direction_LookAt.look_at_up)
                        {
                            chrono_ladder_state += speed_anim_ladder;
                            chrono_sound += 0.2f;
                        }
                        else if (vertical_player_lookAt == Vertical_Direction_LookAt.look_at_down)
                        {
                            chrono_ladder_state -= speed_anim_ladder;
                            chrono_sound += 0.2f;

                        }


                        if (chrono_sound>= temp_player.numberframes/2)
                        {
                            snd_footstep.Play(volume: 0.3f, pitch: 0.78f, pan: 0);
                            chrono_sound = 0;
                        }
                        
                        if (chrono_ladder_state >= temp_player.numberframes)
                        {
                            chrono_ladder_state = 0;
                        }
                        else if (chrono_ladder_state < 0)
                        {
                            chrono_ladder_state = temp_player.numberframes;
                        }



                        temp_player.rectframe = new Rectangle(
                            (int)(chrono_ladder_state) * my_player.frame_width,
                            my_player.current_anim * my_player.frame_width,
                            my_player.frame_width,
                            my_player.frame_height);

                        break;


                    case action_player.attack:
                        ///--------------------------------------------------
                        /// atack
                        ///--------------------------------------------------
                        speed_anim_ladder = 0.3f;
                        current_player_state = action_player.attack;

                        if (old_player_state != current_player_state)
                        {
                            temp_player.ChangeAnimation(action_player.attack);
                        }

                        if (temp_player.chronoAnim >= temp_player.numberframes-1)
                        {
                            if (temp_player.is_touching_ladder == trigger_state.off
                                && temp_player.trigger_colide_down_center==trigger_state.on)
                            {
                               temp_player.ChangeAnimation(action_player.idle);
                                current_player_state = action_player.idle;
                            }
                            else if(temp_player.is_touching_ladder == trigger_state.on 
                                && temp_player.trigger_colide_down_center==trigger_state.off)
                            {
                                temp_player.ChangeAnimation(action_player.climb);
                                current_player_state = action_player.climb;

                            }
                        }


                        break;
                }



                my_player = temp_player;



                foreach (Actor_Object item in list_tirs)
                {
                    switch (item.current_anim)
                    {
                        case 1:
                            if (item.chronoAnim >= item.numberframes - 1)
                            {
                                item.current_anim = 2;
                                item.chronoAnim = 0;
                            }
                            break;
                        case 2:
                            if (item.chronoAnim >= item.numberframes - 1)
                            {
                                item.object_state = 10;
                                item.toRemove = true;
                            }
                            break;
                    }
                }
                /// --- évaluer si une tuile de changement de niveau est touchée ou pas ---
                Get_Next_level_colision();
                old_player_state = current_player_state;
            }
            old_KB = current_KB;
            old_ms = current_ms;
        }

        public void Attack(ref action_player _action_state)
        {
            snd_piouw.Play(volume:0.3f,pitch:0.5f,pan:0);

            ///*******************************
            /// les paramètres du tir sont piochées depuis le fichier player_settings.xml
            ///*******************************

            _action_state = action_player.attack;
            Actor_Object attack = new Actor_Object();

            int positionX = 0;
            int positionY = my_player.rectposition.Y + tile_size/2;

            if (old_horizontal_player_lookAt == Horizontal_Direction_LookAt.go_to_right) 
            {
                positionX = my_player.rectposition.X+tile_size;
            }
            if (old_horizontal_player_lookAt == Horizontal_Direction_LookAt.go_to_left) 
            {
                positionX = my_player.rectposition.X;
                attack.flip_horizontal = SpriteEffects.FlipHorizontally;
            }

            int size_on_screen = player_settings.size_W_on_screen;
            int sword_W = player_settings.sword_W;
            int sword_H = player_settings.sword_H;

            attack.rectposition = new Rectangle(
                positionX,
                positionY,
             size_on_screen,
             size_on_screen * sword_H / sword_W);


            attack.colideBox = new Rectangle(
                positionX,
                positionY,
             size_on_screen,
             size_on_screen * sword_H / sword_W);

            attack.rectframe = new Rectangle(
                 0,
                 0,
              sword_W, sword_H);

            attack.current_anim = 0;
            attack.numberframes = player_settings.sword_number_frames_anim_start; //5;
            attack.speedAnim = 0.2f;
            attack.object_state = 3;
            attack.frame_width = sword_W;
            attack.frame_height = sword_H;
            attack.current_color = Color.White;
            list_tirs.Add(attack);
        }


        public void Move_Tir_Player(ref Actor_Object it)
        {
             int speed_projectile = 0;
               
            it.Update();


            if (it.current_anim == 0)
            {
                speed_projectile = tile_size / 4;
            }

            switch (it.flip_horizontal)
            {
                case SpriteEffects.None:
                    ///--------------------------------------------------
                    /// déplacer le tir à droite
                    ///--------------------------------------------------
                    it.colideBox = Get_New_Rectangle_Position(it.colideBox, speed_projectile, 0);
                    it.rectposition = Get_New_Rectangle_Position(it.rectposition, speed_projectile, 0);
                    break;

                case SpriteEffects.FlipHorizontally:
                    ///--------------------------------------------------
                    /// déplacer le tir à gauche
                    ///--------------------------------------------------
                    it.colideBox = Get_New_Rectangle_Position(it.colideBox, -speed_projectile, 0);
                    it.rectposition = Get_New_Rectangle_Position(it.rectposition, -speed_projectile, 0);

                    break;
            }


            Rectangle temp = Get_New_Rectangle_Position(it.colideBox,
                my_camera.rectposition.X,
                my_camera.rectposition.Y);

            if (temp.X < -mainclass.INT_SCREEN_WIDTH || temp.X > mainclass.INT_SCREEN_WIDTH * 2)
            {
                it.toRemove = true;
            }
        }

        public void Horizontal_Moves(ref action_player _action_state,ref Actor_Object _player)
        {
            ///*******************************
            /// les paramètres sont lues depuis le fichier player_settings.xml
            /// [move_speed]
            ///*******************************

            Get_Player_Horizontal_Colision(ref _player);


            Rectangle player_position_on_world = Get_New_Rectangle_Position(
                _player.rectposition,
                my_camera.rectposition.X,
                my_camera.rectposition.Y);

            if (_player.trigger_colide_down_center==trigger_state.on)
            {
                if ((horizontal_player_lookAt == Horizontal_Direction_LookAt.go_to_left 
                    || horizontal_player_lookAt == Horizontal_Direction_LookAt.go_to_right)
                    && _action_state!=action_player.attack)
                {
                    ///---- joueur part à gauche ou bien à droite ------
                    _action_state = action_player.walk;
                }
            }

            if (horizontal_player_lookAt == Horizontal_Direction_LookAt.go_to_left
                && _player.trigger_colide_left_midle==trigger_state.off
                && _player.trigger_colide_left_down == trigger_state.off)
            {
                ///------------------------------------------------------------------
                /// Le joueur par à GAUCHE
                ///------------------------------------------------------------------
                _player.flip_horizontal = SpriteEffects.FlipHorizontally;

                Rectangle temp_move_left = new Rectangle(
                   delta_cameraX + move_speed,
                    delta_cameraY,
                    my_camera.rectposition.Width,
                    my_camera.rectposition.Height);

                int result = my_grid_game[0].rectposition.X + temp_move_left.X;

                _player.rectposition = Get_New_Rectangle_Position(_player.rectposition,
                        - move_speed, 0);

                if (result <= 0)
                {
                    if (player_position_on_world.X < screen_device_centerX)
                    {
                        my_camera.rectposition = temp_move_left;
                    }
                }
            }

            if (horizontal_player_lookAt == Horizontal_Direction_LookAt.go_to_right 
                && _player.trigger_colide_right_midle == trigger_state.off
                && _player.trigger_colide_right_down == trigger_state.off)

            {
                ///------------------------------------------------------------------
                /// Le joueur par à DROITE
                ///------------------------------------------------------------------

                _player.flip_horizontal = SpriteEffects.None;

                Rectangle temp_move_right = new Rectangle(
                    delta_cameraX - move_speed,
                    delta_cameraY,
                    my_camera.rectposition.Width,
                    my_camera.rectposition.Height);

                int result = my_grid_game[my_grid_game.Length - 1].rectposition.X + temp_move_right.X;

                _player.rectposition = Get_New_Rectangle_Position(_player.rectposition,
                      move_speed, 0);

                if (result >= mainclass.INT_SCREEN_WIDTH
                    && player_position_on_world.X > screen_device_centerX)// midle_horizontal) 
                {
                    my_camera.rectposition = temp_move_right;
                }
            }

            if(horizontal_player_lookAt == Horizontal_Direction_LookAt.none 
                && _action_state != action_player.climb
                && _action_state != action_player.attack)
            {
                _action_state = action_player.idle;
            }

        }


        public void Get_Player_Horizontal_Colision(ref Actor_Object player)
        {
            Point colider_left_U = my_player.Get_Colider_by_Name(player_colider_objects.left_U, delta_cameraX, delta_cameraY);
            Point colider_left_M = my_player.Get_Colider_by_Name(player_colider_objects.left_M, delta_cameraX, delta_cameraY);
            Point colider_left_D = my_player.Get_Colider_by_Name(player_colider_objects.left_D, delta_cameraX, delta_cameraY);

            Point colider_right_U = my_player.Get_Colider_by_Name(player_colider_objects.right_U, delta_cameraX, delta_cameraY);
            Point colider_right_M = my_player.Get_Colider_by_Name(player_colider_objects.right_M, delta_cameraX, delta_cameraY);
            Point colider_right_D = my_player.Get_Colider_by_Name(player_colider_objects.right_D, delta_cameraX, delta_cameraY);


            trigger_state left_u = trigger_state.off;
            trigger_state left_m = trigger_state.off;
            trigger_state left_d = trigger_state.off;

            trigger_state right_u = trigger_state.off;
            trigger_state right_m = trigger_state.off;
            trigger_state right_d = trigger_state.off;


            foreach (ColideBox item in list_colide_boxes)
            {

                Rectangle rect_position_in_world = Get_New_Rectangle_Position(item.colideBox, delta_cameraX, delta_cameraY);

                if (rect_position_in_world.Contains(colider_left_U))
                {
                    left_u = trigger_state.on;
                }
                if (rect_position_in_world.Contains(colider_left_M))
                {
                    left_m = trigger_state.on;
                }
                if (rect_position_in_world.Contains(colider_left_D))
                {
                    left_d = trigger_state.on;
                }

                if (rect_position_in_world.Contains(colider_right_U))
                {
                    right_u = trigger_state.on;
                }

                if (rect_position_in_world.Contains(colider_right_M))
                {
                    right_m = trigger_state.on;
                }

                if (rect_position_in_world.Contains(colider_right_D))
                {
                    right_d = trigger_state.on;
                }
            }
            

            player.trigger_colide_left_up = left_u;
            player.trigger_colide_left_midle = left_m;
            player.trigger_colide_left_down = left_d;

            player.trigger_colide_right_up = right_u;
            player.trigger_colide_right_midle = right_m;
            player.trigger_colide_right_down = right_d;
        }


        public virtual void Vertical_Moves(ref action_player _action_state, ref Actor_Object _player)
        {

            Get_Player_Vertical_Colision(ref _player);

            ///------------------------------------------------------------------
            /// calculer les variables communes
            ///------------------------------------------------------------------
            int midle_vertical = mainclass.INT_SCREEN_HEIGHT / 2 - _player.rectposition.Height / 2;

            Rectangle rect_position_player_in_world = Get_New_Rectangle_Position(_player.rectposition,
                my_camera.rectposition.X,
                my_camera.rectposition.Y);

            

            if ((_action_state!=action_player.jump_up ||_action_state!=action_player.jump_fall)
                &&(_player.trigger_ladder_up == trigger_state.on
                || _player.trigger_ladder_center == trigger_state.on
                || _player.trigger_ladder_down == trigger_state.on))
            {

                ///********************************************************************
                ///   ECHELLE 
                ///********************************************************************

                if ((vertical_player_lookAt == Vertical_Direction_LookAt.look_at_up
                || vertical_player_lookAt == Vertical_Direction_LookAt.look_at_down))
                {
                    ///---- joueur monte ou descend ------
                    if (_player.trigger_ladder_center == trigger_state.on
                        && _player.trigger_ladder_down == trigger_state.on
                        &&_player.trigger_colide_down_center == trigger_state.off)
                    {
                        _action_state = action_player.climb;
                    }
                    else if(_player.trigger_colide_down_center == trigger_state.off
                        && _player.trigger_ladder_down == trigger_state.on)
                    {
                        ///--- centrer l'avatar avec l'échelle --- => provient de get_vertical_colisions(..)
                        _player.rectposition = my_player.ladder_rectposition;
                    }
                    else if(_player.trigger_colide_down_center == trigger_state.on)
                    {
                        _action_state = action_player.idle;
                    }

                    if (vertical_player_lookAt == Vertical_Direction_LookAt.look_at_up
                        && _player.trigger_colide_up_center == trigger_state.off)
                    {
                        ///------------------------------------------------------------------
                        /// Le joueur MONTE
                        ///------------------------------------------------------------------
                        Rectangle temp_move_down = new Rectangle(
                            my_camera.rectposition.X,
                            my_camera.rectposition.Y + move_speed,
                            my_camera.rectposition.Width,
                            my_camera.rectposition.Height);

                        int result = my_grid_game[0].rectposition.Y + temp_move_down.Y;
                        _player.rectposition = Get_New_Rectangle_Position(_player.rectposition,
                         0, -move_speed);

                        if (result <= 0
                            && rect_position_player_in_world.Y < midle_vertical - (tile_size / 2))
                        {
                            my_camera.rectposition = temp_move_down;
                        }
                    }
                    if (vertical_player_lookAt == Vertical_Direction_LookAt.look_at_down
                    && _player.trigger_colide_down_center == trigger_state.off
                    && _player.trigger_ladder_down == trigger_state.on)
                    {
                        ///------------------------------------------------------------------
                        /// Le joueur DESCEND
                        ///------------------------------------------------------------------

                        Rectangle temp_move_down = new Rectangle(
                            my_camera.rectposition.X,
                            my_camera.rectposition.Y - move_speed,
                            my_camera.rectposition.Width,
                            my_camera.rectposition.Height);

                        int result = my_grid_game[my_grid_game.Length - 1].rectposition.Y + temp_move_down.Y + tile_size;
                        
                        _player.rectposition = Get_New_Rectangle_Position(_player.rectposition,
                         0, move_speed);

                        if (result >= mainclass.INT_SCREEN_HEIGHT
                             && rect_position_player_in_world.Y > midle_vertical + (tile_size / 2))
                        {
                            my_camera.rectposition = temp_move_down;
                        }
                    }
                }

            }



            if (_player.trigger_ladder_up == trigger_state.off
                && _player.trigger_ladder_center == trigger_state.off
                && _player.trigger_ladder_down ==trigger_state.off)
            {
                ///********************************************************************
                ///   IL N'Y A PAS DE N'ECHELLE 
                ///********************************************************************

                if (vertical_player_lookAt == Vertical_Direction_LookAt.look_at_up
                    && old_KB.IsKeyUp(Keys.Up)
                    && _player.trigger_colide_up_center == trigger_state.off
                    && _player.trigger_colide_down_center == trigger_state.on)
                {
                    ///------------------------------------------------------------------
                    /// le joueur veux sauter ....
                    ///------------------------------------------------------------------
                    snd_boing.Play();
                    _action_state = action_player.jump_up;
                    gravity_rate = 0;

                }

                if (gravity_rate < jumping_force)
                {

                    _action_state = action_player.jump_up;

                    ///------------------------------------------------------------------
                    /// Player is Jumping
                    ///------------------------------------------------------------------
                    int delta_jump = 0;
                    current_gravity = DEFAULT_GRAVITY;
                    gravity_rate += 0.2f;
                    delta_jump = Convert.ToInt16(current_gravity) - Convert.ToInt16(gravity_rate);

                    if (delta_jump < 0 
                        || _player.trigger_colide_up_center==trigger_state.on)
                    {
                        gravity_rate = jumping_force + 100;
                        return;
                    }

                    Rectangle temp_move_up = new Rectangle(
                        my_camera.rectposition.X,
                        my_camera.rectposition.Y + Convert.ToInt16(delta_jump),
                        my_camera.rectposition.Width,
                        my_camera.rectposition.Height);

                    int result = my_grid_game[0].rectposition.Y + temp_move_up.Y;
                    _player.rectposition = Get_New_Rectangle_Position(_player.rectposition,
                     0, -delta_jump);

                    if (result <= 0
                        && rect_position_player_in_world.Y < midle_vertical - (tile_size * 4))
                    {
                        my_camera.rectposition = new Rectangle(
                       my_camera.rectposition.X,
                       my_camera.rectposition.Y + Convert.ToInt16(delta_jump),
                       my_camera.rectposition.Width,
                       my_camera.rectposition.Height); ;
                    }
                }

                 if (gravity_rate >= jumping_force
                    && _player.trigger_colide_down_center == trigger_state.off)
                {
                    ///------------------------------------------------------------------
                    /// GRAVITY - CHUTE - Pas de sols détecté
                    ///------------------------------------------------------------------
                    
                    _action_state = action_player.jump_fall;

                    if (_player.trigger_colide_down_center == trigger_state.off)
                    {
                        current_gravity += 0.1f;
                        gravity_rate = jumping_force + 100;

                        Rectangle temp_move_up = new Rectangle(
                            my_camera.rectposition.X,
                            my_camera.rectposition.Y - Convert.ToInt16(current_gravity),
                            my_camera.rectposition.Width,
                            my_camera.rectposition.Height);

                        int result = my_grid_game[my_grid_game.Length - 1].rectposition.Y + temp_move_up.Y + tile_size;
                        _player.rectposition = Get_New_Rectangle_Position(_player.rectposition,
                         0, Convert.ToInt16(current_gravity));

                        if (result >= mainclass.INT_SCREEN_HEIGHT
                             && rect_position_player_in_world.Y > midle_vertical + (tile_size / 2))
                        {
                            my_camera.rectposition = temp_move_up;
                        }
                    }
                    if (_player.rectposition.Y > my_grid_game[my_grid_game.Length - 1].rectposition.Y + tile_size * 2)
                    {
                        ///------------------------------------------------------------------
                        /// Player is Out of the panel game then DEAD
                        ///------------------------------------------------------------------
                        gaming_state = game_state.loading;
                        Reset_Camera_to_startPosition(Start_player_positionX, Start_player_positionY);
                    }
                }
           
            }

        }


  
        public void Get_Player_Vertical_Colision(ref Actor_Object player)
        {

            Point center = my_player.Get_Colider_by_Name(player_colider_objects.center, delta_cameraX, delta_cameraY);

            Point colider_up_L = my_player.Get_Colider_by_Name(player_colider_objects.up_L, delta_cameraX, delta_cameraY);
            Point colider_up_M = my_player.Get_Colider_by_Name(player_colider_objects.up_M, delta_cameraX, delta_cameraY);
            Point colider_up_R = my_player.Get_Colider_by_Name(player_colider_objects.up_R, delta_cameraX, delta_cameraY);

            Point colider_down_L = my_player.Get_Colider_by_Name(player_colider_objects.down_L, delta_cameraX, delta_cameraY);
            Point colider_down_M = my_player.Get_Colider_by_Name(player_colider_objects.down_M, delta_cameraX, delta_cameraY);
            Point colider_down_R = my_player.Get_Colider_by_Name(player_colider_objects.down_R, delta_cameraX, delta_cameraY);


            Point colider_foot_L = my_player.Get_Colider_by_Name(player_colider_objects.left_D, delta_cameraX, delta_cameraY);
            Point colider_foot_R = my_player.Get_Colider_by_Name(player_colider_objects.right_D, delta_cameraX, delta_cameraY);


            trigger_state up_l = trigger_state.off;
            trigger_state up_m = trigger_state.off;
            trigger_state up_r = trigger_state.off;

            trigger_state down_l = trigger_state.off;
            trigger_state down_m = trigger_state.off;
            trigger_state down_r = trigger_state.off;

            trigger_state foot_r = trigger_state.off;
            trigger_state foot_l = trigger_state.off;

            trigger_state ladder_up = trigger_state.off;
            trigger_state ladder_center = trigger_state.off;
            trigger_state ladder_down = trigger_state.off;

            foreach (ColideBox item in list_colide_boxes)
            {
                Rectangle rect_in_world = Get_New_Rectangle_Position(item.colideBox, delta_cameraX, delta_cameraY);

                if (rect_in_world.Contains(colider_foot_L))
                {
                    foot_r = trigger_state.on;
                }
                if (rect_in_world.Contains(colider_foot_R))
                {
                    foot_l = trigger_state.on;
                }

                if (rect_in_world.Contains(colider_up_L))
                {
                    up_l = trigger_state.on;
                }
                if (rect_in_world.Contains(colider_up_M))
                {
                    up_m = trigger_state.on;

                }
                if (rect_in_world.Contains(colider_up_R))
                {
                    up_r = trigger_state.on;
                }

                if (rect_in_world.Contains(colider_down_L))
                {
                    down_l = trigger_state.on;
                }
                if (rect_in_world.Contains(colider_down_M))
                {
                    down_m = trigger_state.on;

                }
                if (rect_in_world.Contains(colider_down_R))
                {
                    down_r = trigger_state.on;
                }
            }

            ///------- trigger pour détecter en priorité une colision avec une échelle (point au centre) ----
            trigger_state is_touching_ladder = trigger_state.off;

            foreach(ColideBox item in list_ladder)
            {
                Rectangle rect_in_world = Get_New_Rectangle_Position(item.colideBox, delta_cameraX, delta_cameraY);
               
                if (rect_in_world.Contains(colider_up_M))
                {
                    ladder_up = trigger_state.on;
                    
                }
                if (rect_in_world.Contains(center))
                {
                    ladder_center = trigger_state.on;
                    ///------- trigger pour détecter en priorité une colision avec une échelle (point au centre) ----
                    is_touching_ladder = trigger_state.on;
                }

                if (rect_in_world.Contains(colider_down_M))
                {
                    ladder_down = trigger_state.on;
                    int deltaX = item.colideBox.X + player_settings.size_W_on_screen / 2;
                    
                    ///--- mémorise la position de la tuile échelle ---
                    player.ladder_rectposition = new Rectangle(deltaX, player.rectposition.Y, player_settings.player_sizeW, player_settings.player_sizeH);
                }
            }

            ///------- trigger pour détecter en priorité une colision avec une échelle (point au centre) ----
            if(my_player.is_touching_ladder!=is_touching_ladder)
            {
                my_player.is_touching_ladder = is_touching_ladder;
            }

            player.trigger_colide_up_left = up_l;
            player.trigger_colide_up_center = up_m;
            player.trigger_colide_up_right = up_r;

            player.trigger_colide_down_left = down_l;
            player.trigger_colide_down_center = down_m;
            player.trigger_colide_down_right = down_r;

            player.trigger_colide_left_down = foot_l;
            player.trigger_colide_right_down = foot_r;

            player.trigger_ladder_up = ladder_up;
            player.trigger_ladder_center = ladder_center;
            player.trigger_ladder_down = ladder_down;
        }

        public void Get_Tir_Colision(ref Rectangle it, ref List<Actor_Object> _list_tir_object)
        {
            foreach (Actor_Object item in _list_tir_object)
            {
                if (item.current_anim == 0)
                {
                    Rectangle target = item.colideBox;

                    Point value = new Point(target.X, target.Y);

                    if (it.Contains(value))
                    {

                        if (item.flip_horizontal == SpriteEffects.None)
                        {
                            item.rectposition = new Rectangle(
                                 it.X,
                                item.rectposition.Y,
                                item.rectposition.Width,
                                item.rectposition.Height);
                            item.colideBox = new Rectangle(
                                it.X,
                                item.rectposition.Y,
                                item.rectposition.Width,
                                item.rectposition.Height);
                        }
                        else
                        {
                            item.rectposition = new Rectangle(
                                it.X + tile_size * 2,
                                item.rectposition.Y,
                               item.rectposition.Width,
                               item.rectposition.Height); 
                            item.colideBox = new Rectangle(
                                it.X + tile_size * 2,
                                item.rectposition.Y,
                                item.rectposition.Width,
                                item.rectposition.Height);
                        }

                        item.numberframes = player_settings.sword_number_frames_anim_colliding;
                        item.speedAnim = 0.3f;
                        item.current_anim = 1;
                        item.chronoAnim = 0;
                    }
                }
            }
        }


        public void Update_list_colide_box_in_the_world()
        {
           
            list_coin_temp_for_tilemap.Clear();
            list_tirs_for_Draw.Clear();
          
            foreach (Coin it in list_coin_temp)
            {
                Coin temp = new Coin();
                temp.rectposition =
                    new Rectangle(
                        it.rectposition.X,
                        it.rectposition.Y,
                        tile_size,
                        tile_size);
                temp.colideBox = new Rectangle(
                        it.rectposition.X,
                        it.rectposition.Y,
                        tile_size,
                        tile_size);

                temp.current_anim = 0;
                temp.numberframes = 8;
                temp.speedAnim = 0.1f;
                temp.rectframe = new Rectangle(
                    (int)chrono_hack_anim_coins * 16,
                    0,
                    16, 16);
                temp.frame_height = 16;
                temp.frame_width = 16;
                temp.loading_state = 1;
                temp.object_state = 10;

                temp.rectposition = Get_New_Rectangle_Position(temp.colideBox, my_camera.rectposition.X, my_camera.rectposition.Y);
                list_coin_temp_for_tilemap.Add(temp);
            }
        }
       

        public void Get_Coins()
        {
            int deltaX = my_camera.rectposition.X;
            int deltaY =  my_camera.rectposition.Y;

            Point colide_up = my_player.Get_Colider_by_Name(player_colider_objects.up_M, deltaX, deltaY);    //new Point(positionX1, positionY1);
            Point colide_down = my_player.Get_Colider_by_Name(player_colider_objects.down_M, deltaX, deltaY);//new Point(positionX2, positionY2);
            Point colide_center = my_player.Get_Colider_by_Name(player_colider_objects.center, deltaX, deltaY);//new Point(positionX3, positionY3);

            List<Coin> temp = new List<Coin>();

            foreach (Coin it in list_coin_temp_for_tilemap)
            {
                Rectangle rect = Get_New_Rectangle_Position(
                      it.colideBox, my_camera.rectposition.X,
                  my_camera.rectposition.Y);

                if (rect.Contains(colide_up)
                    || rect.Contains(colide_center)
                    || rect.Contains(colide_down))
                {
                    snd_coin.Play(volume: 0.1f, pitch: -0.3f, pan: 0);
                    int index = list_coin_temp_for_tilemap.IndexOf(it);
                    list_coin_temp[index].object_state = 10;
                }
                else
                {
                    temp.Add(it);
                }
            }

            list_coin_temp_for_tilemap_for_draw = temp;
        }



        public void Draw_Player_Avatar()
        {
            ///------------------------------------------------------------------
            /// dessiner tout l'avatar du joueur
            ///------------------------------------------------------------------
            Rectangle temp = Get_New_Rectangle_Position(
                    my_player.rectposition,
                    delta_cameraX,
                    delta_cameraY);

            spritebatch.Draw(
                all_textures[8],
                temp,
                my_player.rectframe,
                my_player.current_color,
                0,
                new Vector2(16, 16),
                my_player.flip_horizontal,
                0);
        }

        public void Draw_Player_Coliders()
        {
            ///------------------------------------------------------------------
            /// dessiner certains capteurs de colisions du player
            /// pour mémo: on a supprimé la détection de deux capteurs en bas qui bloquent
            /// l'object;
            ///------------------------------------------------------------------
       


            for (int i = 0; i < my_player.coliders_triggers.Length; i++)
            {

                Point colider = my_player.Get_Colider_by_Index(i,delta_cameraX,delta_cameraY);
                Color col = Color.White;

                Rectangle new_position = new Rectangle(colider.X-5,colider.Y-5, 10, 10);


                if(i!=4)
                {

                }
                else
                {
                    col = Color.Red;                }



                spritebatch.Draw(all_textures[5],
                                   new_position,
                                   new Rectangle(0, 0, 38, 38),
                                   col);
            }
        }



        public void Draw_Attack()
        {
            ///------------------------------------------------------------------
            /// dessiner les tirs du joueur
            ///------------------------------------------------------------------
            foreach (Actor_Object it in list_tirs)
            {
                Rectangle temp = Get_New_Rectangle_Position(
                   it.rectposition,
                    my_camera.rectposition.X, my_camera.rectposition.Y);
                spritebatch.Draw(all_textures[10],
                                   temp,
                               it.rectframe,
                                   it.current_color,
                                   0,
                                   new Vector2(it.rectposition.Width / 2, it.rectposition.Height / 2),
                                   it.flip_horizontal,
                                   0);
            }
        }
        public override void Draw()
        {
            base.Draw();



            if (gaming_state == game_state.looping )
            {

                ///------------------------------------------------------------------
                /// game_state 1 => on charge ke jeu
                /// game_state 2 => le jeu est prêt
                ///------------------------------------------------------------------

              

                if (my_player != null)
                {
                    Draw_Player_Avatar();
           
                    if(display_log==triggers.show)
                        Draw_Player_Coliders();
                }

                if (list_tirs.Count > 0)
                {
                    Draw_Attack();
                }

                if (display_log == triggers.show)

                {
                    Rectangle tempf = Get_New_Rectangle_Position(
                   my_grid_game[ID_next_level_tilemap].rectposition,
                   my_camera.rectposition.X, my_camera.rectposition.Y);

                    spritebatch.Draw(
                    all_textures[5],
                   tempf,
                    new Rectangle(0, 0, 38, 38), Color.Red);


                    spritebatch.Draw(
                   all_textures[7],
                  tempf,
                   new Rectangle(0, 0, 38, 38), my_grid_game[ID_next_level_tilemap].current_color * 0.5f);

                }

                ///----------------------------------------------------
                ///  Dessiner les ennnemis
                ///----------------------------------------------------

                foreach (bad_guy_unit item in list_bad_guy)
                {
                    item.Draw(spritebatch,ref creatures_textures,ref delta_cameraX,ref delta_cameraY);
                }

                ///----------------------------------------------------
                ///  Dessiner les effets spéciaux
                ///----------------------------------------------------
                fx_mgr.Draw(spritebatch, ref tex_particles_FX, ref bat_toto, ref delta_cameraX, ref delta_cameraY);
            }

            if(display_log==triggers.show)
            DrawLog();
        }

        public void DrawLog()
        {
           
           spritebatch.DrawString(mainFont, "player_state :  => " + current_player_state + " (int) of 0,9f is " +(int)0.9f , new Vector2(500, 600), Color.Violet);
            spritebatch.DrawString(mainFont, "list_bad guy count :  => " + list_bad_guy.Count  , new Vector2(500, 650), Color.Violet);

        }


      

        public override void KillScene()
        {
            base.KillScene();
        }
    }
}
