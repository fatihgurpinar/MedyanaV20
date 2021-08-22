using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Utilities
{
    public class Messages
    {
        public static string OK_LISTED_MESSAGE = "LISTED";
        public static string OK_ADDED_MESSAGE = "ADDED";
        public static string OK_UPDATED_MESSAGE = "UPDATED";
        public static string OK_DELETED_MESSAGE = "DELETED";

        public static int OK_LISTED_CODE = 4;
        public static int OK_ADDED_CODE = 1;
        public static int OK_UPDATED_CODE = 2;
        public static int OK_DELETED_CODE = 3;

        public static string OK_MESSAGE = "OK";

        public static int EXCEPTION_CODE_IN_CONTROLLER = -500;
        public static string EXCEPTION_MESSAGE_IN_CONTROLLER = "Exception in controller";

        public static int ERROR_CODE = -900;
        public static string ERROR_MESSAGE = "Error";

        public static int NOT_FOUND_ERROR_CODE = -200;
        public static string NOT_FOUND_ERROR_MESSAGE = "Not found";

        public static int DB_ERROR_CODE = -300;
    }
}
