namespace ReimuAPI.ReimuBase.TgData
{
    public class UserInfo
    {
        public int id { get; set; }

        public bool is_bot { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string username { get; set; }

        public string language_code { get; set; }

        public string status { get; set; }

        public string full_name()
        {
            string name = first_name;
            if (last_name != null) name += last_name;
            return name;
        }

        public string GetUserTextInfo()
        {
            string info = "\nID : " + id;
            if (first_name != null) info += "\nFirst name : " + first_name;
            if (last_name != null) info += "\nLast name : " + last_name;
            if (username != null) info += "\nUsername : @" + username;
            if (language_code != null) info += "\nLanguage code : " + language_code;
            return info;
        }

        public string GetUserTextInfo_ESCMD()
        {
            string info = "\nID : " + id;
            if (first_name != null) info += "\nFirst name : " + first_name;
            if (last_name != null) info += "\nLast name : " + last_name;
            if (username != null) info += "\nUsername : @" + username;
            if (language_code != null) info += "\nLanguage code : " + language_code;

            info = RAPI.escapeMarkdown(info);

            return info;
        }
        
        public string GetUserTextInfo_MD()
        {
            string info = "\nID : `" + id + "`";
            if (first_name != null) info += "\nFirst name : `" + RAPI.escapeMarkdown(first_name) + "`";
            if (last_name != null) info += "\nLast name : `" + RAPI.escapeMarkdown(last_name) + "`";
            if (username != null) info += "\nUsername : @" + RAPI.escapeMarkdown(username);
            if (language_code != null) info += "\nLanguage code : `" +  RAPI.escapeMarkdown(language_code) + "`";

            return info;
        }
    }
}