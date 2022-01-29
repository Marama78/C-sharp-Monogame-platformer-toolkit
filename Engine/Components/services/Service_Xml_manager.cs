
using MonogameToolkit.Engine.Components.shared;
using System;
using System.IO;
using System.Xml.Serialization;

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

    public class Service_Xml_manager
    {

        private Grid_Game_Object[] grid_game;
        private string path_tileObject_xml;
        private int tilemap_lenght = -10;

        XMLDataBase.TileMap data_xml_tiles;
        public Service_Xml_manager()
        {


        }


        public void SetTileMap(int length)
        {
            grid_game = new Grid_Game_Object[length];
            tilemap_lenght = length;
        }

        public void Set_Tilemap_Path(string _tile_object_path)
        {
            path_tileObject_xml = _tile_object_path;
        }



       
        public XMLDataBase.TileMap LoadTileMap(string _path)
        {
            ///------------- charger les tuiles de la map depuis le fichier xml -------------------
            return LoadTileMapXML(_path, typeof(XMLDataBase.TileMap));
        }



        public XMLDataBase.TileMap LoadTileMapXML(string _path, Type _type)
        {
            ///------------------------------------------------------------------
            /// Charger une map entière
            ///------------------------------------------------------------------
            if (File.Exists(_path))
            {
                XmlSerializer serializer = new XmlSerializer(_type);
                FileStream streamreader = File.OpenRead(_path);
                var result = (XMLDataBase.TileMap)(serializer.Deserialize(streamreader));
                streamreader.Dispose();
                return result;

            }
            return null;
        }
        public XMLDataBase.Player_Settings Load_Player_Settings(string _path, Type _type, XMLDataBase.Player_Settings _object_to_load)
        {
            ///------------------------------------------------------------------
            /// Charger les paramètres du de l'avatar du joueur
            ///------------------------------------------------------------------
            XmlSerializer serializer = new XmlSerializer(_type);
            FileStream streamreader = File.OpenRead(_path);
            var result = (XMLDataBase.Player_Settings)(serializer.Deserialize(streamreader));
            streamreader.Dispose();
            return result;
        }



        public XMLDataBase.Level_Initialize Load_Initialize_Level(string _path, Type _type, XMLDataBase.Level_Initialize _object_to_load)
        {
            ///------------------------------------------------------------------
            /// Charger les paramètres
            ///------------------------------------------------------------------
            XmlSerializer serializer = new XmlSerializer(_type);
            FileStream streamreader = File.OpenRead(_path);
            var result = (XMLDataBase.Level_Initialize)(serializer.Deserialize(streamreader));
            streamreader.Dispose();
            return result;
        }

        public XMLDataBase.Modding_parameters Load_Modding_XML(string _path, Type _type, XMLDataBase.Modding_parameters _object_to_load)
        {
            ///------------------------------------------------------------------
            /// Charger un mod
            ///------------------------------------------------------------------
            XmlSerializer serializer = new XmlSerializer(_type);
            FileStream streamreader = File.OpenRead(_path);
            var result = (XMLDataBase.Modding_parameters)(serializer.Deserialize(streamreader));
            streamreader.Dispose();
            return result;
        }


        public XMLDataBase.window_boxes_init Load_window_params(ref string _path, Type _type)//, XMLDataBase.window_boxes_init _object_to_load)
        {
            ///------------------------------------------------------------------
            /// Charger les paramètres
            ///------------------------------------------------------------------
            XmlSerializer serializer = new XmlSerializer(_type);
            FileStream streamreader = File.OpenRead(_path);
            var result = (XMLDataBase.window_boxes_init)(serializer.Deserialize(streamreader));
            streamreader.Dispose();
            return result;
        }


        public void Create_NewXML_TileObject()
        {
            ///------------------------------------------------------------------
            /// Créer un fichier de paramètres -Partie 1-
            ///------------------------------------------------------------------
            XMLDataBase.TileMap data_xml_tiles = new XMLDataBase.TileMap();
            data_xml_tiles.list_tile = new XMLDataBase.Tiles[tilemap_lenght-1];

            for (int i = 0; i < tilemap_lenght; i++)
            {
                XMLDataBase.Tiles xml_tiles = new XMLDataBase.Tiles();

                xml_tiles.frame_positionX = 0;
                xml_tiles.frame_positionY = 0;

                xml_tiles.texture_ID = 0;

                xml_tiles.logic_type_index = 0;




                data_xml_tiles.list_tile[i] = xml_tiles;
            }

            Write_TileMap_XML(path_tileObject_xml, typeof(XMLDataBase.TileMap), data_xml_tiles);
        }

        public void Save_tilemap_To_XML()
        {
            ///------------------------------------------------------------------
            /// Créer un fichier de paramètres -Partie 1-
            ///------------------------------------------------------------------
            ///------------------------------------------------------------------
            /// Créer un fichier de paramètres -Partie 1-
            ///------------------------------------------------------------------
            data_xml_tiles = new XMLDataBase.TileMap();
            data_xml_tiles.list_tile = new XMLDataBase.Tiles[tilemap_lenght-1];

            for (int i = 0; i < tilemap_lenght; i++)
            {
                XMLDataBase.Tiles xml_tiles = new XMLDataBase.Tiles();

                xml_tiles.frame_positionX = 0;
                xml_tiles.frame_positionY = 0;

                xml_tiles.texture_ID = 0;

                xml_tiles.logic_type_index = 0;

                data_xml_tiles.list_tile[i] = xml_tiles;
            }
            Write_TileMap_XML(path_tileObject_xml, typeof(XMLDataBase.TileMap), data_xml_tiles);
        }
      

        public void Write_TileMap_XML(string _path, Type _type, XMLDataBase.TileMap _object_to_save)
        {
            ///------------------------------------------------------------------
            /// Ecrire dans le fichier map .. xml
            ///------------------------------------------------------------------
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
            XmlSerializer serializer = new XmlSerializer(_type);
            FileStream stream = File.OpenWrite(_path);
            serializer.Serialize(stream, _object_to_save);
            stream.Dispose();
        }


        public void Write_Player_Settings_XML(string _path, Type _type, XMLDataBase.Player_Settings _object_to_save)
        {
            ///------------------------------------------------------------------
            /// Ecrire dans le fichier [player_settings.xml] 
            ///------------------------------------------------------------------
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
            XmlSerializer serializer = new XmlSerializer(_type);
            FileStream stream = File.OpenWrite(_path);
            serializer.Serialize(stream, _object_to_save);
            stream.Dispose();

        }


        public void CreateNewXML_initialize_level()
        {
            ///------------------------------------------------------------------
            /// Créer un fichier de paramètres -Partie 1-
            ///------------------------------------------------------------------
            XMLDataBase.Level_Initialize item = new XMLDataBase.Level_Initialize()
            {
                nombre_de_tuiles_sur_une_ligne = 150,
                nombre_de_tuiles_sur_une_colonne = 30,
                nombre_de_tuiles_a_afficher_sur_une_ligne = 15,
                la_largeur_des_tuiles = 48,
                la_hauteur_des_tuiles = 48,
            };

            Save_Initialize_Level("xml\\Level_initialize.xml", typeof(XMLDataBase.Level_Initialize), item);
        }

        public void Save_Initialize_Level(string _path, Type _type, XMLDataBase.Level_Initialize _object_to_save)
        {
            ///------------------------------------------------------------------
            /// Créer un fichier de paramètres -Partie 2-
            ///------------------------------------------------------------------
            XmlSerializer serializer = new XmlSerializer(_type);
            FileStream stream = File.OpenWrite(_path);
            serializer.Serialize(stream, _object_to_save);
            stream.Dispose();
        }


        public void Create_new_XML_windows_params(string path)
        {
            ///------------------------------------------------------------------
            /// Créer un fichier de paramètres -Partie 1-
            ///------------------------------------------------------------------
            XMLDataBase.window_boxes_init item = new XMLDataBase.window_boxes_init()
            {
                w_positionX = 400,
                w_positionY = 400,
                w_tex_frameW = 16,
                w_tex_frameH = 16,
                w_button_sizeW = 48,
                w_button_sizeH = 48,
            };

            Save_window_boxes_init(path, typeof(XMLDataBase.window_boxes_init), item);
        }

        public void Save_window_boxes_init(string _path, Type _type, XMLDataBase.window_boxes_init _object_to_save)
        {
            ///------------------------------------------------------------------
            /// Créer un fichier de paramètres -Partie 2-
            ///------------------------------------------------------------------
            XmlSerializer serializer = new XmlSerializer(_type);
            FileStream stream = File.OpenWrite(_path);
            serializer.Serialize(stream, _object_to_save);
            stream.Dispose();
        }


    }
}
