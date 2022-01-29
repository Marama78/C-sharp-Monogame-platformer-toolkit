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
    public class Coin : IObject
    {

       

        public bool toRemove { get; set; }
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
        public float chronoAnim { get; set; }
        public float speedAnim { get; set; }
        public int loading_state = 0;
        public void Update()
        {
            if (loading_state == 1)
            {
                chronoAnim += speedAnim;

                if (chronoAnim >= numberframes)
                {
                    chronoAnim = 0;
                }
                rectframe = new Rectangle((int)chronoAnim * frame_width, current_anim * frame_height, frame_width, frame_height);
            }
        }
    }
}
