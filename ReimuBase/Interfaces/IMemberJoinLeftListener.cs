using ReimuAPI.ReimuBase.TgData;

namespace ReimuAPI.ReimuBase.Interfaces
{
    public interface IMemberJoinLeftListener
    {
        CallbackMessage OnGroupMemberJoinReceive(TgMessage RawMessage, string JsonMessage, UserInfo JoinedUser);
        CallbackMessage OnSupergroupMemberJoinReceive(TgMessage RawMessage, string JsonMessage, UserInfo JoinedUser);

        CallbackMessage OnGroupMemberLeftReceive(TgMessage RawMessage, string JsonMessage, UserInfo JoinedUser);
        CallbackMessage OnSupergroupMemberLeftReceive(TgMessage RawMessage, string JsonMessage, UserInfo JoinedUser);
    }
}