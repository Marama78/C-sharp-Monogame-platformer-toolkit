using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using XMLDataBase;
using Microsoft.Xna.Framework.Media;
using MonogameToolkit.Engine.Components.services;
using MonogameToolkit.Engine.Components.shared;

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
    public struct Texture_object : Itexture
    {
        public Texture2D texture_object { get; set; }

        public string texture_name { get; set; }
    }

    class EditorTemplate : EditorModel
    {

        protected triggers display_log = triggers.show;
        private hover_btn hover_mouse;

        public Rectangle tile_rect_frame_choosen, script_rect_frame_choosen;
        protected int screen_device_centerX;

        protected int delta_cameraX, delta_cameraY;

        protected action_player current_player_state, old_player_state;

        protected int grid_start_position;
        protected XMLDataBase.Window window_tiles_asset1;
        protected XMLDataBase.Window window_script_standard;
        protected XMLDataBase.Window window_script_flying_creatures;
        protected XMLDataBase.Window window_tiles_asset2;
        protected XMLDataBase.Window window_tiles_asset3;

        protected int get_new_level_state = 0;


        protected XMLDataBase.Modding_parameters my_mods;
        protected XMLDataBase.Level_Initialize my_params;

        protected XMLDataBase.window_boxes_init window_tile_params;
        protected XMLDataBase.window_boxes_init window_script_params;

        protected XMLDataBase.Player_Settings player_settings;

        protected Horizontal_Direction_LookAt horizontal_player_lookAt, old_horizontal_player_lookAt;
        protected Vertical_Direction_LookAt vertical_player_lookAt, old_vertical_player_lookAt;

        protected Grid_Game_Object[] my_grid_game;

        protected command_btn command_btn_state = command_btn.is_hidden;

        protected Button[] buttons_command;
        protected Button[] buttons_menu;


        protected ChoosenTile index_tile_to_print;

        protected Camera my_camera, my_tile_panel_camera, my_logic_panel_camera, my_software_panel_camera;


        protected ColideBox next_level_colider;

        protected List<int> list_bad_guy_indexer;
        
        protected List<ColideBox> list_colide_boxes;
        

        protected List<ColideBox> list_go_through_boxes;

        protected List<ColideBox> list_ladder;

        protected List<ColideBox> list_hit_player;

        protected List<Coin> list_coin_temp, list_coin_temp_for_tilemap, list_coin_temp_for_tilemap_for_draw;

        protected List<Actor_Object> list_tirs, list_tirs_for_Draw;

        /// <summary>
        /// - les chemins d'accès -
        /// </summary>
        protected string modding_path, modding_folder, tilemap_xml_path, logicmap_xml_path, myparam_xml_path, player_settings_xml_path;

        protected int tile_size, texture_frameW, texture_frameH;
        protected int level_W, level_H, level_size;
        protected int display_W;
        protected int move_speed = 2;
        protected editor_loader editor_loader_state = editor_loader.none;
        protected int ID_currentTile;
        protected int page_tools_size = 9;
        protected int modding_state = 0;
        protected int ID_tool_selected, old_ID_tool_selected;
        protected active_window editor_behaviour_state;//int editor_behaviour_state = 0;
        protected int old_middle_mouse_positionX, old_middle_mouse_positionY;
        protected int current_layer;
        protected script_type script_to_print = script_type.none;

        protected const int BUTTON_WIDTH = 300;
        protected const int BUTTON_HEIGHT = 107;
        protected const int MESSAGE_BOX_WIDTH = 640;
        protected const int MESSAGE_BOX_HEIGHT = 250;


        protected float chronoReduce = 0;

        protected float chronoResize = 0;


        protected int old_scroll_wheel_value;
        protected int Start_player_positionX;
        protected int Start_player_positionY;

        protected game_state gaming_state;  

        protected Texture2D[] all_textures;
        protected Texture_object[] tilesheet_textures;
        protected Texture2D tex_menu_butons;

        protected Button button_play;
        protected Actor_Object my_player;
        protected Point mousePosition;
        protected MouseState old_ms;
        protected MouseState current_ms;
        protected KeyboardState current_KB, old_KB;
        protected int horizontal_colision_state, vertical_colision_state;
        protected const float DEFAULT_GRAVITY = 8;
        protected float current_gravity, gravity_rate, jumping_force;
        protected int ladder_state;
        protected float chrono_shut_down_go_trough;
        protected int go_trough_state = 0;


        protected float chrono_ladder_state;
        protected int current_ladder_ID = 0; 
        protected int move_horizontal_wanted = 0;
        protected int move_vertical_wanted = 0;

        protected int buttons_fixed_size;

        protected int center_colision = 1;
        protected float chrono_tir_player;
        protected int[] vertical_trigger_state, horizontal_trigger_state;
        protected SpriteFont windowbox_FONT;

        protected int toolbox_header_window_state = 0;
        protected Point delta_mouse_toolbox_panel;

        protected int logic_header_window_state = 0;
        protected Point delta_mouse_logic_panel;

        protected int rectsize_W, rectsize_H;
        protected int ID_next_level_tilemap;

        protected Service_Xml_manager service_xml;
        protected List<XMLDataBase.Window> list_window;
        public EditorTemplate(Mainclass _main, ContentManager _content, SpriteBatch _spritebatch) : base(_main, _content, _spritebatch)
        {
            current_layer = Level_Params.current_level;
        }

        public void Check_active_windowBox(ref MouseState current_ms, ref MouseState old_ms)
        {
            ///----------------------------------------------
            ///  activer ou bien désactiver une window
            ///----------------------------------------------


            if (current_ms.LeftButton == ButtonState.Pressed
                && old_ms.LeftButton == ButtonState.Released)
            {
                if (window_tiles_asset1.Get_windowbox_colideBox().Contains(mousePosition)
                    && window_tiles_asset1.Is_Open() == XMLDataBase.is_open.on)
                {
                    window_tiles_asset1.Edit_Window_Is_Selected(is_selected.on);
                    window_script_standard.Edit_Window_Is_Selected(is_selected.off);
                }

                if (window_script_standard.Get_windowbox_colideBox().Contains(mousePosition)
                   && window_script_standard.Is_Open() == XMLDataBase.is_open.on)
                {
                    window_tiles_asset1.Edit_Window_Is_Selected(is_selected.off);
                    window_script_standard.Edit_Window_Is_Selected(is_selected.on);
                }
            }

        }

        public void Load_Textures()
        {

            tilesheet_textures = new Texture_object[5];
            tilesheet_textures[0].texture_object = content.Load<Texture2D>("tile");
            tilesheet_textures[1].texture_object = content.Load<Texture2D>("grid");
            tilesheet_textures[2].texture_object = content.Load<Texture2D>("tile");
            tilesheet_textures[3].texture_object = content.Load<Texture2D>("tile");
            tilesheet_textures[4].texture_object = content.Load<Texture2D>("tile");



            all_textures = new Texture2D[13];
            all_textures[0] = null; // lire le XML de mods et attribuer le bon objet
            all_textures[1] = content.Load<Texture2D>("UI\\window\\grid");
            /// all_textures[2] = content.Load<Texture2D>("message_box");
            all_textures[3] = content.Load<Texture2D>("UI\\window\\window");
            all_textures[4] = content.Load<Texture2D>("butons");//("logic_tools");
            all_textures[5] = content.Load<Texture2D>("select_tool");
            all_textures[6] = content.Load<Texture2D>("model_button");
            all_textures[7] = content.Load<Texture2D>("choosen_item_icon");
            all_textures[8] = content.Load<Texture2D>("hero");
            all_textures[9] = content.Load<Texture2D>("coin");
            all_textures[10] = content.Load<Texture2D>("sword");
            all_textures[11] = content.Load<Texture2D>("UI\\window\\head_window");
            all_textures[12] = content.Load<Texture2D>("UI\\window\\footer_window");

            tex_menu_butons = content.Load<Texture2D>("model_button");

        }

        public void Load_Services()
        {
            

            window_tiles_asset1 = new XMLDataBase.Window(48, 48, 48, 48, windowbox_FONT);
            window_script_standard = new XMLDataBase.Window(48, 48, 48, 48, windowbox_FONT);
            window_script_standard.Edit_triggers();
            window_tiles_asset1.Edit_triggers();

            Load_Window(ref window_tiles_asset1, 500, 200, tilesheet_textures[0].texture_object);
            Load_Window(ref window_script_standard, 500, 400, all_textures[4]);

            /// éditer les dimensions des boutons
            window_tiles_asset1.Make_buttons(16, 16, 10, 32, 32);// windowbox_init.window_toolbox_button_size_W, windowbox_init.window_toolbox_button_size_W);
            window_script_standard.Make_buttons(32, 32, 10, 32, 32);//windowbox_init.window_toolbox_button_size_W, windowbox_init.window_toolbox_button_size_W);

            ///---- enregistrer mes windows dansune liste ---
            list_window = new List<Window>();

            list_window.Add(window_tiles_asset1);
            list_window.Add(window_script_standard);

            Reset_Logic_list();

        }

        public void Load_window_boxes_Init_params()
        {
            window_script_params = new XMLDataBase.window_boxes_init();
            string path = "xml\\window_script_params.xml";

            if (File.Exists(path))
            {
                window_script_params = service_xml.Load_window_params(ref path, typeof(XMLDataBase.window_boxes_init));
            }
            else
            {
                service_xml.Create_new_XML_windows_params(path);
            }

            window_tile_params = new XMLDataBase.window_boxes_init();
            path = "xml\\window_tile_params.xml";
            if (File.Exists(path))
            {
                window_tile_params = service_xml.Load_window_params(ref path, typeof(XMLDataBase.window_boxes_init));
            }
            else
            {
                service_xml.Create_new_XML_windows_params(path);
            }

        }

        public void Load_mods()
        {
            ///------------------------------------------------------------------
            /// Calculer les dimensions des boutons du niveau
            ///------------------------------------------------------------------
            switch (modding_state)
            {
                case 0:
                    tilesheet_textures[0].texture_object = content.Load<Texture2D>("tile");
                    break;
                case 1:
                    tilesheet_textures[0].texture_object = Get_Image_and_Convert_To_Texture2D("modding\\" + my_mods.nom_du_fichier_avec_extension_svp);

                    break;
            }


        }


        public void Load_path()
        {

            ///------------------------------------------------------------------
            /// lignes de code pour charger les chemins d'accès
            ///------------------------------------------------------------------
            modding_path = "xml\\Initiate_Mods.xml";
            modding_folder = "Content\\modding\\";

            player_settings_xml_path = "xml\\player_settings.xml";

            current_layer = Level_Params.current_level;

            tilemap_xml_path = "xml\\tilemap\\map1_" + current_layer + ".xml";
            logicmap_xml_path = "xml\\logicmap\\logicmap1_" + current_layer + ".xml";
            myparam_xml_path = "xml\\Level_initialize.xml";

            service_xml.Set_Tilemap_Path(tilemap_xml_path);
        }

        public void Load_Setters()
        {
            //   my_logicMap = new XMLDataBase.LogicMap();

            my_params = new XMLDataBase.Level_Initialize();
            my_params = service_xml.Load_Initialize_Level(myparam_xml_path, typeof(XMLDataBase.Level_Initialize), my_params);

            player_settings = service_xml.Load_Player_Settings(player_settings_xml_path, typeof(XMLDataBase.Player_Settings), player_settings);

            ///--- charger la vitesse de l'avatar ---
            move_speed = player_settings.speed_movement;

            screen_device_centerX = mainclass.INT_SCREEN_WIDTH / 2 - player_settings.player_sizeW / 2;

        }

        public override void Load()
        {
            base.Load();

            service_xml = new Service_Xml_manager();

            /// CreateNewXML_player_settings();
            vertical_trigger_state = new int[3];
            horizontal_trigger_state = new int[3];

            my_camera = new Camera();
            my_logic_panel_camera = new Camera();
            my_software_panel_camera = new Camera();
            my_tile_panel_camera = new Camera();

            gravity_rate = jumping_force + 100;
            current_gravity = 8;
            jumping_force = 4 * current_gravity;
         //   list_logic_textures = new List<simple_texture>();

            buttons_fixed_size = 48;


            Load_Textures();
            Load_window_boxes_Init_params();
            Load_path();
            Load_mods();
            Load_Setters();
            Load_Services();

            CheckModding_Attributes();

            
            windowbox_FONT = content.Load<SpriteFont>("dialogFont");



            texture_frameW = my_params.la_largeur_des_tuiles;
            texture_frameH = my_params.la_hauteur_des_tuiles;



            ///------------------------------------------------------------------
            /// Calculer les dimensions du niveau
            ///------------------------------------------------------------------

            level_W = my_params.nombre_de_tuiles_sur_une_ligne;
            level_H = my_params.nombre_de_tuiles_sur_une_colonne;
            level_size = level_H * level_W;



            tile_size = my_params.forcer_taille_tuile_fixe;

            rectsize_W = my_params.forcer_taille_tuile_fixe;
            rectsize_H = my_params.forcer_taille_tuile_fixe;

            display_W = my_params.nombre_de_tuiles_a_afficher_sur_une_ligne;

            ///------------------------------------------------------------------
            /// Charger les textures et construire le tableau du niveau
            ///------------------------------------------------------------------

            XMLDataBase.TileMap tilemap_to_load = new XMLDataBase.TileMap();
            // my_logicMap = new XMLDataBase.LogicMap();

            if (File.Exists(tilemap_xml_path))
            {
                ///------------------------------------------------------------------
                /// un fichier est repéré dans le répertoire, donc on va le charger
                ///------------------------------------------------------------------
                // pour les essais on charge la map1.xml
                tilemap_to_load = service_xml.LoadTileMap(tilemap_xml_path);
            }
            else
            {
                ///------------------------------------------------------------------
                /// sinon on détruit tout et son réécrit tout
                ///------------------------------------------------------------------
                Delete_All_Map_And_Make_EmptyOnes();
                service_xml.SetTileMap(my_grid_game.Length);
                service_xml.Create_NewXML_TileObject();
                tilemap_to_load = service_xml.LoadTileMap(tilemap_xml_path);

            }




            list_coin_temp = new List<Coin>();
            list_coin_temp_for_tilemap = new List<Coin>();
            list_coin_temp_for_tilemap_for_draw = new List<Coin>();

            int end = 0;

            while (end == 0)
            {
                int size = tilemap_to_load.list_tile.Length;
                Make_TileMap(tilemap_to_load, size);
                Edit_Script_on_grid_game();
                end++;
            }
            Load_Buttons_params();

            if (tile_size < buttons_fixed_size)
            {
                buttons_fixed_size = tile_size;
            }

          /*  Create_Colider_Boxes();
            Create_Ladders();*/
          
            editor_loader_state = editor_loader.set_the_arrays;


        }



        public void Get_Next_level_colision()
        {
            Rectangle temp = Get_New_Rectangle_Position(my_grid_game[ID_next_level_tilemap].rectposition, my_camera.rectposition.X, my_camera.rectposition.Y);

            Point colider = my_player.Get_Colider_by_Name(player_colider_objects.left_U, delta_cameraX, delta_cameraY);

            if (temp.Contains(colider))
            {
                ///*******************************
                /// Get new level =>> edit.level.cs
                ///*******************************

                editor_loader_state = 0;
                get_new_level_state = 1;
                Level_Params.gaming_state = 1;
                my_grid_game[ID_next_level_tilemap].current_color = Color.Red;

                mainclass.sceneState.LoadScene(Scene_manager.scenetype.level);
            }
        }

        public void GoToNeXtLevel()
        {
            editor_loader_state = 0;
            get_new_level_state = 1;
            Level_Params.gaming_state = 1;
            my_grid_game[ID_next_level_tilemap].current_color = Color.Red;

            mainclass.sceneState.LoadScene(Scene_manager.scenetype.level);
        }



        public void CheckModding_Attributes()
        {
            ///------------------------------------------------------------------
            /// *** lire le fichier de modding
            /// *** vérifier si le dossier rempli les conditions
            /// *** si oui => activer
            /// *** si non => refuser
            ///------------------------------------------------------------------
         /*   XMLDataBase.Modding_parameters temp_mod_sheet = new XMLDataBase.Modding_parameters();

            temp_mod_sheet = service_xml.Load_Modding_XML(modding_path, typeof(XMLDataBase.Modding_parameters), temp_mod_sheet);

            string _path_to_mod = modding_folder + temp_mod_sheet.nom_du_fichier_avec_extension_svp;
            if (File.Exists(_path_to_mod))
            {
                my_mods = service_xml.Load_Modding_XML(modding_path, typeof(XMLDataBase.Modding_parameters), my_mods);
                modding_state = 1;
            }*/
        }


        public int Convert_logic_type_to_index(script_type value)
        {
            switch (value)
            {
                case script_type.none:
                    return 0;
                case script_type.add_colider:
                    return 1;
                case script_type.player_start_position:
                    return 2;
                case script_type.go_to_nex_level:
                    return 3;
                case script_type.add_ladder:
                    return 4;
                case script_type.add_coin:
                    return 5;
                case script_type.add_go_through:
                    return 6;
                case script_type.flying_bat:
                    return 7;
                case script_type.flying_peach:
                    return 8;
                case script_type.zombie:
                    return 9;
              
            }
            return 0;
        }

        public script_type Convert_index_to_logic_type(int index)
        {
            switch (index)
            {
                case 0:
                    return script_type.none;
                case 1:
                    return script_type.add_colider;
                case 2:
                    return script_type.player_start_position;
                case 3:
                    return script_type.go_to_nex_level;
                case 4:
                    return script_type.add_ladder;
                case 5:
                    return script_type.add_coin;
                case 6:
                    return script_type.add_go_through;
                case 7:
                    return script_type.flying_bat;
                case 8:
                    return script_type.flying_peach;
                case 9:
                    return script_type.zombie;
            }
            return script_type.none;
        }
        public int Make_TileMap(XMLDataBase.TileMap _tilemap, int size)
        {
            int column = 0;
            int line = -1;
            my_grid_game = new Grid_Game_Object[size];

            Level_Params.line_size = level_W;
            Level_Params.tile_size = tile_size;

            for (int i = 0; i < my_grid_game.Length; i++)
            {
                XMLDataBase.Tiles currentTile = new XMLDataBase.Tiles();
                currentTile = _tilemap.list_tile[i];

                if (i % level_W == 0)
                {
                    column = 0;
                    line++;
                }
                else
                {
                    column++;
                }

                int positionX = column * tile_size;
                int positionY = line * tile_size;

                Rectangle rect_standard = new Rectangle(positionX, positionY, tile_size, tile_size);


                my_grid_game[i].grid_ID = i;
                my_grid_game[i].grid_column = column;
                my_grid_game[i].grid_line = line;

                my_grid_game[i].rectposition = rect_standard;
                my_grid_game[i].colideBox = rect_standard;


                int frameX = currentTile.frame_positionX;
                int frameY = currentTile.frame_positionY;

                my_grid_game[i].rectframe = new Rectangle(frameX, frameY, texture_frameW, texture_frameH);

                my_grid_game[i].default_color = Color.White;

                my_grid_game[i].current_color = Color.White;

                my_grid_game[i].texture_ID = currentTile.texture_ID;

                my_grid_game[i].logic_value_index = currentTile.logic_type_index;

                my_grid_game[i].logic_value = Convert_index_to_logic_type(my_grid_game[i].logic_value_index);




                script_type value = my_grid_game[i].logic_value;

                switch (value)
                {
                    case script_type.none:
                        break;

                    case script_type.add_colider:
                        ColideBox new_colide_box = new ColideBox();
                        new_colide_box.colideBox = rect_standard;

                        new_colide_box.grid_ID = my_grid_game[i].grid_ID;
                        new_colide_box.grid_column = my_grid_game[i].grid_column;
                        new_colide_box.grid_line = my_grid_game[i].grid_line;

                        list_colide_boxes.Add(new_colide_box);
                        break;

                    case script_type.player_start_position:
                        grid_start_position = i;
                        break;

                    case script_type.go_to_nex_level:
                        // ID_next_level_tilemap = start_index;
                        break;

                    case script_type.add_ladder:
                        ColideBox new_ladder = new ColideBox();
                        new_ladder.colideBox = rect_standard;
                        new_ladder.grid_ID = my_grid_game[i].grid_ID;
                        new_ladder.grid_column = my_grid_game[i].grid_column;
                        new_ladder.grid_line = my_grid_game[i].grid_line;
                        list_ladder.Add(new_ladder);
                        break;
                    case script_type.add_coin:
                        ///--- dessiner une pièce d'or ----

                        Coin new_coin = new Coin();
                        new_coin.rectposition = rect_standard;
                        new_coin.colideBox = rect_standard;
                        list_coin_temp.Add(new_coin);

                        break;

                    case script_type.add_go_through:
                        ColideBox new_GT = new ColideBox();
                        new_GT.colideBox = rect_standard;

                        new_GT.grid_ID = my_grid_game[i].grid_ID;
                        new_GT.grid_column = my_grid_game[i].grid_column;
                        new_GT.grid_line = my_grid_game[i].grid_line;

                        list_go_through_boxes.Add(new_GT);
                        break;

                    case script_type.Hit_the_player:
                        ColideBox new_hitter = new ColideBox();
                        new_hitter.colideBox = rect_standard;

                        new_hitter.grid_ID = my_grid_game[i].grid_ID;
                        new_hitter.grid_column = my_grid_game[i].grid_column;
                        new_hitter.grid_line = my_grid_game[i].grid_line;

                        list_hit_player.Add(new_hitter);
                        break;
                    case script_type.enter_gate_Index:
                        break;
                    case script_type.exit_gate_Index:
                        break;


                

                }



                if (i == my_grid_game.Length - 1)
                {
                    return 10;
                }
            }
            return 0;
        }


        public void Reset_Logic_list()
        {
            if(list_bad_guy_indexer==null)
            {
                list_bad_guy_indexer = new List<int>();
            }
            else
            {
                list_bad_guy_indexer.Clear();
            }

            if(list_coin_temp == null)
            {
                list_coin_temp = new List<Coin>();
            }
            else
            {
                list_coin_temp.Clear();
            }

            if (list_colide_boxes == null)
            {
                list_colide_boxes = new List<ColideBox>();
            }
            else
            {
                list_colide_boxes.Clear();
            }

            if (list_go_through_boxes == null)
            {
                list_go_through_boxes = new List<ColideBox>();
            }
            else
            {
                list_go_through_boxes.Clear();
            }

            if (list_ladder == null)
            {
                list_ladder = new List<ColideBox>();
            }
            else
            {
                list_ladder.Clear();
            }

            if (list_hit_player == null)
            {
                list_hit_player = new List<ColideBox>();
            }
            else
            {
                list_hit_player.Clear();
            }

        }



        public void Edit_Script_on_grid_game()
        {
            ///---------------------------------
            ///  suite de MakeTIlemap() 
            /// ici, on va écrire dans notre grille
            ///  les scripts
            ///---------------------------------


            int horizontal = 0;
            int vertical = 0;

            int sizeW = tile_size;
            int sizeH = tile_size;

            for (int i = 0; i < my_grid_game.Length; i++)
            {
                int positionX = my_grid_game[i].rectposition.X;
                int positionY = my_grid_game[i].rectposition.Y;

                Rectangle rect_standard = new Rectangle(positionX, positionY, sizeW, sizeH);


                if (i % level_W == 0)
                {
                    horizontal = 0;
                    vertical++;
                }
                else
                {
                    horizontal++;
                }

                script_type value = my_grid_game[i].logic_value;

                switch (value)
                {
                    case script_type.none:
                        break;

                    case script_type.add_colider:

                    

                        break;

                    case script_type.player_start_position:
                        grid_start_position = i;
                        break;

                    case script_type.go_to_nex_level:
                        ColideBox next_level_colider_temp = new ColideBox();
                        next_level_colider_temp.colideBox = rect_standard;

                        next_level_colider_temp.grid_ID = my_grid_game[i].grid_ID;
                        next_level_colider_temp.grid_column = my_grid_game[i].grid_column;
                        next_level_colider_temp.grid_line = my_grid_game[i].grid_line;

                        next_level_colider = next_level_colider_temp;
                        break;

                    case script_type.add_ladder:
                     
                        break;
                    case script_type.add_coin:
                        ///--- dessiner une pièce d'or ----

                        Coin new_coin = new Coin();
                        new_coin.rectposition = rect_standard;
                        new_coin.colideBox = rect_standard;
                        list_coin_temp.Add(new_coin);

                        break;

                    case script_type.add_go_through:
                        ColideBox new_GT = new ColideBox();
                        new_GT.colideBox = rect_standard;

                        new_GT.grid_ID = my_grid_game[i].grid_ID;
                        new_GT.grid_column = my_grid_game[i].grid_column;
                        new_GT.grid_line = my_grid_game[i].grid_line;

                        list_go_through_boxes.Add(new_GT);
                        break;

                    case script_type.Hit_the_player:
                        ColideBox new_hitter = new ColideBox();
                        new_hitter.colideBox = rect_standard;

                        new_hitter.grid_ID = my_grid_game[i].grid_ID;
                        new_hitter.grid_column = my_grid_game[i].grid_column;
                        new_hitter.grid_line = my_grid_game[i].grid_line;

                        list_hit_player.Add(new_hitter);
                        break;
                    case script_type.enter_gate_Index:
                        //todo 
                        break;
                    case script_type.exit_gate_Index:
                        //todo 
                        break;
                    case script_type.flying_peach:
                        if (!list_bad_guy_indexer.Contains(ID_currentTile))
                            list_bad_guy_indexer.Add(ID_currentTile);
                        //todo 
                        break;


                    case script_type.flying_bat:
                        if (!list_bad_guy_indexer.Contains(ID_currentTile))
                            list_bad_guy_indexer.Add(ID_currentTile);

                        //todo 
                        break;

                    case script_type.zombie:
                        if (!list_bad_guy_indexer.Contains(ID_currentTile))
                            list_bad_guy_indexer.Add(ID_currentTile);

                        //todo 
                        break;
                }

            }

            Create_Colider_Boxes();
            Create_Ladders();

        }

    


        public void Load_Buttons_params()
        {
            ///------------------------------------------------------------------
            /// Créer les boutons des onglets de navigation
            ///------------------------------------------------------------------
            Set_Menu_Btn(256, 32, 0, 3);

            ///------------------------------------------------------------------
            /// construire les boutons [quitter] * [enregistrer] * [tout effacer]
            ///------------------------------------------------------------------
            Set_Command_Btn(256, 32, 0, 3);
        }

        public void Hide_Command_Btn()
        {

        }


        /// <summary>
        /// Construire les boutons de commande du menu 'files'
        /// </summary>
        /// <param name="_frameW">largeur de la frame</param>
        /// <param name="_frameH">hauteur de la frame</param>
        /// <param name="texture_index">index dans le tableau des textures</param>
        /// <param name="number_of_buttons">nombre de boutons à créer</param>
        public void Set_Command_Btn(int _frameW, int _frameH, int texture_index, int number_of_buttons)
        {
            buttons_command = new Button[number_of_buttons];

            int sizeW = tile_size*4;
            int sizeH = (sizeW * _frameH) / _frameW;

            int under_the_menu_buttons = buttons_menu[0].rectposition.X + buttons_menu[0].rectposition.Height;
            int positionX = tile_size;

            for (int i = 0; i < buttons_command.Length; i++)
            {

                int positionY = (i * sizeH * 2 ) + under_the_menu_buttons;

                Rectangle standard_rect_position = new Rectangle(positionX, positionY, sizeW, sizeH * 2);


                buttons_command[i].rectposition = standard_rect_position;

                buttons_command[i].ColideBox = standard_rect_position;

                buttons_command[i].rectframe = new Rectangle(0, 0, _frameW, _frameH);

                buttons_command[i].current_color = Color.White;

                buttons_command[i].texture_ID = texture_index;


                switch (i)
                {
                    case 0:
                        buttons_command[i].name_enum = menu_editor.save;
                        buttons_command[i].str_content = "Save";
                        break;

                    case 1:
                        buttons_command[i].name_enum = menu_editor.erase;
                        buttons_command[i].str_content = "Erase";
                        break;

                    case 2:
                        buttons_command[i].name_enum = menu_editor.quit;
                        buttons_command[i].str_content = "Quit";
                        break;
                }


            }
        }


        /// <summary>
        /// Construire les boutons de menu de l'éditeur
        /// </summary>
        /// <param name="_frameW">largeur de la frame</param>
        /// <param name="_frameH">hauteur de la frame</param>
        /// <param name="texture_index">index dans le tableau des textures</param>
        /// <param name="number_of_buttons">nombre de boutons à créer</param>
        public void Set_Menu_Btn(int _frameW, int _frameH, int texture_index, int number_of_buttons)
        {
            buttons_menu = new Button[number_of_buttons];

            int sizeW = tile_size * 4;
            int sizeH = (sizeW * _frameH) / _frameW;

            int locationX = tile_size;
            int deltaX = 10;
            int deltaY = tile_size;
            int positionY = deltaY;

            for (int i = 0; i < buttons_menu.Length; i++)
            {

                int positionX = i * (sizeW + deltaX) + locationX;

                Rectangle standard_rect_position = new Rectangle(positionX, positionY, sizeW, sizeH * 2);


                buttons_menu[i].rectposition = standard_rect_position;

                buttons_menu[i].ColideBox = standard_rect_position;

                buttons_menu[i].rectframe = new Rectangle(0, 0, _frameW, _frameH);

                buttons_menu[i].current_color = Color.White;

                buttons_menu[i].texture_ID = texture_index;


                switch (i)
                {
                    case 0:
                        buttons_menu[i].name_enum = menu_editor.Files;
                        buttons_menu[i].str_content = "Files";
                        break;

                    case 1:
                        buttons_menu[i].name_enum = menu_editor.script;
                        buttons_menu[i].str_content = "Scripts";
                        break;

                    case 2:
                        buttons_menu[i].name_enum = menu_editor.tiles;
                        buttons_menu[i].str_content = "Tiles";
                        break;
                }
            }
        }
     
        
        /// <summary>
        /// méthode pour créer des fenêtre d'outils pour l'éditeur
        /// </summary>
        /// <param name="_window">window vierge à charger</param>
        /// <param name="positionX">positionX de départ à l'écran</param>
        /// <param name="positionY">positionY de départ à l'écran</param>
        /// <param name="_panel_texture">indiquer la texture2D</param>
        public void Load_Window(ref XMLDataBase.Window _window, int positionX, int positionY, Texture2D _panel_texture)
        {
            _window.Set_Position(positionX, positionY);
            _window.SetPoint();
            _window.SetWindowTextures(all_textures[3], all_textures[11], all_textures[12], all_textures[5], _panel_texture);

        }


      
        public void Fix_Camera_Starting_Position()
        {
            ///------------------------------------------------------------------
            /// placer la caméra au bon endroit définit dans mes paramètres
            ///------------------------------------------------------------------
            Start_player_positionX = my_grid_game[grid_start_position].rectposition.X;
            Start_player_positionY = my_grid_game[grid_start_position].rectposition.Y;
            Reset_Camera_to_startPosition(Start_player_positionX, Start_player_positionY);


        }


        /// <summary>
        /// récupère l'index dans la grille de la tuile survolée par le curseur de la souris
        /// </summary>
        /// <param name="mouseposition">en référence : la position du curseur de la souris</param>
        /// <returns></returns>
        public int Get_Current_Tile_ID(ref Point mouseposition)
        {
            for (int i = 0; i < my_grid_game.Length; i++)
            {
                ///------------------------------------------------------------------
                /// Calculer le déplacement de la caméra
                ///------------------------------------------------------------------
                Rectangle colider = Get_New_Rectangle_Position(my_grid_game[i].rectposition,delta_cameraX,delta_cameraY);

                if (colider.Contains(mouseposition))
                {
                    if (hover_mouse == hover_btn.none)
                    {
                        return i;
                    }
                }
            }

            return ID_currentTile;
        }

        /// <summary>
        /// dessiner la tuile choisie dans la window_tile
        /// sur la grille de jeu
        /// </summary>
        public void Print_Tile_on_grid(ref ChoosenTile index_btn, ref Grid_Game_Object[] grid_game)
        {
            if (current_ms.LeftButton == ButtonState.Pressed)
            {
                grid_game[ID_currentTile].rectframe = new Rectangle(
                         index_btn.rectframeX * texture_frameW,
                         index_btn.rectframeY * texture_frameH,
                         texture_frameW, texture_frameH);
            }
        }

        /// <summary>
        /// imprimer le script dans la tuile qui est actuellement survolée par le curseur de la souris
        /// </summary>
        public void Print_Script_On_Tile(ref script_type value, ref Grid_Game_Object[] grid_game)
        {

            
            grid_game[ID_currentTile].logic_value_index = Convert_logic_type_to_index(value);
            grid_game[ID_currentTile].logic_value = value;

            Debug.WriteLine("print index : " + grid_game[ID_currentTile].logic_value_index);
            Debug.WriteLine("print value : " + grid_game[ID_currentTile].logic_value);
            Debug.WriteLine("------------------");
            int sizeW = tile_size;
            int sizeH = tile_size;

            int positionX = my_grid_game[ID_currentTile].rectposition.X;
            int positionY = my_grid_game[ID_currentTile].rectposition.Y;

            Rectangle rect_standard = new Rectangle(positionX, positionY, sizeW, sizeH);

            switch (value)
            {
                case script_type.none:
                    break;

                case script_type.add_colider:
                   
                    break;

                case script_type.player_start_position:
                    if (old_ms.LeftButton == ButtonState.Released)
                    {
                        my_grid_game[grid_start_position].logic_value = script_type.none;
                        my_grid_game[grid_start_position].logic_value_index = 0;
                        grid_start_position = ID_currentTile;
                        Fix_Camera_Starting_Position();
                    }

                    break;

                case script_type.go_to_nex_level:
                    if (old_ms.LeftButton == ButtonState.Released)
                    {
                        ColideBox next_level_colider_temp = new ColideBox();
                        next_level_colider_temp.colideBox = rect_standard;
                        next_level_colider_temp.grid_ID = my_grid_game[ID_currentTile].grid_ID;
                        next_level_colider_temp.grid_column = my_grid_game[ID_currentTile].grid_column;
                        next_level_colider_temp.grid_line = my_grid_game[ID_currentTile].grid_line;

                        next_level_colider = next_level_colider_temp;
                    }
                    break;

                case script_type.add_ladder:
                    
                    break;
                case script_type.add_coin:
                    ///--- dessiner une pièce d'or ----

                    Coin new_coin = new Coin();
                    new_coin.rectposition = rect_standard;
                    new_coin.colideBox = rect_standard;

                    break;

                case script_type.add_go_through:
                    ColideBox new_GT = new ColideBox();
                    new_GT.colideBox = rect_standard;

                    new_GT.grid_ID = my_grid_game[ID_currentTile].grid_ID;
                    new_GT.grid_column = my_grid_game[ID_currentTile].grid_column;
                    new_GT.grid_line = my_grid_game[ID_currentTile].grid_line;
                    break;

                case script_type.Hit_the_player:
                    ColideBox new_hitter = new ColideBox();
                    new_hitter.colideBox = rect_standard;

                    new_hitter.grid_ID = my_grid_game[ID_currentTile].grid_ID;
                    new_hitter.grid_column = my_grid_game[ID_currentTile].grid_column;
                    new_hitter.grid_line = my_grid_game[ID_currentTile].grid_line;
                    break;
                case script_type.enter_gate_Index:
                    //todo 
                    break;
                case script_type.exit_gate_Index:
                    //todo 
                    break;


                case script_type.flying_peach:
                    if(!list_bad_guy_indexer.Contains(ID_currentTile))
                    list_bad_guy_indexer.Add(ID_currentTile);
                    //todo 
                    break;


                case script_type.flying_bat:
                    if(!list_bad_guy_indexer.Contains(ID_currentTile))
                        list_bad_guy_indexer.Add(ID_currentTile);

                    //todo 
                    break;

                case script_type.zombie:
                    if(!list_bad_guy_indexer.Contains(ID_currentTile))
                        list_bad_guy_indexer.Add(ID_currentTile);

                    //todo 
                    break;

            }

            ///------- on va détruire tout les doublons -----------
            
            Create_Colider_Boxes();
            Create_Ladders();
        }



        public void Middle_button_behaviour()
        {
            ///------------------------------------------------------------------
            /// When the player hits the middle mouse button
            /// ** move the tilemap
            ///------------------------------------------------------------------
            if (current_ms.MiddleButton == ButtonState.Pressed)
            {
                if (old_ms.MiddleButton == ButtonState.Released)
                {
                    old_middle_mouse_positionX = mousePosition.X;
                    old_middle_mouse_positionY = mousePosition.Y;
                }

                int deltaX = old_middle_mouse_positionX - mousePosition.X;
                int deltaY = old_middle_mouse_positionY - mousePosition.Y;

                Middle_button_brahviour_move_Camera(-deltaX, deltaY);

                old_middle_mouse_positionX = mousePosition.X;
                old_middle_mouse_positionY = mousePosition.Y;
            }
            else if (current_ms.MiddleButton == ButtonState.Released)
            {
                old_middle_mouse_positionX = 0;
                old_middle_mouse_positionY = 0;
            }

            if (current_ms.ScrollWheelValue != 0)
            {
                int speed_scroll = 20;

                int delta = old_scroll_wheel_value - current_ms.ScrollWheelValue;

                if (delta > 0)
                {
                    Middle_button_beahviour_check_vertical(0, -speed_scroll);
                }
                else if (delta < 0)
                {
                    Middle_button_beahviour_check_vertical(0, speed_scroll);
                }

                old_scroll_wheel_value = current_ms.ScrollWheelValue;

            }

        }

        public void Middle_button_behaviour_check_horizontal(int deltaX, int deltaY)
        {
            Rectangle rect_temp = Get_New_Rectangle_Position(my_camera.rectposition, deltaX, deltaY);

            ///------------------------------------------------------------------
            /// FORCE horizontal transformations and scanning
            /// debug : be more accurate about the spelling of the conditions
            ///------------------------------------------------------------------
            int _deltaX = 4 * tile_size;
            int condition1 = my_grid_game[0].rectposition.X - _deltaX + rect_temp.X;
            int condition2 = my_grid_game[my_grid_game.Length - 1].rectposition.X + _deltaX + rect_temp.X;


            if (deltaX > 0
                && condition1 <= 0)///panel_tool_border)
            {
                my_camera.rectposition = rect_temp;
            }
            else if (deltaX < 0
                && condition2 >= mainclass.INT_SCREEN_WIDTH)
            {
                my_camera.rectposition = rect_temp;
            }
        }

        public void Middle_button_beahviour_check_vertical(int deltaX, int deltaY)
        {
            Rectangle rect_temp = Get_New_Rectangle_Position(my_camera.rectposition, deltaX, deltaY);

            ///------------------------------------------------------------------
            /// FORCE vertical transformations and scanning
            ///------------------------------------------------------------------
            int _deltaY = 4 * tile_size;

            int condition3 = my_grid_game[0].rectposition.Y - _deltaY + rect_temp.Y;
            int condition4 = my_grid_game[my_grid_game.Length - 1].rectposition.Y + _deltaY + tile_size + rect_temp.Y;

            if (deltaY > 0
                && condition3 <= 0)
            {
                my_camera.rectposition = rect_temp;
            }
            else if (deltaY < 0
                && condition4 >= mainclass.INT_SCREEN_HEIGHT)
            {
                my_camera.rectposition = rect_temp;
            }
        }

        public void Middle_button_brahviour_move_Camera(int deltaX, int deltaY)
        {

            if (Math.Abs(deltaX) > Math.Abs(deltaY))
            {
                Middle_button_behaviour_check_horizontal(deltaX, deltaY);
            }
            else
            {
                Middle_button_beahviour_check_vertical(deltaX, deltaY);

            }

        }

        public Rectangle Get_New_Rectangle_Position(Rectangle target, int x, int y)
        {
            return new Rectangle(target.X + x, target.Y + y, target.Width, target.Height);
        }

        public Point Get_To_Print_TILE(ref Rectangle rect_frame)
        {
            Point result = new Point(0, 0);
            int index_tile_to_print = window_tiles_asset1.Get_Index_Button_wanted();
            if (index_tile_to_print >= 0)
            {
                result.X = index_tile_to_print;
                result.Y = 0;
                rect_frame = window_tiles_asset1.Get_Texture_rect_Frame(index_tile_to_print);
            }

            return result;
        }
        public script_type Get_To_Print_SCRIPT(ref Rectangle rect_frame)
        {

            int script_wanted = window_script_standard.Get_Index_Button_wanted();
            rect_frame = window_script_standard.Get_Texture_rect_Frame(script_wanted);

            Debug.WriteLine("script_wanted : " + script_wanted);

            ///-------- la disposition des logic wanted dépend de la texture boutons -----
            switch (script_wanted)
            {
                case 0:
                    return script_type.add_colider;

                case 1:
                    return script_type.player_start_position;

                case 2:
                    return script_type.go_to_nex_level;

                case 3:
                    return script_type.add_coin;

                case 4:
                    return script_type.add_ladder;

                case 5:
                    return script_type.delete_logic_tile;

                case 6:
                    return script_type.add_go_through;

                case 7:
                    return script_type.flying_bat;

                case 8:
                    return script_type.flying_peach;

                case 9:
                    return script_type.zombie;

            }

            return script_type.none;
        }

        public void Get_Btn_MENU()
        {
            Color c_selected = Color.GreenYellow * 1f;
            Color c_unselected = Color.White * 0.5f;

            for (int i = 0; i < buttons_menu.Length; i++)
            {

                /// Rectangle colider = Get_New_Rectangle_Position(array_tab_buttons[i].rectposition, 0, 0);

                if (buttons_menu[i].rectposition.Contains(mousePosition))
                {
                    buttons_menu[i].current_color = c_selected;

                    if (Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == ButtonState.Pressed
                        && old_ms.LeftButton == ButtonState.Released)
                    {
                        ///------------------------------------------------------------------
                        /// Action si click
                        ///------------------------------------------------------------------
                        bool is_not_active = false;
                        switch (i)
                        {
                            case 0:
                                ///------------------------------------------------------------------
                                /// Cliquer sur le bouton [SOFTWARE]
                                ///------------------------------------------------------------------
                                //todo: show software buttons
                              /*  if (my_software_panel_camera.rectposition.X >= 0)
                                {
                                    ///------------------------------------------------------------------
                                    /// Cacher le panneau [SOFTWARE] si il est déjà affiché
                                    ///------------------------------------------------------------------
                                    my_software_panel_camera.rectposition = Get_New_Rectangle_Position(
                                   my_software_panel_camera.rectposition,
                                   -new_software_X, 0);
                                }
                                else
                                {
                                    ///------------------------------------------------------------------
                                    /// Ouvrir le panneau qi il est déjà caché
                                    ///------------------------------------------------------------------
                                    my_software_panel_camera.rectposition = Get_New_Rectangle_Position(
                                    my_software_panel_camera.rectposition,
                                    new_software_X, 0);

                                    ///------------------------------------------------------------------
                                    /// Cacher tout les autres panneaux
                                    ///------------------------------------------------------------------
                                    if (my_logic_panel_camera.rectposition.X >= 0)
                                    {
                                        my_logic_panel_camera.rectposition = Get_New_Rectangle_Position(
                                          my_logic_panel_camera.rectposition,
                                          -new_logic_X, 0);
                                    }

                                    if (my_tile_panel_camera.rectposition.X >= 0)
                                    {
                                        my_tile_panel_camera.rectposition = Get_New_Rectangle_Position(
                                          my_tile_panel_camera.rectposition,
                                        -new_tile_X, 0);
                                    }
                                }*/

                                break;

                            case 1:
                                ///------------------------------------------------------------------
                                /// le panneau [Script]
                                ///------------------------------------------------------------------

                                if(window_script_standard.Is_Open()==is_open.off)
                                {
                                    window_script_standard.OpenWindow();
                                }

                                if(window_script_standard.Is_Selected()==is_selected.off)
                                {

                                    foreach (Window item in list_window)
                                    {
                                        if (item.Is_Open() == is_open.on)
                                        {
                                            /// désactiver toute les fenêtres
                                            item.Edit_Window_Is_Selected(is_selected.off);
                                        }
                                    }
                                    window_script_standard.Edit_Window_Is_Selected(is_selected.on);
                                }


                                break;

                            case 2:
                                ///------------------------------------------------------------------
                                /// le panneau [Tiles]
                                ///------------------------------------------------------------------


                                if (window_tiles_asset1.Is_Open() == XMLDataBase.is_open.off)
                                {
                                    is_not_active = true;
                                }
                                if (window_tiles_asset1.Is_Open() == XMLDataBase.is_open.on && old_ms != current_ms)
                                {
                                    window_tiles_asset1.CloseWindow();
                                }
                                else if (is_not_active && old_ms != current_ms)
                                {
                                    window_tiles_asset1.OpenWindow();
                                    window_script_standard.Edit_Window_Is_Selected(is_selected.off);//0);
                                    window_tiles_asset1.Edit_Window_Is_Selected(is_selected.on);//1);
                                }

                                break;

                        }

                    }
                }
                else
                {
                    ///------------------------------------------------------------------
                    /// si aucune colision détectée, restaurer la couleur d'origine
                    ///------------------------------------------------------------------
                    buttons_menu[i].current_color = c_unselected;
                }
            }
        }

        public void Get_Btn_COMMAND(ref MouseState current_ms, ref MouseState old_ms)
        {
            
            if (command_btn_state == command_btn.is_show)
            {
                Color c_selected = Color.Green * 1f;
                Color c_unselected = Color.White * 0.5f;
                Check_active_windowBox(ref current_ms, ref old_ms);

                if (window_tiles_asset1 != null)
                {
                    for (int i = 0; i < buttons_command.Length; i++)
                    {
                        Rectangle colider = buttons_command[i].ColideBox;



                        if (colider.Contains(mousePosition))
                        {
                            ///------------------------------------------------------------------
                            /// change the color of the button
                            ///------------------------------------------------------------------
                            buttons_command[i].current_color = c_selected;
                            buttons_command[i].colide_state = 1;
                            if (Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == ButtonState.Pressed)
                            {
                                switch (i)
                                {
                                    case 2:
                                        ///------------------------------------------------------------------
                                        /// QUIT EDITOR TOOL
                                        ///------------------------------------------------------------------
                                        mainclass.Exit();
                                        break;
                                    case 0:
                                        ///------------------------------------------------------------------
                                        /// SAVE TO THE XML FILE
                                        ///------------------------------------------------------------------

                                        Save_Tilemap_To_XML();
                                        break;
                                    case 1:
                                        ///------------------------------------------------------------------
                                        /// ERASE THE MAP
                                        ///------------------------------------------------------------------
                                       // Delete_All_Map_And_Make_EmptyOnes();
                                        break;
                                }
                            }
                        }
                        else
                        {
                            buttons_command[i].current_color = c_unselected;
                            buttons_command[i].colide_state = 0;

                        }
                    }


                }
            }
        }

       

      
     

        public hover_btn Hover_Mouse_Position()
        {
            ///------------------------------------------------------------------
            /// identifier l'objet sur lequel survole
            /// le curseur de la souris
            ///  [un bouton du menu] ? - Files - Scripts - Tiles 
            ///  [la fenetre des tuiles] ?
            ///  [la fenetre des scripts] ?
            ///  [un bouton de commande]? - Save - Erase - Quit
            ///------------------------------------------------------------------
           

            for (int i = 0; i < buttons_menu.Length; i++)
            {
                ///  [un bouton du menu] ? - Files - Scripts - Tiles 

                if (buttons_menu[i].rectposition.Contains(mousePosition))
                {
                    switch (buttons_menu[i].name_enum)
                    {
                        case menu_editor.Files:
                            return hover_btn.btn_files;
                        case menu_editor.script:
                            return hover_btn.btn_script;
                        case menu_editor.tiles:
                            return hover_btn.btn_tiles;
                    }
                }
            }

            if (window_tiles_asset1.Is_Open() == XMLDataBase.is_open.on
                && window_tiles_asset1.Is_Selected() == is_selected.on);//1)
            {
                ///  [la fenetre des tuiles] ?


                if (window_tiles_asset1.Get_window_colision(mousePosition) == 1
                || window_tiles_asset1.Get_moving_window_state() == 1)
                {
                    return hover_btn.window_tiles;
                }

            }

            if (window_script_standard.Is_Open() == XMLDataBase.is_open.on
               && window_script_standard.Is_Selected() == is_selected.on)//1)
            {
                ///  [la fenetre des scripts] ?

                if (window_script_standard.Get_window_colision(mousePosition) == 1
                 || window_script_standard.Get_moving_window_state() == 1)
                {
                    return hover_btn.window_script;
                }
            }
            if (command_btn_state == command_btn.is_show)
            {
                for (int i = 0; i < buttons_command.Length; i++)
                {
                    ///  [un bouton de commande]? - Save - Erase - Quit
                    ///   à afficher si et seulement si ils apparaissent !!!
                    if (buttons_command[i].rectposition.Contains(mousePosition))
                    {
                        switch (buttons_command[i].name_enum)
                        {
                            case menu_editor.save:
                                return hover_btn.btn_save;

                            case menu_editor.erase:
                                return hover_btn.btn_erase;

                            case menu_editor.quit:
                                return hover_btn.btn_quit;
                        }
                    }
                }
            }
            return hover_btn.none;
        }

        public void Move_Grid_Vertically_by_KB()
        {
            vertical_player_lookAt = Vertical_Direction_LookAt.none;
            if (current_KB.IsKeyDown(Keys.Down))
            {
                vertical_player_lookAt = Vertical_Direction_LookAt.look_at_down;
                old_vertical_player_lookAt = Vertical_Direction_LookAt.look_at_down;
            }
            if (current_KB.IsKeyDown(Keys.Up))
            {
                vertical_player_lookAt = Vertical_Direction_LookAt.look_at_up;
                old_vertical_player_lookAt = Vertical_Direction_LookAt.look_at_up;
            }

        }
        public void Move_Grid_Horizontally_by_KB()
        {
            horizontal_player_lookAt = Horizontal_Direction_LookAt.none;

            if (current_KB.IsKeyDown(Keys.Left))
            {
                horizontal_player_lookAt = Horizontal_Direction_LookAt.go_to_left;
                old_horizontal_player_lookAt = Horizontal_Direction_LookAt.go_to_left;
            }
            if (current_KB.IsKeyDown(Keys.Right))
            {
                horizontal_player_lookAt = Horizontal_Direction_LookAt.go_to_right;
                old_horizontal_player_lookAt = Horizontal_Direction_LookAt.go_to_right;
            }

        }


        public void Get_Input_Player()
        {
            Move_Grid_Horizontally_by_KB();
            Move_Grid_Vertically_by_KB();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gaming_state = game_state.editing_mode;
                MediaPlayer.Stop();
            }

            if (current_KB.IsKeyDown(Keys.Z) && old_KB.IsKeyUp(Keys.Z))
            {
                GoToNeXtLevel();
            }

            if (current_KB.IsKeyDown(Keys.Tab) && old_KB.IsKeyUp(Keys.Tab))
            {
                switch(display_log)
                {
                    case triggers.show:
                        display_log = triggers.hide;
                        break;

                    case triggers.hide:
                        display_log = triggers.show;
                        break;
                }
            }
        }

        public void Move_Tile_Panel()
        {
            if (window_tiles_asset1.Get_colision_head_of_window(mousePosition) == 1
           && current_ms.LeftButton == ButtonState.Pressed
           && old_ms != current_ms)
            {
                ///------------------------------------------------------------------
                /// cette première partie va calculer un delta et sert à déplacer la fenêtre
                /// sur le plan de travail
                ///------------------------------------------------------------------
                delta_mouse_toolbox_panel =
                    new Point(
                        current_ms.X - window_tiles_asset1.Get_Position().X,
                        current_ms.Y - window_tiles_asset1.Get_Position().Y);
                toolbox_header_window_state = 1;
            }
            if (toolbox_header_window_state == 1)
            {
                ///------------------------------------------------------------------
                /// C'est un calcul pour positionner exactement la fenêtre selon
                /// le DEPLACEMENT de la souris et non pas sur sa position
                ///------------------------------------------------------------------

                Point result = new Point(mousePosition.X - delta_mouse_toolbox_panel.X,
                     mousePosition.Y - delta_mouse_toolbox_panel.Y);

                if (hover_mouse == hover_btn.window_tiles)
                    window_tiles_asset1.Move_Window(result);

            }

            if (current_ms.LeftButton == ButtonState.Released && toolbox_header_window_state == 1)
            {
                ///------------------------------------------------------------------
                /// lorsque l'on relâche le bouton de la souris après l'avoir déplacée
                /// je réinitialise le capteur windowBox_head_moving_state
                ///------------------------------------------------------------------
                toolbox_header_window_state = 0;
            }
        }

        public void Move_Script_Panel()
        {
            if (window_script_standard.Get_colision_head_of_window(mousePosition) == 1
           && current_ms.LeftButton == ButtonState.Pressed
           && old_ms != current_ms)
            {
                ///------------------------------------------------------------------
                /// cette première partie va calculer un delta et sert à déplacer la fenêtre
                /// sur le plan de travail
                ///------------------------------------------------------------------
                delta_mouse_logic_panel =
                    new Point(
                        current_ms.X - window_script_standard.Get_Position().X,
                        current_ms.Y - window_script_standard.Get_Position().Y);
                logic_header_window_state = 1;
            }
            if (logic_header_window_state == 1)
            {
                ///------------------------------------------------------------------
                /// C'est un calcul pour positionner exactement la fenêtre selon
                /// le DEPLACEMENT de la souris et non pas sur sa position
                ///------------------------------------------------------------------

                Point result = new Point(mousePosition.X - delta_mouse_logic_panel.X,
                     mousePosition.Y - delta_mouse_logic_panel.Y);

                if (hover_mouse == hover_btn.window_script)
                    window_script_standard.Move_Window(result);

            }

            if (current_ms.LeftButton == ButtonState.Released && logic_header_window_state == 1)
            {
                ///------------------------------------------------------------------
                /// lorsque l'on relâche le bouton de la souris après l'avoir déplacée
                /// je réinitialise le capteur windowBox_head_moving_state
                ///------------------------------------------------------------------
                logic_header_window_state = 0;
            }
        }

     

        public override void Update()
        {
            base.Update();

            current_ms = Mouse.GetState();
            mousePosition = current_ms.Position;
            current_KB = Keyboard.GetState();
            Get_Input_Player();

            if (editor_loader_state == editor_loader.set_the_arrays)
            {
                Fix_Camera_Starting_Position();
                ///------------------------------------------------------------------
                /// Indiquer à mon programme que les calculs sont terminés
                ///------------------------------------------------------------------
                editor_loader_state = editor_loader.is_now_operating;

                gaming_state = game_state.editing_mode;
            }


            if (editor_loader_state == editor_loader.is_now_operating
                && gaming_state == game_state.editing_mode)
            {
                ID_currentTile = Get_Current_Tile_ID(ref mousePosition);



                foreach (Window item in list_window)
                {
                    if (item.Is_Open() == is_open.off && item.Is_Selected() == is_selected.on)
                    {
                        item.Edit_Window_Is_Selected(is_selected.off);
                    }
                    else if (item.Is_Open() == is_open.on)
                    {
                        item.Update(mousePosition, current_ms, old_ms);
                    }
                }



                if (window_tiles_asset1.Is_Selected()==is_selected.on)
                {
                    Move_Tile_Panel();
                }

                if (window_script_standard.Is_Selected() == is_selected.on)
                {
                    Move_Script_Panel();

                }

                Check_active_windowBox(ref current_ms, ref old_ms);

                if (current_ms.LeftButton == ButtonState.Released)
                {
                 ///--- détecter l'objet qe survole le curseur de la souris ---
                 ///--- le bouton de gauche n'est pas appuyé !! ----
                    hover_mouse = Hover_Mouse_Position();
                }

                switch (hover_mouse)
                {
                    case hover_btn.none:
                        ///-- valable uniquement aucune window n'est sélectionnée --
                        if (window_tiles_asset1.Is_Selected() == is_selected.on)
                        {
                            if (current_ms.LeftButton == ButtonState.Pressed)
                            { /// ----- imprimmer les tilemap----
                                Print_Tile_on_grid(ref index_tile_to_print, ref my_grid_game);
                            }
                        }
                        else if (window_script_standard.Is_Selected() == is_selected.on)
                        {
                            if (current_ms.LeftButton == ButtonState.Pressed)
                            { /// ----- imprimmer les scripts----
                                Print_Script_On_Tile(ref script_to_print, ref my_grid_game);
                            }
                        }
                        break;

                    case hover_btn.window_tiles:
                        ///-- valable uniquement si la window tile est sélectionnée --
                        if (window_tiles_asset1.Is_Selected() == is_selected.on)
                        {
                            Point temp = Get_To_Print_TILE(ref tile_rect_frame_choosen);
                            
                            
                            index_tile_to_print.rectframeX = temp.X;
                            index_tile_to_print.rectframeY = temp.Y;
                        }
                        break;

                    case hover_btn.window_script:
                        ///-- valable uniquement si la window script est sélectionnée --
                        if (window_script_standard.Is_Selected() == is_selected.on)
                        {
                            if (current_ms.LeftButton == ButtonState.Pressed && old_ms.LeftButton == ButtonState.Released)
                            {
                                script_to_print = Get_To_Print_SCRIPT(ref script_rect_frame_choosen);
                            }
                        }

                        break;

                    case hover_btn.btn_files:

                        if (current_ms.LeftButton == ButtonState.Pressed && old_ms.LeftButton == ButtonState.Released)
                        {
                            if (command_btn_state == command_btn.is_show)
                            {
                                command_btn_state = command_btn.is_hidden;

                                for (int i = 0; i < buttons_command.Length; i++)
                                {

                                }
                            }
                            else
                            {
                                command_btn_state = command_btn.is_show;

                                for (int i = 0; i < buttons_command.Length; i++)
                                {

                                }
                            }
                        }

                        break;


                }



                ///------------------------------------------------------------------
                /// colisions avec les boutons [QUITTER] - [ENREGISTRER] - [TOUT EFFACE]
                ///------------------------------------------------------------------

                if (command_btn_state == command_btn.is_show)
                {
                    Get_Btn_COMMAND(ref current_ms, ref old_ms);
                }
               
                
                Get_Btn_MENU();

               /// Get_Play_Buton();
                ///------------------------------------------------------------------
                /// Check the player's inputs
                ///------------------------------------------------------------------

                Middle_button_behaviour();


                if (old_ID_tool_selected != ID_tool_selected)
                    old_ID_tool_selected = ID_tool_selected;

            }


            delta_cameraX = my_camera.rectposition.X;
            delta_cameraY = my_camera.rectposition.Y;

        }


        public void Reset_Camera_to_startPosition(int positionX, int positionY)
        {
            ///------------------------------------------------------------------
            /// placer le startposition exactement au centre de l'écran
            /// ** calculer les points en haut à gauche qui seront les
            ///    coordonnées du rectposition de notre caméra
            /// *** Attention !!! tenir compte des ancres pour éviter que la caméra ne montre 
            ///     autre chose que les tilemap
            ///------------------------------------------------------------------

            int screen_W = mainclass.INT_SCREEN_WIDTH;
            int screen_H = mainclass.INT_SCREEN_HEIGHT;
            int middle_W = screen_W / 2;
            int middle_H = screen_H / 2;

            int centerX = positionX - middle_W;
            int centerY = positionY - middle_H;

            int resultX = -centerX;
            int resultY = centerY;



            if (positionX < my_grid_game[0].rectposition.X + mainclass.INT_SCREEN_WIDTH / 2)
            {
                resultX = 0;
            }
            if (positionX > (my_grid_game[my_grid_game.Length - 1].rectposition.X + tile_size) - (mainclass.INT_SCREEN_WIDTH / 2))
            {

                resultX = (my_grid_game[my_grid_game.Length - 1].rectposition.X + (mainclass.INT_SCREEN_WIDTH));
            }

            if (centerY < my_grid_game[0].rectposition.Y + mainclass.INT_SCREEN_HEIGHT / 2)
            {
                my_camera.rectposition = new Rectangle(resultX, my_grid_game[0].rectposition.Y,
               my_camera.rectposition.Width, my_camera.rectposition.Height);
                return;
            }

            if (positionY + mainclass.INT_SCREEN_HEIGHT / 2 >=
                (my_grid_game[my_grid_game.Length - 1].rectposition.Y + tile_size) - (mainclass.INT_SCREEN_HEIGHT))
            {
                ///------------------------------------------------------------------
                /// SI TU AUGMENTES ResultY =>> la tilemap descend
                ///------------------------------------------------------------------

                resultY = -(my_grid_game[my_grid_game.Length - 1].rectposition.Y - mainclass.INT_SCREEN_HEIGHT + tile_size);
            }

            my_camera.rectposition = new Rectangle(resultX, resultY,
                my_camera.rectposition.Width, my_camera.rectposition.Height);
        }

      
        public void Reset_Tile_Optimized()
        {
            for (int i = 0; i < my_grid_game.Length; i++)
            {
                my_grid_game[i].is_optimized = colider_optimized.off;
            }
        }

        public void Create_Colider_Boxes()
        {
            Grid_Game_Object item = new Grid_Game_Object();
            list_colide_boxes.Clear();
            Reset_Tile_Optimized();
            for (int i = 0; i < my_grid_game.Length; i++)
            {
                item = my_grid_game[i];

                if (item.is_optimized == colider_optimized.off)
                {

                    my_grid_game[i].is_optimized = colider_optimized.on;

                    if (my_grid_game[i].logic_value == script_type.add_colider)
                    {
                        int rect_position_H = tile_size;
                        int rect_position_W = tile_size;

                        int counter = 1;
                        int algorithm_state = 0;
                        while (true)
                        {
                            if (i + counter * level_W < level_size && algorithm_state == 0)
                            {

                                if (my_grid_game[i + counter * level_W].logic_value == script_type.add_colider && tile_size > 1)
                                {
                                    rect_position_H += tile_size;
                                    my_grid_game[i + counter * level_W].is_optimized = colider_optimized.on;

                                }
                                else if (counter == 1)
                                {
                                    rect_position_H = tile_size;
                                    algorithm_state = 1;
                                    counter = 0;
                                }
                                else if (rect_position_H > tile_size)
                                {
                                    break;
                                }
                            }

                            else if (((i + counter) % level_W != 0) && algorithm_state == 1)
                            {


                                if (my_grid_game[i + counter].logic_value == script_type.add_colider)
                                {
                                    rect_position_W += tile_size;
                                    my_grid_game[i + counter].is_optimized = colider_optimized.on;

                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }

                            counter++;
                        }

                        Rectangle result = my_grid_game[i].rectposition;

                        ColideBox new_CB = new ColideBox();

                        new_CB.colideBox = new Rectangle(result.X, result.Y, rect_position_W, rect_position_H);

                        list_colide_boxes.Add(new_CB);
                    }

                }
            }

        }


        public void Create_Ladders()
        {
              Grid_Game_Object item = new Grid_Game_Object();
              list_ladder.Clear();

              for (int i = 0; i < my_grid_game.Length; i++)
              {
                  item = my_grid_game[i];

                if (my_grid_game[i].logic_value == script_type.add_ladder)
                {

                    int rect_position_H = tile_size;
                    int rect_position_W = tile_size;

                    int counter = 1;
                    int algorithm_state = 0;
                    while (true)
                    {
                        if (i + counter * level_W < level_size && algorithm_state == 0)
                        {

                            if (my_grid_game[i + counter * level_W].logic_value == script_type.add_ladder && tile_size > 1)
                            {
                                rect_position_H += tile_size;

                            }
                            else if (counter == 1)
                            {
                                rect_position_H = tile_size;
                                algorithm_state = 1;
                                counter = 0;
                            }
                            else if (rect_position_H > tile_size)
                            {
                                break;
                            }
                        }

                        else if (((i + counter) % level_W != 0) && algorithm_state == 1)
                        {


                            if (my_grid_game[i + counter].logic_value == script_type.add_ladder)
                            {
                                rect_position_W += tile_size;

                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }

                        counter++;
                    }

                    Rectangle result = my_grid_game[i].rectposition;

                    ColideBox new_CB = new ColideBox();

                    new_CB.colideBox = new Rectangle(result.X, result.Y, rect_position_W, rect_position_H);

                    list_ladder.Add(new_CB);


                }


                  
              }
            

        }

        public Texture2D Get_Image_and_Convert_To_Texture2D(string _name)
        {
            ///------------------------------------------------------------------
            /// charger une image directement depuis le dossier [Content\\modding]
            ///------------------------------------------------------------------
            FileStream fileStream = new FileStream("Content\\" + _name, FileMode.Open);
            Texture2D spriteAtlas = Texture2D.FromStream(mainclass.GraphicsDevice, fileStream);
            fileStream.Dispose();

            return spriteAtlas;
        }

        public void Delete_All_Map_And_Make_EmptyOnes()
        {

            if (File.Exists(tilemap_xml_path))
            {
                File.Delete(tilemap_xml_path);
            }

            int horizontal = 0;
            int vertical = -1;
            my_grid_game = new Grid_Game_Object[level_size];


            XMLDataBase.TileMap tilemap_converter = new XMLDataBase.TileMap();
            tilemap_converter.list_tile = new XMLDataBase.Tiles[my_grid_game.Length];

            for (int i = 0; i < my_grid_game.Length; i++)
            {
                if (i % level_W == 0)
                {
                    horizontal = 0;
                    vertical++;
                }
                else
                {
                    horizontal++;
                }
                my_grid_game[i].texture_ID = 0;
                my_grid_game[i].rectposition = new Rectangle(horizontal * tile_size, tile_size * vertical, tile_size, tile_size);
                my_grid_game[i].colideBox = new Rectangle(horizontal * tile_size, tile_size * vertical, tile_size, tile_size);
                my_grid_game[i].rectframe = new Rectangle(0, 0, texture_frameW, texture_frameH);
                my_grid_game[i].default_color = Color.White;
                my_grid_game[i].current_color = Color.White;
                my_grid_game[i].ID = "" + i + "for" + horizontal + "up" + vertical;
                my_grid_game[i].logic_value_index = 0;
                my_grid_game[i].logic_value = script_type.none;

                tilemap_converter.list_tile[i].frame_positionX = 0;
                tilemap_converter.list_tile[i].frame_positionY = 0;
                tilemap_converter.list_tile[i].texture_ID = 0;
                tilemap_converter.list_tile[i].logic_type_index = 0;


            }
            service_xml.Write_TileMap_XML(tilemap_xml_path, typeof(XMLDataBase.TileMap), tilemap_converter);

            service_xml.Save_Initialize_Level(myparam_xml_path, typeof(XMLDataBase.Level_Initialize), my_params);
        }

        public void Save_Tilemap_To_XML()
        {

            if (File.Exists(tilemap_xml_path))
            {
                File.Delete(tilemap_xml_path);
            }

            int horizontal = 0;
            int vertical = -1;


            XMLDataBase.TileMap tilemap_converter = new XMLDataBase.TileMap();
            tilemap_converter.list_tile = new XMLDataBase.Tiles[my_grid_game.Length];

            for (int i = 0; i < my_grid_game.Length; i++)
            {
                if (i % level_W == 0)
                {
                    horizontal = 0;
                    vertical++;
                }
                else
                {
                    horizontal++;
                }

                tilemap_converter.list_tile[i].frame_positionX = my_grid_game[i].rectframe.X;
                tilemap_converter.list_tile[i].frame_positionY = my_grid_game[i].rectframe.Y;
                tilemap_converter.list_tile[i].texture_ID = my_grid_game[i].texture_ID;
                tilemap_converter.list_tile[i].logic_type_index = my_grid_game[i].logic_value_index;



            }
            service_xml.Write_TileMap_XML(tilemap_xml_path, typeof(XMLDataBase.TileMap), tilemap_converter);
        }

        public void Draw_Tilemap()
        {
            for (int i = 0; i < my_grid_game.Length; i++)
            {
                ///------------------------------------------------------------------
                /// Dessiner les tuiles dans le world
                ///------------------------------------------------------------------

                Rectangle result_camera = Get_New_Rectangle_Position(my_grid_game[i].rectposition, delta_cameraX, delta_cameraY);


                if (my_grid_game[i].texture_ID >= 0)
                {
                    ///------------------------------------------------------------------
                    /// les objets avec des index négatifs ne doivent pas être affichés
                    ///------------------------------------------------------------------

                    spritebatch.Draw(
                      tilesheet_textures[my_grid_game[i].texture_ID].texture_object,
                      result_camera,
                     my_grid_game[i].rectframe,
                     Color.White);
                }

                ///------------------------------------------------------------------
                /// Dessiner la GRILLE
                ///------------------------------------------------------------------
                spritebatch.Draw(
                   all_textures[1], ///grid_texture
                       result_camera,
                   Color.White);//my_tilemap[i].current_color);
            }


        }

        public void Draw_Script_Icon()
        {
            

            for (int i = 0; i < my_grid_game.Length; i++)
            {
                Rectangle temp = Get_New_Rectangle_Position(my_grid_game[i].rectposition,
                  delta_cameraX,
                  delta_cameraY);


                switch (my_grid_game[i].logic_value)
                {
                    case script_type.add_colider:
                        spritebatch.Draw(
                       tilesheet_textures[my_grid_game[i].texture_ID].texture_object,
                        temp,
                        window_script_standard.Get_Texture_rect_Frame(0),
                        Color.White * 0.2f);
                        break;

                    case script_type.player_start_position:

                        spritebatch.Draw(
                        all_textures[4],
                        temp,
                        window_script_standard.Get_Texture_rect_Frame(1),
                        Color.White);
                        break;

                    case script_type.go_to_nex_level:

                        spritebatch.Draw(
                        all_textures[4],
                        temp,
                         window_script_standard.Get_Texture_rect_Frame(2),
                        Color.White);


                        spritebatch.Draw(
                     all_textures[5],
                      Get_New_Rectangle_Position(next_level_colider.colideBox,delta_cameraX,delta_cameraY),
                      Color.Red*0.3f);
                        break;

                    case script_type.add_coin:

                        spritebatch.Draw(
                        all_textures[4],
                        temp,
                        window_script_standard.Get_Texture_rect_Frame(3),
                        Color.White);
                        break;

                    case script_type.add_ladder:

                        spritebatch.Draw(
                       tilesheet_textures[my_grid_game[i].texture_ID].texture_object,
                        temp,
                         window_script_standard.Get_Texture_rect_Frame(4),
                        Color.Blue * 0.5f);
                        break;

                 

                    case script_type.add_go_through:

                        spritebatch.Draw(
                      all_textures[4],//    all_textures[0],
                        temp,
                         window_script_standard.Get_Texture_rect_Frame(6),
                        Color.Green * 0.5f);
                        break;

                    case script_type.flying_bat:

                        spritebatch.Draw(
                      all_textures[4],//    all_textures[0],
                        temp,
                         window_script_standard.Get_Texture_rect_Frame(7),
                        Color.Green * 0.5f);
                        break;

                    case script_type.flying_peach:

                        spritebatch.Draw(
                      all_textures[4],//    all_textures[0],
                        temp,
                         window_script_standard.Get_Texture_rect_Frame(8),
                        Color.Green * 0.5f);
                        break;

                    case script_type.zombie:

                        spritebatch.Draw(
                     all_textures[4],//    all_textures[0],
                        temp,
                         window_script_standard.Get_Texture_rect_Frame(9),
                        Color.Green * 0.5f);
                        break;
                }


            }

        }

        public void Draw_Menu_btn()
        {
            ///------------------------------------------------------------------
            /// dessinner les boutons de script
            ///------------------------------------------------------------------
            for (int i = 0; i < buttons_menu.Length; i++)
            {
                spritebatch.Draw(
                     all_textures[6],///buttons_texture,
                     buttons_menu[i].rectposition,
                     buttons_menu[i].rectframe,
                    buttons_menu[i].current_color);

                int positionX = buttons_menu[i].rectposition.X + tile_size;
                int positionY = buttons_menu[i].rectposition.Y + buttons_menu[i].rectposition.Height / 10;

                Color c_selected = Color.Black;
                Color c_unselected = Color.White;

                spritebatch.DrawString(mainFont, buttons_menu[i].str_content, new Vector2(positionX, positionY), c_unselected);

                switch (hover_mouse)
                {

                    case hover_btn.none:
                        spritebatch.DrawString(mainFont, buttons_menu[i].str_content, new Vector2(positionX, positionY), c_unselected);
                        break;

                    case hover_btn.btn_files:
                        if (buttons_menu[i].name_enum == menu_editor.Files) spritebatch.DrawString(mainFont, buttons_menu[i].str_content, new Vector2(positionX, positionY), c_selected);
                        break;

                    case hover_btn.btn_script:
                        if (buttons_menu[i].name_enum == menu_editor.script) spritebatch.DrawString(mainFont, buttons_menu[i].str_content, new Vector2(positionX, positionY), c_selected);
                        break;

                    case hover_btn.btn_tiles:
                        if (buttons_menu[i].name_enum == menu_editor.tiles) spritebatch.DrawString(mainFont, buttons_menu[i].str_content, new Vector2(positionX, positionY), c_selected);
                        break;


                }


            }


        }

        public void Draw_Command_btn()
        {
            ///------------------------------------------------------------------
            /// dessinner les boutons [QUITTER] * [ENREGISTRER] * [TOUT EFFACER]
            ///------------------------------------------------------------------
            for (int i = 0; i < buttons_command.Length; i++)
            {
                Rectangle temp = Get_New_Rectangle_Position(buttons_command[i].rectposition,
                    my_software_panel_camera.rectposition.X,
                    my_software_panel_camera.rectposition.Y);

                spritebatch.Draw(
                     all_textures[6],
                    temp,
                     buttons_command[i].rectframe,
                       buttons_command[i].current_color
                    );

                int positionX = buttons_command[i].rectposition.X + texture_frameW/4;
                int positionY = buttons_command[i].rectposition.Y + texture_frameW/8;
                Color c_selected = Color.White;
                Color c_unselected = Color.DarkGray*0.7f;
                if (buttons_command[i].colide_state==1)
                {
                    spritebatch.DrawString(mainFont, buttons_command[i].str_content, new Vector2(positionX, positionY), c_selected);

                }
                else
                {
                    spritebatch.DrawString(mainFont, buttons_command[i].str_content, new Vector2(positionX, positionY), c_unselected);

                }

            }
        }

    
        public void Draw_tile_on_mouseposition()
        {
            ///------------------------------------------------------------------
            /// Dessiner l'item sur le curseur
            ///------------------------------------------------------------------
            Rectangle rect_position_image = new Rectangle(
                   my_grid_game[ID_currentTile].rectposition.X + my_camera.rectposition.X,
                   my_grid_game[ID_currentTile].rectposition.Y + my_camera.rectposition.Y,
                   tile_size,
                   tile_size);

            spritebatch.Draw(
                    tilesheet_textures[0].texture_object,//       all_textures[0],
                     rect_position_image,
                   tile_rect_frame_choosen,  ///*************///,
                  Color.White);
        }

      

        public override void Draw()
        {
            base.Draw();
            if (editor_loader_state == editor_loader.is_now_operating)
            {
                ///------------------------------------------------------------------
                /// Dessiner la map
                ///------------------------------------------------------------------
                Draw_Tilemap();


            if(display_log==triggers.show)
                    Draw_Script_Icon();


                ///------------------------------------------------------------------
                /// dessiner les boutons [QUITTER] * [ENREGISTRER] * [TOUT EFFACER]
                ///------------------------------------------------------------------

                if (command_btn_state == command_btn.is_show)
                {
            if(display_log==triggers.show)
                        Draw_Command_btn();
                }

                ///------------------------------------------------------------------
                /// Dessiner un repère visuel sur le curseur de la souris
                ///------------------------------------------------------------------

                if (window_tiles_asset1.Is_Open() == is_open.on)
            if(display_log==triggers.show)
                        Draw_tile_on_mouseposition();


                ///------------------------------------------------------------------
                /// dessiner les boutons de navigation
                ///------------------------------------------------------------------
            if(display_log==triggers.show)
                    Draw_Menu_btn();


                ///------------------------------------------------------------------
                /// dessiner un repère visuel sur la tuile en cours
                ///------------------------------------------------------------------

            if(display_log==triggers.show)
                    spritebatch.Draw(all_textures[5], Get_New_Rectangle_Position(my_grid_game[ID_currentTile].rectposition,
                delta_cameraX,delta_cameraY), Color.Red);


                if (list_colide_boxes.Count > 0)
                {
            if(display_log==triggers.show)
                        Draw_ColideBoxes_Post_Traited();
                }

                if (list_coin_temp_for_tilemap_for_draw.Count > 0)
                {
                    Draw_Coins();
                }

                if (list_ladder.Count > 0)
                {
            if(display_log==triggers.show)
                        Draw_Lader_Post_Traited();
                }

               

            }


            if (gaming_state != game_state.looping
                    && editor_loader_state == editor_loader.is_now_operating)
            {
                if (window_tiles_asset1.Is_Open() == XMLDataBase.is_open.on)
                {
                    window_tiles_asset1.Draw(spritebatch, Color.Red);
                }
                if (window_script_standard.Is_Open() == XMLDataBase.is_open.on)
                {
                    window_script_standard.Draw(spritebatch, Color.Red);
                }
            }

           /* if (display_log == triggers.show)
                Draw_log_infos();*/
        }

        public void Draw_log_infos()
        {
            spritebatch.DrawString(mainFont, "current ID : " + ID_currentTile, new Vector2(800, 300), Color.White);
            spritebatch.DrawString(mainFont, "value script to print : " + script_to_print, new Vector2(800, 350), Color.White);

            spritebatch.DrawString(mainFont, "Hover : " + hover_mouse, new Vector2(500, 100), Color.Red);

           // spritebatch.DrawString(mainFont, "list fx count : " + bad, new Vector2(1200, 200), Color.White);
            spritebatch.DrawString(mainFont, "window_tile_is_selected? : " + window_tiles_asset1.Is_Selected(), new Vector2(1200, 250), Color.White);
            spritebatch.DrawString(mainFont, "window_script_is_open? : " + window_script_standard.Is_Open(), new Vector2(1200, 300), Color.White);
            spritebatch.DrawString(mainFont, "window_script_is_selected? : " + window_script_standard.Is_Selected(), new Vector2(1200, 350), Color.White);

            spritebatch.DrawString(mainFont, "startX : " + Start_player_positionX, new Vector2(100, 600), Color.White);
            spritebatch.DrawString(mainFont, "startY : " + Start_player_positionY, new Vector2(100, 650), Color.White);



        }
        public void Draw_Coins()
        {
            ///------------------------------------------------------------------
            /// dessiner les pièces d'or
            ///------------------------------------------------------------------
            foreach (Coin it in list_coin_temp_for_tilemap_for_draw)
            {
                Rectangle temp = Get_New_Rectangle_Position(it.rectposition, my_camera.rectposition.X, my_camera.rectposition.Y);
                Rectangle combat = new Rectangle(500, 500, 64, 64);
                Rectangle joe = new Rectangle(0, 0, 48, 48);
                spritebatch.Draw(
                      all_textures[9],
          it.rectposition,
                   it.rectframe,
                      Color.White);
            }
        }

        public void Draw_ColideBoxes_Post_Traited()
        {
            ///------------------------------------------------------------------
            /// dessiner tout les collides boxes qui sont touchées par le joueur dans le world
            ///------------------------------------------------------------------

            foreach (ColideBox it in list_colide_boxes)
            {
                Rectangle temp = Get_New_Rectangle_Position(
                  it.colideBox,
                   my_camera.rectposition.X, my_camera.rectposition.Y);

                spritebatch.Draw(
                all_textures[5],
               temp,
                new Rectangle(0, 0, 38, 38),
                Color.White * 0.3f);
            }

        }

        public void Draw_Lader_Post_Traited()
        {
            ///------------------------------------------------------------------
            /// dessiner tout les collides boxes actifs dans le world
            ///------------------------------------------------------------------

            foreach (ColideBox it in list_ladder)
            {
                Rectangle temp = Get_New_Rectangle_Position(it.colideBox, delta_cameraX, delta_cameraY);

                spritebatch.Draw(
             all_textures[5],
          temp,
             new Rectangle(0, 0, 38, 38),
             Color.Blue * 0.2f);
            }

        }

    }
}
