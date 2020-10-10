namespace TakeProject.Server.Constants
{
    public class ServerMessageConstants
    {
        public const string PROVIDE_NICKNAME = "*** Welcome to our chat server.Please provide a nickname: ";
        public const string NICKNAME_ALREADY_TAKEN = "*** Sorry, the nickname {0} is already taken. Please choose a different one: ";
        public const string NICKNAME_INVALID = "*** Sorry, the nickname {0} is invalid. Please choose a different one: ";
        public const string SUCCESSFULLY_REGISTERED = "*** You are registered as {0}. Joining #general";
        public const string JOINED_GENERAL_CHANNEL = "\"{0}\" has joined #general";
        public const string GENERAL_MESSAGE = "{0} says: {1}";
        public const string PRIVATE_MESSAGE = "{0} says privately to {1}: {2}";
        public const string USER_NOT_FOUND = "The user {0} was not found.";
        public const string DISCONNECT_MESSAGE = "*** Disconected. Bye!";
        public const string COMMAND_INVALID = "*** Sorry, the command {0} is invalid. If in doubt, use /help to list the commands.";
        public const string SUCCESSFULLY_CHANGED_NICKNAME = "*** Your nickname has been successfully changed to {0}.";
        public static string GetMessage(string message, params string[] args) => string.Format(message, args);
    }
}
