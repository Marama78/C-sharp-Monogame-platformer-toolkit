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
    public struct Tiles : ILayer
    {
        public int frame_positionX { get; set; }
        public int frame_positionY { get; set; }
        public int texture_ID { get; set; }
        public int logic_type_index { get; set; }


    }
    public class TileMap
    {
        public Tiles[] list_tile;
    }

    public class bad_guys_settings
    {
        public int grid_index { get; set; }
        public int unit_style { get; set; }

    }

    public class Level_Initialize
    {
        public int nombre_de_tuiles_sur_une_ligne;
        public int nombre_de_tuiles_sur_une_colonne;
        public int la_largeur_des_tuiles;
        public int la_hauteur_des_tuiles;
        public string le_nom_de_la_texture;
        public int nombre_de_tuiles_a_afficher_sur_une_ligne;
        public int forcer_taille_tuile_fixe;

    }

    public class window_boxes_init
    {
        /// <summary>
        /// cette classe contient les informations pour paramétrer
        /// mes windows
        /// </summary>
        public int w_positionX;
        public int w_positionY;
        public int w_tex_frameW;
        public int w_tex_frameH;
        public int w_button_sizeW;
        public int w_button_sizeH;
    }

    public class Modding_parameters
    {
        public string nom_du_fichier_avec_extension_svp;
        public int largeur_de_la_frame;
        public int hauteur_du_frame;
    }

    public class Player_Settings
    {

        public int speed_movement { get; set; }

        public int player_sizeW { get; set; }
        public int player_sizeH { get; set; }
        public Rectangle rect_frame { get; set; }
        public int number_frames_start { get; set; }
        public int current_anim_start { get; set; }
        public float speed_anim_start { get; set; }

        public int sword_W { get; set; }
        public int sword_H { get; set; }

        public int size_W_on_screen { get; set; }

        public int sword_number_frames_anim_start { get; set; }
        public int sword_number_frames_anim_colliding { get; set; }
        public int sword_number_frames_anim_exploding { get; set; }



    }

    public struct Colider_Object : Icolider
    {
        public Rectangle rectposition { get; set; }
        public Rectangle rectframe { get; set; }
        public Color current_color { get; set; }
        public int grid_ID { get; set; }
        public int grid_line { get; set; }
        public int grid_column { get; set; }

    }
}
