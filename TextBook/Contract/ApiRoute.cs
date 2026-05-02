namespace TweetBook.Contract
{
    public static class ApiRoute
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = $"{Root}/{Version}";
        public static class Posts
        {
            public const string GetAll = $"{Base}/posts";
            public const string Get = $"{Root}/user";
            public const string Create = "create";
            public const string Update = "update";
            public static readonly string Delete = "delete";
        }

    }
}
