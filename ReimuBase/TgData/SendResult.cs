﻿namespace ReimuAPI.ReimuBase.TgData
{
    public class SendResult
    {
        public bool ok { get; set; }
        public int error_code { get; set; }
        public string description { get; set; }
        public ApiResult httpContent { get; set; }
    }

    public class SendMessageResult : SendResult
    {
        public TgMessage result { get; set; }
    }

    public class SetActionResult : SendResult
    {
        public bool result { get; set; }
    }

    public class ChatInfoRequest : SendResult
    {
        public ChatInfo result { get; set; }
    }

    public class UserInfoRequest : SendResult
    {
        public UserInfo result { get; set; }
    }

    public class MembersCountResult : SendResult
    {
        public int result { get; set; }
    }
}