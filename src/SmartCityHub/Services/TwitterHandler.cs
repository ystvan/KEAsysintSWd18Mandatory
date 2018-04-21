
using System;
using Tweetinvi;

namespace SmartCity911.Services
{
    public static class TwitterHandler
    {
        private static string APIKey = "d8xhCmtEhpiAAD8TA8o1XJlnF";
        private static string APISecret = "pIWUnfEKdJTtPsdMZ3cxVv1pNrRzsFrdffprtSznShVh3OGTUR";
        private static string AccessToken = "983017295353188353-K92gi9Bj7RJu0n8o8v0CTGaeeKhMZjm";
        private static string AccessTokenSecret = "Dtee5PhpusAQzMBEWMa34vuoOWttunNWwa6wMPZB17J7d";

        public static void InitAuth()
        {
            try
            {
                Auth.SetUserCredentials(APIKey, APISecret, AccessToken, AccessTokenSecret);
                var user = User.GetAuthenticatedUser();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
           
        }

        public static void Alarm(string msg)
        {
            try
            {
                Tweet.PublishTweet(msg);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
      
    }

  
}
