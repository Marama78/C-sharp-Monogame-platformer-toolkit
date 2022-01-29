using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace XMLDataBase
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

    public class Window
    {
        Texture2D body_texture;
        Texture2D tilesheet_texture;
        Texture2D header_texture;
        Texture2D footer_texture;
        Texture2D selector_texture;
        SpriteFont dialogFont;

        int current_height;
        window_style window_state = window_style.none;
        int dialog_box_max_width;
        int dialog_box_max_height;
        int locker_toolbox_panel = 0;
        int tile_frame_width, tile_frame_height, positionX, positionY;
        int window_tile_W, window_tile_H;
        int font_Size = 32;
        int rect_size_W = -1;
        int rect_size_H = -1;
        int marge_Gauche = 48;
        int marge_Haut = 48;
        int espacement = 10;
        int total_horizontal_button;
        int index_button_wanted = 0;
        int window_size_W, window_size_H;
       
        int btn_W, btn_H;
        is_open window_is_open = XMLDataBase.is_open.off; // show or hide the window
        is_selected window_is_selected = XMLDataBase.is_selected.off; // fades the window if [off]
       

        Rectangle rect_frame, rect_position;
        Rectangle head_colide_box;
        Rectangle window_colide_box;
        Colider_Object resizer_colide_box;


        Point[] anchors;
        Colider_Object[] button_colider;
        Colider_Object[] action_buttons;
        List<Colider_Object> list_window_body;
        List<Colider_Object> list_window_header;
        List<Colider_Object> list_window_elevators;//, righter_area;
        string[] lines;
        int limit_W, limit_H;
        int delta_resize_W, delta_resize_H;
        int my_windowBox_W, my_windowBox_H;
        
        Point first_resize_point, current_resize_point, delta_point;
        float fading_value = 1f;
        int moving_window_state = 0;
        int bordeless_state = 0;
        int window_making_state = 0;

        Point memorized_mouse_position;
        Colider_Object memorized_colider_acsenseur;
        int vertical_move_elevator_state, horizontal_move_elevator_state;
        Point delta_elevator;


        public Window(int _tile_width, int _tile_height, int _window_tile_W, int _window_tile_H, SpriteFont _dialog_font)
        {
            tile_frame_width = _tile_width;
            tile_frame_height = _tile_height;
            window_tile_W = _window_tile_W;
            window_tile_H = _window_tile_H;
            dialogFont = _dialog_font;

            resizer_colide_box = new Colider_Object();
            window_size_W = 5;
            window_size_H = 5;

            delta_resize_W = 0;
            delta_resize_H = 0;
            marge_Gauche = espacement * 2;
            marge_Haut = espacement * 2;

            list_window_elevators = new List<Colider_Object>();
            list_window_body = new List<Colider_Object>();
            list_window_header = new List<Colider_Object>();

            action_buttons = new Colider_Object[8];
            // SetColors();


            my_windowBox_W = -1;
            my_windowBox_H = -1;

            window_is_selected = is_selected.on;//1;
        }


        public void Edit_triggers()
        {
           
            action_buttons[0].current_color = Color.White;
            action_buttons[1].current_color = Color.White;
            action_buttons[2].current_color = Color.White;
            action_buttons[3].current_color = Color.White;
            action_buttons[4].current_color = Color.White;
            action_buttons[5].current_color = Color.White;
            action_buttons[6].current_color = Color.White;
            action_buttons[7].current_color = Color.White;
        }

    
        public int Get_horizontal_trigger_mover(Point mouse_position)
        {
            if (action_buttons[2].rectposition.Contains(mouse_position))
            {
                action_buttons[2].current_color = Color.RosyBrown;
                return 1;
            }
            else
            {
                action_buttons[2].current_color = Color.White;

                return 0;
            }
        }

        public int Get_vertical_trigger_mover(Point mouse_position)
        {
            if (action_buttons[3].rectposition.Contains(mouse_position))
            {
                action_buttons[3].current_color = Color.RosyBrown;
                return 1;
            }
            else
            {
                action_buttons[3].current_color = Color.White;

                return 0;
            }
        }

        public int Get_moving_window_state()
        {
            return moving_window_state;
        }

        public void Update(Point mouse_position, MouseState current_MS, MouseState old_MS)
        {

            Get_colision_button(mouse_position, current_MS, old_MS);
         
            
            if(  Get_vertical_trigger_mover(mouse_position) == 1)
            {
                ///+++++++++++++++++++++++++++
                /// gérer l'ascenseur vertical
                ///+++++++++++++++++++++++++++
                
                if (current_MS.LeftButton == ButtonState.Pressed
                    && old_MS.LeftButton == ButtonState.Released)
                {
                    //to do move vertically buttons
                    memorized_mouse_position = mouse_position;
                    memorized_colider_acsenseur = action_buttons[3];
                    vertical_move_elevator_state = 1;
                }
            }

            
            if (Get_horizontal_trigger_mover(mouse_position) == 1)
            {
                ///+++++++++++++++++++++++++++
                /// gérer l'ascenseur horizontal
                ///+++++++++++++++++++++++++++
                
                if (current_MS.LeftButton == ButtonState.Pressed)
                {
                    memorized_mouse_position = mouse_position;
                    memorized_colider_acsenseur = action_buttons[2];
                    horizontal_move_elevator_state = 1;

                }
            }


            if (vertical_move_elevator_state == 1)
            {
                ///+++++++++++++++++++++++++++
                /// un mouvement vertical est demandé, tant que le bouton de la souris est appuyé
                /// on modifie la position du curseur
                ///+++++++++++++++++++++++++++

                if (current_MS.LeftButton == ButtonState.Pressed)
                {
                    Rectangle temp = Get_new_Rectangle(memorized_colider_acsenseur.rectposition, 0, mouse_position.Y - memorized_mouse_position.Y);


                    if(temp.Height + temp.Y< action_buttons[1].rectposition.Y
                        && temp.Y>(action_buttons[0].rectposition.Y + action_buttons[0].rectposition.Height))
                    {
                        action_buttons[3].rectposition = temp;
                        delta_elevator.Y = positionY - temp.Y + rect_size_H*2;// marge_Haut;

                    }


                }
                else
                {
                    vertical_move_elevator_state = 0;
                }
            }

            if (horizontal_move_elevator_state == 1)
            {
                ///+++++++++++++++++++++++++++
                /// un mouvement vertical est demandé, tant que le bouton de la souris est appuyé
                /// on modifie la position du curseur
                ///+++++++++++++++++++++++++++

                if (current_MS.LeftButton == ButtonState.Pressed)
                {
                    Rectangle temp = Get_new_Rectangle(memorized_colider_acsenseur.rectposition, mouse_position.X - memorized_mouse_position.X, 0);

                    if (temp.X + temp.Width < action_buttons[1].rectposition.X
                        && temp.X > (action_buttons[4].rectposition.X))// + triggers[4].rectposition.Width))
                    {
                        action_buttons[2].rectposition = temp;
                        delta_elevator.X = positionX - temp.X;

                    }



                    ///  triggers[2].rectposition = Get_new_Rectangle(memorized_colider_acsenseur.rectposition,  mouse_position.X - memorized_mouse_position.X,0);
                }
                else
                {
                    horizontal_move_elevator_state = 0;
                }
            }



            if (action_buttons[0].rectposition.Contains(mouse_position))
            {
                locker_toolbox_panel = 0;

                action_buttons[0].current_color = Color.Green;
                ///+++++++++++++++++++++++++++
                /// triggers[0] => c'est l'icone de fermeture de la fenêtre
                /// ** si l'utilisateur appuie dessus, on ferme la fenêtre
                ///+++++++++++++++++++++++++++
                if (current_MS.LeftButton == ButtonState.Pressed
                    && old_MS.LeftButton == ButtonState.Released)
                {
                    if (window_is_open == XMLDataBase.is_open.on)
                    {
                        window_is_open = XMLDataBase.is_open.off;
                    }
                }
            }
            else
            {
                locker_toolbox_panel = 1;

                action_buttons[0].current_color = Color.Yellow;

            }

            if (action_buttons[1].rectposition.Contains(mouse_position))
            {
                action_buttons[1].current_color = Color.Green;
                locker_toolbox_panel = 0;

                ///+++++++++++++++++++++++++++
                /// triggers[1] => c'est l'icone de redimensionnement de la fenêtre
                /// ** si l'utilisateur appuie dessus, on modifie la taille de la fenêtre
                ///+++++++++++++++++++++++++++
                if (current_MS.LeftButton == ButtonState.Pressed
                    && old_MS.LeftButton == ButtonState.Released)
                {

                    first_resize_point = mouse_position;
                    current_resize_point = new Point(my_windowBox_W, window_size_H);
                    my_windowBox_W = delta_resize_W;
                    my_windowBox_H = delta_resize_H;
                }

            }
            else
            {
                action_buttons[1].current_color = Color.Yellow;

            }


            if (first_resize_point.X > 0)
            {
                locker_toolbox_panel = 0;
                if (current_MS.LeftButton == ButtonState.Pressed)
                {


                    delta_point.X = mouse_position.X - first_resize_point.X;
                    delta_point.Y = mouse_position.Y - first_resize_point.Y;


                    delta_resize_W = current_resize_point.X + (delta_point.X / (tile_frame_width + espacement));
                    delta_resize_H = current_resize_point.Y + (delta_point.Y / (tile_frame_height + espacement));

                    my_windowBox_W = delta_resize_W;
                    my_windowBox_H = delta_resize_H;

                }

                if (current_MS.LeftButton == ButtonState.Released
                    && old_MS.LeftButton == ButtonState.Pressed)
                {
                    locker_toolbox_panel = 1;
                    first_resize_point.X = 0;
                    window_size_H = my_windowBox_H;
                    window_size_W = my_windowBox_W;
                }

            }

            if (old_MS.LeftButton == ButtonState.Released)
            {
               // memorized_game_object_state = game_object_state;
                moving_window_state = 0;
            }


            if (window_is_selected == is_selected.on)
            {
                fading_value = 1;
            }
            else
            {
                fading_value = 0.5f;
            }
        }

        public is_selected Is_Selected()
        {
            return window_is_selected;
        }
        public void Edit_Window_Is_Selected(is_selected _value)
        {
            window_is_selected = _value;
        }

     
        public void Set_Position(int _positionX, int _positionY)
        {
            positionX = _positionX;
            positionY = _positionY;
            Update_colidebox_head(_positionX, _positionY);
        }

        public int Get_resizer(Point mouse_position)
        {
          if(resizer_colide_box.rectposition.Contains(mouse_position))
            {
                return 1;
            }
            return 0;
        }
        public void Update_colidebox_head(int posX, int posY)
        {
            head_colide_box = new Rectangle(posX, posY, head_colide_box.Width, head_colide_box.Height);
        }
        public void SetPoint()
        {
            ///--------------------------------------------------------
            /// Les points qui sont dans cette méthode ne servent que ...
            /// pour le découpage du tilesheet utilisateur
            /// ** en gros, je découpe en 9 cellules le tile sheet
            ///--------------------------------------------------------
            if (anchors != null)
            {
                anchors = null;
            }

            if (anchors == null)
            {
                anchors = new Point[9];

                Point up_left = new Point(0, 0);
                Point up_center = new Point(tile_frame_width, 0);
                Point up_right = new Point(tile_frame_width * 2, 0);

                Point down_left = new Point(0, tile_frame_height * 2);
                Point down_center = new Point(tile_frame_width, tile_frame_height * 2);
                Point down_right = new Point(tile_frame_width * 2, tile_frame_height * 2);

                Point left = new Point(0, tile_frame_height);
                Point center = new Point(tile_frame_width, tile_frame_height);
                Point right = new Point(tile_frame_width * 2, tile_frame_height);

                anchors[0] = up_left;
                anchors[1] = up_center;
                anchors[2] = up_right;

                anchors[3] = down_left;
                anchors[4] = down_center;
                anchors[5] = down_right;


                anchors[6] = left;
                anchors[7] = center;
                anchors[8] = right;
            }
        }

        public void SetFont(SpriteFont _spriteFont)
        {
            dialogFont = _spriteFont;
        }

        public Point Get_Position()
        {
            return new Point(positionX, positionY);
        }

   
        /// <summary>
        /// définir les textures
        /// </summary>
        /// <param name="_body_texture">texture du corps de la fenêtre</param>
        /// <param name="_header_texture">texture de l'en-tête</param>
        /// <param name="_footer_texture">texture du pieds de page</param>
        /// <param name="_selector_texture">texture du bouton de sélection</param>
       
        public void SetWindowTextures(Texture2D _body_texture, Texture2D _header_texture, Texture2D _footer_texture, Texture2D _selector_texture , Texture2D _tilesheet)
        {
            body_texture = _body_texture;
            header_texture = _header_texture;
            footer_texture = _footer_texture;
            selector_texture = _selector_texture;
            tilesheet_texture = _tilesheet;
        }

      

        public void Draw(SpriteBatch _spritebatch, Color color_of_object)
        {
           

            switch (window_state)
            {
              

                case window_style.simple_messageText:
                    ///--------------------------------------------------------
                    /// cet état affiche uniquement un message
                    ///--------------------------------------------------------

                    if (lines.Length > 0)
                    {
                        Make_Window_Box(dialog_box_max_width, dialog_box_max_height, _spritebatch, Color.White);

                        for (int i = 0; i < lines.Length; i++)
                        {

                            _spritebatch.DrawString(
                            dialogFont,
                            lines[i],
                            new Vector2(window_tile_W + positionX,
                            font_Size * i + positionY + window_tile_H),
                             color_of_object);

                        }


                    }
                    break;

                case window_style.message_two_buttons:
                    ///--------------------------------------------------------
                    /// cet état affiche uniquement un message
                    /// et un bouton [ok] et un bouton [annuler]
                    ///--------------------------------------------------------

                    if (lines.Length > 0)
                    {

                        Make_Window_Box(dialog_box_max_width, dialog_box_max_height, _spritebatch, Color.White);

                        for (int i = 0; i < lines.Length; i++)
                        {
                            _spritebatch.DrawString(
                                dialogFont,
                                lines[i],
                                new Vector2(window_tile_W + positionX,
                                font_Size * i + positionY + window_tile_H),
                                color_of_object);
                        }

                        for (int i = 0; i < lines.Length; i++)
                        {

                            _spritebatch.DrawString(
                               dialogFont,
                               lines[i],
                               new Vector2(window_tile_W + positionX + 2,
                               font_Size * i + positionY + window_tile_H),
                               color_of_object);
                            _spritebatch.DrawString(
                              dialogFont,
                              lines[i],
                              new Vector2(window_tile_W + positionX + 4,
                              font_Size * i + positionY + window_tile_H),
                              Color.AliceBlue);
                        }
                    }
                    break;

                case window_style.toolbox:
                  int index=  Get_Index_Button_wanted();

                    Make_Window_Box(window_size_W, window_size_H, _spritebatch, Color.White);

                    foreach(Colider_Object it in list_window_body)
                    {
                        _spritebatch.Draw(body_texture,
                        it.rectposition,
                      it.rectframe,
                            it.current_color * fading_value);
                    }

                    foreach (Colider_Object it in list_window_header)
                    {
                        _spritebatch.Draw(header_texture,
                        it.rectposition,
                      it.rectframe,
                            it.current_color * fading_value);
                    }
                    for (int i = 0; i < button_colider.Length; i++)
                    {
                        ///------ dessiner les boutons-outils -------

                        Rectangle temp = Get_new_Rectangle(button_colider[i].rectposition, delta_elevator.X, delta_elevator.Y);

                        int limitX = (window_colide_box.Width + positionX - rect_size_W);
                        int limitY = (window_colide_box.Height + positionY - rect_size_H);

                        int startX = positionX;
                        int startY = action_buttons[0].rectposition.Y + action_buttons[0].rectposition.Height;

                        if (temp.X < limitX && temp.X > startX
                            && temp.Y < limitY && temp.Y > startY)
                        {
                            _spritebatch.Draw(tilesheet_texture, temp,
                                button_colider[i].rectframe,
                                button_colider[i].current_color * fading_value);

                            if (i == index)
                            {
                                _spritebatch.Draw(selector_texture,
                                temp,
                                Color.Red);
                            }
                        }
                    }

                    foreach (Colider_Object it in list_window_elevators)
                    {
                        _spritebatch.Draw(footer_texture, it.rectposition, it.rectframe, it.current_color * fading_value);

                    }


                    for (int i = 1; i < 5; i++)
                    {
                        Rectangle rectframe = new Rectangle(48 * 2, 0, tile_frame_width, tile_frame_height);

                        _spritebatch.Draw(footer_texture,
                           action_buttons[i].rectposition,
                           action_buttons[i].rectframe,
                           Color.White* fading_value);

                      /*  _spritebatch.Draw(selector_texture,
                         action_buttons[i].rectposition,
                         action_buttons[i].current_color * fading_value);*/
                    }

           
                    break;
            }

        }

        public Rectangle Get_windowbox_colideBox()
        {
            return new Rectangle(positionX, positionY, window_size_W * tile_frame_width, (window_size_H + 1) * tile_frame_height);

        }

        public void Set_String(string[] list_of_string)
        {
            current_height = 0;
            int total_lines = list_of_string.Length;

            lines = new string[total_lines];

            for (int i = 0; i < total_lines; i++)
            {
                lines[i] = list_of_string[i];

                current_height++;
            }

            current_height++;
        }

        public void Write_Text(string[] your_strings)
        {
            if (current_height == 0)
            {
                Set_String(your_strings);
                dialog_box_max_height = current_height;
                dialog_box_max_width = GetMaxWidth(your_strings);
            }
            window_state = window_style.message_two_buttons;
        }

        public int GetMaxWidth(string[] string_object)
        {
            int total = string_object.Length;
            int max_size = 0;

            for (int i = 0; i < total; i++)
            {
                int size = string_object[i].Length;

                if (size > max_size)
                {
                    max_size = size;
                }
            }

            return max_size / 5;
        }

        public void Move_Window(Point _new_position)
        {
            if (window_is_selected== is_selected.on)//
            {
                positionX = _new_position.X;
                positionY = _new_position.Y;

                Update_toolBox_colide_box();
                Update_colidebox_head(positionX, positionY);

            moving_window_state = 1;
           }
        }


        public int Get_window_colision(Point mouse_position)
        {
            int result = 0;
            if (window_colide_box.Contains(mouse_position))
            {
                return 1;
            }
            return result;
        }

        public int Get_colision_head_of_window(Point _new_position)
        {
            if (head_colide_box.Contains(_new_position) && window_is_selected== is_selected.on)//
            {
                return 1;
            }

            return 0;
        }

        public int Get_trigger_close_window(Point mouse_position)
        {
            if (action_buttons[0].rectposition.Contains(mouse_position) && window_is_selected== is_selected.on)//
            return 1;

            return 0;
        }


       

        public is_open Is_Open()
        {
            return window_is_open;
        }

        public void CloseWindow()
        {
            window_is_open = XMLDataBase.is_open.off;
        }

        public void OpenWindow()
        {
            window_is_open = XMLDataBase.is_open.on;
        }

        /// <summary>
        /// dessiner une boite de dialogue vierge
        /// + dessiner l'entête
        /// + dessiner le pieds
        /// + dessiner icone en bas à droite pour redimensionner la fenêtre
        /// </summary>
        /// <param name="width">(ce n'est pas en pixels mais en unités) mettre le nombre de tuiles horizontales</param>
        /// <param name="height">(ce n'est pas en pixels mais en unités) mettre le nombre de tuiles verticales</param>
        /// <param name="_spritebatch"></param>
       public void Make_Window_Box(int _width, int _height, SpriteBatch _spritebatch, Color custom_color)
        {
            if(first_resize_point.X>0 
                || window_making_state == 0
                || moving_window_state==1)
            {  
                
                list_window_elevators.Clear();
            list_window_body.Clear();
                list_window_header.Clear();

            if (my_windowBox_W < 0)
            {
                /// largeur d'un frame
                my_windowBox_W = _width;
                window_size_W = my_windowBox_W;
            }

            if(my_windowBox_H<0)
            {
                my_windowBox_H = _height;
                window_size_H = my_windowBox_H;
            }

                if (my_windowBox_H > 0 && my_windowBox_W > 0)
                {
                    head_colide_box = new Rectangle(positionX, positionY, my_windowBox_W * window_tile_H, window_tile_H);

                    window_colide_box = new Rectangle(positionX, positionY, my_windowBox_W * window_tile_H, my_windowBox_H * window_tile_H + window_tile_H);

                    int anchor_up_right = my_windowBox_W;
                    int anchor_down_left = my_windowBox_H * my_windowBox_W - (my_windowBox_W);
                    int anchor_down_right = my_windowBox_H * my_windowBox_W;
                    int size = my_windowBox_W * my_windowBox_H;


                    int k = 0;
                    int j = -1;

                    if (bordeless_state == 0)
                    { ///+++++++++++++++++++++++++++
                      /// dessiner l'EN TETE de la fenêtre
                      ///+++++++++++++++++++++++++++
                        for (int i = 0; i < my_windowBox_W * 2; i++)
                        {
                            if (i == 0)
                            {
                    Colider_Object header = new Colider_Object();
                                header.rectposition = new Rectangle(positionX, positionY + tile_frame_height / 2, tile_frame_width / 2, tile_frame_height / 2);
                                header.rectframe = new Rectangle(0, 0, tile_frame_width, tile_frame_height);
                                header.current_color = Color.White;
                                list_window_header.Add(header);
                            }
                            else if (i == my_windowBox_W * 2 - 1)
                            {
                    Colider_Object header = new Colider_Object();
                                header.rectposition = new Rectangle(i * tile_frame_width / 2 + positionX, positionY + tile_frame_height / 2, tile_frame_width / 2, tile_frame_height / 2);
                                header.rectframe = new Rectangle(2 * tile_frame_width, 0, tile_frame_width, tile_frame_height);
                                header.current_color = Color.White;
                                list_window_header.Add(header);
                            }
                            else
                            {

                    Colider_Object header = new Colider_Object();
                                header.rectposition = new Rectangle(i * tile_frame_width / 2 + positionX, positionY + tile_frame_height / 2, tile_frame_width / 2, tile_frame_height / 2);
                                    header.rectframe = new Rectangle(tile_frame_width, 0, tile_frame_width, tile_frame_height);
                                header.current_color = Color.White;

                                list_window_header.Add(header);
                              
                                if (bordeless_state == 0)
                                {
                                    action_buttons[0].rectposition = new Rectangle(
                                        i * tile_frame_width / 2 + positionX + tile_frame_width / 2,
                                        positionY + tile_frame_height / 2,
                                        tile_frame_width / 2, tile_frame_height / 2);
                                }
                            }
                        }

                    }

                    ///+++++++++++++++++++++++++++
                    /// dessiner le corps de la fenêtre
                    ///+++++++++++++++++++++++++++
                    for (int i = 0; i < size; i++)
                    {

                        if (i % my_windowBox_W == 0)
                        {
                            j++;
                            k = 0;
                        }
                        else
                        {
                            k++;
                        }

                        rect_position = new Rectangle(
                                k * window_tile_W + positionX,
                                j * window_tile_H + positionY + window_tile_H,
                                window_tile_W,
                                window_tile_H);


                        if (i == 0)
                        {

                            ///+++++++++++++++++++++++++++
                            /// dessiner le coin en haut à gauche
                            ///+++++++++++++++++++++++++++

                            rect_frame = new Rectangle(
                            anchors[0].X * tile_frame_width,
                            anchors[0].Y,
                            tile_frame_width,
                            tile_frame_height);
                        }
                        if (i > 0 && i < anchor_up_right)
                        {

                            ///+++++++++++++++++++++++++++
                            /// dessiner le haut au centre
                            ///+++++++++++++++++++++++++++

                            rect_frame = new Rectangle(
                           anchors[1].X,
                           anchors[1].Y,
                           tile_frame_width,
                           tile_frame_height);
                        }
                        if (i == anchor_up_right - 1)
                        {
                            ///+++++++++++++++++++++++++++
                            /// dessiner le coin en haut à droite
                            ///+++++++++++++++++++++++++++

                            rect_frame = new Rectangle(
                           anchors[2].X,
                           anchors[2].Y,
                           tile_frame_width,
                           tile_frame_height);

                            if (bordeless_state == 0)
                            {
                                Colider_Object temp = new Colider_Object();

                                temp.rectposition = Get_new_Rectangle(rect_position, tile_frame_width / 2, 0, tile_frame_width / 2, tile_frame_height / 2);
                                temp.rectframe = new Rectangle(tile_frame_width * 3, tile_frame_height, tile_frame_width, tile_frame_height);
                                temp.current_color = Color.White;
                                list_window_elevators.Add(temp);
                               
                                temp.rectposition = Get_new_Rectangle(rect_position, tile_frame_width / 2, window_tile_H / 2, tile_frame_width / 2, tile_frame_height / 2);
                                temp.rectframe = new Rectangle(tile_frame_width * 3, tile_frame_height, tile_frame_width, tile_frame_height); ;
                                temp.current_color = Color.White;
                                list_window_elevators.Add(temp);


                                /// le bouton ascenseur vertical
                                action_buttons[3].rectposition = Get_new_Rectangle(rect_position, window_tile_W / 2, 0, window_tile_W / 2, window_tile_H / 2);
                                action_buttons[3].rectframe = new Rectangle(tile_frame_width, 0, tile_frame_width, tile_frame_height);
                            }
                        }

                        if (k == 0
                            && i >= anchor_up_right
                            && i < anchor_down_right)
                        {
                            ///+++++++++++++++++++++++++++
                            /// dessiner le rebord de la fenêtre à gauche
                            ///+++++++++++++++++++++++++++
                            rect_frame = new Rectangle(
                           anchors[6].X,
                           anchors[6].Y,
                           tile_frame_width,
                           tile_frame_height);
                        }


                        if (i > anchor_up_right
                            && k != 0
                             && i < anchor_down_left)
                        {
                            ///+++++++++++++++++++++++++++
                            /// dessiner le milieu de la fenêtre
                            ///+++++++++++++++++++++++++++
                            rect_frame = new Rectangle(
                            anchors[7].X,
                            anchors[7].Y,
                            tile_frame_width,
                            tile_frame_height);
                        }

                        if (k == my_windowBox_W - 1
                            && i > anchor_up_right)
                        {
                            ///+++++++++++++++++++++++++++
                            /// dessiner le rebord de la fenêtre à droite
                            ///+++++++++++++++++++++++++++
                            rect_frame = new Rectangle(
                            anchors[8].X,
                            anchors[8].Y,
                            tile_frame_width,
                            tile_frame_height);

                            if (bordeless_state == 0)
                            {          ///+++++++++++++++++++++++++++
                                       /// placer l'acensceur à droite
                                       /// et calculer son frame
                                       ///+++++++++++++++++++++++++++
                                Colider_Object temp = new Colider_Object();

                                temp.rectposition = Get_new_Rectangle(rect_position, window_tile_W / 2, 0, window_tile_W / 2, window_tile_H / 2);
                                temp.rectframe = new Rectangle(tile_frame_width * 3, tile_frame_height, tile_frame_width, tile_frame_height);
                                temp.current_color = Color.White;
                                list_window_elevators.Add(temp);
                                temp.rectposition = Get_new_Rectangle(rect_position, window_tile_W / 2, window_tile_H / 2, window_tile_W / 2, window_tile_H / 2);
                                temp.rectframe = new Rectangle(tile_frame_width * 3, tile_frame_height, tile_frame_width, tile_frame_height); ;
                                temp.current_color = Color.White;
                                list_window_elevators.Add(temp);

                            }

                        }

                        if (i == anchor_down_left)
                        {
                            ///+++++++++++++++++++++++++++
                            /// dessiner le rebord de la fenêtre en bas à gauche
                            ///+++++++++++++++++++++++++++
                            rect_frame = new Rectangle(
                            anchors[3].X,
                            anchors[3].Y,
                            tile_frame_width,
                            tile_frame_height);

                            if (bordeless_state == 0)
                            {  ///+++++++++++++++++++++++++++
                               /// placer l'acensceur en bas
                               /// et calculer son frame
                               ///+++++++++++++++++++++++++++
                                Colider_Object temp = new Colider_Object();

                                temp.rectposition = Get_new_Rectangle(rect_position, 0, window_tile_H / 2, window_tile_W / 2, window_tile_H / 2);
                                temp.rectframe = new Rectangle(tile_frame_width, tile_frame_height * 3, tile_frame_width, tile_frame_height);
                                temp.current_color = Color.White;
                                list_window_elevators.Add(temp);

                                temp.rectposition = Get_new_Rectangle(rect_position, window_tile_W / 2, window_tile_H / 2, window_tile_W / 2, window_tile_H / 2);
                                temp.rectframe = new Rectangle(tile_frame_width, tile_frame_height * 3, tile_frame_width, tile_frame_height); ;
                                temp.current_color = Color.White;
                                list_window_elevators.Add(temp);


                                if (bordeless_state == 0)
                                {
                                    action_buttons[4].rectposition = new Rectangle(
                                        rect_position.X,
                                       rect_position.Y+ tile_frame_height / 2,
                                        tile_frame_width / 2, tile_frame_height / 2);

                                    /// le bouton ascenseur horizontal
                                   //// triggers[2].rectposition = Get_new_Rectangle(rect_position, 0, window_tile_H / 2, window_tile_W / 2, window_tile_H / 2);
                                    action_buttons[2].rectposition = new Rectangle(
                                        rect_position.X,
                                       rect_position.Y + window_tile_H / 2,
                                        window_tile_W / 2, window_tile_H / 2);
                                    action_buttons[2].rectframe = new Rectangle(0, 0, tile_frame_width, tile_frame_height);

                                }
                            }
                        }
                        if (i > anchor_down_left
                         && i % my_windowBox_W != 0)
                        {
                            ///+++++++++++++++++++++++++++
                            /// dessiner le bas de la fenêtre, au milieu
                            ///+++++++++++++++++++++++++++
                            rect_frame = new Rectangle(
                            anchors[4].X,
                            anchors[4].Y,
                            tile_frame_width,
                            tile_frame_height);

                            if (bordeless_state == 0)
                            {  ///+++++++++++++++++++++++++++
                               /// placer l'acensceur en bas
                               /// et calculer son frame
                               ///+++++++++++++++++++++++++++
                                Colider_Object temp = new Colider_Object();

                                temp.rectposition = Get_new_Rectangle(rect_position, 0, window_tile_W / 2, window_tile_W / 2, window_tile_H / 2);
                                temp.rectframe = new Rectangle(tile_frame_width, tile_frame_height * 3, tile_frame_width, tile_frame_height);
                                temp.current_color = Color.White;
                                list_window_elevators.Add(temp);

                                temp.rectposition = Get_new_Rectangle(rect_position, window_tile_W / 2, tile_frame_height / 2, window_tile_W / 2, tile_frame_height / 2);
                                temp.rectframe = new Rectangle(tile_frame_width, tile_frame_height * 3, tile_frame_width, tile_frame_height); ;
                                temp.current_color = Color.White;
                                list_window_elevators.Add(temp);
                            }
                        }
                        if (i == anchor_down_right - 1)
                        {
                            ///+++++++++++++++++++++++++++
                            /// dessiner le rebords en bas à droite
                            ///+++++++++++++++++++++++++++
                            rect_frame = new Rectangle(
                            anchors[5].X,
                            anchors[5].Y,
                            tile_frame_width,
                            tile_frame_height);

                            resizer_colide_box.rectposition = rect_position;
                            resizer_colide_box.rectframe = rect_frame;
                            if (bordeless_state == 0)
                            {
                                /// le bouton pour redimensionner la fenêtre
                                action_buttons[1].rectposition = Get_new_Rectangle(rect_position, window_tile_W / 2, window_tile_H / 2, window_tile_W / 2, window_tile_H / 2);
                                action_buttons[1].rectframe = new Rectangle(tile_frame_width * 2, 0, tile_frame_width, tile_frame_height);

                              

                              
                            }
                        }

                        ///+++++++++++++++++++++++++++
                        /// dessiner le tile de la fenêtre complète
                        ///+++++++++++++++++++++++++++
                        /* _spritebatch.Draw(body_texture,
                             rect_position,
                            rect_frame,
                                custom_color * fading_value);*/

                        Colider_Object window_body_object = new Colider_Object();
                        window_body_object.rectposition = rect_position;
                        window_body_object.rectframe = rect_frame;
                        window_body_object.current_color = Color.White;
                        list_window_body.Add(window_body_object);

                        window_making_state = 1;
                    }
                }
            }

        }

        /// <summary>
        /// tansformer notre tilesheet en boutons
        /// </summary>
        /// <param name="button_W">largeur du frame</param>
        /// <param name="button_H">hauteur du frame</param>
        /// <param name="_total_horizontal_button">nombre de boutons que l'on veux sur une ligne</param>
        /// <param name="_sprite_sheet">la texture2D du tilesheet</param>
        /// <param name="_spritebatch"></param>
        public void Make_buttons(int button_W, int button_H, int _total_horizontal_button, int button_Rectsize_W, int button_Rectsize_H)//, SpriteBatch _spritebatch)
        {

            btn_W = button_W;
            btn_H = button_H;

            ///+++++++++++++++++++++++++++
            /// dans cette méthode, on va découper en morceaux
            /// le tilesheet
            ///+++++++++++++++++++++++++++
            if (window_state != window_style.toolbox && tilesheet_texture!=null)
            {


                total_horizontal_button = _total_horizontal_button;

                int horizontal_texture_size = tilesheet_texture.Width / button_W;

                int vertical_texture_size = tilesheet_texture.Height / button_H;

                int texture_size = 10;// horizontal_texture_size * vertical_texture_size;

                button_colider = new Colider_Object[texture_size];
                Debug.WriteLine("" + button_colider.Length);

                if (rect_size_W < 0)
                {
                    rect_size_W = button_Rectsize_W;
                    rect_size_H = button_Rectsize_H;
                }
                ///  tilesheet_texture = _sprite_sheet;

                ///+++++++++++++++++++++++++++
                /// on commence par définir des frames
                ///+++++++++++++++++++++++++++


                int k = 0;
                int j = -1;
                for (int i = 0; i < texture_size; i++)
                {
                    if (i % horizontal_texture_size == 0)
                    {
                        k = 0;
                        j++;
                    }
                    else
                    {
                        k++;
                    }
                    if (bordeless_state == 0)
                    {
                        button_colider[i].rectframe = new Rectangle(k * button_W, j * button_H, button_W, button_H);
                    }
                }



                ///+++++++++++++++++++++++++++
                /// on continue en calibrant les rectposition
                ///+++++++++++++++++++++++++++
              //  colider[] temp = new colider[texture_size];

                int hauteur = button_colider[0].rectposition.Height;
                k = 0;
                j = -1;

                for (int i = 0; i < texture_size; i++)
                {
                    if (i % total_horizontal_button == 0)
                    {
                        k = 0;
                        j++;
                    }
                    else
                    {
                        k++;
                    }

                      button_colider[i].rectposition = new Rectangle(
                      k * rect_size_W + marge_Gauche + positionX + k * espacement,
                      j * rect_size_H + marge_Haut + positionY + j * espacement + rect_size_H,
                      rect_size_W, rect_size_H);
                    if (bordeless_state == 0)
                    {
                        button_colider[i].current_color = Color.Gray;
                    }
                }


                window_state = window_style.toolbox;
            }
        }

        public Rectangle Get_new_Rectangle(Rectangle origin, int deltaX, int deltaY)
        {
            return new Rectangle(origin.X + deltaX, origin.Y + deltaY, origin.Width, origin.Height);
        }

        public Rectangle Get_new_Rectangle(Rectangle origin, int deltaX, int deltaY, int new_width, int _new_height)
        {
            return new Rectangle(origin.X + deltaX, origin.Y + deltaY, new_width, _new_height);
        }

        public window_style Get_window_state()
        {
            return window_state;
        }
        public void Update_toolBox_colide_box()
        {
            ///**************************************************************
            /// lorsque l'on déplace la fenêtre, on doit mettre à jour..
            /// le position de tous les boutons.
            ///**************************************************************
            int k = 0;
            int j = -1;
            for (int i = 0; i < button_colider.Length; i++)
            {
                if (i % total_horizontal_button == 0)
                {
                    k = 0;
                    j++;
                }
                else
                {
                    k++;
                }

                ///**************************************************************
                /// ATTENTION !!! ne pas oublier de cloner dans la méthode
                /// Draw_tool_button() si tu changes de valeurs
                ///**************************************************************


                button_colider[i].rectposition = new Rectangle(
                    k * rect_size_W + marge_Gauche + positionX + k * espacement,
                j * rect_size_H + marge_Haut + positionY + j * espacement + rect_size_H,
                    rect_size_W, rect_size_H);

            }
        }

        public Rectangle Get_Texture_rect_Frame(int index)
        {
            return new Rectangle(index* btn_W, 0* btn_H, btn_W, btn_H);
        }
    
      

        public int Get_Index_Button_wanted()
        {
          
            return index_button_wanted;
        }

        public Rectangle Get_Rectframe(int positionX, int positionY, ref MouseState mouse_state, ref MouseState old_mouse_state)
        {
            Point mouseposition = new Point(positionX, positionY);
         
            int frameX = 0;
            int frameY = 0;

            int columns = 0;
            int lines = -1;
            if (button_colider != null)
            {
                limit_W = action_buttons[0].rectposition.X;

                limit_H =
                   (button_colider.Length / total_horizontal_button) * button_colider[0].rectposition.Height
                   + positionY
                   + button_colider[0].rectposition.Height * 2
                   + (button_colider.Length / total_horizontal_button) * espacement;

              

                for (int i = 0; i < button_colider.Length; i++)
                {

                    if(i%total_horizontal_button==0)
                    {
                        columns = 0;
                        lines++;
                    }
                    else
                    {
                        columns++;
                    }


                    if (positionX < limit_W
                       && positionY < limit_H)
                    {
                        Rectangle temp = Get_new_Rectangle(button_colider[i].rectposition, delta_elevator.X, delta_elevator.Y);

                        if (temp.Contains(mouseposition))
                        {
                            button_colider[i].current_color = Color.Orange;
                            if (mouse_state.LeftButton == ButtonState.Pressed
                              && old_mouse_state.LeftButton == ButtonState.Released)
                            {
                                frameX = columns * rect_frame.Width;
                                frameY = lines * rect_frame.Height;
                                return new Rectangle(frameX, frameY, rect_frame.Width, rect_frame.Height);
                            }
                        }
                        else
                        {
                            button_colider[i].current_color = Color.White;

                        }
                    }

                }
            }
            return new Rectangle(frameX, frameY, rect_frame.Width, rect_frame.Height);
        }


        public int Get_colision_button(Point mouse_position, MouseState mouse_state, MouseState old_mouse_state)
        {
            ///**************************************************************
            /// CE PROGRAMME renvoie l'index du bouton choisi
            ///**************************************************************


            ///+++++++++++++++++++++++++++
            /// détecter les colisions avec les boutons
            /// brancher sur bon comportement
            ///+++++++++++++++++++++++++++

            if (button_colider != null)
            {
                ///+++++++++++++++++++++++++++
                /// Tout les boutons ne sont pas bons à prendre
                /// surtout ceux qui dépassent la fenêtre
                ///+++++++++++++++++++++++++++

                limit_W = action_buttons[0].rectposition.X;

                 limit_H = 
                    (button_colider.Length / total_horizontal_button)*button_colider[0].rectposition.Height 
                    + positionY
                    + button_colider[0].rectposition.Height*2
                    + (button_colider.Length / total_horizontal_button)*espacement;
               
                for (int i = 0; i < button_colider.Length; i++)
                {

                    ///+++++++++++++++++++++++++++
                    /// VERIFIER la colision entre le curseur de la souris
                    /// et un bouton
                    ///+++++++++++++++++++++++++++
             
                    if(  mouse_position.X< limit_W
                       && mouse_position.Y < limit_H)
                    {
                        Rectangle temp = Get_new_Rectangle(button_colider[i].rectposition, delta_elevator.X, delta_elevator.Y);

                        if (temp.Contains(mouse_position))
                        {
                            button_colider[i].current_color = Color.White*0.8f;

                            if (mouse_state.LeftButton == ButtonState.Pressed
                                && old_mouse_state.LeftButton == ButtonState.Released)
                            {
                                index_button_wanted = i;
                            }
                        }
                        else
                        {
                            button_colider[i].current_color = Color.White*0.2f;

                        }
                    }
                   
                }
            }

            if(index_button_wanted>=0)
            button_colider[index_button_wanted].current_color = Color.GreenYellow*1f;

            return index_button_wanted;
        }
    }
}
