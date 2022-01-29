using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public interface ITile
    {
        public int positionX { get; set; }
        public int positionY { get; set; }
    }

    public interface ILayer
    {
        public int frame_positionX { get; set; }
        public int frame_positionY { get; set; }
        public int texture_ID { get; set; }
    }

    public interface Icolider
    {
        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }

        public Color current_color { get; set; }

        public int grid_ID { get; set; }
        public int grid_line { get; set; }
        public int grid_column { get; set; }

    }

    public struct Layer : ILayer
    {
        public int frame_positionX { get; set; }
        public int frame_positionY { get; set; }

        public int texture_ID { get; set; }

        public int logic_type_index { get; set; }

        public bad_guy_units unit_to_print{get;set;}
    }
   
}
