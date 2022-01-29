using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameToolkit.Engine;
using System;
using System.Collections.Generic;
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
    class SplashScreen : EditorModel
    {
        Texture2D logo, jam;
        int scene_state = 0;
        float chrono_loading;
        float fading = 0;

        Rectangle rect_position_logo;
        public SplashScreen(Mainclass _main, ContentManager _content, SpriteBatch _spritebatch) : base(_main, _content, _spritebatch)
        {
        }



        public override void Load()
        {
            base.Load();
            logo = content.Load<Texture2D>("logo"); // 2433 - 512
            jam = content.Load<Texture2D>("gamejam33"); // 2433 - 512
            int rect_W = (mainclass.GraphicsDevice.Viewport.Width / 10) * 6;
            int rect_H = rect_W * 512 / 2433;
            int posX = (mainclass.GraphicsDevice.Viewport.Width / 10) * 2;
            int posY = (mainclass.GraphicsDevice.Viewport.Height - rect_H) / 2;
            rect_position_logo = new Rectangle(posX, posY, rect_W, rect_H);
            scene_state = 1;
        }

        public override void Update()
        {
            base.Update();
            switch (scene_state)
            {
                case 1:
                    fading += 0.005f;
                    if (fading >= 1)
                    {
                        scene_state = 2;
                    }
                    break;
                case 2:
                    fading = 1;
                    chrono_loading += 0.005f;
                    if (chrono_loading >= 1)
                    {
                        scene_state = 3;
                    }
                    break;
                case 3:
                    fading -= 0.009f;
                    if (fading <= 0)
                    {
                        scene_state = 4;
                        chrono_loading = 0;
                    }
                    break;
                case 4:
                    chrono_loading += 0.01f;
                    if (chrono_loading >= 1)
                    {
                        scene_state = 5;
                    }
                    break;

                case 5:
                    fading += 0.005f;
                    if (fading >= 1)
                    {
                        scene_state = 6;
                        chrono_loading = 0;

                    }
                    break;
                case 6:
                    fading = 1;
                    chrono_loading += 0.005f;
                    if (chrono_loading >= 1)
                    {
                        scene_state = 7;
                    }
                    break;
                case 7:
                    fading -= 0.009f;
                    if (fading <= 0)
                    {
                        scene_state = 8;
                        chrono_loading = 0;
                    }
                    break;
                case 8:
                    chrono_loading += 0.01f;
                    if (chrono_loading >= 1)
                    {
                        mainclass.sceneState.LoadScene(Scene_manager.scenetype.menu);
                    }
                    break;
            }


        }


        public override void Draw()
        {
            base.Draw();
            if (scene_state == 1
                || scene_state == 2
                || scene_state == 3)
            {
                spritebatch.Draw(logo, rect_position_logo, Color.White * fading);
            }
            if (scene_state == 5
                || scene_state == 6
                || scene_state == 7)
            {
                spritebatch.Draw(jam, rect_position_logo, Color.White * fading);
            }
        }
    }
}
