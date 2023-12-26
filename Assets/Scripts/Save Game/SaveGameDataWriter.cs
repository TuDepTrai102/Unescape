using System;
using System.IO;
using UnityEngine;

namespace NT
{
    public class SaveGameDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string dataSaveFileName = "";

        public string saveDataDirectoryPath_02 = "";
        public string dataSaveFileName_02 = "";

        public string saveDataDirectoryPath_bonfire = "";
        public string dataSaveFileName_bonfire = "";

        public CharacterSaveData LoadCharacterDataFromJson()
        {
            string savePath = Path.Combine(saveDataDirectoryPath, dataSaveFileName);

            CharacterSaveData loadedSaveData = null;

            if (File.Exists(savePath))
            {
                try
                {
                    string saveDataToLoad = "";

                    using (FileStream stream = new FileStream(savePath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            saveDataToLoad = reader.ReadToEnd();
                        }
                    }

                    //DESERIALIZE DATA
                    loadedSaveData = JsonUtility.FromJson<CharacterSaveData>(saveDataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(ex.Message);
                }
            }
            else
            {
                Debug.Log("SAVE FILE DOES NOT EXIST");
            }

            return loadedSaveData;
        }

        public void WriteCharacterDataToSaveFile(CharacterSaveData characterData)
        {
            //CREATES PATH TO SAVE OUR FILE
            string savePath = Path.Combine(saveDataDirectoryPath, dataSaveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("SAVE PATH = " + savePath);

                //SERIALIZE THE C# GAME DATA OBJECT TO JSON TYPE
                string dataToStore = JsonUtility.ToJson(characterData, true);

                //WRITE THE FILE TO OUR SYSTEM
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR WHILE TRYING TO SAVE DATA, GAME COULD NOT BE SAVED" + ex);
            }
        }

        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, dataSaveFileName));
        }

        public bool CheckIfSaveFileExists()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, dataSaveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //START SCREEN DATA FILE
        public _CharacterStartScreenData LoadCharacterDataFromJson_START()
        {
            string savePath = Path.Combine(saveDataDirectoryPath_02, dataSaveFileName_02);

            _CharacterStartScreenData loadedSaveData = null;

            if (File.Exists(savePath))
            {
                try
                {
                    string saveDataToLoad = "";

                    using (FileStream stream = new FileStream(savePath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            saveDataToLoad = reader.ReadToEnd();
                        }
                    }

                    //DESERIALIZE DATA
                    loadedSaveData = JsonUtility.FromJson<_CharacterStartScreenData>(saveDataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(ex.Message);
                }
            }
            else
            {
                Debug.Log("SAVE FILE DOES NOT EXIST");
            }

            return loadedSaveData;
        }

        public void WriteCharacterDataToSaveFile_START(_CharacterStartScreenData characterData)
        {
            //CREATES PATH TO SAVE OUR FILE
            string savePath = Path.Combine(saveDataDirectoryPath_02, dataSaveFileName_02);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("SAVE PATH = " + savePath);

                //SERIALIZE THE C# GAME DATA OBJECT TO JSON TYPE
                string dataToStore = JsonUtility.ToJson(characterData, true);

                //WRITE THE FILE TO OUR SYSTEM
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR WHILE TRYING TO SAVE DATA, GAME COULD NOT BE SAVED" + ex);
            }
        }

        public void DeleteSaveFile_START()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath_02, dataSaveFileName_02));
        }

        public bool CheckIfSaveFileExists_START()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath_02, dataSaveFileName_02)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //BONFIRE DATA FILE
        public BonfireActivatedData LoadCharacterDataFromJson_Bonfire()
        {
            string savePath = Path.Combine(saveDataDirectoryPath_bonfire, dataSaveFileName_bonfire);

            BonfireActivatedData loadedSaveData = null;

            if (File.Exists(savePath))
            {
                try
                {
                    string saveDataToLoad = "";

                    using (FileStream stream = new FileStream(savePath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            saveDataToLoad = reader.ReadToEnd();
                        }
                    }

                    //DESERIALIZE DATA
                    loadedSaveData = JsonUtility.FromJson<BonfireActivatedData>(saveDataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(ex.Message);
                }
            }
            else
            {
                Debug.Log("SAVE FILE DOES NOT EXIST");
            }

            return loadedSaveData;
        }

        public void WriteCharacterDataToSaveFile_Bonfire(BonfireActivatedData bonfireData)
        {
            //CREATES PATH TO SAVE OUR FILE
            string savePath = Path.Combine(saveDataDirectoryPath_bonfire, dataSaveFileName_bonfire);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("SAVE PATH = " + savePath);

                //SERIALIZE THE C# GAME DATA OBJECT TO JSON TYPE
                string dataToStore = JsonUtility.ToJson(bonfireData, true);

                //WRITE THE FILE TO OUR SYSTEM
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR WHILE TRYING TO SAVE DATA, GAME COULD NOT BE SAVED" + ex);
            }
        }

        public void DeleteSaveFile_Bonfire()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath_bonfire, dataSaveFileName_bonfire));
        }

        public bool CheckIfSaveFileExists_Bonfire()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath_bonfire, dataSaveFileName_bonfire)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}