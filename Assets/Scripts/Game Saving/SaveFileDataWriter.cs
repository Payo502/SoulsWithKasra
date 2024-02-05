using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    // BEFORE CREATING A NEW SAVE FILE, CHECK TO SEE IF ONE OF THIS CHARACTER ALREADY EXISTS
    public bool CheckToSeeIfFileExists()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // DELETE SAVE FILES
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }

    // CREATE A SAVE FILE 
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            // CREATE THE DIRECTORY THE FILE WILL BE WRITTEN TO
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("CREATING SAVE FILE, AT SAVE PATH: " + savePath);

            // SERIALIZE THE C# GAME DATA OBJECT INTO JSON
            string dataToStore = JsonUtility.ToJson(characterData, true);

            // WRITE THE FILE TO THE COMPUTER
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR WHILE TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED" + savePath + "\n" + ex);
        }
    }

    // USED TO LOAD A SAVE FILE
    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;

        // NAME PATH TO LOAD THE FILE FROM
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try
            {

                string dataToLoad = "";

                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // DESERIALIZE THE DATA FROM JSON BACK TO UNITY
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch(Exception ex)
            {

            }
        }


        return characterData;
    }
}
