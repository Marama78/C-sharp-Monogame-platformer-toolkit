using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameToolkit.Engine.Components.shared;
using MonogameToolkit.Engine.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

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
    public class Scene_manager
    {
        public enum scenetype
        {
            splashscreen,
            menu,
            level,
            gameover
        }

        protected Mainclass mainclass;
        protected ContentManager content;
        protected SpriteBatch spritebatch;

        public EditorModel currentScene;
        private int loading_scene_state = 0;
        public Scene_manager(Mainclass _main, ContentManager _content, SpriteBatch _spritebatch)
        {
            mainclass = _main;
            content = _content;
            spritebatch = _spritebatch;
        }

        public void LoadScene(scenetype scene)
        {
            if (currentScene != null)
            {
                currentScene.KillScene();
                loading_scene_state = 0;
            }


            switch (scene)
            {
                case scenetype.splashscreen:
                    // currentScene = new  SplashScreen(mainclass,content,spritebatch);
                    currentScene = new Level_template(mainclass, content, spritebatch);
                    break;

                case scenetype.menu:
                    currentScene = new Menu(mainclass, content, spritebatch);
                    break;

                case scenetype.level:
                    Level_Params.current_level++;
                    currentScene = new Level_template(mainclass, content, spritebatch);
                    break;

                case scenetype.gameover:
                    break;
            }

            if (loading_scene_state == 0)
            {
                currentScene.Load();
                loading_scene_state = 1;
            }
        }

    }
}
