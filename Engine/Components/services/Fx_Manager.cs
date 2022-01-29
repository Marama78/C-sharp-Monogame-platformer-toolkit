using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameToolkit.Engine.Components.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameToolkit.Engine.Components.services
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
    class Fx_Manager
    {
        public List<particles_FX> list_particles { get; set; }
        int tile_size, line_W;

        float chrono_update;
        public Fx_Manager()
        {
            list_particles = new List<particles_FX>();
            tile_size = Level_Params.tile_size;
            line_W = Level_Params.line_size;
        }

        public void Add_Particles(ref List<particles_FX> list_new_particles)
        {
            List<particles_FX> temp = list_new_particles;

            for (int i = 0; i < temp.Count; i++)
            {
                list_particles.Add(temp[i]);
            }
        }

        public void Destroy_Particles()
        {
            List<particles_FX> temp = new List<particles_FX>();


            for (int i = 0; i < list_particles.Count; i++)
            {

                particles_FX newbie = list_particles[i];

                newbie.fading -= 0.1f;

                if (newbie.fading <= 0)
                {
                    newbie.fading = 0;
                    newbie.to_remove = 1;

                }

                temp.Add(newbie);
                if (i >= list_particles.Count - 1)
                {
                    list_particles.Clear();
                    list_particles = temp;
                }

                if (i == list_particles.Count - 1)
                {
                    list_particles.Clear();
                    list_particles = temp;
                }
            }
        }

        public Rectangle Get_New_Rectangle_Position(Rectangle target, int x, int y)
        {
            return new Rectangle(target.X + x, target.Y + y, target.Width, target.Height);
        }

        public void Update(ref Grid_Game_Object[] _grid_game)
        {
            List<particles_FX> temp_list = new List<particles_FX>();
            chrono_update += 0.8f;

            if (chrono_update >= 1)
            {
                for (int i = 0; i < list_particles.Count; i++)
                {
                    particles_FX temp = list_particles[i];

                    temp.fading -= 0.01f;

                    if (temp.fading > 0)
                    {
                        int index_grid = (int)(list_particles[i].position.X / tile_size) + (int)(list_particles[i].position.Y / tile_size) * line_W;
                        if (index_grid >= 0 && index_grid < 4500)
                        {
                            Grid_Game_Object tile_colision = _grid_game[index_grid];

                            if (tile_colision.logic_value == script_type.add_colider)
                            {
                                temp.color = Color.Red;
                                temp.auto_remove += 0.2f;
                                if (temp.auto_remove > 1)
                                {
                                    temp.to_remove = 1;
                                }

                            }
                            else
                            {
                                temp.gravity += 0.2f;
                                temp.position = new Rectangle((int)(temp.position.X + temp.horizontal_force / 25), (int)(temp.position.Y + temp.gravity), temp.rect_size, temp.rect_size);

                            }
                        }
                        else
                        {
                            temp.to_remove = 1;
                        }

                        temp_list.Add(temp);

                        if (i == list_particles.Count - 1)
                        {
                            list_particles.Clear();
                            list_particles = temp_list;
                        }
                    }
                    else
                    {
                        temp.to_remove = 1;

                    }
                    chrono_update = 0;
                }

            }


            list_particles.RemoveAll(item => item.to_remove == 1);

        }

        public void Draw(SpriteBatch _sprite_batch, ref Texture2D _particles_FX, ref Texture2D _texture_array, ref int delta_cameraX, ref int delta_cameraY) //ref Texture2D[] _texture_array,
        {
            if (list_particles.Count > 0)
            {
                foreach (particles_FX item in list_particles)
                {
                    Rectangle temp_part = Get_New_Rectangle_Position(item.position, delta_cameraX, delta_cameraY);
                    _sprite_batch.Draw(_particles_FX, temp_part, item.color * item.fading);

                }
            }

        }
    }
}
