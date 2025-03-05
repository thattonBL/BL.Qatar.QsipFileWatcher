namespace BL.Sip.Utilities.Core
{
    public enum ErrorCodes
    {
        // 0 = Unknown (NOT USED)
        DigiAcqErrorCode0000 = 0,

        // File errors are from 100 - 199
        // Could not load xml document
        DigiAcqErrorCode0100 = 100,
        // Could not save xml document
        DigiAcqErrorCode0101,
        // Could not load file
        DigiAcqErrorCode0102,
        // File does not exist
        DigiAcqErrorCode0103,
        // Cannot determine filename
        DigiAcqErrorCode0104,
        // Cannot delete file
        DigiAcqErrorCode0105,
        // Cannot delete directory
        DigiAcqErrorCode0106,
        // Cannot access directory
        DigiAcqErrorCode0107,
        // Cannot copy file
        DigiAcqErrorCode0108,
        // Cannot move file
        DigiAcqErrorCode0109,
        //Cannot find subfolders
        DigiAcqErrorCode0110,

        //DdexErrors are from 200 - 299
        //Cannot locate source metadata
        DigiAcqErrorCode0200,
        //Could not get the latest release date
        DigiAcqErrorCode0201,

        //Version data errors from 300 - 399
        //Cannot find mdWrap element 
        DigiAcqErrorCode0300,
        //Cannot find xml data element
        DigiAcqErrorCode0301,
        //Could not find metsHdr element
        DigiAcqErrorCode0302,
        //Catch all for version data creation errors
        DigiAcqErrorCode0303,

        // Publisher errors are from 400 to 499
        //Missing Download folder
        DigiAcqErrorCode0405,
        //Missing Input Folder
        DigiAcqErrorCode0406,
        //Missing Working folder
        DigiAcqErrorCode0408,
        // Missing server name connection parameter
        DigiAcqErrorCode0414,
        // Missing subfolder connection parameter
        DigiAcqErrorCode0415,
        // Folder parameter. Missing root folder
        DigiAcqErrorCode0416,
        // Folder parameter. Root folder must be a share
        DigiAcqErrorCode0417,
        // Folder parameter. Missing download folder
        DigiAcqErrorCode0418,
        // Folder parameter. Missing working folder
        DigiAcqErrorCode0419,
        //Error reading content file extentsions from Suppliers.xml
        DigiAcqErrorCode0420,

        // Cannot open mets file
        DigiAcqErrorCode0501,
        // Error finding element or attribute in mets
        DigiAcqErrorCode0502,
        // Error adding access Elements to the mets
        DigiAcqErrorCode0503,
        //Duplicate transformation file reference
        DigiAcqErrorCode0504,
        //Cannot create content file
        DigiAcqErrorCode0505,
        //Cannot validate hash
        DigiAcqErrorCode0506,

        //Create mets failed
        DigiAcqErrorCode0600,
        //Cannot save the file
        DigiAcqErrorCode0601,
        //Missing Mets Element
        DigiAcqErrorCode0602,
        //Cannot determine filename
        DigiAcqErrorCode0604,
        //Cannot save dsami data file
        DigiAcqErrorCode0605,
        //Cannot Find DDEX for adding rights to mets
        DigiAcqErrorCode0606,

        // Ftp errors are from 700 - 799
        // Cant get file list
        DigiAcqErrorCode0700 = 700,
        // Cant download file
        DigiAcqErrorCode0701,
        // Could not upload file
        DigiAcqErrorCode0702,
        // Could not connect to ftp site
        DigiAcqErrorCode0703,

        //Transcoding Errors are form 800 - 899
        //There are no source audio files found
        DigiAcqErrorCode0800,
        //Unable to define a source location for the mp4
        DigiAcqErrorCode0801,
        //Unable to contact the AvTransformation Service
        DigiAcqErrorCode0802,

        //Sami Import File Errors from 0900
        //Error Appending Sami Text File
        DigiAcqErrorCode0900,
        //Error Uploading Sami Text File
        DigiAcqErrorCode0901,

        // Validation Error Directory specified doesn't exist
        DigiAcqErrorCode1600,
        // Validation Error missing file types specified in suppliers xml
        DigiAcqErrorCode1601,
        // Validation Error unexpected file types found in submission
        DigiAcqErrorCode1602,

        // Misc errors are from 8000 - 8999
        // Could not mint an Ark
        DigiAcqErrorCode8001,
        // Could not mint an Ark
        DigiAcqErrorCode8009,
        // Could not clear workflow entity claim and annotation
        DigiAcqErrorCode8010,

        // Misc errors are from 9000 - 9199
        // Missing parameters in context
        DigiAcqErrorCode9000,
        // Submission file location missing
        DigiAcqErrorCode9001,
        // Could not add content link
        DigiAcqErrorCode9003,
        // Unity could not resolve interface
        DigiAcqErrorCode9004,
        // Xml file is invalid 
        DigiAcqErrorCode9005,
        // Image file is invalid
        DigiAcqErrorCode9006,
        // Error saving content files
        DigiAcqErrorCode9007,
        // Error finding ingestable content file paths
        DigiAcqErrorCode9008,
        // Error running MediaInfo
        DigiAcqErrorCode9009,
        //Cannot read transformation message from queue
        DigiAcqErrorCode9010,
        //Unable to find transformed content file
        DigiAcqErrorCode9011,
        //Cannot read Workflow Entityt from Context
        DigiAcqErrorCode9012,
        //Unexpected droid puid found
        DigiAcqErrorCode9013,
        //Droid puid not found in allowed list
        DigiAcqErrorCode9014,
        //Error parsing droid output
        DigiAcqErrorCode9015,
        //Cannot convert app settings config time to seconds
        DigiAcqErrorCode9016,
        // Workflow entity has no children and at least 1 is expected
        DigiAcqErrorCode9017,
        // Can not delete parent submission after parent was rejected because there are some children created which have not been completed yet
        DigiAcqErrorCode9018,
        //Error transforming ddex to marc
        DigiAcqErrorCode9019,
        //Error validation mp4 files
        DigiAcqErrorCode9020,
        //Error adding mediaMD to mets
        DigiAcqErrorCode9021,
        //Error transforming media info to mediaMD
        DigiAcqErrorCode9022,
        //Mismatch between number of mets and marc recordings
        DigiAcqErrorCode9023,
        //Error Saving Wf Entity
        DigiAcqErrorCode9024
    }
}
