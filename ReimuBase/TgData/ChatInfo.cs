﻿using ReimuAPI.ReimuBase.TgData.MediaMessage;

namespace ReimuAPI.ReimuBase.TgData
{
    public class ChatInfo
    {
        public long id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string username { get; set; }

        public string type { get; set; }

        public string title { get; set; }

        public bool all_members_are_administrators { get; set; } = false;

        public Photo photo { get; set; }

        public string description { get; set; }

        public string invite_link { get; set; }

        public TgMessage pinned_message { get; set; }

        public string GetChatTextInfo()
        {
            string ChatInfo = "ID: " + id;
            ChatInfo += "\nType: " + type;
            if (first_name != null) ChatInfo += "\nFirst name: " + first_name;
            if (title != null) ChatInfo += "\nLast name: " + title;
            if (title != null) ChatInfo += "\nTitle: " + title;
            if (username != null) ChatInfo += "\nUsername : @" + username;
            if (all_members_are_administrators) ChatInfo += "\nAll members are admin: true";
            return ChatInfo;
        }
        
        public string GetChatTextInfoMarkdown()
        {
            string ChatInfo = "ID: `" + id + "`";
            ChatInfo += "\nType: `" + type + "`";
            if (first_name != null) ChatInfo += "\nFirst name: `" + RAPI.escapeMarkdown(first_name) + "`";
            if (title != null) ChatInfo += "\nLast name: `" + RAPI.escapeMarkdown(title) + "`";
            if (title != null) ChatInfo += "\nTitle: `" + RAPI.escapeMarkdown(title) + "`";
            if (username != null) ChatInfo += "\nUsername : @" + RAPI.escapeMarkdown(username);
            if (all_members_are_administrators) ChatInfo += "\nAll members are admin: `true`";
            return ChatInfo;
        }
        
    }
}