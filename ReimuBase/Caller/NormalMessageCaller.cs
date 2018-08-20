using System;
using System.Collections.Generic;
using ReimuAPI.ReimuBase.TgData;

namespace ReimuAPI.ReimuBase.Caller
{
    public class NormalMessageCaller
    {
        private static readonly string myUsername = TgApi.getDefaultApiConnection().getMe().username.ToLower();
        private static readonly int myUsernameLength = TgApi.getDefaultApiConnection().getMe().username.Length;

        public void call(TgMessage message, string JsonMessage)
        {
            if (RAPI.getIsDebugEnv()) Console.WriteLine("Message Caller : NormalMessageCaller");
            if (TempData.pluginsList == null) RAPI.loadPlugins();
            List<PluginObject> plugins = TempData.pluginsList;
            string messageType = message.chat.type.Substring(0, 1).ToUpper() + message.chat.type.Substring(1).ToLower();
            if (message.text != null)
            {
                if (message.entities != null) // 收到蓝字
                    if (message.entities[0].type == "bot_command")
                    {
                        if (message.chat.type == "private" && message.text.Length >= 6)
                            if (message.text.Substring(0, 6) == "/start")
                            {
                                if (RAPI.getIsDebugEnv())
                                    Console.WriteLine("Message Caller : NormalMessageCaller -> OnStartReceive");
                                if (message.text.Length > 7)
                                    PluginsCaller.callStartReceiver(
                                        plugins,
                                        "OnStartReceive",
                                        JsonMessage,
                                        new object[] {message, JsonMessage, message.text.Substring(7)}
                                    );
                                else
                                    PluginsCaller.callStartReceiver(
                                        plugins,
                                        "OnStartReceive",
                                        JsonMessage,
                                        new object[] {message, JsonMessage}
                                    );
                                return;
                            }

                        if (message.entities[0].offset == 0)
                        {
                            string command = message.text.Substring(0, message.entities[0].length).ToLower();
                            if (RAPI.getIsDebugEnv())
                                Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType +
                                                  "CommandReceive");
                            if (command.IndexOf("@") != -1)
                            {
                                if (command.IndexOf("@" + TgApi.getDefaultApiConnection().getMe().username.ToLower()) !=
                                    -1)
                                    if (command.Substring(command.Length - myUsernameLength) == myUsername)
                                    {
                                        PluginsCaller.callCommandReceiver(
                                            plugins,
                                            "On" + messageType + "CommandReceive",
                                            JsonMessage,
                                            new object[]
                                            {
                                                message, JsonMessage,
                                                command.Substring(0, command.Length - myUsernameLength - 1)
                                            });
                                        return; // 收到命令
                                    }
                            }
                            else
                            {
                                PluginsCaller.callCommandReceiver(
                                    plugins,
                                    "On" + messageType + "CommandReceive",
                                    JsonMessage,
                                    new object[] {message, JsonMessage, command});
                                return; // 收到命令
                            }
                        }
                    }

                if (message.forward_from != null)
                {
                    if (RAPI.getIsDebugEnv())
                        Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType +
                                          "ForwardedUserMessageReceive");
                    PluginsCaller.callTextReceiver(plugins, "On" + messageType + "ForwardedUserMessageReceive",
                        JsonMessage, new object[] {message, JsonMessage, message.forward_from});
                    return; // 收到转发自某个用户的消息
                }

                if (message.forward_from_chat != null)
                {
                    if (RAPI.getIsDebugEnv())
                        Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType +
                                          "ForwardedChatMessageReceive");
                    PluginsCaller.callTextReceiver(plugins, "On" + messageType + "ForwardedChatMessageReceive",
                        JsonMessage, new object[] {message, JsonMessage, message.forward_from_chat});
                    return; // 收到转发自某个频道的消息
                }

                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "MessageReceive");
                PluginsCaller.callTextReceiver(plugins, "On" + messageType + "MessageReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.text});
                return; // 收到普通信息
            }

            if (message.new_chat_member != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "MemberJoinReceive");
                PluginsCaller.callMemberJoinReceiver(plugins, "On" + messageType + "MemberJoinReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.new_chat_member});
                return; // 收到新成员加入或被拉入的消息
            }

            if (message.left_chat_member != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "MemberLeftReceive");
                PluginsCaller.callMemberJoinReceiver(plugins, "On" + messageType + "MemberLeftReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.left_chat_member});
                return; // 收到成员退出群组或被踢出的消息
            }

            if (message.audio != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "AudioReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "AudioReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.audio});
                return; // 收到音频文件
            }

            if (message.document != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "DocumentReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "DocumentReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.document});
                return; // 收到文档
            }

            if (message.game != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "GameReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "GameReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.game});
                return; // 收到
            }

            if (message.photo != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "PhotoReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "PhotoReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.photo});
                return; // 收到 Photo
            }

            if (message.sticker != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "StickerReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "StickerReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.sticker});
                return; // 收到贴图（表情）
            }

            if (message.video != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "VideoReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "VideoReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.video});
                return; // 收到 Video
            }

            if (message.voice != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "VoiceReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "VoiceReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.voice});
                return; // 收到 Voice
            }

            if (message.video_note != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "VideoNoteReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "VideoNoteReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.video_note});
                return; // 收到 Voice Note
            }

            if (message.contact != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "ContactReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "ContactReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.contact});
                return; // 收到联系人
            }

            if (message.location != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "LocationReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "LocationReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.location});
                return; // 收到位置信息
            }

            if (message.venue != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "VenueReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "VenueReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.venue});
                return; // 收到实体地点信息
            }

            if (message.new_chat_title != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine(
                        "Message Caller : NormalMessageCaller -> On" + messageType + "NewChatTitleReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "NewChatTitleReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.new_chat_title});
                return; // 收到新的群标题
            }

            if (message.new_chat_photo != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine(
                        "Message Caller : NormalMessageCaller -> On" + messageType + "NewChatPhotoReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "NewChatPhotoReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.new_chat_photo});
                return; // 收到新的群组头像
            }

            if (message.delete_chat_photo)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType +
                                      "ChatPhotoDeletedReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "ChatPhotoDeletedReceive", JsonMessage,
                    new object[] {message});
                return; // 群头被删了
            }

            if (message.group_chat_created)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> OnGroupCreatedReceive");
                PluginsCaller.callPlugins(plugins, "OnGroupCreatedReceive", JsonMessage, new object[] {message});
                return; // 收到群组被创建
            }

            if (message.supergroup_chat_created)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> OnSupergroupCreatedReceive");
                PluginsCaller.callPlugins(plugins, "OnSupergroupCreatedReceive", JsonMessage, new object[] {message});
                return; // 收到超级群被创建
            }

            if (message.channel_chat_created)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> OnChannelCreatedReceive");
                PluginsCaller.callPlugins(plugins, "OnChannelCreatedReceive", JsonMessage, new object[] {message});
                return; // 收到频道被创建
            }

            if (message.migrate_to_chat_id != -1)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> OnMigrateToChatReceive");
                PluginsCaller.callPlugins(plugins, "OnMigrateToChatReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.migrate_to_chat_id});
                return; // 收到群组 ID 变更去那里
            }

            if (message.migrate_from_chat_id != -1)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> OnMigrateFromChatReceive");
                PluginsCaller.callPlugins(plugins, "OnMigrateFromChatReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.migrate_from_chat_id});
                return; // 收到群组 ID 从那里变更
            }

            if (message.invoce != null)
            {
                if (RAPI.getIsDebugEnv())
                    Console.WriteLine("Message Caller : NormalMessageCaller -> On" + messageType + "InvoiceReceive");
                PluginsCaller.callPlugins(plugins, "On" + messageType + "InvoiceReceive", JsonMessage,
                    new object[] {message, JsonMessage, message.invoce});
                return; // 收到账单
            }

            if (RAPI.getIsDebugEnv()) Console.WriteLine("Message Caller : NormalMessageCaller -> ReceiveOtherMessage");

            PluginsCaller.callOtherMessageReceiver(plugins, "ReceiveOtherMessage", JsonMessage,
                new object[] {message, JsonMessage}); // 未知的消息类型，统一 Call 其他
        }
    }
}