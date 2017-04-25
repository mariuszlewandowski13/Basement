
public abstract class FilesInfoLoader {

    #region Private Properties

    protected static string filesPath = "C:/files/";

    #endregion

    #region Constructors

    protected FilesInfoLoader(string path)
    {
        filesPath = path;
    }

    protected FilesInfoLoader()
    {
    }

    #endregion



}
