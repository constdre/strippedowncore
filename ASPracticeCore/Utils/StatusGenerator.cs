using ASPracticeCore.Utils;

public class StatusGenerator{

    public static string getSuccessMessage(string custom){
        string message = Constants.SUCCESS + "_" + custom;
        return message;
    }
    public static string getFailedMessage(string custom){
        string message = Constants.FAILED + "_" + custom;
        return message;
    }
}