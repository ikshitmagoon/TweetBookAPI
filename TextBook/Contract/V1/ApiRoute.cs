namespace TweetBook.Contract.V1
{
    public static class ApiRoute
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = $"{Root}/{Version}";
        public static class Posts
        {
            public const string GetAll = $"{Base}/posts";
            public const string Get = $"{Base}"+"/posts/{postId}";
            //public const string GetTest= $"{Base}" + "/posts/{postId?}";
            public const string Create = Base+ "/posts";
            public const string Update = $"{Base}" + "/posts/{postId}";
            public const string Delete = $"{Base}" + "/posts/{postId}";
        }

        public static class Identity
        {
            public const string Register = Base + "/identity/register";
            public const string Login = Base + "/identity/login";
        }

    }
}
